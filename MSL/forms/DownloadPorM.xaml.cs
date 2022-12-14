using CurseForge.APIClient.Models.Mods;
using MSL.controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static MSL.DownloadWindow;
using MessageBox = System.Windows.MessageBox;

namespace MSL
{
    /// <summary>
    /// DownloadPorM.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadPorM : Window
    {
        string Url;
        string Filename;
        List<int> modIds = new List<int>();
        List<string> modVersions = new List<string>();
        List<string> modVersionurl = new List<string>();
        List<string> modUrls = new List<string>();
        List<string> imageUrls = new List<string>();
        List<string> backList = new List<string>();
        public DownloadPorM()
        {
            InitializeComponent();
        }
        class MODsInfo
        {
            public string Icon { set; get; }
            public string State { set; get; }

            public MODsInfo(string icon, string state)
            {
                this.Icon = icon;
                this.State = state;
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lCircle.Visibility = Visibility.Visible;
            lb01.Visibility = Visibility.Visible;
            var cfApiClient = new CurseForge.APIClient.ApiClient("", "");
            var featuredMods = await cfApiClient.GetFeaturedModsAsync(new GetFeaturedModsRequestBody
            {
                GameId = 432,
                ExcludedModIds = new List<int>(),
                GameVersionTypeId = null
            });
            imageUrls.Clear();
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

                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold=0");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));

                        a = a.Replace("<em>", "");
                        a = a.Replace("</em>", "");

                        listBox.Items.Add(new MODsInfo(featuredMods.Data.Popular[i].Logo.ThumbnailUrl, featuredMods.Data.Popular[i].Name + "(" + a + ")"));
                        imageUrls.Add(featuredMods.Data.Popular[i].Logo.ThumbnailUrl);
                        backList.Add(featuredMods.Data.Popular[i].Name + "(" + a + ")");
                    }
                    else
                    {
                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + featuredMods.Data.Popular[i].Name + "&filter=1&mold=0");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));

                        a = a.Replace("<em>", "");
                        a = a.Replace("</em>", "");

