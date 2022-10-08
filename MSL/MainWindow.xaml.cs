using Microsoft.Win32;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MSL
{
    public delegate void DeleControl();
    /// <summary>
    /// xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string update = "v2.5.6";
        //public static string notice = "";
        //public static string mslConfig = string.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"False\",\n\"notice\": \"0\",\n\"frpcversion\": \"2\",\n\"skin\": \"default\"\n");
        public static string mslConfig = string.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"False\",\n\"notice\": \"0\",\n\"frpcversion\": \"3\"\n");
        pages.Home _homePage = new pages.Home();
        pages.Cmdoutlog _cmdPage = new pages.Cmdoutlog();
        pages.SettingsPage _setPage = new pages.SettingsPage();
        pages.DownloadServer _dserverPage = new pages.DownloadServer();
        pages.FrpcPage _frpcPage = new pages.FrpcPage();
        pages.PluginsgsOrMods _pluginsOrmodsPage = new pages.PluginsgsOrMods();
        pages.About _aboutPage = new pages.About();
        public static string serverjava = "Java";
        public static string serverserver = "";
        public static string serverJVM;
        public static string serverJVMcmd = "";
        public static string serverbase;
        public static string frpc;
        public static string serverLink;
        bool autogoset = false;
        public static double PhisicalMemory;
        public static bool notifyIcon;
        //public static event DeleControl ServerReadOutputStd;
        public static bool ServerReadOutputStd = false;
        DispatcherTimer timer1 = new DispatcherTimer();
        //public static Process SERVERCMD = new Process();
        //public event DelReadStdOutput ReadStdOutput;

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            //pages.Home home = new pages.Home();
            frame.Content = _homePage;
            pages.Home.FramePageControl += Func;
            pages.Cmdoutlog.StartServerControl += Func1;
            pages.Cmdoutlog.StopServerControl += Func2;
            pages.SettingsPage.C_NotifyIcon += Func3;
            pages.SettingsPage.DownServerCtrl += Func4;
            pages.DownloadServer.DownComplete += Func5;
            //pages.Cmdoutlog.FramePageControl += Func;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
            //Thread.Sleep(1000);
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Start();
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (LodingForm.isClose != true)
            {
                return;
            }
            timer1.Stop();
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
            /*
            //皮肤（有bug，无法做到多个page皮肤相同）
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                reader.Close();
                MessageBox.Show("aaa");
                if (jsonObject["skin"] == null)
                {
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                if (jsonObject["skin"].ToString() == "default")
                {
                    MessageBox.Show("aaa");
                    Brush aaa = new SolidColorBrush(Color.FromRgb(241, 243, 248));//blue
                    titleGrid.Background = aaa;
                    mainBorder.Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "red")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(248, 241, 241));//red
                    titleGrid.Background = aaa;
                    mainBorder.Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "yellow")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(248, 248, 241));//yellow
                    titleGrid.Background = aaa;
                    mainBorder.Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "green")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(241, 248, 244));//green
                    titleGrid.Background = aaa;
                    mainBorder.Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "pink")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(247, 241, 248));//pink
                    titleGrid.Background = aaa;
                    mainBorder.Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "purple")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(237, 231, 245));//purple
                    titleGrid.Background = aaa;
                    mainBorder.Background = aaa;
                }
            }
            catch
            {
                Brush aaa = new SolidColorBrush(Color.FromRgb(241, 243, 248));//blue
                titleGrid.Background = aaa;
                mainBorder.Background = aaa;
            }
            */
            //公告
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                //notice
                byte[] pageData = MyWebClient.DownloadData(serverLink + @"/web/notice.txt");
                string notice = Encoding.UTF8.GetString(pageData);
                //version
                byte[] pageData1 = MyWebClient.DownloadData(serverLink + @"/web/noticeversion.txt");
                string nv1 = Encoding.UTF8.GetString(pageData1);
                string noticeversion = nv1.Substring(0, 1);
                //RefreshLink();
                //notice = Ru2.Substring(0, Ru2.IndexOf("#"));
                try
                {
                    //noticeLab.Text = "公告：\n" + notice;
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    if (jsonObject["notice"] == null)
                    {
                        MessageBox.Show("配置文件错误，即将修复");
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", mslConfig);
                        Process.Start(Application.ResourceAssembly.Location);
                        Process.GetCurrentProcess().Kill();
                    }
                    string noticeversion1 = jsonObject["notice"].ToString();
                    //MessageBox.Show(noticeversion1);
                    reader.Close();
                    if (noticeversion1 != noticeversion)
                    {
                        //MessageBox.Show(notice, "Notice");
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "公告",
                            CloseButtonText = "确定",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            Content = notice,
                        };
                        dialog.ShowAsync();
                        try
                        {
                            //StreamReader reader = File.OpenText(Application.StartupPath+@"\server\MSL.json", System.Text.Encoding.UTF8);
                            string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                            JObject jobject = JObject.Parse(jsonString);
                            jobject["notice"] = noticeversion.ToString();
                            string convertString = Convert.ToString(jobject);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
                        }
                        catch (Exception a)
                        {
                            //MessageBox.Show(a.Message, "ERROR");
                            ContentDialog dialog1 = new ContentDialog()
                            {
                                Title = "ERROR",
                                CloseButtonText = "确定",
                                IsPrimaryButtonEnabled = false,
                                DefaultButton = ContentDialogButton.Primary,
                                Content = a.Message,
                            };
                            dialog1.ShowAsync();
                        }
                    }
                    notice = null;
                }
                catch
                { notice = null; }
            }
            catch
            { }
            this.Topmost = true;
            this.Topmost = false;
            this.ShowInTaskbar = true;
            var story = (Storyboard)this.Resources["LoadWindow"];
            if (story != null)
            {
                story.Begin(this);
            }
            this.WindowState = WindowState.Normal;
            //检测是否配置过服务器
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL\server.ini"))
            {
                Window wn = new CreateServer();
                wn.ShowDialog();
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
                GetServerConfig();
            }
            //获取电脑内存
            PhisicalMemory = GetPhisicalMemory();
            //垃圾回收
            Thread thread = new Thread(WasteRecyle);
            thread.Start();
            //IntPtr pHandle = GetCurrentProcess();
            //SetProcessWorkingSetSize(pHandle, -1, -1);
            //GC.Collect();
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();
        void WasteRecyle()
        {
            for(int i=595;i!=0;i++)
            {
                Thread.Sleep(1000);
                if (i == 600)
                {
                    try
                    {
                        if (pages.Cmdoutlog.SERVERCMD.HasExited == true)
                        {
                            Process proc = Process.GetCurrentProcess();
                            long usedMemory = proc.PrivateMemorySize64;
                            if (usedMemory > 1024 * 1024 * 50)
                            {
                                IntPtr pHandle = GetCurrentProcess();
                                SetProcessWorkingSetSize(pHandle, -1, -1);
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                                {
                                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                                }
                            }
                        }
                        else
                        {
                            Process proc = Process.GetCurrentProcess();
                            long usedMemory = proc.PrivateMemorySize64;

                            if (usedMemory > 1024 * 1024 * 1024)
                            {
                                IntPtr pHandle = GetCurrentProcess();
                                SetProcessWorkingSetSize(pHandle, -1, -1);
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                                {
                                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                                }
                            }
                        }
                    }
                    catch
                    {
                        Process proc = Process.GetCurrentProcess();
                        long usedMemory = proc.PrivateMemorySize64;
                        if (usedMemory > 1024 * 1024 * 50)
                        {
                            IntPtr pHandle = GetCurrentProcess();
                            SetProcessWorkingSetSize(pHandle, -1, -1);
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            {
                                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                            }
                        }
                    }
                    i = 1;
                }
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
        public async void GetServerConfig()
        {
            try
            {
                string line = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\server.ini");

                int IndexofA2 = line.IndexOf("-j ");
                string Ru2 = line.Substring(IndexofA2 + 3);
                string a200 = Ru2.Substring(0, Ru2.IndexOf("|"));
                //serverjavalist.Items.Add(a200);
                serverjava = a200;

                int IndexofA3 = line.IndexOf("-s ");
                string Ru3 = line.Substring(IndexofA3 + 3);
                string a300 = Ru3.Substring(0, Ru3.IndexOf("|"));
                //serverserverlist.Items.Add(a300);
                serverserver = a300;

                int IndexofA4 = line.IndexOf("-a ");
                string Ru4 = line.Substring(IndexofA4 + 3);
                string a400 = Ru4.Substring(0, Ru4.IndexOf("|"));
                //serverJVMlist.Items.Add(a400);
                serverJVM = a400;

                int IndexofA5 = line.IndexOf("-b ");
                string Ru5 = line.Substring(IndexofA5 + 3);
                string a500 = Ru5.Substring(0, Ru5.IndexOf("|"));
                //serverbaselist.Items.Add(a500);
                serverbase = a500;

                int IndexofA6 = line.IndexOf("-c ");
                string Ru6 = line.Substring(IndexofA6 + 3);
                string a600 = Ru6.Substring(0, Ru6.IndexOf("|"));
                //serverbaselist.Items.Add(a500);
                serverJVMcmd = a600;
            }
            catch
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "ERROR",
                    PrimaryButtonText = "确定",
                    CloseButtonText = "取消",
                    IsPrimaryButtonEnabled = true,
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "开服器检测到您可能没有创建服务器或配置文件出现了错误，\n是否重新配置服务器？",
                };
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Window wn = new CreateServer();
                    wn.ShowDialog();
                }
            }
        }
        private void closewindow_Click(object sender, RoutedEventArgs e)
        {
            if (notifyIcon == true)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                try
                {
                    if (pages.Cmdoutlog.SERVERCMD.HasExited == false || pages.FrpcPage.FRPCMD.HasExited == false)
                    {
                        //System.Windows.Forms.MessageBox.Show("您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "警告⚠",
                            CloseButtonText = "确定",
                            IsPrimaryButtonEnabled = true,
                            DefaultButton = ContentDialogButton.Primary,
                            Content = "您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！",
                        };
                        dialog.ShowAsync();
                    }
                    else
                    {
                        var story = (Storyboard)this.Resources["CloseWindow"];
                        if (story != null)
                        {
                            story.Completed += delegate { Close(); };
                            story.Begin(this);
                        }
                        //Process.GetCurrentProcess().Kill();
                    }
                }
                catch
                {
                    try
                    {
                        if (pages.FrpcPage.FRPCMD.HasExited == false)
                        {
                            //System.Windows.Forms.MessageBox.Show("内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            ContentDialog dialog = new ContentDialog()
                            {
                                Title = "警告⚠",
                                CloseButtonText = "确定",
                                IsPrimaryButtonEnabled = true,
                                DefaultButton = ContentDialogButton.Primary,
                                Content = "内网映射正在运行中，请确保完全关闭后再关闭软件！",
                            };
                            dialog.ShowAsync();
                        }
                        else
                        {
                            var story = (Storyboard)this.Resources["CloseWindow"];
                            if (story != null)
                            {
                                story.Completed += delegate { Close(); };
                                story.Begin(this);
                            }
                            //Process.GetCurrentProcess().Kill();
                        }
                    }
                    catch
                    {
                        var story = (Storyboard)this.Resources["CloseWindow"];
                        if (story != null)
                        {
                            story.Completed += delegate { Close(); };
                            story.Begin(this);
                        }
                        //Process.GetCurrentProcess().Kill();
                    }
                }
            }
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
                    if (pages.Cmdoutlog.SERVERCMD.HasExited == false || pages.FrpcPage.FRPCMD.HasExited == false)
                    {
                        //System.Windows.Forms.MessageBox.Show("您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "警告⚠",
                            CloseButtonText = "确定",
                            IsPrimaryButtonEnabled = true,
                            DefaultButton = ContentDialogButton.Primary,
                            Content = "您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！",
                        };
                        dialog.ShowAsync();
                        e.Cancel = true;
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
                        if (pages.FrpcPage.FRPCMD.HasExited == false)
                        {
                            //System.Windows.Forms.MessageBox.Show("内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            ContentDialog dialog = new ContentDialog()
                            {
                                Title = "警告⚠",
                                CloseButtonText = "确定",
                                IsPrimaryButtonEnabled = true,
                                DefaultButton = ContentDialogButton.Primary,
                                Content = "内网映射正在运行中，请确保完全关闭后再关闭软件！",
                            };
                            dialog.ShowAsync();
                            e.Cancel = true;
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
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                var story1 = (Storyboard)this.Resources["ShowWindow"];
                if (story1 != null)
                {
                    story1.Begin(this);
                }
            }
        }
        /*
        private async void tabcontrol1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabcontrol1.SelectedIndex == 0)
            {
                timer2.Stop();
            }
            if (tabcontrol1.SelectedIndex == 1)
            {
                timer2.Stop();
            }
            if (tabcontrol1.SelectedIndex == 2)
            {
                
            }
            if (tabcontrol1.SelectedIndex == 3)
            {
                timer2.Stop();
                
            }
            if (tabcontrol1.SelectedIndex == 4)
            {
                timer2.Stop();
                
            }
        }
        
        */
        void Func()//FramePageControl
        {
            outlogPage.IsSelected = true;
            frame.Content = _cmdPage;
        }
        void Func1()//StartServerControl
        {
            settingsPage.IsEnabled = false;
        }
        void Func2()//StopServerControl
        {
            settingsPage.IsEnabled = true;
        }
        void Func3()//C_NotifyIcon
        {
            NotifyForm fw = new NotifyForm();
            fw.Show();
            fw.NotifyFormShowEvent += NotifyFormShow;
        }
        void Func4()//DownServerCtrl
        {
            dserverPage.IsSelected = true;
            frame.Content = _dserverPage;
            autogoset = true;
        }
        void Func5()//DownloadComplete
        {
            if (autogoset == true)
            {
                settingsPage.IsSelected = true;
                frame.Content = _setPage;
                autogoset = false;
            }
        }
        private void homePage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _homePage;
        }

        private void outlogPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            frame.Content = _cmdPage;
        }

        private void settingsPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _setPage;
        }

        private void dserverPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _dserverPage;
        }
        private void frpcPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _frpcPage;
        }
        private void pluginsOrmodsPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _pluginsOrmodsPage;
        }

        private void aboutPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _aboutPage;
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void titleGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void maxWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void minWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
