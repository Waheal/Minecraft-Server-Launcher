using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MGSL
{
    /// <summary>
    /// LodingForm.xaml 的交互逻辑
    /// </summary>
    public partial class LodingForm : Window
    {
        public static bool isClose = false;
        public LodingForm()
        {
            InitializeComponent();
        }

        [DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            //welcomelabel.Content = "欢迎使用我的世界开服器！版本：" + update;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"MGSL");
            }
            //MessageBox.Show("wqqq");
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", MainWindow.mslConfig);
                Process.Start(Application.ResourceAssembly.Location);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                //检测是否配置了内网映射
                try
                {
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    if (jsonObject["frpc"] == null)
                    {
                        MessageBox.Show("配置文件错误，即将修复");
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MGSL\config.json", MainWindow.mslConfig);
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
                //get serverlink
                try
                {
                    WebClient MyWebClient = new WebClient();
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageData = MyWebClient.DownloadData("https://msl2.oss-cn-hangzhou.aliyuncs.com/");
                    MainWindow.serverLink = Encoding.UTF8.GetString(pageData);
                }
                catch
                {
                    MainWindow.serverLink = "";
                }
                //自动更新
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
                        string strtempa1 = "* ";
                        int IndexofA1 = pageHtml.IndexOf(strtempa1);
                        string Ru1 = pageHtml.Substring(IndexofA1 + 2);
                        string aaa1 = Ru1.Substring(0, Ru1.IndexOf(" *"));
                        DownloadWindow.downloadurl = aaa1;
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory;
                        DownloadWindow.filename = "MGSL" + aaa + ".exe";
                        DownloadWindow.downloadinfo = "下载新版本中……";
                        Window window = new DownloadWindow();
                        window.Owner = this;
                        window.ShowDialog();
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "MGSL" + aaa + ".exe"))
                        {
                            string vBatFile = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\DEL.bat";
                            using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
                            {
                                vStreamWriter.Write(string.Format(":del\r\n del \"" + System.Windows.Forms.Application.ExecutablePath + "\"\r\n " + "if exist \"" + System.Windows.Forms.Application.ExecutablePath + "\" goto del\r\n " + "start /d \"" + AppDomain.CurrentDomain.BaseDirectory + "\" MGSL" + aaa + ".exe" + "\r\n" + " del %0\r\n", AppDomain.CurrentDomain.BaseDirectory));
                            }
                            WinExec(vBatFile, 0);
                            Process.GetCurrentProcess().Kill();
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
                isClose = true;
                Close();

            }
        }
    }
}
