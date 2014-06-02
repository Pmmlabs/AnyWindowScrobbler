using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using Lastfm.Services;
using Lastfm.Scrobbling;
using Lastfm;

namespace AnyWindowScrobbler
{
    public partial class FormMain : Form
    {
        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);
        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        List<IntPtr> hWnds;
        IntPtr CurHwnd;
        string API_KEY = "9ef93f02fa952d79ef45e670c32be729";
        string API_SECRET = "9a7a52089a6656640669ea2a2af3b523";
        int SCROBBLE_INTERVAL = 120; // number of seconds, that must come before track will be scrobbled
        List<string> SystemProcesses = new List<string>();
        List<string> MusicExtensions = new List<string>();

        public FormMain()
        {
            InitializeComponent();
            
            // processes, which must be ignored when constructing windows list
            SystemProcesses.Add(@"C:\Windows\Explorer.EXE");
            SystemProcesses.Add(@"C:\Windows\system32\Dwm.exe");
            SystemProcesses.Add(@"C:\Program Files\Windows Sidebar\sidebar.exe");
            SystemProcesses.Add(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            MusicExtensions.Add(".mp3");
            MusicExtensions.Add(".flac");
            MusicExtensions.Add(".wav");
            MusicExtensions.Add(".ape");
            MusicExtensions.Add(".m4v");
        }

        private void Log(string p)
        {
            using (StreamWriter outfile = new StreamWriter(@"Log.txt", true))
            {
                outfile.WriteLine("[" + DateTime.Now.ToString() + "] " + p);
                outfile.Flush();
            }
        }
        // Refresh list of windows
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            hWnds = new List<IntPtr>();
            EnumWindows((hWnd, lParam) =>
            {                
                if ((IsWindowVisible(hWnd) || IsIconic(hWnd)) && GetWindowTextLength(hWnd) != 0)
                {
                    string procname = GetProcnameByHwnd(hWnd);
                    if (!SystemProcesses.Contains(procname))
                    {
                        listBox.Items.Add(GetWindowText(hWnd));
                        hWnds.Add(hWnd);
                        if (procname == Properties.Settings.Default.Process)
                            CurHwnd = hWnd;
                    }
                }
                return true;
            }, IntPtr.Zero);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
            if (Properties.Settings.Default.StartMinimized)
            {
                WindowState = FormWindowState.Minimized;
                FormMain_Resize(this, null);
                cbStartMin.Checked = true;
            }
            if (Properties.Settings.Default.Process != null && Properties.Settings.Default.Process.Length != 0)
                lblPlayer.Text = "Current player: " + Properties.Settings.Default.Process;
            else
                lblPlayer.Text = "Player not set!";
        }
        /*  < winapi functions>   */
        private string GetWindowText(IntPtr hWnd)
        {
            int len = GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(len);
            len = GetWindowText(hWnd, sb, len);
            return sb.ToString(0, len);
        }

