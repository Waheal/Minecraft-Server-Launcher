using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using System.Windows.Threading;
using HandyControl.Controls;
using MessageBox = System.Windows.MessageBox;
using MSL.controls;

namespace MSL.pages
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Home : System.Windows.Controls.Page
    {
        public static event DeleControl SetNormalColor;
        public static event DeleControl SetBlackWhiteColor;
        public static event DeleControl FramePageControl;
        public Home()
        {
            InitializeComponent();
            MainWindow.SetControlsColor += ChangeControlsColor;
            //Cmdoutlog.StartServerControl += Func;
            //Cmdoutlog.StopServerControl += Func1;
        }
        void ChangeControlsColor()
        {
            if (MainWindow.ControlsColor == 0)
            {
                Brush brush = new SolidColorBrush(Color.FromRgb(50, 108, 243));
                startServer.Background = brush;
            }
            if (MainWindow.ControlsColor == 1)
            {
                Brush brush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                startServer.Background = brush;
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(GetNotice);
            thread.Start();
            welcomelabel.Content = "MSL Version：" + MainWindow.update;
        }
        void GetNotice()
        {//公告
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                //notice
                string notice="";
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    if (noticeLab.Text == "")
                    {
                        try
                        {
                            byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/notice.txt");
                            notice = Encoding.UTF8.GetString(pageData);
                            noticeLab.Text = notice;
                        }
                        catch
                        {
                            noticeLab.Text = "获取公告失败！请检查网络连接是否正常或联系作者进行解决！";
                        }
                    }
                });
                //version
                byte[] pageData1 = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/noticeversion.txt");
                string noticeversion = Encoding.UTF8.GetString(pageData1);
                //RefreshLink();
                //notice = Ru2.Substring(0, Ru2.IndexOf("#"));
                try
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        //noticeLab.Text = "公告：\n" + notice;
                        StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
                        JsonTextReader jsonTextReader = new JsonTextReader(reader);
                        JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                        if (jsonObject["notice"] == null)
                        {
                            MessageBox.Show("配置文件错误，即将修复");
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", MainWindow.mslConfig);
                            Process.Start(Application.ResourceAssembly.Location);
                            Process.GetCurrentProcess().Kill();
                        }
                        string noticeversion1 = jsonObject["notice"].ToString();
                        //MessageBox.Show(noticeversion1);
                        reader.Close();
                        if (noticeversion1 != noticeversion)
                        {
                            //MessageBox.Show(notice, "Notice");
                            /*
                            ContentDialog dialog = new ContentDialog()
                            {
                                Title = "公告",
                                CloseButtonText = "确定",
                                IsPrimaryButtonEnabled = false,
                                DefaultButton = ContentDialogButton.Primary,
                                Content = notice,
                            };
                            dialog.ShowAsync();*/
                            byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/notice.txt");
                            notice = Encoding.UTF8.GetString(pageData);
                            noticeLab.Text = notice;

                            MessageDialogShow.Show(notice, "公告", false, "", "确定");
                            MessageDialog messageDialog = new MessageDialog();
                            var mainwindow = (MainWindow)System.Windows.Window.GetWindow(this);
                            messageDialog.Owner = mainwindow;
                            messageDialog.ShowDialog();

                            if (notice.StartsWith("*"))
                            {
                                SetBlackWhiteColor();
                            }
                            else if (MainWindow.ControlsColor == 1)
                            {
                                SetNormalColor();
                            }
                            //MessageBox.Show(notice);
                            //TextDialog.TextDialogTitle = "公告";
                            //TextDialog.TextDialogBody = notice;
                            //Dialog.Show(new TextDialog());
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
                                /*
                                ContentDialog dialog1 = new ContentDialog()
                                {
                                    Title = "ERROR",
                                    CloseButtonText = "确定",
                                    IsPrimaryButtonEnabled = false,
                                    DefaultButton = ContentDialogButton.Primary,
                                    Content = a.Message,
                                };
                                dialog1.ShowAsync();*/
                                MessageBox.Show(a.Message);
                            }
                        }
                        notice = null;
                    });
                }
                catch
                {
                    notice = null;
                }
            }
            catch
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    noticeLab.Text = "获取公告失败！请检查网络连接是否正常或联系作者进行解决！";
                });
            }
        }

        private void startServer_Click(object sender, RoutedEventArgs e)
        {
            FramePageControl();
        }
        /*
private void Func()
{
   startServer.Content = "关闭服务器";
}
void Func1()
{
   startServer.Content = "开启服务器";
}


/*
private void startServer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
{
if (startServer.Content.ToString() == "关闭服务器")
{
Cmdoutlog.SERVERCMD.Kill();
MessageBox.Show("服务器已强制关闭！");
}
}*/
    }
}