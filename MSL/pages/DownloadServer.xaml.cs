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
using static MSL.DownloadWindow;

namespace MSL.pages
{
    /// <summary>
    /// DownloadServer.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadServer : Page
    {
        public static event DeleControl DownComplete;
        string autoupdate;
        string mserversurl;
        //public static string autoupdateserver="&";
        string domain;
        public DownloadServer()
        {
            InitializeComponent();
        }
        //服务端下载
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
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL";
                        filename = serverlist.SelectedItem.ToString().Substring(0, autoupdate.IndexOf("（")) + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                    catch
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL";
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
                        MainWindow.serverserver = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL\" + filename + "\"";
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
                    if (serverlist1.SelectedItem.ToString().IndexOf("（") + 1 != 0)
                    {
                            downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL";
                            filename = serverlist.SelectedItem.ToString().Substring(0, autoupdate.IndexOf("（")) + "-" + serverlist1.SelectedItem.ToString().Substring(0, serverlist1.SelectedItem.ToString().IndexOf("（")) + ".jar";
                    }
                    else
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL";
                        filename = serverlist.SelectedItem.ToString().Substring(0, autoupdate.IndexOf("（")) + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                    
                }
                else
                {
                    if (serverlist1.SelectedItem.ToString().IndexOf("（") + 1 != 0)
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL";
                        filename = serverlist.SelectedItem.ToString() + "-" + serverlist1.SelectedItem.ToString().Substring(0, serverlist1.SelectedItem.ToString().IndexOf("（")) + ".jar";
                    }
                    else
                    {
                        downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL";
                        filename = serverlist.SelectedItem.ToString() + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                }
                //MessageBox.Show(downloadurl);
                //downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL";
                //filename = serverlist.SelectedItem.ToString() + serverlist1.SelectedItem.ToString() + ".jar";
                downloadinfo = "下载服务端中……";
                Window window = new DownloadWindow();
                var mainwindow = (MainWindow)Window.GetWindow(this);
                window.Owner = mainwindow;
                window.ShowDialog();
                //MessageBox.Show(Url,Filename);
                try
                {
                    MainWindow.serverserver = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL\" + filename + "\"";
                    DownComplete();
                }
                catch
                {
                    DownComplete();
                }
                //}
            }
        }
        void GetServer()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                serverlist.Items.Clear();
            serverlist1.Items.Clear();
            serverurl.Items.Clear();
            serverdownurl.Items.Clear();
            });
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
                        FileStream so1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL/" + autoupdate + ".json", FileMode.Create);
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
                    StreamReader reader1 = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL/paperlist.json");
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
                    FileStream so = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL/serverlist.json", FileMode.Create);
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
                    string aa = Encoding.UTF8.GetString(pageData);
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
                    pageHtml = null;
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
                    //File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL/serverlist.json");
                });
                //timer7.Stop();
            }
        }

        private void serverlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serverlist.Items.Count!=0)
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
                    //StreamReader reader1 = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL/" + serverlist.SelectedItem.ToString() + ".json");
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
                    pageHtml = null;
                }
                catch
                {
                    try
                    {
                        domain = "";
                        //MessageBox.Show(mserversurl);
                        //StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL/serverlist.json");
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
                        pageHtml = null;
                    }
                    catch
                    {
                        MessageBox.Show("获取下载链接失败！");
                    }
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL/serverlist.json"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL/serverlist.json");
            }
            int i = serverlist.Items.Count;
            int x = 0;
            while (x != i)
            {
                string aaa = serverlist.Items[x].ToString();
                x++;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL/" + aaa + ".json"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL/" + aaa + ".json");
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(GetServer);
            thread.Start();
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(GetServer);
            thread.Start();
        }
    }
}
