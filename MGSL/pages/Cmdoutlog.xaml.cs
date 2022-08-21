using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace MGSL.pages
{
    public delegate void DelReadStdOutput(string result);
    /// <summary>
    /// Cmdoutlog.xaml 的交互逻辑
    /// </summary>
    public partial class Cmdoutlog : System.Windows.Controls.Page
    {
        public static event DeleControl StartServerControl;
        public static event DeleControl StopServerControl;
        //public static event DeleControl FramePageControl;
        public static Process SERVERCMD = new Process();
        public static string ShieldLog;
        public event DelReadStdOutput ReadStdOutput;
        public static bool autoserver = false;
        DispatcherTimer timer1 = new DispatcherTimer();
        public Cmdoutlog()
        {
            //MessageBox.Show("aaa");
            ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);

            Home.StartServerCmd += Func;
            InitializeComponent();
        }
        private void Func()
        {
            try
            {
                //startServer.Background = new SolidColorBrush(Color.FromRgb(139, 0, 0));
                //serverState.Foreground = new SolidColorBrush(Color.FromRgb(0, 139, 68));
                cmdtext.IsEnabled = true;
                sendcmd.IsEnabled = true;
                fastCMD.IsEnabled = true;
                outlog.Document.Blocks.Clear();
                ShowLog("正在开启服务器，请稍等...", Brushes.Black);
                //serverState.Content = "开服中";
                StartServer(MainWindow.serverJVM + " " + MainWindow.serverJVMcmd + " -jar " + MainWindow.serverserver + " nogui ");
            }
            catch (Exception a)
            {
                MessageBox.Show("出现错误！开服失败！\n错误代码: " + a.Message, "", MessageBoxButton.OK, MessageBoxImage.Question);
                //serversettings.IsEnabled = true;
                //startServer.Content = "开启服务器";
                //startServer.Background = new SolidColorBrush(Color.FromRgb(0, 139, 68));
                //StopServerControl();
                cmdtext.IsEnabled = false;
                sendcmd.IsEnabled = false;
                fastCMD.IsEnabled = false;
            }
        }
        private void StartServer(string StartFileArg)
        {
            try
            {
                Directory.CreateDirectory(MainWindow.serverbase);
                StartServerControl();
                //cmdtext.IsEnabled = true;
                //sendcmd.IsEnabled = true;
                SERVERCMD.StartInfo.FileName = MainWindow.serverjava;
                //SERVERCMD.StartInfo.FileName = StartFileName;
                SERVERCMD.StartInfo.Arguments = StartFileArg;
                Directory.SetCurrentDirectory(MainWindow.serverbase);
                SERVERCMD.StartInfo.CreateNoWindow = true;
                SERVERCMD.StartInfo.UseShellExecute = false;
                SERVERCMD.StartInfo.RedirectStandardInput = true;
                SERVERCMD.StartInfo.RedirectStandardOutput = true;
                SERVERCMD.StartInfo.RedirectStandardError = true;
                SERVERCMD.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                SERVERCMD.ErrorDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                SERVERCMD.Start();
                SERVERCMD.BeginOutputReadLine();
                SERVERCMD.BeginErrorReadLine();
                cmdtext.Text = "";
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = TimeSpan.FromSeconds(1);
                timer1.Start();
                //SERVERCMD.StandardInput.WriteLine(StartFileArg + "&exit");
                //serverTime = 0;
                //timer2.Tick += new EventHandler(timer2_Tick);
                //timer2.Interval = TimeSpan.FromSeconds(1);
                //timer2.Start();
            }
            catch (Exception e)
            {
                timer1.Stop();
                StopServerControl();
                ShowLog("出现错误，正在检查问题...", Brushes.Black);
                string a = MainWindow.serverjava.Replace("\"", "");
                if (File.Exists(a))
                {
                    ShowLog("Java路径正常", Brushes.Green);
                }
                else
                {
                    ShowLog("Java路径有误", Brushes.Red);
                }
                string b = MainWindow.serverserver.Replace("\"", "");
                if (File.Exists(b))
                {
                    ShowLog("服务端路径正常", Brushes.Green);
                }
                else
                {
                    ShowLog("服务端路径有误", Brushes.Red);
                }
                //string c = MainWindow.serverbase.Replace("\"", "");
                if (Directory.Exists(MainWindow.serverbase))
                {
                    ShowLog("服务器目录正常", Brushes.Green);
                }
                else
                {
                    ShowLog("服务器目录有误", Brushes.Red);
                }
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "错误",
                    CloseButtonText = "确定",
                    IsPrimaryButtonEnabled = false,
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "出现错误，开服器已检测完毕，请根据检测信息对服务器设置进行更改！\n错误代码:" + e.Message,
                };
                dialog.ShowAsync();
                //MessageBox.Show("出现错误，开服器已检测完毕，请根据检测信息对服务器设置进行更改！\n错误代码:" + e.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                //startServer.Content = "开启服务器";
                //StartServerControl();
                //serverState.Foreground = new SolidColorBrush(Color.FromRgb(139, 0, 0));
                try
                {
                    SERVERCMD.CancelOutputRead();
                    SERVERCMD.CancelErrorRead();
                    SERVERCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    SERVERCMD.ErrorDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                }
                catch
                { }
                //ReadStdOutput = null;
                //ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
                //outlog.AppendText("\n服务器已关闭！输入start来开启服务器.");

                //serversettings.IsEnabled = true;

                //setdefault.IsEnabled = true;
                cmdtext.IsEnabled = false;
                sendcmd.IsEnabled = false;
                fastCMD.IsEnabled = false;

            }
        }
        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Dispatcher.Invoke(ReadStdOutput, new object[] { e.Data });
            }
        }
        void ShowLog(string msg, Brush color)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (ThreadStart)delegate ()
            {
                string s = string.Format("{1}", DateTime.Now, msg);
                Paragraph p = new Paragraph(new Run(s));
                p.Foreground = color;
                outlog.Document.Blocks.Add(p);
                outlog.ScrollToEnd();
            });
        }
        private delegate void AddMessageHandler(string msg);
        private void ReadStdOutputAction(string msg)
        {
            if (outlog.Document.Blocks.Count >= 500)
            {
                outlog.Document.Blocks.Clear();
            }
            if (closeOutlog_Copy.Content.ToString() == "屏蔽关键字日志:开")
            {
                if (msg.IndexOf(ShieldLog) + 1 != 0)
                {
                    return;
                }
            }
            if (msg.IndexOf("EULA") + 1 != 0)
            {
                //outlog.AppendText("[信息]" + msg);
                //MessageBoxResult msgr = MessageBox.Show("检测到您没有接受Mojang的EULA条款！是否阅读并接受EULA条款并继续开服？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                ShowLog("[信息]" + msg, Brushes.Green);
                var result = MessageBox.Show("检测到您没有接受Mojang的EULA条款！是否阅读并接受EULA条款并继续开服？", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        string path1 = MainWindow.serverbase + @"\eula.txt";
                        FileStream fs = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                        string line;
                        line = sr.ReadToEnd();
                        line = line.Replace("eula=false", "eula=true");
                        string path = MainWindow.serverbase + @"\eula.txt";
                        StreamWriter streamWriter = new StreamWriter(path);
                        streamWriter.WriteLine(line);
                        streamWriter.Flush();
                        streamWriter.Close();
                        timer1.Stop();
                        //serverState.Content = "重启中";
                        SERVERCMD.CancelOutputRead();
                        SERVERCMD.CancelErrorRead();
                        SERVERCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                        SERVERCMD.ErrorDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                        //ReadStdOutput = null;
                        //ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
                        outlog.Document.Blocks.Clear();
                        ShowLog("正在重启服务器...", Brushes.Black);
                        StartServer(MainWindow.serverJVM + " -jar " + MainWindow.serverserver + " nogui");
                    }
                    catch (Exception a)
                    {
                        ContentDialog dialog1 = new ContentDialog()
                        {
                            Title = "ERROR",
                            CloseButtonText = "确定",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            Content = "出现错误，请手动修改eula文件或重试:" + a.Message,
                        };
                        _ = dialog1.ShowAsync();
                        //MessageBox.Show("出现错误，请手动修改eula文件或重试:" + "r0x2", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    Process.Start("https://account.mojang.com/documents/minecraft_eula");
                    /*
                    outlog.Text = "";
                    efflabel.Content = "已开启";
                    timelabel.Content = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
                    sw = new Stopwatch();
                    timer4.Tick += new EventHandler(timer4_Tick);
                    timer4.Interval = TimeSpan.FromSeconds(1);
                    sw.Start();
                    timer4.Start();
                    */
                    //MessageBox.Show("阅读完毕后请点击“开启服务器”按钮以开服！");
                }

            }
            else
            {
                if (msg.IndexOf("Unable to access jarfile") + 1 != 0)
                {
                    ShowLog("[错误]" + msg + "\r\n警告：无法访问JAR文件！请检查服务器设置是否正确或更换服务端再试！", Brushes.Red);
                }
                else
                {
                    if (msg.IndexOf("Done") + 1 != 0)
                    {
                        //outlog.AppendText("[信息]" + msg + "已成功开启服务器！在没有改动服务器IP和端口的情况下，请使用127.0.0.1:25565进入服务器；要让他人进服，需要进行内网映射或使用公网IP。");
                        ShowLog("[信息]" + msg + "\r\n已成功开启服务器！你可以输入stop来关闭服务器！\r\n服务器本地IP通常为:127.0.0.1，想要远程进入服务器，需要开通公网IP或使用内网映射，详情参照开服器的内网映射界面。", Brushes.Black);
                        //serverState.Content = "已开服";
                        try
                        {
                            string path1 = MainWindow.serverbase + @"\server.properties";
                            FileStream fs = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                            string line;
                            line = sr.ReadToEnd();
                            if (line.IndexOf("online-mode=true") + 1 != 0)
                            {
                                onlineMode.IsEnabled = true;
                                ShowLog("\r\n检测到您没有关闭正版验证，如果客户端为离线登录的话，请点击“更多功能”里“关闭正版验证”按钮以关闭正版验证。否则离线账户将无法进入服务器！\r\n", Brushes.Black);
                            }
                        }
                        catch
                        { }

                    }
                    else
                    {
                        if (msg.IndexOf("Stopping server") + 1 != 0)
                        {
                            //outlog.AppendText("[信息]" + msg + "正在关闭服务器！");
                            ShowLog("[信息]" + msg + "\r\n正在关闭服务器！", Brushes.Black);
                            //serverState.Content = "关服中";
                        }
                        else
                        {
                            if (msg.IndexOf("FAILED") + 1 != 0)
                            {
                                if (msg.IndexOf("PORT") + 1 != 0)
                                {
                                    ShowLog("\r\n警告：由于端口占用，服务器已自动关闭！请检查您的服务器是否多开或者有其他软件占用端口！\r\n解决方法：您可尝试通过重启电脑解决！", Brushes.Red);
                                    //outlog.AppendText("[错误]" + msg + "\r\n警告：由于端口占用，服务器已自动关闭！请检查您的服务器是否多开或者有其他软件占用端口！\r\n");

                                }
                            }
                            else
                            {
                                if (closeOutlog.Content.ToString() == "关闭日志输出")
                                {
                                    if (msg.IndexOf("INFO") + 1 != 0)
                                    {
                                        //outlog.AppendText("[信息]" + msg);
                                        ShowLog("[信息]" + msg, Brushes.Green);

                                    }
                                    else
                                    {
                                        if (msg.IndexOf("WARN") + 1 != 0)
                                        {
                                            //outlog.AppendText("[警告]" + msg);
                                            ShowLog("[警告]" + msg, Brushes.Orange);
                                        }
                                        else
                                        {
                                            if (msg.IndexOf("ERROR") + 1 != 0)
                                            {
                                                //outlog.AppendText("[错误]" + msg);
                                                ShowLog("[错误]" + msg, Brushes.Red);

                                            }
                                            else
                                            {
                                                //outlog.AppendText(msg);
                                                ShowLog(msg, Brushes.Green);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (SERVERCMD.HasExited == true)
            {
                timer1.Stop();
                if (autoserver == true)
                {
                    //serverState.Content = "重启中";
                    SERVERCMD.CancelOutputRead();
                    SERVERCMD.CancelErrorRead();
                    SERVERCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    SERVERCMD.ErrorDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    //ReadStdOutput = null;
                    //ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
                    outlog.Document.Blocks.Clear();
                    ShowLog("正在重启服务器...", Brushes.Black);
                    //outlog.AppendText("正在重启服务器...");
                    StartServer(MainWindow.serverJVM + " -jar " + MainWindow.serverserver + " nogui");
                }
                else
                {
                    StopServerControl();
                    cmdtext.Text = "服务器已关闭";
                    //serverState.Content = "已关服";
                    //startServer.Content = "开启服务器";
                    //serverState.Foreground = new SolidColorBrush(Color.FromRgb(139, 0, 0));
                    //tabbtn3.IsEnabled = true;

                    //serversettings.IsEnabled = true;

                    //setdefault.IsEnabled = true;
                    cmdtext.IsEnabled = false;
                    sendcmd.IsEnabled = false;
                    fastCMD.IsEnabled = false;
                    SERVERCMD.CancelOutputRead();
                    SERVERCMD.CancelErrorRead();
                    SERVERCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    SERVERCMD.ErrorDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    //ReadStdOutput = null;
                    //ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
                    //outlog.AppendText("\n服务器已关闭！输入start来开启服务器.");
                }
            }
        }
        private void sendcmd_Click(object sender, RoutedEventArgs e)
        {
            if (fastCMD.SelectedIndex == 1)
            {
                SERVERCMD.StandardInput.WriteLine("op " + cmdtext.Text);
                cmdtext.Text = "";
            }
            if (fastCMD.SelectedIndex == 2)
            {
                SERVERCMD.StandardInput.WriteLine("deop " + cmdtext.Text);
                cmdtext.Text = "";
            }
            if (fastCMD.SelectedIndex == 3)
            {
                SERVERCMD.StandardInput.WriteLine("ban " + cmdtext.Text);
                cmdtext.Text = "";
            }
            if (fastCMD.SelectedIndex == 4)
            {
                SERVERCMD.StandardInput.WriteLine("say " + cmdtext.Text);
                cmdtext.Text = "";
            }
            if (fastCMD.SelectedIndex == 5)
            {
                SERVERCMD.StandardInput.WriteLine("pardon " + cmdtext.Text);
                cmdtext.Text = "";
            }
            SERVERCMD.StandardInput.WriteLine(cmdtext.Text);
            cmdtext.Text = "";
        }

        private void cmdtext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (fastCMD.SelectedIndex == 1)
                {
                    SERVERCMD.StandardInput.WriteLine("op " + cmdtext.Text);
                    cmdtext.Text = "";
                }
                if (fastCMD.SelectedIndex == 2)
                {
                    SERVERCMD.StandardInput.WriteLine("deop " + cmdtext.Text);
                    cmdtext.Text = "";
                }
                if (fastCMD.SelectedIndex == 3)
                {
                    SERVERCMD.StandardInput.WriteLine("ban " + cmdtext.Text);
                    cmdtext.Text = "";
                }
                if (fastCMD.SelectedIndex == 4)
                {
                    SERVERCMD.StandardInput.WriteLine("say " + cmdtext.Text);
                    cmdtext.Text = "";
                }
                if (fastCMD.SelectedIndex == 5)
                {
                    SERVERCMD.StandardInput.WriteLine("pardon " + cmdtext.Text);
                    cmdtext.Text = "";
                }
                SERVERCMD.StandardInput.WriteLine(cmdtext.Text);
                cmdtext.Text = "";
            }
        }

        private void autostartServer_Click(object sender, RoutedEventArgs e)
        {
            if (autoStartserver.Content.ToString() == "关服自动开服:禁用")
            {
                autoserver = true;
                autoStartserver.Content = "关服自动开服:启用";
            }
            else
            {
                autoserver = false;
                autoStartserver.Content = "关服自动开服:禁用";
            }
        }
        private void onlineMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SERVERCMD.HasExited == false)
                {
                    MessageBox.Show("检测到服务器正在运行，正在关闭服务器...");
                    SERVERCMD.StandardInput.WriteLine("stop");
                }
                try
                {
                    string path1 = MainWindow.serverbase + @"\server.properties";
                    FileStream fs = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                    string line;
                    line = sr.ReadToEnd();
                    line = line.Replace("online-mode=true", "online-mode=false");
                    string path = MainWindow.serverbase + @"\server.properties";
                    StreamWriter streamWriter = new StreamWriter(path);
                    streamWriter.WriteLine(line);
                    streamWriter.Flush();
                    streamWriter.Close();
                    MessageBox.Show("修改完毕！请重新启动服务器！");

                }
                catch (Exception a)
                {
                    MessageBox.Show("出现错误，请手动修改server.properties文件或重试:" + a.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                try
                {
                    string path1 = MainWindow.serverbase + @"\server.properties";
                    FileStream fs = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                    string line;
                    line = sr.ReadToEnd();
                    line = line.Replace("online-mode=true", "online-mode=false");
                    string path = MainWindow.serverbase + @"\server.properties";
                    StreamWriter streamWriter = new StreamWriter(path);
                    streamWriter.WriteLine(line);
                    streamWriter.Flush();
                    streamWriter.Close();
                    MessageBox.Show("修改完毕！请重新启动服务器！");

                }
                catch (Exception a)
                {
                    MessageBox.Show("出现错误，请手动修改server.properties文件或重试:" + a.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void moreTools_Click(object sender, RoutedEventArgs e)
        {
            if (moreTools.Content.ToString() != "收起")
            {
                sendcmd.Visibility = Visibility.Hidden;
                cmdtext.Visibility = Visibility.Hidden;
                moreTools.Content = "收起";
            }
            else
            {
                sendcmd.Visibility = Visibility.Visible;
                cmdtext.Visibility = Visibility.Visible;
                moreTools.Content = "更多功能";
            }
        }

        private void closeOutlog_Click(object sender, RoutedEventArgs e)
        {
            if (closeOutlog.Content.ToString() == "关闭日志输出")
            {
                closeOutlog.Content = "打开日志输出";
            }
            else
            {
                closeOutlog.Content = "关闭日志输出";
            }

        }

        private void closeOutlog_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (closeOutlog_Copy.Content.ToString() == "屏蔽关键字日志:关")
            {
                ShieldLog = Microsoft.VisualBasic.Interaction.InputBox("输入你想屏蔽的关键词", "输入", "", -1, -1);
                closeOutlog_Copy.Content = "屏蔽关键字日志:开";
            }
            else
            {
                ShieldLog = null;
                closeOutlog_Copy.Content = "屏蔽关键字日志:关";
            }
        }
    }
}
