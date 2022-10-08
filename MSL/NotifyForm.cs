using MSL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSL
{
    public partial class NotifyForm : Form
    {
        public NotifyForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.Hide();
        }
        public event Action NotifyFormShowEvent;
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NotifyFormShowEvent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pages.Cmdoutlog.SERVERCMD.HasExited == false || pages.FrpcPage.FRPCMD.HasExited == false)
                {
                    System.Windows.Forms.MessageBox.Show("您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    Close();
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch
            {
                try
                {
                    if (pages.FrpcPage.FRPCMD.HasExited == false)
                    {

                        System.Windows.Forms.MessageBox.Show("内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Close();
                        Process.GetCurrentProcess().Kill();
                    }
                }
                catch
                {
                    Close();
                    Process.GetCurrentProcess().Kill();
                }

            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MainWindow.notifyIcon == false)
            {
                notifyIcon1.Visible = false;
                this.Dispose();
                this.Close();
            }
        }
    }
}
