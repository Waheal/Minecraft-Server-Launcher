using MSL.controls;
using MSL.pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace MSL
{
    public delegate void DeleControl();
    /// <summary>
    /// xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string update = "v3.3.6";
        //public static string notice = "";
        //public static string mslConfig = string.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"False\",\n\"notice\": \"0\",\n\"frpcversion\": \"2\",\n\"skin\": \"default\"\n");
        public static string mslConfig = string.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"False\",\n\"notice\": \"0\",\n\"sidemenu\": \"0\",\n\"autoOpenServer\": \"False\",\n\"autoOpenFrpc\": \"False\",\n\"frpcversion\": \"2\"\n");
        pages.Home _homePage = new pages.Home();
        pages.ServerList _listPage = new pages.ServerList();
        pages.SettingsPage _setPage = new pages.SettingsPage();
        //pages.DownloadServer _dserverPage = new pages.DownloadServer();
        pages.FrpcPage _frpcPage = new pages.FrpcPage();
        pages.About _aboutPage = new pages.About();
        public static int ControlsColor = 0;
        public static event DeleControl SetControlsColor;
        public static event DeleControl AutoOpenServer;
        public static event DeleControl AutoOpenFrpc;
        public static string serverid;
        public static string servername;
        public static string serverjava;
        public static string serverserver;
        public static string serverJVM;
        public static string serverJVMcmd;
        public static string serverbase;
        public static string frpc;
        public static string serverLink;
        //bool autogoset = false;
        public static double PhisicalMemory;
        public static bool notifyIcon;
        //public static event DeleControl ServerReadOutputStd;
        public static bool ServerReadOutputStd = false;
        //public static Process SERVERCMD = new Process();
        //public event DelReadStdOutput ReadStdOutput;
        public void NormalColor()
        {
            //Brush aaa = new SolidColorBrush(Color.FromRgb(248, 241, 241));//red
            ControlsColor = 0;
            SetControlsColor();
            Brush brush = new SolidColorBrush(Color.FromRgb(238, 244, 249));
            SideMenu.Background = brush;
            this.Style = Resources["NormalColorStyle"] as Style;
        }
        public void BlackWhiteColor()
        {
            //Brush aaa = new SolidColorBrush(Color.FromRgb(248, 241, 241));//red
            ControlsColor = 1;
            SetControlsColor();
            Brush brush = new SolidColorBrush(Color.FromRgb(234, 234, 234));
            SideMenu.Background = brush;
            this.Style = Resources["BlackWhiteColorStyle"] as Style;
        }

        public MainWindow()
        {
            InitializeComponent();
            frame.Content = _homePage;
            Home.FramePageControl += Func;
            Home.SetNormalColor += NormalColor;
            Home.SetBlackWhiteColor += BlackWhiteColor;
            SettingsPage.C_NotifyIcon += Func1;

        }
        [DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //get serverlink
            MainWindow.serverLink = "";
            //welcomelabel.Content = "欢迎使用我的世界开服器！版本：" + update;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"MSL");
            }
            //MessageBox.Show("wqqq");
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", MainWindow.mslConfig);
                Process.Start(Application.ResourceAssembly.Location);
                Process.GetCurrentProcess().Kill();
            }
            //检测是否配置了内网映射
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["frpc"] == null)
                {
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", MainWindow.mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                MainWindow.frpc = jsonObject["frpc"].ToString();
                reader.Close();
                if (MainWindow.frpc == "")
                {
                    MainWindow.frpc = null;
                }
            }
            catch
            {
                MainWindow.frpc = null;
            }
            //托盘图标检测
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["notifyIcon"] == null)
                {
                    reader.Close();
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                if (jsonObject["notifyIcon"].ToString() == "True")
                {
                    notifyIcon = true;
                }
                else
                {
                    notifyIcon = false;
                    //setnotifyIcon.Content = "开启托盘图标";
                }
                reader.Close();
            }
            catch
            {
                notifyIcon = false;
            }
            if (notifyIcon == true)
            {
                NotifyForm fw = new NotifyForm();
                fw.Show();
                fw.NotifyFormShowEvent += NotifyFormShow;
            }
            //侧边栏检测
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["sidemenu"] == null)
                {
                    reader.Close();
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                if (jsonObject["sidemenu"].ToString() == "0")
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        sideMenuContextOpen.Width = 100;
                        SideMenu.Width = 100;
                        frame.Margin = new Thickness(100, 0, 0, 0);
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        sideMenuContextOpen.Width = 50;
                        SideMenu.Width = 50;
                        frame.Margin = new Thickness(50, 0, 0, 0);
                    });
                }
                reader.Close();
            }
            catch
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    sideMenuContextOpen.Width = 100;
                    SideMenu.Width = 100;
                    frame.Margin = new Thickness(100, 0, 0, 0);
                });
            }
            //更新
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/update.txt");
                string pageHtml = Encoding.UTF8.GetString(pageData);
                string strtempa = "#";
                int IndexofA = pageHtml.IndexOf(strtempa);
                string Ru = pageHtml.Substring(IndexofA + 1);
                string aaa = Ru.Substring(0, Ru.IndexOf("#"));
                if (aaa != MainWindow.update)
                {
                    byte[] _updatelog = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/updatelog.txt");
                    string updatelog = Encoding.UTF8.GetString(_updatelog);
                    MessageDialogShow.Show("发现新版本，版本号为：" + aaa + "，是否进行更新？\n更新日志：\n" + updatelog, "更新", true, "确定", "取消");
                    MessageDialog messageDialog = new MessageDialog();
                    messageDialog.Owner = this;
                    messageDialog.ShowDialog();
                    if (MessageDialog._dialogReturn == true)
                    {
                        MessageDialog._dialogReturn = false;
                        string strtempa1 = "* ";
                        int IndexofA1 = pageHtml.IndexOf(strtempa1);
                        string Ru1 = pageHtml.Substring(IndexofA1 + 2);
                        string aaa1 = Ru1.Substring(0, Ru1.IndexOf(" *"));
                        DownloadWindow.downloadurl = aaa1;
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory;
                        DownloadWindow.filename = "MSL" + aaa + ".exe";
                        DownloadWindow.downloadinfo = "下载新版本中……";
                        Window window = new DownloadWindow();
                        window.Owner = this;
                        window.ShowDialog();
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MSL" + aaa + ".exe"))
                        {
                            string vBatFile = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\DEL.bat";
                            using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
                            {
                                vStreamWriter.Write(string.Format(":del\r\n del \"" + System.Windows.Forms.Application.ExecutablePath + "\"\r\n " + "if exist \"" + System.Windows.Forms.Application.ExecutablePath + "\" goto del\r\n " + "start /d \"" + AppDomain.CurrentDomain.BaseDirectory + "\" MSL" + aaa + ".exe" + "\r\n" + " del %0\r\n", AppDomain.CurrentDomain.BaseDirectory));
                            }
                            WinExec(vBatFile, 0);
                            Process.GetCurrentProcess().Kill();
                        }
                        else
                        {
                            MessageBox.Show("更新失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("您取消了下载新版本，新版本可能修复了此版本中存在的bug，若您不更新新版本，在此版本中遇到的问题请不要上报作者或在Q群里询问！", "警告⚠", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch
            {
            }
            //检测是否配置过服务器
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL\ServerList.ini"))
            {
                /*
                Window wn = new CreateServer();
                wn.Owner = this;
                wn.Show();*/
                try
                {
                    WebClient MyWebClient = new WebClient();
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageData = MyWebClient.DownloadData(serverLink + @"/web/help.txt");
                    string notice = Encoding.UTF8.GetString(pageData);
                    Process.Start(notice);
                }
                catch { }
            }
            else
            {
                //GetServerConfig();
            }
            //获取电脑内存
            PhisicalMemory = GetPhisicalMemory();
            //自动开启服务器
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["autoOpenServer"] == null)
                {
                    reader.Close();
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                if (jsonObject["autoOpenServer"].ToString() != "False")
                {
                    string servers = jsonObject["autoOpenServer"].ToString();
                    while (servers != "")
                    {
                        int aserver = servers.IndexOf(",");
                        serverid = servers.Substring(0, aserver);
                        AutoOpenServer();
                        servers = servers.Replace(serverid + ",", "");
                    }
                    
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Err" + ex.Message);
            }
            //自动开启Frpc
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["autoOpenFrpc"] == null)
                {
                    reader.Close();
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                if (jsonObject["autoOpenFrpc"].ToString() != "False")
                {
                    AutoOpenFrpc();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err" + ex.Message);
            }
        }
        private void NotifyFormShow()
        {
            this.Visibility = Visibility.Visible;
        }
        private static long GetPhisicalMemory()
        {
            long amemory = 0;
            //获得物理内存 
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    amemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
            return amemory;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (notifyIcon == true)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                try
                {
                    if (ServerList.RunningServerIDs != "" || FrpcPage.FRPCMD.HasExited == false)
                    {
                        MessageDialogShow.Show("您的服务器或内网映射正在运行中，关闭软件可能会让服务器进程在后台一直运行并占用资源！确定要继续关闭吗？\n注：如果想隐藏主窗口的话，请前往设置打开托盘图标", "警告", true, "确定", "取消");
                        MessageDialog messageDialog = new MessageDialog();
                        messageDialog.Owner = this;
                        messageDialog.ShowDialog();
                        if (MessageDialog._dialogReturn == true)
                        {
                            MessageDialog._dialogReturn = false;
                            Process.GetCurrentProcess().Kill();
                        }
                        else
                        {
                            e.Cancel = true;
                        }

                    }
                    else
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
                catch
                {
                    try
                    {
                        if (FrpcPage.FRPCMD.HasExited == false)
                        {
                            MessageDialogShow.Show("内网映射正在运行中，关闭软件可能会让内网映射进程在后台一直运行并占用资源！确定要继续关闭吗？\n如果想隐藏主窗口的话，请前往设置打开托盘图标", "警告", true, "确定", "取消");
                            MessageDialog messageDialog = new MessageDialog();
                            messageDialog.Owner = this;
                            messageDialog.ShowDialog();
                            if (MessageDialog._dialogReturn == true)
                            {
                                MessageDialog._dialogReturn = false;
                                Process.GetCurrentProcess().Kill();
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                    catch
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
                //Process.GetCurrentProcess().Kill();
            }
        }
        void Func()//FramePageControl
        {
            serverlistPage.IsSelected = true;
            homePage.IsSelected = false;
            frame.Content = _listPage;
        }
        /*
        void Func1()//SetServerConfig
        {
            settingsPage.IsSelected = true;
            frame.Content = _setPage;
        }
        void Func2()//SetServerComplete
        {
            listPage.IsSelected = true;
            frame.Content = _listPage;
        }*/
        void Func1()//C_NotifyIcon
        {
            NotifyForm fw = new NotifyForm();
            fw.Show();
            fw.NotifyFormShowEvent += NotifyFormShow;
        }

        /*
        void Func4()//DownServerCtrl
        {
            dserverPage.IsSelected = true;
            frame.Content = _dserverPage;
            autogoset = true;
        }*/

        ////////////////////////////////这里应该也有用
        /*
        void Func5()//DownloadComplete
        {
            if (autogoset == true)
            {
                settingsPage.IsSelected = true;
                frame.Content = _setPage;
                autogoset = false;
            }
        }
        */
        /*
        void Func2()//SetPorM
        {
            managePage.IsSelected = true;
            serverlistPage.IsSelected=false;
            frame.Content = _pluginsOrmodsPage;
        }*/

        private void homePage_Selected(object sender, RoutedEventArgs e)
        {
            frame.Content = _homePage;
            if (serverlistPage.IsSelected == true)
            {
                serverlistPage.IsSelected = false;
            }
        }

        private void serverlistPage_Selected(object sender, RoutedEventArgs e)
        {
            frame.Content = _listPage;
        }

        private void frpPage_Selected(object sender, RoutedEventArgs e)
        {
            frame.Content = _frpcPage;
            if (serverlistPage.IsSelected == true)
            {
                serverlistPage.IsSelected = false;
            }
        }
        /*
        private void managePage_Selected(object sender, RoutedEventArgs e)
        {
            frame.Content = _pluginsOrmodsPage;
        }*/

        private void settingsPage_Selected(object sender, RoutedEventArgs e)
        {
            frame.Content = _setPage;
            if (serverlistPage.IsSelected == true)
            {
                serverlistPage.IsSelected = false;
            }
        }

        private void aboutPage_Selected(object sender, RoutedEventArgs e)
        {
            frame.Content = _aboutPage;
            if (serverlistPage.IsSelected == true)
            {
                serverlistPage.IsSelected = false;
            }
        }

        private void sideMenuContextOpen_Click(object sender, RoutedEventArgs e)
        {
            if (sideMenuContextOpen.Width == 50)
            {
                sideMenuContextOpen.Width = 100;
                SideMenu.Width = 100;
                frame.Margin = new Thickness(100, 0, 0, 0);
                string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                JObject jobject = JObject.Parse(jsonString);
                jobject["sidemenu"] = "0";
                string convertString = Convert.ToString(jobject);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
            }
            else
            {
                sideMenuContextOpen.Width = 50;
                SideMenu.Width = 50;
                frame.Margin = new Thickness(50, 0, 0, 0);
                string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                JObject jobject = JObject.Parse(jsonString);
                jobject["sidemenu"] = "1";
                string convertString = Convert.ToString(jobject);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
            }
        }
    }
}
