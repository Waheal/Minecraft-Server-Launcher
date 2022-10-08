using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MSL.pages
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Home : Page
    {
        public static event DeleControl StartServerCmd;
        public static event DeleControl FramePageControl;
        public Home()
        {
            InitializeComponent();
            Cmdoutlog.StartServerControl += Func;
            Cmdoutlog.StopServerControl += Func1;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(GetNotice);
            thread.Start();
            welcomelabel.Content = "欢迎使用MSL开服器！版本：" + MainWindow.update;
        }
        void GetNotice()
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                //notice
                byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/notice.txt");
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    noticeLab.Text = Encoding.UTF8.GetString(pageData);
                });
            }
            catch
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    noticeLab.Text = "获取公告失败！请检查网络连接是否正常或联系作者进行解决！";
                });
            }
        }
        private void Func()
        {
            startServer.Content = "关闭服务器";
        }
        void Func1()
        {
            startServer.Content = "开启服务器";
        }
        private void startServer_Click(object sender, RoutedEventArgs e)
        {
            if (startServer.Content.ToString() == "开启服务器")
            {
                FramePageControl();
                //tabcontrol1.SelectedIndex = 1;
                //serversettings.IsEnabled = false;
                //setdefault.IsEnabled = false;
                startServer.Content = "关闭服务器";
                StartServerCmd();
            }
            else
            {
                Thread.Sleep(200);
                //startServer.Content = "正在关服...";
                Cmdoutlog.SERVERCMD.StandardInput.WriteLine("stop");
                FramePageControl();
            }
        }
        private void startServer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (startServer.Content.ToString() == "关闭服务器")
            {
                Cmdoutlog.SERVERCMD.Kill();
                MessageBox.Show("服务器已强制关闭！");
            }
        }
    }
}
