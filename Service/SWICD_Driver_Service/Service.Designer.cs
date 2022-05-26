namespace SWICD_Driver_Service
{
    partial class Service
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CheckDriverStatusTimer = new System.Windows.Forms.Timer(this.components);
            // 
            // CheckDriverStatusTimer
            // 
            this.CheckDriverStatusTimer.Tick += new System.EventHandler(this.CheckDriverStatusTimer_Tick);
            // 
            // Service1
            // 
            this.ServiceName = "Service1";

        }

        #endregion

        private System.Windows.Forms.Timer CheckDriverStatusTimer;
    }
}
