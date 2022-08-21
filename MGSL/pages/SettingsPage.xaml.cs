using Microsoft.Win32;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : System.Windows.Controls.Page
    {
        public static event DeleControl C_NotifyIcon;
        public static event DeleControl DownServerCtrl;
        //public static event DeleControl OpenHelp;
        public static string line;
        string DownjavaName;
        DispatcherTimer timer1 = new DispatcherTimer();
        DispatcherTimer cmdtimer = new DispatcherTimer();
        public SettingsPage()
        {
            InitializeComponent();
            pages.DownloadServer.DownComplete += Func;
        }
        public async void GetServerConfig()
        {
            try
            {
                string line = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini");

                int IndexofA2 = line.IndexOf("-j ");
                string Ru2 = line.Substring(IndexofA2 + 3);
                string a200 = Ru2.Substring(0, Ru2.IndexOf("|"));
                //serverjavalist.Items.Add(a200);
                MainWindow.serverjava = a200;

                int IndexofA3 = line.IndexOf("-s ");
                string Ru3 = line.Substring(IndexofA3 + 3);
                string a300 = Ru3.Substring(0, Ru3.IndexOf("|"));
                //serverserverlist.Items.Add(a300);
                MainWindow.serverserver = a300;

                int IndexofA4 = line.IndexOf("-a ");
                string Ru4 = line.Substring(IndexofA4 + 3);
                string a400 = Ru4.Substring(0, Ru4.IndexOf("|"));
                //serverJVMlist.Items.Add(a400);
                MainWindow.serverJVM = a400;

                int IndexofA5 = line.IndexOf("-b ");
                string Ru5 = line.Substring(IndexofA5 + 3);
                string a500 = Ru5.Substring(0, Ru5.IndexOf("|"));
                //serverbaselist.Items.Add(a500);
                MainWindow.serverbase = a500;

                int IndexofA6 = line.IndexOf("-c ");
                string Ru6 = line.Substring(IndexofA6 + 3);
                string a600 = Ru6.Substring(0, Ru6.IndexOf("|"));
                //serverbaselist.Items.Add(a500);
                MainWindow.serverJVMcmd = a600;
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
        private void doneBtn1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!System.IO.Path.IsPathRooted(jAva.Text))
                {
                    jAva.Text = AppDomain.CurrentDomain.BaseDirectory + jAva.Text;
                }
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                //version
                byte[] pageData1 = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/noticeversion.txt");
                string nv1 = Encoding.UTF8.GetString(pageData1);
                //frpc
                int IndexofA1 = nv1.IndexOf("* ");
                string Ru1 = nv1.Substring(IndexofA1 + 2);
                string nv2 = nv1.Replace("* " + Ru1.Substring(0, Ru1.IndexOf(" *")) + " *", "");
                //jv83 *已删除
                //jv86
                int IndexofA3 = nv2.IndexOf("* ");
                string Ru3 = nv2.Substring(IndexofA3 + 2);
                string _dnjv86 = Ru3.Substring(0, Ru3.IndexOf(" *"));
                string nv4 = nv2.Replace("* " + _dnjv86 + " *", "");
                //jv16
                int IndexofA4 = nv4.IndexOf("* ");
                string Ru4 = nv4.Substring(IndexofA4 + 2);
                string _dnjv16 = Ru4.Substring(0, Ru4.IndexOf(" *"));
                string nv5 = nv4.Replace("* " + _dnjv16 + " *", "");
                //jv17
                int IndexofA5 = nv5.IndexOf("* ");
                string Ru5 = nv5.Substring(IndexofA5 + 2);
                string _dnjv17 = Ru5.Substring(0, Ru5.IndexOf(" *"));
                string nv6 = nv5.Replace("* " + _dnjv17 + " *", "");
                //jv18
                int IndexofA6 = nv6.IndexOf("* ");
                string Ru6 = nv6.Substring(IndexofA6 + 2);
                string _dnjv18 = Ru6.Substring(0, Ru6.IndexOf(" *"));
                doneBtn1.IsEnabled = false;
                if (useDownJv.IsChecked == true)
                {
                    if (selectJava.SelectedIndex == 0)
                    {
                        DownloadJava("Java8", _dnjv86);

                        /*
                        sJVM.IsSelected = true;
                        sJVM.IsEnabled = true;
                        sserver.IsEnabled = false;
                        MainWindow.serverserver = txb3.Text;*/
                    }
                    if (selectJava.SelectedIndex == 1)
                    {
                        DownloadJava("Java16", _dnjv16);

                        /*
                        sJVM.IsSelected = true;
                        sJVM.IsEnabled = true;
                        sserver.IsEnabled = false;
                        MainWindow.serverserver = txb3.Text;*/
                    }
                    if (selectJava.SelectedIndex == 2)
                    {
                        DownloadJava("Java17", _dnjv17);
                        /*
                    sJVM.IsSelected = true;
                    sJVM.IsEnabled = true;
                    sserver.IsEnabled = false;
                    MainWindow.serverserver = txb3.Text;*/
                    }
                    if (selectJava.SelectedIndex == 3)
                    {
                        DownloadJava("Java18", _dnjv18);
                        /*
                    sJVM.IsSelected = true;
                    sJVM.IsEnabled = true;
                    sserver.IsEnabled = false;
                    MainWindow.serverserver = txb3.Text;*/
                    }
                }
                if (useSelf.IsChecked == true)
                {
                    if (useJVMauto.IsChecked == true)
                    {
                        Directory.CreateDirectory(bAse.Text);
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        GetServerConfig();
                    }
                    else
                    {
                        Directory.CreateDirectory(bAse.Text);
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        GetServerConfig();
                    }
                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "信息",
                        CloseButtonText = "确定",
                        IsPrimaryButtonEnabled = false,
                        DefaultButton = ContentDialogButton.Primary,
                        Content = "保存完毕！",
                    };
                    dialog.ShowAsync();
                    //MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    doneBtn1.IsEnabled = true;
                }
                if (useJvpath.IsChecked == true)
                {
                    if (useJVMauto.IsChecked == true)
                    {
                        Directory.CreateDirectory(bAse.Text);
                        line = "*|-j Java|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        GetServerConfig();
                    }
                    else
                    {
                        Directory.CreateDirectory(bAse.Text);
                        line = "*|-j Java|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        GetServerConfig();
                    }
                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "信息",
                        CloseButtonText = "确定",
                        IsPrimaryButtonEnabled = false,
                        DefaultButton = ContentDialogButton.Primary,
                        Content = "保存完毕！",
                    };
                    dialog.ShowAsync();
                    //MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    doneBtn1.IsEnabled = true;
                }
            }
            catch (Exception err)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "ERROR",
                    CloseButtonText = "确定",
                    IsPrimaryButtonEnabled = false,
                    DefaultButton = ContentDialogButton.Primary,
                    Content = "出现错误！请重试:\n" + err.ToString(),
                };
                dialog.ShowAsync();
                //MessageBox.Show("出现错误！请重试:\n" + err.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                doneBtn1.IsEnabled = true;
            }
        }

        private void DownloadJava(string fileName,string downUrl)
        {
            jAva.Text = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\"+fileName+@"\bin\java.exe";
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + fileName))
            {
                if (useJVMauto.IsChecked == true)
                {
                    Directory.CreateDirectory(bAse.Text);
                    line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(line);
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    GetServerConfig();
                }
                else
                {
                    Directory.CreateDirectory(bAse.Text);
                    line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(line);
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    GetServerConfig();
                }
                StackPanel panel = new StackPanel()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };
                panel.Children.Add(new TextBlock() { Text = "保存完毕！" });
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "信息",
                    CloseButtonText = "确定",
                    IsPrimaryButtonEnabled = false,
                    DefaultButton = ContentDialogButton.Primary,
                    Content = panel,
                };
                dialog.ShowAsync();
                //MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                doneBtn1.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + fileName + @"\bin\java.exe"))
                {
                    DownjavaName = fileName;
                    //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/Java8.exe";
                    DownloadWindow.downloadurl = downUrl;
                    DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + "MGSL";
                    DownloadWindow.filename = "Java.exe";
                    DownloadWindow.downloadinfo = "下载"+ fileName + "中……";
                    Window window = new DownloadWindow();
                    var mainwindow = (MainWindow)Window.GetWindow(this);
                    window.Owner = mainwindow;
                    window.ShowDialog();
                    try
                    {
                        Process SERVERCMD = new Process();
                        SERVERCMD.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe";
                        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MGSL");
                        SERVERCMD.Start();
                        timer1.Tick += new EventHandler(timer1_Tick);
                        timer1.Interval = TimeSpan.FromSeconds(3);
                        timer1.Start();
                        downout.Content = "安装中...";
                        /*
                        try
                        {
                            File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe");
                            outlog.Content = "完成";
                            sJVM.IsSelected = true;
                            sJVM.IsEnabled = true;
                            sserver.IsEnabled = false;
                            MainWindow.serverserver = txb3.Text;
                            next3.IsEnabled = true;
                        }
                        catch
                        {
                            return;
                        }*/
                    }
                    catch
                    {
                        ContentDialog dialog1 = new ContentDialog()
                        {
                            Title = "ERROR",
                            CloseButtonText = "确定",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            Content = "安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！",
                        };
                        dialog1.ShowAsync();
                        //MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        downout.Content = "安装失败！";
                        doneBtn1.IsEnabled = true;
                    }
                    /*
                    Form4 fw = new Form4();
                    fw.ShowDialog();*/
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + DownjavaName + @"\bin\java.exe"))
            {
                try
                {
                    if (useJVMauto.IsChecked == true)
                    {
                        Directory.CreateDirectory(bAse.Text);
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        GetServerConfig();
                    }
                    else
                    {
                        Directory.CreateDirectory(bAse.Text);
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|-c " + jVMcmd.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        GetServerConfig();
                    }
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe");
                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "信息",
                        CloseButtonText = "确定",
                        IsPrimaryButtonEnabled = false,
                        DefaultButton = ContentDialogButton.Primary,
                        Content = "保存完毕！",
                    };
                    dialog.ShowAsync();
                    //MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    downout.Content = "安装成功！";
                    doneBtn1.IsEnabled = true;
                    timer1.Stop();
                }
                catch
                {
                    return;
                }
            }
        }

        private void a01_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件";
            openfile.Filter = "JAR文件|*.jar|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                server.Text = openfile.FileName;
            }
        }

        private void downServer_Click(object sender, RoutedEventArgs e)
        {
            DownServerCtrl();
            /*
            if (File.Exists(MainWindow.serverserver.Replace("\"", "")))
            {
                server.Text = MainWindow.serverserver.Replace("\"", "");
            }*/
        }
        void Func()
        {
            if (File.Exists(MainWindow.serverserver.Replace("\"", "")))
            {
                server.Text = MainWindow.serverserver.Replace("\"", "");
            }
        }

        private void a02_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bAse.Text = dialog.SelectedPath;
            }
        }

        private void a03_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件，通常为java.exe";
            openfile.Filter = "EXE文件|*.exe|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                jAva.Text = openfile.FileName;
            }
        }
        private void useJVMauto_Click(object sender, RoutedEventArgs e)
        {
            if (useJVMauto.IsChecked == true)
            {
                memorySlider.IsEnabled = false;
                useJVMself.IsChecked = false;
            }
            else
            {
                useJVMauto.IsChecked = true;
            }
        }

        private void useJVMself_Click(object sender, RoutedEventArgs e)
        {
            if (useJVMself.IsChecked == true)
            {
                memorySlider.IsEnabled = true;
                useJVMauto.IsChecked = false;
            }
            else
            {
                useJVMself.IsChecked = true;
            }
        }
        private void setServerconfig_Click(object sender, RoutedEventArgs e)
        {
            Window window = new SetServerconfig();
            window.ShowDialog();
        }
        private void mulitDownthread_Click(object sender, RoutedEventArgs e)
        {
            DownloadWindow.downloadthread = int.Parse(downthreadCount.Text);
        }
        private void setdefault_Click(object sender, RoutedEventArgs e)
        {
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", String.Format("{{{0}}}", "\n\"server\": \"\",\n\"java\": \"\",\n\"b\": \"\",\n\"frpc\": \"\",\n\"bcolor\": \"\",\n\"fcolor\": \"\",\n\"notifyIcon\": \"true\"\n"));
            try
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MGSL", true);
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"MGSL");
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", MainWindow.mslConfig);
            }
            catch
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", MainWindow.mslConfig);
            }
            Process.Start(Application.ResourceAssembly.Location);
            Process.GetCurrentProcess().Kill();
        }
        private void notifyIconbtn_Click(object sender, RoutedEventArgs e)
        {
            if (notifyIconbtn.Content.ToString() == "关闭托盘图标")
            {
                notifyIconbtn.Content = "打开托盘图标";
                MainWindow.notifyIcon = false;
                try
                {
                    //StreamReader reader = File.OpenText(Application.StartupPath+@"\server\MGSL.json", System.Text.Encoding.UTF8);
                    string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", System.Text.Encoding.UTF8);
                    JObject jobject = JObject.Parse(jsonString);
                    jobject["notifyIcon"] = "False";
                    string convertString = Convert.ToString(jobject);
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", convertString, System.Text.Encoding.UTF8);
                }
                catch
                {
                    return;
                }
            }
            else
            {
                notifyIconbtn.Content = "关闭托盘图标";
                MainWindow.notifyIcon = true;
                C_NotifyIcon();
                try
                {
                    //StreamReader reader = File.OpenText(Application.StartupPath+@"\server\MGSL.json", System.Text.Encoding.UTF8);
                    string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", System.Text.Encoding.UTF8);
                    JObject jobject = JObject.Parse(jsonString);
                    jobject["notifyIcon"] = "True";
                    string convertString = Convert.ToString(jobject);
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", convertString, System.Text.Encoding.UTF8);
                }
                catch
                {
                    return;
                }
            }
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                server.Text = MainWindow.serverserver.Replace("\"", "");
                //jVM.Text = serverJVM;
                memorySlider.Maximum = MainWindow.PhisicalMemory / 1024.0 / 1024.0;
                bAse.Text = MainWindow.serverbase;
                jVMcmd.Text = MainWindow.serverJVMcmd;
                jAva.Text = MainWindow.serverjava.Replace("\"", "");
                if (jAva.Text == "Java")
                {
                    useJvpath.IsChecked = true;
                }
                if (jAva.Text == AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java8\bin\java.exe")
                {
                    selectJava.SelectedIndex = 0;
                }
                if (jAva.Text == AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java16\bin\java.exe")
                {
                    selectJava.SelectedIndex = 1;
                }
                if (jAva.Text == AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java17\bin\java.exe")
                {
                    selectJava.SelectedIndex = 2;
                }
                if (jAva.Text == AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java18\bin\java.exe")
                {
                    selectJava.SelectedIndex = 3;
                }
                if (MainWindow.serverJVM == "")
                {
                    memorySlider.IsEnabled = false;
                    useJVMself.IsChecked = false;
                    useJVMauto.IsChecked = true;
                    memoryInfo.Content = "现在为自动分配";
                }
                else
                {
                    memorySlider.IsEnabled = true;
                    useJVMauto.IsChecked = false;
                    useJVMself.IsChecked = true;
                    int IndexofA6 = MainWindow.serverJVM.IndexOf("-Xmx");
                    string Ru6 = MainWindow.serverJVM.Substring(IndexofA6 + 4);
                    string a600 = Ru6.Substring(0, Ru6.IndexOf("M"));

                    memorySlider.Value = int.Parse(a600);
                    memoryInfo.Content = "分配内存：" + a600 + "M";
                }

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
            if (MainWindow.notifyIcon == true)
            {
                notifyIconbtn.Content = "关闭托盘图标";
            }
            else
            {
                notifyIconbtn.Content = "打开托盘图标";
            }
        }

        private void memorySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            memoryInfo.Content = "分配内存：" + memorySlider.Value.ToString("f0") + "M";
        }

        private void useidea_Click(object sender, RoutedEventArgs e)
        {
            Browser browser = new Browser();
            var mainwindow = (MainWindow)Window.GetWindow(this);
            browser.Owner = mainwindow;
            browser.ShowDialog();
        }

        private void getLaunchercode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(MainWindow.serverbase + @"\StartServer.bat",  MainWindow.serverjava+ MainWindow.serverJVM + " " + MainWindow.serverJVMcmd + " -jar " + MainWindow.serverserver + " nogui ");
                MessageBox.Show("脚本已生成至" + MainWindow.serverbase + "文件夹");
                Process.Start("explorer.exe", MainWindow.serverbase);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void startTimercmd_Click(object sender, RoutedEventArgs e)
        {
            if (startTimercmd.Content.ToString() == "启动定时任务")
            {
                cmdtimer.Tick += new EventHandler(cmdtimer_Tick);
                cmdtimer.Interval = TimeSpan.FromSeconds(int.Parse(timercmdTime.Text));
                cmdtimer.Start();
                startTimercmd.Content = "停止定时任务";
            }
            else
            {
                cmdtimer.Stop();
                startTimercmd.Content = "启动定时任务";
            }
        }
        private void cmdtimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Cmdoutlog.SERVERCMD.HasExited == false)
                {
                    Cmdoutlog.SERVERCMD.StandardInput.WriteLine(timercmdCmd.Text);
                    timerCmdout.Content = "执行成功  时间：" + DateTime.Now.ToString("F");
                }
            }
            catch 
            {
                timerCmdout.Content = "执行失败，请检查服务器是否开启  时间：" + DateTime.Now.ToString("F");
            }
        }

            /*
            //skin
            private void defaultSkin_Checked(object sender, RoutedEventArgs e)
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                jsonObject["skin"] = "default";
                reader.Close();
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", jsonObject.ToString());
                Brush aaa = new SolidColorBrush(Color.FromRgb(241, 243, 248));//blue
                Background = aaa;
            }

            private void redSkin_Checked(object sender, RoutedEventArgs e)
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                jsonObject["skin"] = "red";
                reader.Close();
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", jsonObject.ToString());
                Brush aaa = new SolidColorBrush(Color.FromRgb(248, 241, 241));//red
                Background = aaa;
            }

            private void yellowSkin_Checked(object sender, RoutedEventArgs e)
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                jsonObject["skin"] = "yellow";
                reader.Close();
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", jsonObject.ToString());
                Brush aaa = new SolidColorBrush(Color.FromRgb(248, 248, 241));//yellow
                Background = aaa;
            }

            private void greenSkin_Checked(object sender, RoutedEventArgs e)
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                jsonObject["skin"] = "green";
                reader.Close();
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", jsonObject.ToString());
                Brush aaa = new SolidColorBrush(Color.FromRgb(241, 248, 244));//green
                Background = aaa;
            }

            private void pinkSkin_Checked(object sender, RoutedEventArgs e)
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                jsonObject["skin"] = "pink";
                reader.Close();
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", jsonObject.ToString());
                Brush aaa = new SolidColorBrush(Color.FromRgb(247, 241, 248));//pink
                Background = aaa;
            }

            private void purpleSkin_Checked(object sender, RoutedEventArgs e)
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                jsonObject["skin"] = "purple";
                reader.Close();
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", jsonObject.ToString());
                Brush aaa = new SolidColorBrush(Color.FromRgb(237, 231, 245));//purple
                Background = aaa;
            }
            */
        }
}