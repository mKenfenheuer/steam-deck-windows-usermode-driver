using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SWICD_Lib;
using SWICD_Lib.Config;

namespace SWICD_Configurator
{
    public partial class MainWindow : Form
    {
        Configuration Configuration = new Configuration();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (File.Exists("app_config.conf")) ;
            Configuration = ConfigLoader.GetConfiguration("app_config.conf");
            foreach (string executable in Configuration.BlacklistedProcesses)
            {
                lbBlacklistedProcesses.Items.Add(executable);
            }
            foreach (string executable in Configuration.WhitelistedProcesses)
            {
                lbWhitelistedProcesses.Items.Add(executable);
            }
            cbOperationMode.SelectedIndex = (int)Configuration.OperationMode;
        }

        private void btnAddBlacklistedProcess_Click(object sender, EventArgs e)
        {
            ofdSelectExecutable.FileName = null;
            if(ofdSelectExecutable.ShowDialog() == DialogResult.OK)
            {
                string filename = Path.GetFileName(ofdSelectExecutable.FileName);
                if (!lbBlacklistedProcesses.Items.Contains(filename))
                {
                    lbBlacklistedProcesses.Items.Add(filename);
                    Configuration.BlacklistedProcesses.Add(filename);
                } else
                {
                    MessageBox.Show("Cannot add same executable twice.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigLoader.SaveConfiguration(Configuration, "app_config.conf");
        }

        private void cbOperationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Configuration.OperationMode = (OperationMode)Enum.Parse(typeof(OperationMode), cbOperationMode.SelectedItem.ToString());
        }

        private void btnAddWhitelistedProcess_Click(object sender, EventArgs e)
        {
            ofdSelectExecutable.FileName = null;
            if (ofdSelectExecutable.ShowDialog() == DialogResult.OK)
            {
                string filename = Path.GetFileName(ofdSelectExecutable.FileName);
                if (!lbWhitelistedProcesses.Items.Contains(filename))
                {
                    lbWhitelistedProcesses.Items.Add(filename);
                    Configuration.WhitelistedProcesses.Add(filename);
                }
                else
                {
                    MessageBox.Show("Cannot add same executable twice.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRemoveWhitelistedProcess_Click(object sender, EventArgs e)
        {
            if (lbWhitelistedProcesses.SelectedItem == null)
                return;
            string selection = lbWhitelistedProcesses.SelectedItem.ToString();
            lbWhitelistedProcesses.Items.Remove(selection);
            Configuration.WhitelistedProcesses.Remove(selection);
        }

        private void btnRemoveBlacklistedProcess_Click(object sender, EventArgs e)
        {
            if (lbBlacklistedProcesses.SelectedItem == null)
                return;
            string selection = lbBlacklistedProcesses.SelectedItem.ToString();
            lbBlacklistedProcesses.Items.Remove(selection);
            Configuration.BlacklistedProcesses.Remove(selection);
        }
    }
}