        private string GetProcnameByHwnd(IntPtr hwnd)
        {
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p.MainModule.FileName;
        }
        /*  </ winapi functions> */
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                tbName.Text = listBox.SelectedItem.ToString();
                tbName.Focus();
                tbName.SelectAll();
            }            
        }

        // Save settings of player
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "" || tbName.SelectionLength == 0)
                MessageBox.Show("Nothing is selected");
            else
                if (tbName.SelectedText.IndexOf('-') == -1)
                    MessageBox.Show("No defis in selection");
                else
                {
                    Properties.Settings.Default.Process = GetProcnameByHwnd(hWnds[listBox.SelectedIndex]);
                    CurHwnd = hWnds[listBox.SelectedIndex];
                    lblPlayer.Text = "Current player: " + Properties.Settings.Default.Process;

                    // Detecting rules, how get track title from window title
                    string TextBefore = tbName.Text.Substring(0, tbName.SelectionStart);
                    string TextAfter =  tbName.Text.Substring(tbName.SelectionStart + tbName.SelectionLength,
                                                                tbName.Text.Length - tbName.SelectionLength - tbName.SelectionStart);
                    string patternBrakes = @"\s*(\(|\[)[^\)\]]+(\)|\])\s*"; // something in brakes (circled or squared)
                    Regex rgx = new Regex(patternBrakes, RegexOptions.IgnoreCase);
                    MatchCollection matches;
                    
                    if (rgx.Match(TextAfter).Success)
                        Properties.Settings.Default.CuttedAfter = patternBrakes;
                    else
                        Properties.Settings.Default.CuttedAfter = TextAfter;
                    
                    if (rgx.Match(TextBefore).Success)
                        Properties.Settings.Default.CuttedBefore = patternBrakes;
                    else
                    {
                        string patternNumber = @"^\d{1,3}(\.\s|\s-\s)"; // number of track ("12." or "12 - ")
                        rgx = new Regex(patternNumber, RegexOptions.IgnoreCase);
                        if (rgx.Match(TextBefore).Success)
                        {
                            matches = rgx.Matches(TextBefore);
                            Properties.Settings.Default.CuttedBefore = @"(\d{1,3}" + Regex.Escape(matches[0].Groups[1].Value)+")?";
                        }
                        else   // text before - not regexp, just text which needed to cut.
                            Properties.Settings.Default.CuttedBefore = TextBefore;
                    }
                    
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Saved");
                }
        }
        /*  < Last.Fm functions>  */
        Session session;
        Connection connection;
        AuthenticatedUser user;
        string curtrack = "";
        int timeCounter = 0;
        bool scrobbled = false;
             
        void onSuccessLogin()
        {
            user = AuthenticatedUser.GetUser(session);
            lblStatus.Text = "Connected: " + user.Name;

            timerScrobble.Enabled = true;
            timerConnection.Enabled = false;
            connection = new Connection(session);           
        }

        // Connect to Last.fm
        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.SessionKey.Length == 32)
                    session = new Session(API_KEY, API_SECRET, Properties.Settings.Default.SessionKey);
                else
                    session = new Session(API_KEY, API_SECRET);
                if (!session.Authenticated)
                {
                    Process.Start(session.GetWebAuthenticationURL());
                    timerConnection.Enabled = true;
                }
                else
                    onSuccessLogin();
            }
            catch (WebException)
            {
                lblStatus.Text = "Network error. Reconnecting...";
                notifyIcon.Text = lblStatus.Text;
                timerReconnection.Enabled = true; // after 5 seconds retry to connect
            }
        }

        // Retrying to connect to Last.fm
        private void timerConnection_Tick(object sender, EventArgs e)
        {
            try
            {
                session.AuthenticateViaWeb();
                if (session.Authenticated)
                {
                    Properties.Settings.Default.SessionKey = session.SessionKey;
                    Properties.Settings.Default.Save();
                    onSuccessLogin();
                }
                else
                    lblStatus.Text = "Connecting...";
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
        // reconnection if error while authorization
        private void timerReconnection_Tick(object sender, EventArgs e)
        {
            timerReconnection.Enabled = false;
            Form1_Shown(this, null);            
        }

        // Check every 5 seconds and scrobble if needed
        private void timerScrobble_Tick(object sender, EventArgs e)
        {
            if ((int)CurHwnd != 0) // if window exists, i.e. player is running
            {
                string windowTitle = GetWindowText(CurHwnd);
                // Check if window title matches conditions
                string pattern = Properties.Settings.Default.CuttedBefore + @"(?<txt>[\s\S]+)" + Properties.Settings.Default.CuttedAfter;
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(windowTitle);
                if (matches.Count > 0)
                {
                    windowTitle = matches[0].Groups["txt"].Value; // if matches, take group named "txt" - it is name of playing track
                    // remove extensions from window title
                    foreach (string ext in MusicExtensions)
                    {
                        int indexof = windowTitle.IndexOf(ext);
                        if (indexof != -1)
                        {
                            windowTitle = windowTitle.Remove(indexof, ext.Length);
                            break;
                        }
                    }

                    if (!curtrack.Equals(windowTitle))
                    {   // song switched
                        curtrack = windowTitle;
                        Text = "Now playing: " + curtrack;
                        notifyIcon.Text = (Text.Length > 63 ? Text.Substring(0,62) : Text); // notifyIcon.Text is limited by 64 chars
                        string[] trackInfo = new string[2];
                        if (curtrack.IndexOf('-') != -1) // if in track title no defis, send "Unnamed Artist" instead 
                            curtrack = "Unnamed Artist - " + curtrack;
                        trackInfo = curtrack.Split('-'); 
                        try
                        {
                            connection.ReportNowplaying(new NowplayingTrack(trackInfo[0].Trim(), trackInfo[1].Trim()));
                            timeCounter = 0;
                            scrobbled = false;
                        }
                        catch (Exception ex)
                        {
                            Log(ex.Message);
                        }                                                
                    }
                    else
                    {   // song continues
                        timeCounter += 5; // timerScrobble.Interval / 1000;
                        if (!scrobbled && timeCounter > SCROBBLE_INTERVAL) // Song is scrobbling after 2 minutes of playing
                        {
                            string[] trackInfo = curtrack.Split('-');
                            connection.Scrobble(new Entry(trackInfo[0].Trim(), trackInfo[1].Trim()));
                            scrobbled = true;
                            Text += " [scrobbled]";
                            notifyIcon.Text = (Text.Length > 63 ? Text.Substring(0, 62) : Text); // notifyIcon.Text is limited by 64 chars
                        }
                    }
                }
                else
                {
                    timeCounter = 0;
                    Text = "Player is not running";
                    notifyIcon.Text = Text;
                }
            }
            else
            {
                timeCounter = 0;
                Text = "Player is not running";
                notifyIcon.Text = Text;
            }                   
        }
         /*  </ Last.Fm functions>  */
        
        /*                     <Tray Icon>                                       */
        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon.Visible = true;
                Hide();
                ShowInTaskbar = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void itemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void itemShow_Click(object sender, EventArgs e)
        {
            notifyIcon_MouseDoubleClick(this, null);
        }

        private void cbStartMin_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = cbStartMin.Checked;
            Properties.Settings.Default.Save();
        }       
        /*                     </Tray Icon>                                       */
    }
}
