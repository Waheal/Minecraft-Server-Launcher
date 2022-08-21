using Microsoft.Win32;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using static MGSL.DownloadWindow;

namespace MGSL
{
    /// <summary>
    /// CreateServer.xaml 的交互逻辑
    /// </summary>
    public partial class CreateServer : Window
    {
        string DownjavaName;
        string autoupdate;
        //public static string autoupdateserver="&";
        string domain;
        string mserversurl;
        bool safeClose = false;
        //public static Process CmdProcess = new Process();
        DispatcherTimer timer1 = new DispatcherTimer();
        public CreateServer()
        {
            InitializeComponent();
        }
        private void next3_Click(object sender, RoutedEventArgs e)
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            //version
            byte[] pageData1 = MyWebClient.DownloadData(MainWindow.serverLink +@"/web/noticeversion.txt");
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
            next3.IsEnabled = false;
            return1.IsEnabled = false;
            if (usejv8.IsChecked == true)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java8\bin\java.exe"))
                {
                    MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java8\bin\java.exe" + "\"";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        DownjavaName = "Java8";
                        //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/Java8";
                        DownloadWindow.downloadurl = _dnjv86;
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                        DownloadWindow.filename = "Java.exe";
                        DownloadWindow.downloadinfo = "下载Java8中……";
                        Window window = new DownloadWindow();
                        window.Owner = this;
                        window.ShowDialog();
                        try
                        {
                            Process CmdProcess = new Process();
                            CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe";
                            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MGSL");
                            CmdProcess.Start();
                            timer1.Tick += new EventHandler(timer1_Tick);
                            timer1.Interval = TimeSpan.FromSeconds(3);
                            timer1.Start();
                            outlog.Content = "安装中...";
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
                            MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            next3.IsEnabled = true;
                            return1.IsEnabled = true;
                            outlog.Content = "安装失败！";
                        }
                        /*
                        Form4 fw = new Form4();
                        fw.ShowDialog();*/
                    //MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java8\bin\java.exe" + "\"";
                    //MainWindow.serverserver = "\"" + txb3.Text + "\"";
                }

