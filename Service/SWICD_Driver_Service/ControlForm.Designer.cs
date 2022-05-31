namespace SWICD_Driver_Service
{
    partial class ControlForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlForm));
            this.notificationIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lbDriverLog = new System.Windows.Forms.ListBox();
            this.lblDriverLog = new System.Windows.Forms.Label();
            this.timerGuiUpdate = new System.Windows.Forms.Timer(this.components);
            this.gbDriverStatus = new System.Windows.Forms.GroupBox();
            this.lblDriverStatus = new System.Windows.Forms.Label();
            this.gbDriverSettings = new System.Windows.Forms.GroupBox();
            this.cmsNotification = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStartStopDriver = new System.Windows.Forms.ToolStripMenuItem();
            this.gbDriverStatus.SuspendLayout();
            this.cmsNotification.SuspendLayout();
            this.SuspendLayout();
            // 
            // notificationIcon
            // 
            this.notificationIcon.BalloonTipText = "Steam Deck Controller Driver";
            this.notificationIcon.BalloonTipTitle = "SWICD Driver";
            this.notificationIcon.ContextMenuStrip = this.cmsNotification;
            this.notificationIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notificationIcon.Icon")));
            this.notificationIcon.Text = "SWICD Driver";
            this.notificationIcon.Visible = true;
            this.notificationIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // lbDriverLog
            // 
            this.lbDriverLog.FormattingEnabled = true;
            this.lbDriverLog.Location = new System.Drawing.Point(12, 222);
            this.lbDriverLog.Name = "lbDriverLog";
            this.lbDriverLog.Size = new System.Drawing.Size(791, 238);
            this.lbDriverLog.TabIndex = 0;
            // 
            // lblDriverLog
            // 
            this.lblDriverLog.AutoSize = true;
            this.lblDriverLog.Location = new System.Drawing.Point(12, 206);
            this.lblDriverLog.Name = "lblDriverLog";
            this.lblDriverLog.Size = new System.Drawing.Size(56, 13);
            this.lblDriverLog.TabIndex = 1;
            this.lblDriverLog.Text = "Driver Log";
            // 
            // timerGuiUpdate
            // 
            this.timerGuiUpdate.Enabled = true;
            this.timerGuiUpdate.Interval = 1000;
            this.timerGuiUpdate.Tick += new System.EventHandler(this.timerGuiUpdate_Tick);
            // 
            // gbDriverStatus
            // 
            this.gbDriverStatus.Controls.Add(this.lblDriverStatus);
            this.gbDriverStatus.Location = new System.Drawing.Point(12, 12);
            this.gbDriverStatus.Name = "gbDriverStatus";
            this.gbDriverStatus.Size = new System.Drawing.Size(221, 191);
            this.gbDriverStatus.TabIndex = 2;
            this.gbDriverStatus.TabStop = false;
            this.gbDriverStatus.Text = "Driver Status";
            // 
            // lblDriverStatus
            // 
            this.lblDriverStatus.BackColor = System.Drawing.Color.ForestGreen;
            this.lblDriverStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriverStatus.ForeColor = System.Drawing.Color.White;
            this.lblDriverStatus.Location = new System.Drawing.Point(6, 16);
            this.lblDriverStatus.Name = "lblDriverStatus";
            this.lblDriverStatus.Size = new System.Drawing.Size(209, 166);
            this.lblDriverStatus.TabIndex = 1;
            this.lblDriverStatus.Text = "Running";
            this.lblDriverStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDriverStatus.Click += new System.EventHandler(this.label1_Click);
            // 
            // gbDriverSettings
            // 
            this.gbDriverSettings.Location = new System.Drawing.Point(239, 12);
            this.gbDriverSettings.Name = "gbDriverSettings";
            this.gbDriverSettings.Size = new System.Drawing.Size(564, 191);
            this.gbDriverSettings.TabIndex = 2;
            this.gbDriverSettings.TabStop = false;
            this.gbDriverSettings.Text = "Settings";
            // 
            // cmsNotification
            // 
            this.cmsNotification.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExit,
            this.tsmiShow,
            this.tsmiStartStopDriver});
            this.cmsNotification.Name = "cmsNotification";
            this.cmsNotification.Size = new System.Drawing.Size(181, 92);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(180, 22);
            this.tsmiExit.Text = "Exit";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiShow
            // 
            this.tsmiShow.Name = "tsmiShow";
            this.tsmiShow.Size = new System.Drawing.Size(180, 22);
            this.tsmiShow.Text = "Show";
            this.tsmiShow.Click += new System.EventHandler(this.tsmiShow_Click);
            // 
            // tsmiStartStopDriver
            // 
            this.tsmiStartStopDriver.Name = "tsmiStartStopDriver";
            this.tsmiStartStopDriver.Size = new System.Drawing.Size(180, 22);
            this.tsmiStartStopDriver.Text = "Stop Driver";
            this.tsmiStartStopDriver.Click += new System.EventHandler(this.tsmiStartStopDriver_Click);
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 474);
            this.Controls.Add(this.gbDriverSettings);
            this.Controls.Add(this.gbDriverStatus);
            this.Controls.Add(this.lblDriverLog);
            this.Controls.Add(this.lbDriverLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ControlForm";
            this.Text = "SWICD Control Panel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ControlForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ControlForm_FormClosed);
            this.gbDriverStatus.ResumeLayout(false);
            this.cmsNotification.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notificationIcon;
        private System.Windows.Forms.ListBox lbDriverLog;
        private System.Windows.Forms.Label lblDriverLog;
        private System.Windows.Forms.Timer timerGuiUpdate;
        private System.Windows.Forms.GroupBox gbDriverStatus;
        private System.Windows.Forms.Label lblDriverStatus;
        private System.Windows.Forms.GroupBox gbDriverSettings;
        private System.Windows.Forms.ContextMenuStrip cmsNotification;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiShow;
        private System.Windows.Forms.ToolStripMenuItem tsmiStartStopDriver;
    }
}