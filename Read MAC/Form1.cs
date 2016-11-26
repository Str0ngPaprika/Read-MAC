using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;

namespace Read_MAC
{
    public partial class frmMain : Form
    {
        List<string> lstPCs = new List<string>();

        public frmMain()
        {
            string strInputFile="//cnsccscfs002/itadmin/Win2k/Remote Agents/Aruba orders/Input.txt";
    
            InitializeComponent();
            lstPCs = Utilities.ReadAllText(strInputFile);
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            int intProgressCount = 0;
            
            btnGo.Enabled = false;

            progressBar.Minimum = 0;
            progressBar.Maximum = lstPCs.Count;


            txtBox.Clear();
            
            foreach (string strPC in lstPCs)
            {
    
                string strProgressCount="";

                intProgressCount += 1;

                if (intProgressCount<10)
                    strProgressCount = "000" + intProgressCount + " ";
                else
                    if (intProgressCount < 100)
                        strProgressCount = "00" + intProgressCount + " ";
                    else
                        if (intProgressCount < 1000)
                            strProgressCount = "0" + intProgressCount + " ";

                txtBox.AppendText(strProgressCount + strPC + " ", Color.Black);

                if (Utilities.isPCOnline(strPC))
                {
                    txtBox.AppendText("Online", Color.Green);
                }
                else
                {
                    txtBox.AppendText("Offline", Color.Red);
                }
               
                txtBox.AppendText(Environment.NewLine);

                progressBar.Value=intProgressCount;
            }

            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText("Complete", Color.Blue);
            btnGo.Enabled = true;
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }

    public static class Utilities
    {
        public static bool isPCOnline(string strPC)
        {
            ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", strPC));
            ManagementClass os = new ManagementClass(scope, new ManagementPath("Win32_OperatingSystem"), null);
            try
            {
                ManagementObjectCollection instances = os.GetInstances();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public static List<string> ReadAllText(string strFile)
        {
            string line;
            List<string> lstData = new List<string>();
          
            using (StreamReader streamReader = new StreamReader(strFile, Encoding.UTF8))
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    lstData.Add(line);
                }
            }
            return lstData;
        }
        
    }
}