                /*
                sJVM.IsSelected = true;
                sJVM.IsEnabled = true;
                sserver.IsEnabled = false;
                MainWindow.serverserver = txb3.Text;*/
            }
            if (usejv16.IsChecked == true)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java16\bin\java.exe"))
                {
                    MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java16\bin\java.exe" + "\"";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    DownjavaName = "Java16";
                    //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/Java16";
                    DownloadWindow.downloadurl = _dnjv16;
                    DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                    DownloadWindow.filename = "Java.exe";
                    DownloadWindow.downloadinfo = "下载Java16中……";
                    Window window = new DownloadWindow();
                    window.Owner = this;
                    window.ShowDialog();
                    try
                    {
                        Process CmdProcess = new Process();
                        CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe";
                        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MGSL");
                        CmdProcess.Start();
                        timer1.Tick += new EventHandler(timer1_Tick);
                        timer1.Interval = TimeSpan.FromSeconds(3);
                        timer1.Start();
                        outlog.Content = "安装中...";
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
                        MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        next3.IsEnabled = true;
                        return1.IsEnabled = true;
                        outlog.Content = "安装失败！";
                    }
                    /*
                    Form4 fw = new Form4();
                    fw.ShowDialog();*/
                    //MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java16\bin\java.exe" + "\"";
                }/*
                sJVM.IsSelected = true;
                sJVM.IsEnabled = true;
                sserver.IsEnabled = false;
                MainWindow.serverserver = txb3.Text;*/
            }
            if (usejv17.IsChecked == true)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java17\bin\java.exe"))
                {
                    MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java17\bin\java.exe" + "\"";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    DownjavaName = "Java17";
                    //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/Java17";
                    DownloadWindow.downloadurl = _dnjv17;
                    DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                    DownloadWindow.filename = "Java.exe";
                    DownloadWindow.downloadinfo = "下载Java17中……";
                    Window window = new DownloadWindow();
                    window.Owner = this;
                    window.ShowDialog();
                    try
                    {
                        Process CmdProcess = new Process();
                        CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe";
                        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MGSL");
                        CmdProcess.Start();
                        timer1.Tick += new EventHandler(timer1_Tick);
                        timer1.Interval = TimeSpan.FromSeconds(3);
                        timer1.Start();
                        outlog.Content = "安装中...";
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
                    catch (Exception aaa)
                    {
                        MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！\n" + aaa, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        next3.IsEnabled = true;
                        return1.IsEnabled = true;
                        outlog.Content = "安装失败！";
                    }
                    /*
                    Form4 fw = new Form4();
                    fw.ShowDialog();*/
                    //MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java17\bin\java.exe" + "\"";
                }
                /*
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserver.IsEnabled = false;
            MainWindow.serverserver = txb3.Text;*/
            }
            if (usejv18.IsChecked == true)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java18\bin\java.exe"))
                {
                    MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java18\bin\java.exe" + "\"";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    DownjavaName = "Java18";
                    //DownloadWindow.downloadurl = MainWindow.serverLink +@"/web/Java17";
                    DownloadWindow.downloadurl = _dnjv18;
                    DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                    DownloadWindow.filename = "Java.exe";
                    DownloadWindow.downloadinfo = "下载Java18中……";
                    Window window = new DownloadWindow();
                    window.Owner = this;
                    window.ShowDialog();
                    try
                    {
                        Process CmdProcess = new Process();
                        CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe";
                        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MGSL");
                        CmdProcess.Start();
                        timer1.Tick += new EventHandler(timer1_Tick);
                        timer1.Interval = TimeSpan.FromSeconds(3);
                        timer1.Start();
                        outlog.Content = "安装中...";
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
                    catch (Exception aaa)
                    {
                        MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！\n" + aaa, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        next3.IsEnabled = true;
                        return1.IsEnabled = true;
                        outlog.Content = "安装失败！";
                    }
                    /*
                    Form4 fw = new Form4();
                    fw.ShowDialog();*/
                    //MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java17\bin\java.exe" + "\"";
                }
                /*
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserver.IsEnabled = false;
            MainWindow.serverserver = txb3.Text;*/
            }
            if (useJVself.IsChecked == true)
            {
                MainWindow.serverjava = "\"" + txjava.Text + "\"";
                MainWindow.serverserver = "\"" + txb3.Text + "\"";
                next3.IsEnabled = true;
                return1.IsEnabled = true;
                javagrid.Visibility = Visibility.Hidden;
                servergrid.Visibility = Visibility.Visible;
                label3.Visibility = Visibility.Visible;
                downloadjava.Visibility = Visibility.Visible;
                selectjava.Visibility = Visibility.Visible;
                return2.Visibility = Visibility.Visible;
            }
            if (usejvPath.IsChecked == true)
            {
                MainWindow.serverjava = "Java";
                MainWindow.serverserver = "\"" + txb3.Text + "\"";
                next3.IsEnabled = true;
                return1.IsEnabled = true;
                javagrid.Visibility = Visibility.Hidden;
                servergrid.Visibility = Visibility.Visible;
                label3.Visibility = Visibility.Visible;
                downloadjava.Visibility = Visibility.Visible;
                selectjava.Visibility = Visibility.Visible;
                return2.Visibility = Visibility.Visible;
            }
        }

        private void next4_Click(object sender, RoutedEventArgs e)
        {
            sserverbase.IsSelected = true;
            sserverbase.IsEnabled = true;
            sJVM.IsEnabled = false;
            if (usedefault.IsChecked == true)
            {
                MainWindow.serverJVM = "";
            }
            else
            {
                MainWindow.serverJVM = "-Xms" + txb4.Text + "M -Xmx" + txb5.Text + "M";
            }
            if (usebasicfastJvm.IsChecked == true)
            {
                MainWindow.serverJVMcmd = "-XX:+AggressiveOpts";
            }
            if (usefastJvm.IsChecked == true)
            {
                MainWindow.serverJVMcmd = "-XX:+UseG1GC -XX:+UnlockExperimentalVMOptions -XX:+ParallelRefProcEnabled -XX:MaxGCPauseMillis=200 -XX:+UnlockExperimentalVMOptions -XX:+DisableExplicitGC -XX:+AlwaysPreTouch -XX:G1NewSizePercent=30 -XX:G1MaxNewSizePercent=40 -XX:G1HeapRegionSize=8M -XX:G1ReservePercent=20 -XX:G1HeapWastePercent=5 -XX:G1MixedGCCountTarget=4 -XX:InitiatingHeapOccupancyPercent=15 -XX:G1MixedGCLiveThresholdPercent=90 -XX:G1RSetUpdatingPauseTimePercent=5 -XX:SurvivorRatio=32 -XX:+PerfDisableSharedMem -XX:MaxTenuringThreshold=1 -Dusing.aikars.flags=https://mcflags.emc.gs -Daikars.new.flags=true";
            }
        }

        private void usedefault_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                txb4.IsEnabled = false;
                txb5.IsEnabled = false;
            }
        }

        private void useJVM_Checked(object sender, RoutedEventArgs e)
        {
            txb4.IsEnabled = true;
            txb5.IsEnabled = true;
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            safeClose = true;
            if (System.IO.Path.IsPathRooted(txb6.Text))
            {
                MainWindow.serverbase = txb6.Text;
            }
            else
            {
                txb6.Text = AppDomain.CurrentDomain.BaseDirectory + txb6.Text;
                MainWindow.serverbase = txb6.Text;
            }
            try
            {
                Directory.CreateDirectory(MainWindow.serverbase);
                StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini");
                sw.Write("*|-j " + MainWindow.serverjava + "|-s " + "\"" + MainWindow.serverserver + "\"" + "|-a " + MainWindow.serverJVM + "|-b " + MainWindow.serverbase + "|-c " + MainWindow.serverJVMcmd + "|*");
                sw.Flush();
                sw.Close();
                sw.Dispose();
                MessageBox.Show("创建完毕，请点击“开启服务器”按钮以开服", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch
            {
                MessageBox.Show("出现错误，请重试" + "c0x1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void return3_Click(object sender, RoutedEventArgs e)
        {
            sserver.IsSelected = true;
            sserver.IsEnabled = true;
            sJVM.IsEnabled = false;
            label3.Visibility = Visibility.Visible;
            downloadjava.Visibility = Visibility.Visible;
            selectjava.Visibility = Visibility.Visible;
            label5.Visibility = Visibility.Hidden;
            usejv8.Visibility = Visibility.Hidden;
            usejv16.Visibility = Visibility.Hidden;
            usejv17.Visibility = Visibility.Hidden;
            outlog.Visibility = Visibility.Hidden;
            jvhelp.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            usejvPath.Visibility = Visibility.Hidden;
            useJVself.Visibility = Visibility.Hidden;
            txjava.Visibility = Visibility.Hidden;
            a0002_Copy.Visibility = Visibility.Hidden;
            next3.Visibility = Visibility.Hidden;
        }

        private void a0002_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件";
            openfile.Filter = "JAR文件|*.jar|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                txb3.Text = openfile.FileName;
            }
        }

        private void a0003_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txb6.Text = dialog.SelectedPath;
            }
        }

        private void return4_Click(object sender, RoutedEventArgs e)
        {
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserverbase.IsEnabled = false;
        }

        private void a0002_Copy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件，通常为java.exe";
            openfile.Filter = "EXE文件|*.exe|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                txjava.Text = openfile.FileName;
            }
        }

        private void useJVself_Checked(object sender, RoutedEventArgs e)
        {
            txjava.IsEnabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + DownjavaName + @"\bin\java.exe"))
            {
                //CmdProcess.CancelOutputRead();
                //ReadStdOutput = null;
                //CmdProcess.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                try
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\Java.exe");
                    outlog.Content = "完成";
                    MainWindow.serverjava = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + DownjavaName + @"\bin\java.exe";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                    timer1.Stop();
                }
                catch
                {
                    return;
                }
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            sserver.IsSelected = true;
            sserver.IsEnabled = true;
            welcome.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (safeClose == false)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private void usejvPath_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                txjava.IsEnabled = false;
            }
        }

        private void usejv8_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                next3.Visibility = Visibility.Visible;
                txjava.IsEnabled = false;
            }
        }

        private void usejv16_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                next3.Visibility = Visibility.Visible;
                txjava.IsEnabled = false;
            }
        }

        private void usejv17_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                next3.Visibility = Visibility.Visible;
                txjava.IsEnabled = false;
            }
        }

        private void usejv18_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                next3.Visibility = Visibility.Visible;
                txjava.IsEnabled = false;
            }
        }
        void GetServer()
        {
            try
            {
                WebClient MyWebClient1 = new WebClient();
                MyWebClient1.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData1 = MyWebClient1.DownloadData(MainWindow.serverLink +@"/web/getserver.txt");
                string pageHtml1 = Encoding.UTF8.GetString(pageData1);

                int IndexofA0 = pageHtml1.IndexOf("*");
                string Ru0 = pageHtml1.Substring(IndexofA0 + 1);
                string pageHtml = Ru0.Substring(0, Ru0.IndexOf("*"));
                //MessageBox.Show(pageHtml);
                if (pageHtml1.IndexOf("#") + 1 != 0)
                {
                    while (pageHtml1.IndexOf("#") != -1)
                    {
                        int IndexofA = pageHtml1.IndexOf("#");
                        string Ru = pageHtml1.Substring(IndexofA + 1);
                        autoupdate = Ru.Substring(0, Ru.IndexOf("|"));

                        int IndexofA1 = pageHtml1.IndexOf("|");
                        string Ru1 = pageHtml1.Substring(IndexofA1 + 1);
                        string autoupdate1 = Ru1.Substring(0, Ru1.IndexOf("\n"));

                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            serverlist.Items.Add(autoupdate);
                            serverurl.Items.Add(autoupdate1);
                        });
                        //MessageBox.Show(autoupdate);
                        //MessageBox.Show(autoupdate1);
                        //MessageBox.Show(pageHtml1);

                        /*
                        HttpWebRequest Myrq1 = (HttpWebRequest)HttpWebRequest.Create(autoupdate1);
                        HttpWebResponse myrp1;
                        myrp1 = (HttpWebResponse)Myrq1.GetResponse();
                        long totalBytes1 = myrp1.ContentLength;
                        Stream st1 = myrp1.GetResponseStream();
                        FileStream so1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/" + autoupdate + ".json", FileMode.Create);
                        long totalDownloadedByte1 = 0;
                        byte[] by1 = new byte[1024];
                        int osize1 = st1.Read(by1, 0, (int)by1.Length);
                        while (osize1 > 0)
                        {
                            totalDownloadedByte1 = osize1 + totalDownloadedByte1;
                            DispatcherHelper.DoEvents();
                            so1.Write(by1, 0, osize1);
                            osize1 = st1.Read(by1, 0, (int)by1.Length);
                            float percent = 0;
                            percent = (float)totalDownloadedByte1 / (float)totalBytes1 * 100;
                            DispatcherHelper.DoEvents();
                        }
                        so1.Close();
                        st1.Close();*/

                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            if (serverlist.SelectedIndex == -1)
                            {
                                serverlist.SelectedIndex = 0;
                                getservermsg.Visibility = Visibility.Hidden;
                            }
                        });
                        int IndexofA3 = pageHtml1.IndexOf("|");
                        string Ru3 = pageHtml1.Substring(IndexofA3 + 1);
                        pageHtml1 = Ru3;

                        //autoupdateserver = autoupdateserver +",&"+ autoupdate;
                        //MessageBox.Show(autoupdate);
                        //MessageBox.Show(autoupdate1);
                        //MessageBox.Show(pageHtml1);
                    }

                    /*
                    //分类服务端
                    StreamReader reader1 = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/paperlist.json");
                    JsonTextReader jsonTextReader1 = new JsonTextReader(reader1);
                    JObject jsonObject1 = (JObject)JToken.ReadFrom(jsonTextReader1);

                    foreach (var x in jsonObject1)
                    {
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            serverlist.Items.Add(x.Key);
                        });
                        //MessageBox.Show( x.Value.ToString(), x.Key);
                    }
                    reader1.Close();*/
                }

                try
                {
                    /*
                    HttpWebRequest Myrq = (HttpWebRequest)HttpWebRequest.Create(pageHtml);
                    HttpWebResponse myrp;
                    myrp = (HttpWebResponse)Myrq.GetResponse();
                    long totalBytes = myrp.ContentLength;
                    Stream st = myrp.GetResponseStream();
                    FileStream so = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/serverlist.json", FileMode.Create);
                    long totalDownloadedByte = 0;
                    byte[] by = new byte[1024];
                    int osize = st.Read(by, 0, (int)by.Length);
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        DispatcherHelper.DoEvents();
                        so.Write(by, 0, osize);
                        osize = st.Read(by, 0, (int)by.Length);
                        float percent = 0;
                        percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                        DispatcherHelper.DoEvents();
                    }
                    so.Close();
                    st.Close();*/
                    mserversurl = pageHtml;
                    WebClient MyWebClient = new WebClient();
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageData = MyWebClient.DownloadData(pageHtml);
                    string aa= Encoding.UTF8.GetString(pageData);
                    //MessageBox.Show(servers);
                    //分类服务端
                    TextReader reader = new StringReader(aa);
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    //MessageBox.Show(jsonObject.ToString());
                    foreach (var x in jsonObject)
                    {
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            serverlist.Items.Add(x.Key);
                        });
                        //MessageBox.Show( x.Value.ToString(), x.Key);
                    }
                    reader.Close();

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        serverlist.SelectedIndex = 0;
                        getservermsg.Visibility = Visibility.Hidden;
                    });
                }
                catch
                {
                }
            }
            catch (Exception a)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    getservermsg.Text = "获取服务端失败！请重试" + "w4x2" + a;
                    //File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/serverlist.json");
                });
                //timer7.Stop();
            }
        }
        private void serverlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (serverlist1.SelectedIndex != -1)
            {
                /*
                if (serverurl.)
                {
                    int url = serverlist1.SelectedIndex;
                    //string filename = serverlist.SelectedItem.ToString();
                    //MessageBox.Show(Url);
                    try
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                        filename = serverlist.SelectedItem.ToString().Substring(0, autoupdate.IndexOf("（")) + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                    catch
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                        filename = serverlist.SelectedItem.ToString() + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                    downloadurl = domain + serverdownurl.Items[url].ToString();
                    downloadinfo = "下载服务端中……";
                    Window window = new DownloadWindow();
                    var mainwindow = (MainWindow)Window.GetWindow(this);
                    window.Owner = mainwindow;
                    window.ShowDialog();
                    //MessageBox.Show(Url,Filename);
                    try
                    {
                        MainWindow.serverserver = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + filename + "\"";
                        //DownComplete();
                    }
                    catch
                    {
                        //DownComplete();
                    }
                }
                else
                {*/
                int url = serverlist1.SelectedIndex;
                //string filename = serverlist.SelectedItem.ToString();
                downloadurl = domain + serverdownurl.Items[url].ToString();
                //MessageBox.Show(downloadurl);
                if (serverlist.SelectedItem.ToString().IndexOf("（") + 1 != 0)
                {
                    try
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                        filename = serverlist.SelectedItem.ToString().Substring(0, autoupdate.IndexOf("（")) + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                    catch
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                        filename = serverlist.SelectedItem.ToString() + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                }
                else
                {
                    downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                    filename = serverlist.SelectedItem.ToString() + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                }
                //MessageBox.Show(downloadurl);
                //downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MGSL";
                //filename = serverlist.SelectedItem.ToString() + serverlist1.SelectedItem.ToString() + ".jar";
                downloadinfo = "下载服务端中……";
                Window window = new DownloadWindow();
                window.Owner = this;
                window.ShowDialog();
                //MessageBox.Show(Url,Filename);
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + filename))
                {
                    MainWindow.serverserver = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + filename + "\"";
                    tabCtrl.Visibility = Visibility.Visible;
                    downserverGrid.Visibility = Visibility.Hidden;
                    //Window wn = new DownloadServer();
                    //wn.ShowDialog();
                    txb3.Text = MainWindow.serverserver.Replace("\"", "");
                    sJVM.IsSelected = true;
                    sJVM.IsEnabled = true;
                    sserver.IsEnabled = false;
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    //DownComplete();
                }
                else
                {
                    MessageBox.Show("下载失败!");
                    //DownComplete();
                }
                //}
            }
        }
        private void serverlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                autoupdate = serverlist.SelectedItem.ToString();
                int a = serverlist.SelectedIndex;
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData = MyWebClient.DownloadData(serverurl.Items[a].ToString());
                string pageHtml = Encoding.UTF8.GetString(pageData);
                //MessageBox.Show(servers);
                //分类服务端
                TextReader reader1 = new StringReader(pageHtml);
                //StreamReader reader1 = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/" + serverlist.SelectedItem.ToString() + ".json");
                JsonTextReader jsonTextReader1 = new JsonTextReader(reader1);
                JObject jsonObject11 = (JObject)JToken.ReadFrom(jsonTextReader1);
                domain = jsonObject11["domain"].ToString();
                updatetime.Content = "最新下载源更新时间：\n" + jsonObject11["updatetime"].ToString();
                //MessageBox.Show(serverlist.SelectedItem.ToString());
                //string abc1 = serverlist.SelectedItem.ToString();
                JObject jsonObject111 = (JObject)jsonObject11["versions"];
                serverlist1.Items.Clear();
                serverdownurl.Items.Clear();
                foreach (var x in jsonObject111)
                {
                    serverlist1.Items.Add(x.Key);
                    serverdownurl.Items.Add(x.Value.ToString());
                    //MessageBox.Show(x.Value.ToString(), x.Key);
                }
                reader1.Close();
            }
            catch
            {
                try
                {
                    domain = "";
                    //MessageBox.Show(mserversurl);
                    //StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/serverlist.json");
                    autoupdate = serverlist.SelectedItem.ToString();
                    WebClient MyWebClient = new WebClient();
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageData = MyWebClient.DownloadData(mserversurl);
                    
                    string pageHtml = Encoding.UTF8.GetString(pageData);
                    //MessageBox.Show(servers);
                    //分类服务端
                    TextReader reader = new StringReader(pageHtml);
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    //MessageBox.Show(serverlist.SelectedItem.ToString());
                    string abc = serverlist.SelectedItem.ToString();
                    JObject jsonObject1 = (JObject)jsonObject[abc];
                    updatetime.Content = "最新下载源更新时间：" + "\n无";
                    serverlist1.Items.Clear();
                    serverdownurl.Items.Clear();
                    foreach (var x in jsonObject1)
                    {
                        serverlist1.Items.Add(x.Key);
                        serverdownurl.Items.Add(x.Value.ToString());
                        //MessageBox.Show(x.Value.ToString(), x.Key);
                    }
                    reader.Close();
                }
                catch
                {
                    MessageBox.Show("获取下载链接失败！");
                }
            }
        }

        private void openSpigot_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.spigotmc.org/");
        }

        private void openPaper_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://papermc.io/");
        }

        private void openMojang_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.minecraft.net/zh-hans/download/server");
        }
        /*
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/serverlist.json"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/serverlist.json");
            }
            int i = serverlist.Items.Count;
            int x = 0;
            while (x != i)
            {
                string aaa = serverlist.Items[x].ToString();
                x++;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/" + aaa + ".json"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MGSL/" + aaa + ".json");
                }
            }
        }*/

        private void downloadserver_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(GetServer);
            thread.Start();
            tabCtrl.Visibility = Visibility.Hidden;
            downserverGrid.Visibility = Visibility.Visible;
            //Window wn = new DownloadServer();
            //wn.ShowDialog();
            /*
            txb3.Text = MainWindow.serverserver.Replace("\"", "");
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserver.IsEnabled = false;
            next3.IsEnabled = true;
            return1.IsEnabled = true;*/
        }

        private void selectserver_Click(object sender, RoutedEventArgs e)
        {
            downloadserver.Visibility = Visibility.Hidden;
            selectserver.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            return2.Visibility = Visibility.Visible;
            txb3.Visibility = Visibility.Visible;
            next2.Visibility = Visibility.Visible;
            a0002.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
        }

        private void next2_Click(object sender, RoutedEventArgs e)
        {
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserver.IsEnabled = false;
            next3.IsEnabled = true;
            return1.IsEnabled = true;
        }

        private void selectjava_Click(object sender, RoutedEventArgs e)
        {
            label4.Visibility = Visibility.Visible;
            usejvPath.Visibility = Visibility.Visible;
            useJVself.Visibility = Visibility.Visible;
            txjava.Visibility = Visibility.Visible;
            a0002_Copy.Visibility = Visibility.Visible;
            next3.Visibility = Visibility.Visible;
            return1.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Hidden;
            downloadjava.Visibility = Visibility.Hidden;
            selectjava.Visibility = Visibility.Hidden;
        }

        private void downloadjava_Click(object sender, RoutedEventArgs e)
        {
            label5.Visibility = Visibility.Visible;
            usejv8.Visibility = Visibility.Visible;
            usejv16.Visibility = Visibility.Visible;
            usejv17.Visibility = Visibility.Visible;
            outlog.Visibility = Visibility.Visible;
            jvhelp.Visibility = Visibility.Visible;
            next3.Visibility = Visibility.Visible;
            return1.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Hidden;
            downloadjava.Visibility = Visibility.Hidden;
            selectjava.Visibility = Visibility.Hidden;
        }

        private void return1_Click(object sender, RoutedEventArgs e)
        {
            downloadserver.Visibility = Visibility.Visible;
            selectserver.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
            javagrid.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            downloadjava.Visibility = Visibility.Visible;
            selectjava.Visibility = Visibility.Visible;
            return1.Visibility = Visibility.Visible;
            servergrid.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Hidden;
            usejv8.Visibility = Visibility.Hidden;
            usejv16.Visibility = Visibility.Hidden;
            usejv17.Visibility = Visibility.Hidden;
            outlog.Visibility = Visibility.Hidden;
            jvhelp.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            return1.Visibility = Visibility.Hidden;
            return2.Visibility = Visibility.Hidden;
            usejvPath.Visibility = Visibility.Hidden;
            useJVself.Visibility = Visibility.Hidden;
            txjava.Visibility = Visibility.Hidden;
            a0002_Copy.Visibility = Visibility.Hidden;
            next3.Visibility = Visibility.Hidden;
            next2.Visibility = Visibility.Hidden;
            txb3.Visibility = Visibility.Hidden;
            next2.Visibility = Visibility.Hidden;
            a0002.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;
        }

        private void skip_Click(object sender, RoutedEventArgs e)
        {
            safeClose = true;
            try
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\server.ini");
                Close();
            }
            catch
            {
                MessageBox.Show("出现错误，请重试" + "c0x1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void usebasicfastJvm_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("使用优化参数需要手动设置大小相同的内存，请对上面的内存进行更改！", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            useJVM.IsChecked = true;
            txb4.Text = "1024";
            txb5.Text = "1024";
        }
        private void usefastJvm_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("使用优化参数需要手动设置大小相同的内存，请对上面的内存进行更改！", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            useJVM.IsChecked = true;
            txb4.Text = "2048";
            txb5.Text = "2048";
        }

        private void usefastJvmPro_Checked(object sender, RoutedEventArgs e)
        {
            useJVM.IsChecked = true;
            txb4.Text = "10240";
            txb5.Text = "10240";
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void mainBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
