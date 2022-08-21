using CurseForge.APIClient.Models.Mods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
    /// DownloadPorM.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadPorM : Window
    {
        string Url;
        string Filename;
        List<uint> modIds = new List<uint>();
        List<string> modVersionurl = new List<string>();
        List<string> modUrls = new List<string>();
        public DownloadPorM()
        {
            InitializeComponent();
        }

        private async void getFeature_Click(object sender, RoutedEventArgs e)
        {
            loadingRing.IsActive = true;
            lb01.Visibility = Visibility.Visible;
            var cfApiClient = new CurseForge.APIClient.ApiClient("$2a$10$GMZXi.aieoL.ul.UJK6WYuTOBa.jzrcfZEWRD0Lff7ekk/4i4i.2K", "2035582067@qq.com");
            var featuredMods = await cfApiClient.GetFeaturedModsAsync(new GetFeaturedModsRequestBody
            {
                GameId = 432,
                ExcludedModIds = new List<uint>(),
                GameVersionTypeId = null
            });
            listBox.Items.Clear();
            modIds.Clear();
            modVersionurl.Clear();
            modUrls.Clear();
            for (int i=0; i< featuredMods.Data.Popular.Count; i++)
            {
                //listBox.Items.Add(featuredMods.Data.Popular[i].Name);
                try
                {
                    if (featuredMods.Data.Popular[i].Name.IndexOf("(") + 1 != 0)
                    {
                        string aaa = featuredMods.Data.Popular[i].Name.Substring(0, featuredMods.Data.Popular[0].Name.IndexOf("("));
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold="));

                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold=");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));

                        listBox.Items.Add(featuredMods.Data.Popular[i].Name + "(" + a + ")");
                    }
                    else
                    {
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + featuredMods.Data.Popular[i].Name + "&filter=1&mold="));
                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + featuredMods.Data.Popular[i].Name + "&filter=1&mold=");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));
                        listBox.Items.Add(featuredMods.Data.Popular[i].Name+"("+a+")");
                    }
                }
                catch
                {
                    listBox.Items.Add(featuredMods.Data.Popular[i].Name);
                }
                modIds.Add(featuredMods.Data.Popular[i].Id);
                //fileIds.Add(featuredMods.Data.Popular[i].MainFileId);
                modUrls.Add(featuredMods.Data.Popular[i].Links.WebsiteUrl.ToString());
            }
            loadingRing.IsActive = false;
            lb01.Visibility = Visibility.Hidden;
        }
        
        private async void searchMod_Click(object sender, RoutedEventArgs e)
        {
            loadingRing.IsActive = true;
            lb01.Visibility = Visibility.Visible;
            var cfApiClient = new CurseForge.APIClient.ApiClient("$2a$10$GMZXi.aieoL.ul.UJK6WYuTOBa.jzrcfZEWRD0Lff7ekk/4i4i.2K", "2035582067@qq.com");
            var searchedMods = await cfApiClient.SearchModsAsync(432, null, null,null, textBox1.Text);
            listBox.Items.Clear();
            modIds.Clear();
            modVersionurl.Clear();
            modUrls.Clear();
            for (int i = 0; i < searchedMods.Data.Count; i++)
            {
                //listBox.Items.Add(searchedMods.Data[i].Name);
                try
                {
                    if (searchedMods.Data[i].Name.IndexOf("(") + 1 != 0)
                    {
                        string aaa = searchedMods.Data[i].Name.Substring(0, searchedMods.Data[i].Name.IndexOf("("));
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold="));

                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold=");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));

                        listBox.Items.Add(searchedMods.Data[i].Name + "(" + a + ")");
                    }
                    else
                    {
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + featuredMods.Data.Popular[i].Name + "&filter=1&mold="));
                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + searchedMods.Data[i].Name + "&filter=1&mold=");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));
                        listBox.Items.Add(searchedMods.Data[i].Name + "(" + a + ")");
                    }
                }
                catch
                {
                    listBox.Items.Add(searchedMods.Data[i].Name);
                }

                modIds.Add(searchedMods.Data[i].Id);
                //fileIds.Add(searchedMods.Data[i].MainFileId);
                modUrls.Add(searchedMods.Data[i].Links.WebsiteUrl.ToString());
            }
            //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + searchedMods.Data[0].Name + "&filter=1&mold="));
            loadingRing.IsActive = false;
            lb01.Visibility = Visibility.Hidden;
        }

        private async void downloadMod_Click(object sender, RoutedEventArgs e)
        {
            loadingRing.IsActive = true;
            lb01.Visibility = Visibility.Visible;
            var cfApiClient = new CurseForge.APIClient.ApiClient("$2a$10$GMZXi.aieoL.ul.UJK6WYuTOBa.jzrcfZEWRD0Lff7ekk/4i4i.2K", "2035582067@qq.com");
            label1.Content = "找不到服务器根目录中的mods文件夹，将下载至MGSL文件夹中";
            try
            {
                modVersions.Items.Clear();
                var modFiles = await cfApiClient.GetModFilesAsync(modIds[listBox.SelectedIndex]);
                for (int i = 0; i < modFiles.Data.Count; i++)
                {
                    modVersions.Items.Add(modFiles.Data[i].DisplayName);
                    modVersionurl.Add(modFiles.Data[i].DownloadUrl);
                }
                //var modFileDownloadUrl = await cfApiClient.GetModFileDownloadUrlAsync(modIds[listBox.SelectedIndex], fileIds[listBox.SelectedIndex]);
                
                /*DownloadWindow.downloadurl = modFileDownloadUrl.Data.ToString();
                DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + "MGSL";
                DownloadWindow.filename = listBox.SelectedItem.ToString()+".jar";
                DownloadWindow.downloadinfo = "下载"+ listBox.SelectedItem.ToString()+"中……";
                Window window = new DownloadWindow();
                window.Owner = this;
                window.ShowDialog();*/
            }
            catch { }
            loadingRing.IsActive = false;
            lb01.Visibility = Visibility.Hidden;
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //MessageBox.Show(modUrls[listBox.SelectedIndex]);
                Process.Start(modUrls[listBox.SelectedIndex]);
            }
            catch { }
        }

        private void modVersions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(modVersionurl[modVersions.SelectedIndex]);
            if (Directory.Exists(MainWindow.serverbase + @"\mods"))
            {
                Filename = MainWindow.serverbase + @"\mods" + modVersions.SelectedItem.ToString();
            }
            else
            {
                Filename = AppDomain.CurrentDomain.BaseDirectory + @"MGSL\" + modVersions.SelectedItem.ToString();
            }
            Url = modVersionurl[modVersions.SelectedIndex];
            //Process.Start(modVersionurl[modVersions.SelectedIndex]);
            Thread thread = new Thread(DownloadFile);
            thread.Start();
        }
        public string GetHtml(string Url)
        {
            string datas = "";
            //远程访问
            WebRequest request = WebRequest.Create(Url);
            request.Method = "GET";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader;
            //抓取的网页是压缩状态
            if (response.ContentEncoding.ToLower() == "gzip")
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        reader = new StreamReader(zipStream, Encoding.GetEncoding("UTF-8"));
                        datas = reader.ReadToEnd();
                    }
                }
            }
            else
            {
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                datas = reader.ReadToEnd();
            }
            return datas;
        }

        void DownloadFile()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                //serverlist3.IsEnabled = false;
                //serverlist4.IsEnabled = false;
                //serverlist5.IsEnabled = false;
                label1.Content = "连接下载地址中...";
            });
            try
            {
                HttpWebRequest Myrq = (HttpWebRequest)HttpWebRequest.Create(Url);
                HttpWebResponse myrp;
                myrp = (HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                Stream st = myrp.GetResponseStream();
                FileStream so = new FileStream(Filename, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    if (pbar != null)
                    {
                        pbar.Maximum = (int)totalBytes;
                    }
                });
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    DispatcherHelper.DoEvents();
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                    float percent = 0;
                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        if (pbar != null)
                        {
                            pbar.Value = (int)totalDownloadedByte;
                        }
                        label1.Content = "下载中，进度" + percent.ToString("f2") + "%";
                    });
                    DispatcherHelper.DoEvents();
                }
                so.Close();
                st.Close();
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    label1.Content = "下载成功";
                    //serverlist3.IsEnabled = true;
                    //serverlist4.IsEnabled = true;
                    //serverlist5.IsEnabled = true;
                });
            }
            catch(Exception ex)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MessageBox.Show(ex.Message);
                    label1.Content = "发生错误，请重试:" + ex;
                });
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadingRing.IsActive = true;
            lb01.Visibility = Visibility.Visible;
            var cfApiClient = new CurseForge.APIClient.ApiClient("$2a$10$GMZXi.aieoL.ul.UJK6WYuTOBa.jzrcfZEWRD0Lff7ekk/4i4i.2K", "2035582067@qq.com");
            var featuredMods = await cfApiClient.GetFeaturedModsAsync(new GetFeaturedModsRequestBody
            {
                GameId = 432,
                ExcludedModIds = new List<uint>(),
                GameVersionTypeId = null
            });
            listBox.Items.Clear();
            modIds.Clear();
            modVersionurl.Clear();
            modUrls.Clear();
            for (int i = 0; i < featuredMods.Data.Popular.Count; i++)
            {
                //listBox.Items.Add(featuredMods.Data.Popular[i].Name);
                try
                {
                    if (featuredMods.Data.Popular[i].Name.IndexOf("(") + 1 != 0)
                    {
                        string aaa = featuredMods.Data.Popular[i].Name.Substring(0, featuredMods.Data.Popular[0].Name.IndexOf("("));
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold="));

                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold=");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));

                        listBox.Items.Add(featuredMods.Data.Popular[i].Name + "(" + a + ")");
                    }
                    else
                    {
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + featuredMods.Data.Popular[i].Name + "&filter=1&mold="));
                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + featuredMods.Data.Popular[i].Name + "&filter=1&mold=");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));
                        listBox.Items.Add(featuredMods.Data.Popular[i].Name + "(" + a + ")");
                    }
                }
                catch
                {
                    listBox.Items.Add(featuredMods.Data.Popular[i].Name);
                }
                modIds.Add(featuredMods.Data.Popular[i].Id);
                //fileIds.Add(featuredMods.Data.Popular[i].MainFileId);
                modUrls.Add(featuredMods.Data.Popular[i].Links.WebsiteUrl.ToString());
            }
            loadingRing.IsActive = false;
            lb01.Visibility = Visibility.Hidden;
        }
    }
}
