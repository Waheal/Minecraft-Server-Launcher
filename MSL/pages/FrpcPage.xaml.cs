using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace MSL.pages
{
    /// <summary>
    /// FrpcPage.xaml 的交互逻辑
    /// </summary>
    public partial class FrpcPage : Page
    {
        public delegate void DelReadStdOutput(string result);
        public static Process FRPCMD = new Process();
        public event DelReadStdOutput ReadStdOutput;
        string _dnfrpc;
        public FrpcPage()
        {
            MainWindow.SetControlsColor += ChangeControlsColor;
            MainWindow.AutoOpenFrpc += StartFrpc;
            InitializeComponent();
            ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
        }
        void ChangeControlsColor()
        {
            if (MainWindow.ControlsColor == 0)
            {
                Brush brush = new SolidColorBrush(Color.FromRgb(50, 108, 243));
                startfrpc.Background = brush;
            }
            if (MainWindow.ControlsColor == 1)
            {
                Brush brush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                startfrpc.Background = brush;
            }
        }
        private void StartFrpc()
        {
            try
            {
                startfrpc.Content = "关闭内网映射";
                FRPCMD.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc.exe";
                //CmdProcess.StartInfo.FileName = StartFileName;
                FRPCMD.StartInfo.Arguments = "-c frpc";
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL");
                FRPCMD.StartInfo.CreateNoWindow = true;
                FRPCMD.StartInfo.UseShellExecute = false;
                FRPCMD.StartInfo.RedirectStandardInput = true;
                FRPCMD.StartInfo.RedirectStandardOutput = true;
                FRPCMD.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                FRPCMD.Start();
                FRPCMD.BeginOutputReadLine();
            }
            catch (Exception e)
            {
                MessageBox.Show("出现错误，请检查是否有杀毒软件误杀并重试:" + e.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                //Close();
            }
        }
        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Dispatcher.Invoke(ReadStdOutput, new object[] { e.Data });
            }
        }
        private void ReadStdOutputAction(string msg)
        {
            frpcOutlog.Text = frpcOutlog.Text + msg + "\n";
            if (msg.IndexOf("login") + 1 != 0)
            {
                if (msg.IndexOf("failed") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "内网映射桥接失败！\n";
                    if (msg.IndexOf("invalid meta token") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "QQ号密码填写错误或付费资格已过期，请重新配置或续费！\n";
                    }
                    if (msg.IndexOf("user or meta token can not be empty") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "用户名或密码不能为空！\n";
                    }
                    try
                    {
                        FRPCMD.Kill();
                        FRPCMD.CancelOutputRead();
                        //ReadStdOutput = null;
                        FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                        setfrpc.IsEnabled = true;
                        startfrpc.Content = "启动内网映射";
                    }
                    catch
                    {
                        try
                        {
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                        catch
                        {
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                    }
                }
                if (msg.IndexOf("success") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "登录服务器成功！\n";
                }
                if (msg.IndexOf("match") + 1 != 0)
                {
                    if (msg.IndexOf("token") + 1 != 0)
                    {
                        try
                        {
                            frpcOutlog.Text = frpcOutlog.Text + "重新连接服务器...\n";
                            Thread.Sleep(1000);
                            string frpcserver = MainWindow.frpc.Substring(0, MainWindow.frpc.IndexOf(".")) + "*";
                            int frpcserver1 = MainWindow.frpc.IndexOf(".") + 1;
                            WebClient MyWebClient = new WebClient();
                            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                            byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/CC/frpcserver.txt");
                            string pageHtml1 = Encoding.UTF8.GetString(pageData);

                            int IndexofA = pageHtml1.IndexOf(frpcserver);
                            string Ru = pageHtml1.Substring(IndexofA + frpcserver1);
                            string a111 = Ru.Substring(0, Ru.IndexOf("*"));
                            //MessageBox.Show(a111);

                            WebClient MyWebClient1 = new WebClient();
                            //MyWebClient1.Credentials = CredentialCache.DefaultCredentials;
                            byte[] pageData1 = MyWebClient1.DownloadData(a111);
                            string pageHtml = Encoding.UTF8.GetString(pageData1);
                            //string decode = DecodeBase64(pageHtml);
                            string aaa = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc");
                            int IndexofA2 = aaa.IndexOf("token=");
                            string Ru2 = aaa.Substring(IndexofA2);
                            string a200 = Ru2.Substring(0, Ru2.IndexOf("\n"));
                            aaa = aaa.Replace(a200, "token=" + pageHtml);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc", aaa);
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                            StartFrpc();
                        }
                        catch (Exception aa)
                        {
                            MessageBox.Show("内网映射桥接失败！请查看是否有杀毒软件删除了frpc.exe并重启开服器再试！\n错误代码：" + aa.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            try
                            {
                                FRPCMD.CancelOutputRead();
                                //ReadStdOutput = null;
                                FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }
                            catch
                            {
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }

                        }
                    }
                }
            }
            if (msg.IndexOf("reconnect") + 1 != 0)
            {
                if (msg.IndexOf("error") + 1 != 0)
                {
                    if (msg.IndexOf("token") + 1 != 0)
                    {
                        try
                        {
                            frpcOutlog.Text = frpcOutlog.Text + "重新连接服务器...\n";
                            Thread.Sleep(5000);
                            string frpcserver = MainWindow.frpc.Substring(0, MainWindow.frpc.IndexOf(".")) + "*";
                            int frpcserver1 = MainWindow.frpc.IndexOf(".") + 1;
                            WebClient MyWebClient = new WebClient();
                            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                            byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/CC/frpcserver.txt");
                            string pageHtml1 = Encoding.UTF8.GetString(pageData);

                            int IndexofA = pageHtml1.IndexOf(frpcserver);
                            string Ru = pageHtml1.Substring(IndexofA + frpcserver1);
                            string a111 = Ru.Substring(0, Ru.IndexOf("*"));
                            //MessageBox.Show(a111);

                            WebClient MyWebClient1 = new WebClient();
                            MyWebClient1.Credentials = CredentialCache.DefaultCredentials;
                            byte[] pageData1 = MyWebClient1.DownloadData(a111);
                            string pageHtml = Encoding.UTF8.GetString(pageData1);
                            //string decode = DecodeBase64(pageHtml);
                            string aaa = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc");
                            int IndexofA2 = aaa.IndexOf("token=");
                            string Ru2 = aaa.Substring(IndexofA2);
                            string a200 = Ru2.Substring(0, Ru2.IndexOf("\n"));
                            aaa = aaa.Replace(a200, "token=" + pageHtml);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc", aaa);
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                            StartFrpc();
                        }
                        catch (Exception aa)
                        {
                            MessageBox.Show("内网映射桥接失败！请重试！\n错误代码：" + aa.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            try
                            {
                                FRPCMD.CancelOutputRead();
                                //ReadStdOutput = null;
                                FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }
                            catch
                            {
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }

                        }

                    }
                }
            }
            if (msg.IndexOf("start") + 1 != 0)
            {
                if (msg.IndexOf("success") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "内网映射桥接成功！\n";
                }
                if (msg.IndexOf("error") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "内网映射桥接失败！\n";
                    try
                    {
                        FRPCMD.Kill();
                        FRPCMD.CancelOutputRead();
                        //ReadStdOutput = null;
                        FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                        setfrpc.IsEnabled = true;
                        startfrpc.Content = "启动内网映射";
                    }
                    catch
                    {
                        try
                        {
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                        catch
                        {
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                    }
                    if (msg.IndexOf("port already used") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "远程端口被占用，请重新配置！\n";
                    }
                    if (msg.IndexOf("port not allowed") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "端口被占用，请重新配置！\n";
                    }
                    if (msg.IndexOf("proxy name") + 1 != 0)
                    {
                        if (msg.IndexOf("already in use") + 1 != 0)
                        {
                            frpcOutlog.Text = frpcOutlog.Text + "此QQ号已被占用！请重启电脑再试或联系作者！\n";
                        }
                    }
                    //frpcOutlog.Text = frpcOutlog.Text + "\n端口被占用！请检查是否有进程占用端口或重新配置内网映射！\n";
                }
            }
            frpcOutlog.ScrollToEnd();
        }

        private void setfrpc_Click(object sender, RoutedEventArgs e)
        {
            SetFrpc fw = new SetFrpc();
            var mainwindow = (MainWindow)Window.GetWindow(this);
            fw.Owner = mainwindow;
            fw.ShowDialog();
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc"))
                {
                    if (MainWindow.frpc != null)
                    {
                        frplab1.Content = "您的内网映射已就绪，请点击“启动内网映射”来开启";
                        frplab3.Content = MainWindow.frpc;
                        copyFrpc.IsEnabled = true;
                        startfrpc.IsEnabled = true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("出现错误，请重试:" + "m0x3");
            }
        }

        void RefreshLink()
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            //version
            byte[] pageData1 = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/otherdownload.txt");
            string nv1 = Encoding.UTF8.GetString(pageData1);
            //frpc
            int IndexofA1 = nv1.IndexOf("* ");
            string Ru1 = nv1.Substring(IndexofA1 + 2);
            _dnfrpc = Ru1.Substring(0, Ru1.IndexOf(" *"));
            string nv2 = nv1.Replace("* " + _dnfrpc + " *", "");
        }
        private void startfrpc_Click(object sender, RoutedEventArgs e)
        {
            if (startfrpc.Content.ToString() == "启动内网映射")
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc.exe"))
                {
                    //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/frpc.exe";
                    RefreshLink();
                    //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/frpc.exe";
                    DownloadWindow.downloadurl = _dnfrpc;
                    DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + "MSL";
                    DownloadWindow.filename = "frpc.exe";
                    DownloadWindow.downloadinfo = "下载内网映射中...";
                    Window window = new DownloadWindow();
                    var mainwindow = (MainWindow)Window.GetWindow(this);
                    window.Owner = mainwindow;
                    window.ShowDialog();
                    string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                    JObject jobject = JObject.Parse(jsonString);
                    jobject["frpcversion"] = "2";
                    string convertString = Convert.ToString(jobject);
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
                }
                //内网映射版本检测
                try
                {
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    reader.Close();
                    if (jsonObject["frpcversion"] == null)
                    {
                        RefreshLink();
                        MessageBox.Show("配置文件错误，即将修复");
                        //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/frpc.exe";
                        DownloadWindow.downloadurl = _dnfrpc;
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + "MSL";
                        DownloadWindow.filename = "frpc.exe";
                        DownloadWindow.downloadinfo = "下载内网映射中...";
                        Window window = new DownloadWindow();
                        var mainwindow = (MainWindow)Window.GetWindow(this);
                        window.Owner = mainwindow;
                        window.ShowDialog();
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", MainWindow.mslConfig);
                        Process.Start(Application.ResourceAssembly.Location);
                        Process.GetCurrentProcess().Kill();
                    }
                    if (jsonObject["frpcversion"].ToString() != "2")
                    {
                        RefreshLink();
                        //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/frpc.exe";
                        DownloadWindow.downloadurl = _dnfrpc;
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + "MSL";
                        DownloadWindow.filename = "frpc.exe";
                        DownloadWindow.downloadinfo = "下载内网映射中...";
                        Window window = new DownloadWindow();
                        var mainwindow = (MainWindow)Window.GetWindow(this);
                        window.Owner = mainwindow;
                        window.ShowDialog();
                        string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                        JObject jobject = JObject.Parse(jsonString);
                        jobject["frpcversion"] = "2";
                        string convertString = Convert.ToString(jobject);
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
                    }
                }
                catch { }
                
                StartFrpc();
                setfrpc.IsEnabled = false;
                frpcOutlog.Text = "启动中，请稍后……\n";
            }
            else
            {
                try
                {
                    FRPCMD.Kill();
                    Thread.Sleep(200);
                    FRPCMD.CancelOutputRead();
                    //ReadStdOutput = null;
                    FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    setfrpc.IsEnabled = true;
                    startfrpc.Content = "启动内网映射";
                }
                catch
                //(Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    try
                    {
                        FRPCMD.CancelOutputRead();
                        //ReadStdOutput = null;
                        FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                        catch
                        {
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                    }
                }
        }

        private void copyFrpc_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(MainWindow.frpc);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL\frpc"))
                {
                    if (MainWindow.frpc != null)
                    {
                        frplab1.Content = "您的内网映射已就绪，请点击“启动内网映射”来开启";
                        frplab3.Content = MainWindow.frpc;
                        copyFrpc.IsEnabled = true;
                        startfrpc.IsEnabled = true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("出现错误，请重试:" + "m0x3");
            }
        }
    }
}
