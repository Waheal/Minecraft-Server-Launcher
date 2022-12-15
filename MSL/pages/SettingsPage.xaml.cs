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
using Windows.UI.Popups;

namespace MSL.pages
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : System.Windows.Controls.Page
    {
        public static event DeleControl C_NotifyIcon;
        //public static event DeleControl DownServerCtrl;
        //public static event DeleControl SetServerComplete;
        //public static event DeleControl OpenHelp;
        //DispatcherTimer timer1 = new DispatcherTimer();
        public SettingsPage()
        {
            //timer1.Tick += new EventHandler(timer1_Tick);
            InitializeComponent();
            
            //DownloadServer.DownComplete += Func;
            //ServerList.SetServerConfig += Func1;
        }
        private void mulitDownthread_Click(object sender, RoutedEventArgs e)
        {
            DownloadWindow.downloadthread = int.Parse(downthreadCount.Text);
        }
        private void setdefault_Click(object sender, RoutedEventArgs e)
        {
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", String.Format("{{{0}}}", "\n\"server\": \"\",\n\"java\": \"\",\n\"b\": \"\",\n\"frpc\": \"\",\n\"bcolor\": \"\",\n\"fcolor\": \"\",\n\"notifyIcon\": \"true\"\n"));
            try
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL", true);
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"MSL");
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", MainWindow.mslConfig);
            }
            catch
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", MainWindow.mslConfig);
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
                    //StreamReader reader = File.OpenText(Application.StartupPath+@"\server\MSL.json", System.Text.Encoding.UTF8);
                    string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                    JObject jobject = JObject.Parse(jsonString);
                    jobject["notifyIcon"] = "False";
                    string convertString = Convert.ToString(jobject);
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
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
                    //StreamReader reader = File.OpenText(Application.StartupPath+@"\server\MSL.json", System.Text.Encoding.UTF8);
                    string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                    JObject jobject = JObject.Parse(jsonString);
                    jobject["notifyIcon"] = "True";
                    string convertString = Convert.ToString(jobject);
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
                }
                catch
                {
                    return;
                }
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainWindow.notifyIcon == true)
            {
                notifyIconbtn.Content = "关闭托盘图标";
            }
            else
            {
                notifyIconbtn.Content = "打开托盘图标";
            }
            StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            if (jsonObject["autoOpenServer"].ToString() == "False")
            {
                openserversOnStart.Content = "关闭";
            }
            else
            {
                openserversOnStart.Content = "打开";
                openserversOnStartList.Text = jsonObject["autoOpenServer"].ToString();
            }
            if (jsonObject["autoOpenFrpc"].ToString() == "False")
            {
                openfrpOnStart.Content = "关闭";
            }
            else
            {
                openfrpOnStart.Content = "打开";
            }
            reader.Close();
        }

        private void useidea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink + @"/web/help.txt");
                string notice = Encoding.UTF8.GetString(pageData);
                Process.Start(notice);
            }
            catch 
            {
                MessageBox.Show("获取教程失败！", "err", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void openserversOnStart_Click(object sender, RoutedEventArgs e)
        {
            if (openserversOnStart.Content.ToString() == "关闭")
            {
                openserversOnStart.Content = "打开";
                if (openserversOnStartList.Text == "")
                {
                    MessageBox.Show("请先在下面列表中输入服务器ID！");
                    openserversOnStart.Content = "关闭";
                    return;
                }
                string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                JObject jobject = JObject.Parse(jsonString);
                jobject["autoOpenServer"] = openserversOnStartList.Text;
                string convertString = Convert.ToString(jobject);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
            }
            else
            {
                openserversOnStart.Content = "关闭";
                string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                JObject jobject = JObject.Parse(jsonString);
                jobject["autoOpenServer"] = false;
                string convertString = Convert.ToString(jobject);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
            }
        }

        private void openfrpOnStart_Click(object sender, RoutedEventArgs e)
        {
            if (openfrpOnStart.Content.ToString() == "关闭")
            {
                openfrpOnStart.Content = "打开";
                string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                JObject jobject = JObject.Parse(jsonString);
                jobject["autoOpenFrpc"] = true;
                string convertString = Convert.ToString(jobject);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
            }
            else
            {
                openfrpOnStart.Content = "关闭";
                string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", System.Text.Encoding.UTF8);
                JObject jobject = JObject.Parse(jsonString);
                jobject["autoOpenFrpc"] = false;
                string convertString = Convert.ToString(jobject);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", convertString, System.Text.Encoding.UTF8);
            }
        }

        /*
        //skin
        private void defaultSkin_Checked(object sender, RoutedEventArgs e)
        {
            StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            jsonObject["skin"] = "default";
            reader.Close();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", jsonObject.ToString());
            Brush aaa = new SolidColorBrush(Color.FromRgb(241, 243, 248));//blue
            Background = aaa;
        }

        private void redSkin_Checked(object sender, RoutedEventArgs e)
        {
            StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            jsonObject["skin"] = "red";
            reader.Close();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", jsonObject.ToString());
            Brush aaa = new SolidColorBrush(Color.FromRgb(248, 241, 241));//red
            Background = aaa;
        }

        private void yellowSkin_Checked(object sender, RoutedEventArgs e)
        {
            StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            jsonObject["skin"] = "yellow";
            reader.Close();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", jsonObject.ToString());
            Brush aaa = new SolidColorBrush(Color.FromRgb(248, 248, 241));//yellow
            Background = aaa;
        }

        private void greenSkin_Checked(object sender, RoutedEventArgs e)
        {
            StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            jsonObject["skin"] = "green";
            reader.Close();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", jsonObject.ToString());
            Brush aaa = new SolidColorBrush(Color.FromRgb(241, 248, 244));//green
            Background = aaa;
        }

        private void pinkSkin_Checked(object sender, RoutedEventArgs e)
        {
            StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            jsonObject["skin"] = "pink";
            reader.Close();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", jsonObject.ToString());
            Brush aaa = new SolidColorBrush(Color.FromRgb(247, 241, 248));//pink
            Background = aaa;
        }

        private void purpleSkin_Checked(object sender, RoutedEventArgs e)
        {
            StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json");
            JsonTextReader jsonTextReader = new JsonTextReader(reader);
            JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
            jsonObject["skin"] = "purple";
            reader.Close();
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL\config.json", jsonObject.ToString());
            Brush aaa = new SolidColorBrush(Color.FromRgb(237, 231, 245));//purple
            Background = aaa;
        }
        */
    }
}