                        listBox.Items.Add(new MODsInfo(featuredMods.Data.Popular[i].Logo.ThumbnailUrl, featuredMods.Data.Popular[i].Name + "(" + a + ")"));
                        imageUrls.Add(featuredMods.Data.Popular[i].Logo.ThumbnailUrl);
                        backList.Add(featuredMods.Data.Popular[i].Name + "(" + a + ")");
                    }
                }
                catch
                {
                    listBox.Items.Add(new MODsInfo(featuredMods.Data.Popular[i].Logo.ThumbnailUrl, featuredMods.Data.Popular[i].Name));
                    imageUrls.Add(featuredMods.Data.Popular[i].Logo.ThumbnailUrl);
                    backList.Add(featuredMods.Data.Popular[i].Name);
                    //listBox.Items.Add(featuredMods.Data.Popular[i].Name);
                }
                modIds.Add(featuredMods.Data.Popular[i].Id);
                modUrls.Add(featuredMods.Data.Popular[i].Links.WebsiteUrl.ToString());
            }
            //listBox. = modAssets;
            lCircle.Visibility = Visibility.Hidden;
            lb01.Visibility = Visibility.Hidden;
            backBtn.IsEnabled = false;
            listBoxColumnName.Header = "模组列表（双击获取该模组的版本）：";
        }
        private async void searchMod_Click(object sender, RoutedEventArgs e)
        {
            lCircle.Visibility = Visibility.Visible;
            lb01.Visibility = Visibility.Visible;
            if (Regex.IsMatch(textBox1.Text, @"[\u4e00-\u9fa5]"))
            {
                MessageDialogShow.Show("检测到您输入了中文，为确保搜索准确性，是否让开服器将其自动翻译为英文？", "提示", true, "确定", "取消");
                MessageDialog messageDialog = new MessageDialog();
                messageDialog.Owner = this;
                messageDialog.ShowDialog();
                if (MessageDialog._dialogReturn == true)
                {
                    MessageDialog._dialogReturn = false;
                    string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + textBox1.Text + "&filter=1&mold=0");
                    int IndexofA = pageHtml.IndexOf("耗时:");
                    string Ru = pageHtml.Substring(IndexofA);
                    string aa = Ru.Substring(Ru.IndexOf("</em>"), Ru.IndexOf("</a></div>"));

                    textBox1.Text = aa.Substring(aa.IndexOf("(") + 1, aa.IndexOf(")")- (aa.IndexOf("(") + 1));
                    textBox1.Text=textBox1.Text.Replace("<em>", "");
                    textBox1.Text=textBox1.Text.Replace("</em>", "");
                }
            }
            
            var cfApiClient = new CurseForge.APIClient.ApiClient("", "");
            var searchedMods = await cfApiClient.SearchModsAsync(432, null, null,null, textBox1.Text);
            listBox.Items.Clear();
            modIds.Clear();
            modVersionurl.Clear();
            modUrls.Clear();
            imageUrls.Clear();
            for (int i = 0; i < searchedMods.Data.Count; i++)
            {
                //listBox.Items.Add(searchedMods.Data[i].Name);

                try
                {
                    if (searchedMods.Data[i].Name.IndexOf("(") + 1 != 0)
                    {
                        string aaa = searchedMods.Data[i].Name.Substring(0, searchedMods.Data[i].Name.IndexOf("("));
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold="));

                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + aaa + "&filter=1&mold=0");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));

                        a=a.Replace("<em>", "");
                        a=a.Replace("</em>", "");

                        //listBox.ItemsSource = (System.Collections.IEnumerable)searchedMods.Data[i].Logo;
                        //listBox.Items.Add(searchedMods.Data[i].Name + "(" + a + ")");
                        listBox.Items.Add(new MODsInfo(searchedMods.Data[i].Logo.ThumbnailUrl, searchedMods.Data[i].Name + "(" + a + ")"));
                        imageUrls.Add(searchedMods.Data[i].Logo.ThumbnailUrl);
                        backList.Add(searchedMods.Data[i].Name + "(" + a + ")");
                    }
                    else
                    {
                        //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + featuredMods.Data.Popular[i].Name + "&filter=1&mold="));
                        string pageHtml = GetHtml("https://search.mcmod.cn/s?key=" + searchedMods.Data[i].Name + "&filter=1&mold=0");
                        int IndexofA = pageHtml.IndexOf("耗时:");
                        string Ru = pageHtml.Substring(IndexofA);
                        string aa = Ru.Substring(0, Ru.IndexOf("</div><div class=\"foot\">"));


                        int IndexofB = aa.IndexOf("html\">");
                        string Ru2 = aa.Substring(IndexofB + 6);
                        string a = Ru2.Substring(0, Ru2.IndexOf(" ("));

                        a = a.Replace("<em>", "");
                        a = a.Replace("</em>", "");

                        listBox.Items.Add(new MODsInfo(searchedMods.Data[i].Logo.ThumbnailUrl, searchedMods.Data[i].Name + "(" + a + ")"));
                        imageUrls.Add(searchedMods.Data[i].Logo.ThumbnailUrl);
                        backList.Add(searchedMods.Data[i].Name + "(" + a + ")");
                        
                    }
                    
                }
                catch
                {
                    listBox.Items.Add(new MODsInfo(searchedMods.Data[i].Logo.ThumbnailUrl, searchedMods.Data[i].Name));
                    imageUrls.Add(searchedMods.Data[i].Logo.ThumbnailUrl);
                    backList.Add(searchedMods.Data[i].Name);
                    //listBox.Items.Add(searchedMods.Data[i].Name);
                }

                modIds.Add(searchedMods.Data[i].Id);
                //fileIds.Add(searchedMods.Data[i].MainFileId);
                modUrls.Add(searchedMods.Data[i].Links.WebsiteUrl.ToString());
            }
            //MessageBox.Show(GetHtml("https://search.mcmod.cn/s?key=" + searchedMods.Data[0].Name + "&filter=1&mold="));
            lCircle.Visibility = Visibility.Hidden;
            lb01.Visibility = Visibility.Hidden;
            backBtn.IsEnabled = false;
            listBoxColumnName.Header = "模组列表（双击获取该模组的版本）：";
        }
        private async void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (listBoxColumnName.Header.ToString() == "模组列表（双击获取该模组的版本）：")
                {
                    string imageurl = imageUrls[listBox.SelectedIndex];
                    //MessageBox.Show(imageurl);
                    backBtn.IsEnabled = true;
                    lCircle.Visibility = Visibility.Visible;
                    lb01.Visibility = Visibility.Visible;
                    try
                    {
                        var cfApiClient = new CurseForge.APIClient.ApiClient("", "");
                        var modFiles = await cfApiClient.GetModFilesAsync(modIds[listBox.SelectedIndex]);
                        listBox.Items.Clear();
                        modVersions.Clear();

                        for (int i = 0; i < modFiles.Data.Count; i++)
                        {
                            listBox.Items.Add(new MODsInfo(imageurl, modFiles.Data[i].DisplayName));
                            modVersions.Add(modFiles.Data[i].DisplayName);
                            modVersionurl.Add(modFiles.Data[i].DownloadUrl);
                        }
                    }
                    catch { }
                    lCircle.Visibility = Visibility.Hidden;
                    lb01.Visibility = Visibility.Hidden;
                    listBoxColumnName.Header = "模组版本列表（双击下载）：";
                }
                else
                {
                    if (Directory.Exists(MainWindow.serverbase + @"\mods"))
                    {
                        Filename = MainWindow.serverbase + @"\mods" + modVersions[listBox.SelectedIndex].ToString();
                    }
                    else
                    {
                        FolderBrowserDialog dialog = new FolderBrowserDialog();
                        dialog.Description = "请选择MOD存放文件夹";
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Filename = dialog.SelectedPath + @"\" + modVersions[listBox.SelectedIndex].ToString();
                        }
                    }
                    Url = modVersionurl[listBox.SelectedIndex];
                    //Process.Start(modVersionurl[modVersions.SelectedIndex]);
                    Thread thread = new Thread(DownloadFile);
                    thread.Start();
                }
            }
            catch
            {
                return;
            }
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
                    if (Directory.Exists(MainWindow.serverbase + @"\mods"))
                    {
                        label1.Content = "下载成功，模组已存放至该服务器的mods文件夹中";
                    }
                    else
                    {
                        label1.Content = "下载成功";
                    }
                    
                });
            }
            catch(Exception ex)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    label1.Content = "发生错误，请重试:" + ex;
                });
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
            for(int i = 0; i < backList.Count; i++)
            {
                listBox.Items.Add(new MODsInfo(imageUrls[i],backList[i]));
            }
            listBoxColumnName.Header = "模组列表（双击获取该模组的版本）：";
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listBox.Items.Clear();
            modIds.Clear();
            modVersions.Clear();
            modVersionurl.Clear();
            modUrls.Clear();
            imageUrls.Clear();
            backList.Clear();
            GC.Collect();
        }
    }
}
