namespace SWICD_Configurator
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRemoveWhitelistedProcess = new System.Windows.Forms.Button();
            this.btnAddWhitelistedProcess = new System.Windows.Forms.Button();
            this.btnRemoveBlacklistedProcess = new System.Windows.Forms.Button();
            this.btnAddBlacklistedProcess = new System.Windows.Forms.Button();
            this.lblWhitelistedProcesses = new System.Windows.Forms.Label();
            this.lbWhitelistedProcesses = new System.Windows.Forms.ListBox();
            this.lblBlacklistedProcesses = new System.Windows.Forms.Label();
            this.lbBlacklistedProcesses = new System.Windows.Forms.ListBox();
            this.btnServiceControl = new System.Windows.Forms.Button();
            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.lblOperationMode = new System.Windows.Forms.Label();
            this.cbOperationMode = new System.Windows.Forms.ComboBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.ofdSelectExecutable = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemoveWhitelistedProcess);
            this.groupBox1.Controls.Add(this.btnAddWhitelistedProcess);
            this.groupBox1.Controls.Add(this.btnRemoveBlacklistedProcess);
            this.groupBox1.Controls.Add(this.btnAddBlacklistedProcess);
            this.groupBox1.Controls.Add(this.lblWhitelistedProcesses);
            this.groupBox1.Controls.Add(this.lbWhitelistedProcesses);
            this.groupBox1.Controls.Add(this.lblBlacklistedProcesses);
            this.groupBox1.Controls.Add(this.lbBlacklistedProcesses);
            this.groupBox1.Controls.Add(this.btnServiceControl);
            this.groupBox1.Controls.Add(this.lblServiceStatus);
            this.groupBox1.Controls.Add(this.lblOperationMode);
            this.groupBox1.Controls.Add(this.cbOperationMode);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(960, 214);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // btnRemoveWhitelistedProcess
            // 
            this.btnRemoveWhitelistedProcess.Location = new System.Drawing.Point(761, 185);
            this.btnRemoveWhitelistedProcess.Name = "btnRemoveWhitelistedProcess";
            this.btnRemoveWhitelistedProcess.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveWhitelistedProcess.TabIndex = 12;
            this.btnRemoveWhitelistedProcess.Text = "Remove";
            this.btnRemoveWhitelistedProcess.UseVisualStyleBackColor = true;
            this.btnRemoveWhitelistedProcess.Click += new System.EventHandler(this.btnRemoveWhitelistedProcess_Click);
            // 
            // btnAddWhitelistedProcess
            // 
            this.btnAddWhitelistedProcess.Location = new System.Drawing.Point(680, 185);
            this.btnAddWhitelistedProcess.Name = "btnAddWhitelistedProcess";
            this.btnAddWhitelistedProcess.Size = new System.Drawing.Size(75, 23);
            this.btnAddWhitelistedProcess.TabIndex = 11;
            this.btnAddWhitelistedProcess.Text = "Add";
            this.btnAddWhitelistedProcess.UseVisualStyleBackColor = true;
            this.btnAddWhitelistedProcess.Click += new System.EventHandler(this.btnAddWhitelistedProcess_Click);
            // 
            // btnRemoveBlacklistedProcess
            // 
            this.btnRemoveBlacklistedProcess.Location = new System.Drawing.Point(480, 185);
            this.btnRemoveBlacklistedProcess.Name = "btnRemoveBlacklistedProcess";
            this.btnRemoveBlacklistedProcess.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveBlacklistedProcess.TabIndex = 10;
            this.btnRemoveBlacklistedProcess.Text = "Remove";
            this.btnRemoveBlacklistedProcess.UseVisualStyleBackColor = true;
            this.btnRemoveBlacklistedProcess.Click += new System.EventHandler(this.btnRemoveBlacklistedProcess_Click);
            // 
            // btnAddBlacklistedProcess
            // 
            this.btnAddBlacklistedProcess.Location = new System.Drawing.Point(399, 185);
            this.btnAddBlacklistedProcess.Name = "btnAddBlacklistedProcess";
            this.btnAddBlacklistedProcess.Size = new System.Drawing.Size(75, 23);
            this.btnAddBlacklistedProcess.TabIndex = 9;
            this.btnAddBlacklistedProcess.Text = "Add";
            this.btnAddBlacklistedProcess.UseVisualStyleBackColor = true;
            this.btnAddBlacklistedProcess.Click += new System.EventHandler(this.btnAddBlacklistedProcess_Click);
            // 
            // lblWhitelistedProcesses
            // 
            this.lblWhitelistedProcesses.AutoSize = true;
            this.lblWhitelistedProcesses.Location = new System.Drawing.Point(678, 16);
            this.lblWhitelistedProcesses.Name = "lblWhitelistedProcesses";
            this.lblWhitelistedProcesses.Size = new System.Drawing.Size(111, 13);
            this.lblWhitelistedProcesses.TabIndex = 8;
            this.lblWhitelistedProcesses.Text = "Whitelisted Processes";
            // 
            // lbWhitelistedProcesses
            // 
            this.lbWhitelistedProcesses.FormattingEnabled = true;
            this.lbWhitelistedProcesses.Location = new System.Drawing.Point(681, 32);
            this.lbWhitelistedProcesses.Name = "lbWhitelistedProcesses";
            this.lbWhitelistedProcesses.Size = new System.Drawing.Size(273, 147);
            this.lbWhitelistedProcesses.TabIndex = 7;
            // 
            // lblBlacklistedProcesses
            // 
            this.lblBlacklistedProcesses.AutoSize = true;
            this.lblBlacklistedProcesses.Location = new System.Drawing.Point(396, 16);
            this.lblBlacklistedProcesses.Name = "lblBlacklistedProcesses";
            this.lblBlacklistedProcesses.Size = new System.Drawing.Size(110, 13);
            this.lblBlacklistedProcesses.TabIndex = 6;
            this.lblBlacklistedProcesses.Text = "Blacklisted Processes";
            // 
            // lbBlacklistedProcesses
            // 
            this.lbBlacklistedProcesses.FormattingEnabled = true;
            this.lbBlacklistedProcesses.Location = new System.Drawing.Point(399, 32);
            this.lbBlacklistedProcesses.Name = "lbBlacklistedProcesses";
            this.lbBlacklistedProcesses.Size = new System.Drawing.Size(273, 147);
            this.lbBlacklistedProcesses.TabIndex = 5;
            // 
            // btnServiceControl
            // 
            this.btnServiceControl.BackColor = System.Drawing.Color.Green;
            this.btnServiceControl.ForeColor = System.Drawing.Color.White;
            this.btnServiceControl.Location = new System.Drawing.Point(6, 79);
            this.btnServiceControl.Name = "btnServiceControl";
            this.btnServiceControl.Size = new System.Drawing.Size(121, 35);
            this.btnServiceControl.TabIndex = 4;
            this.btnServiceControl.Text = "Started";
            this.btnServiceControl.UseVisualStyleBackColor = false;
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.AutoSize = true;
            this.lblServiceStatus.Location = new System.Drawing.Point(3, 63);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(76, 13);
            this.lblServiceStatus.TabIndex = 3;
            this.lblServiceStatus.Text = "Service Status";
            // 
            // lblOperationMode
            // 
            this.lblOperationMode.AutoSize = true;
            this.lblOperationMode.Location = new System.Drawing.Point(3, 16);
            this.lblOperationMode.Name = "lblOperationMode";
            this.lblOperationMode.Size = new System.Drawing.Size(83, 13);
            this.lblOperationMode.TabIndex = 1;
            this.lblOperationMode.Text = "Operation Mode";
            // 
            // cbOperationMode
            // 
            this.cbOperationMode.FormattingEnabled = true;
            this.cbOperationMode.Items.AddRange(new object[] {
            "Blacklist",
            "Whitelist"});
            this.cbOperationMode.Location = new System.Drawing.Point(6, 32);
            this.cbOperationMode.Name = "cbOperationMode";
            this.cbOperationMode.Size = new System.Drawing.Size(121, 21);
            this.cbOperationMode.TabIndex = 0;
            this.cbOperationMode.SelectedIndexChanged += new System.EventHandler(this.cbOperationMode_SelectedIndexChanged);
            // 
            // ofdSelectExecutable
            // 
            this.ofdSelectExecutable.Filter = "Programme (*.exe) | *.exe";
            this.ofdSelectExecutable.Title = "Select executable";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainWindow";
            this.Text = "SWICD Configurator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnServiceControl;
        private System.Windows.Forms.Label lblServiceStatus;
        private System.Windows.Forms.Label lblOperationMode;
        private System.Windows.Forms.ComboBox cbOperationMode;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label lblWhitelistedProcesses;
        private System.Windows.Forms.ListBox lbWhitelistedProcesses;
        private System.Windows.Forms.Label lblBlacklistedProcesses;
        private System.Windows.Forms.ListBox lbBlacklistedProcesses;
        private System.Windows.Forms.Button btnAddBlacklistedProcess;
        private System.Windows.Forms.Button btnRemoveWhitelistedProcess;
        private System.Windows.Forms.Button btnAddWhitelistedProcess;
        private System.Windows.Forms.Button btnRemoveBlacklistedProcess;
        private System.Windows.Forms.OpenFileDialog ofdSelectExecutable;
    }
}

