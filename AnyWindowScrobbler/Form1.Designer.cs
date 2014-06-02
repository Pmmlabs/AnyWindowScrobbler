namespace AnyWindowScrobbler
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.listBox = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.labelHint2 = new System.Windows.Forms.Label();
            this.labelHint = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timerConnection = new System.Windows.Forms.Timer(this.components);
            this.timerScrobble = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.itemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblPlayer = new System.Windows.Forms.Label();
            this.cbStartMin = new System.Windows.Forms.CheckBox();
            this.timerReconnection = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.HorizontalScrollbar = true;
            this.listBox.Location = new System.Drawing.Point(12, 29);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(373, 303);
            this.listBox.TabIndex = 0;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(172, 382);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(12, 356);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(373, 20);
            this.tbName.TabIndex = 2;
            // 
            // labelHint2
            // 
            this.labelHint2.AutoSize = true;
            this.labelHint2.Location = new System.Drawing.Point(9, 340);
            this.labelHint2.Name = "labelHint2";
            this.labelHint2.Size = new System.Drawing.Size(201, 13);
            this.labelHint2.TabIndex = 3;
            this.labelHint2.Text = "Select actual song title and click \"Save\":";
            // 
            // labelHint
            // 
            this.labelHint.AutoSize = true;
            this.labelHint.Location = new System.Drawing.Point(9, 13);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(147, 13);
            this.labelHint.TabIndex = 3;
            this.labelHint.Text = "Start player and select its title:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(9, 417);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(70, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Connecting...";
            // 
            // timerConnection
            // 
            this.timerConnection.Interval = 1000;
            this.timerConnection.Tick += new System.EventHandler(this.timerConnection_Tick);
            // 
            // timerScrobble
            // 
            this.timerScrobble.Interval = 5000;
            this.timerScrobble.Tick += new System.EventHandler(this.timerScrobble_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "AnyWindowScrobbler";
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemExit,
            this.itemShow});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(117, 48);
            // 
            // itemExit
            // 
            this.itemExit.Name = "itemExit";
            this.itemExit.Size = new System.Drawing.Size(116, 22);
            this.itemExit.Text = "Exit";
            this.itemExit.Click += new System.EventHandler(this.itemExit_Click);
            // 
            // itemShow
            // 
            this.itemShow.Name = "itemShow";
            this.itemShow.Size = new System.Drawing.Size(116, 22);
            this.itemShow.Text = "Settings";
            this.itemShow.Click += new System.EventHandler(this.itemShow_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(310, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblPlayer
            // 
            this.lblPlayer.AutoSize = true;
            this.lblPlayer.Location = new System.Drawing.Point(9, 441);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new System.Drawing.Size(0, 13);
            this.lblPlayer.TabIndex = 4;
            // 
            // cbStartMin
            // 
            this.cbStartMin.AutoSize = true;
            this.cbStartMin.Location = new System.Drawing.Point(289, 416);
            this.cbStartMin.Name = "cbStartMin";
            this.cbStartMin.Size = new System.Drawing.Size(96, 17);
            this.cbStartMin.TabIndex = 7;
            this.cbStartMin.Text = "Start minimized";
            this.cbStartMin.UseVisualStyleBackColor = true;
            this.cbStartMin.CheckedChanged += new System.EventHandler(this.cbStartMin_CheckedChanged);
            // 
            // timerReconnection
            // 
            this.timerReconnection.Interval = 5000;
            this.timerReconnection.Tick += new System.EventHandler(this.timerReconnection_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 470);
            this.Controls.Add(this.cbStartMin);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblPlayer);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.labelHint);
            this.Controls.Add(this.labelHint2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.listBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormMain";
            this.Text = "AnyWindowScrobbler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label labelHint2;
        private System.Windows.Forms.Label labelHint;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timerConnection;
        private System.Windows.Forms.Timer timerScrobble;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem itemExit;
        private System.Windows.Forms.ToolStripMenuItem itemShow;
        private System.Windows.Forms.Label lblPlayer;
        private System.Windows.Forms.CheckBox cbStartMin;
        private System.Windows.Forms.Timer timerReconnection;
    }
}

