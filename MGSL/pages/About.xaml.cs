using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MGSL.pages
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
        }
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://msdoc.nstarmc.cn/");
        }
        private void openSource_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/Waheal/MGSL");
        }

        private void support_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://afdian.net/@makabaka123?tab=sponsor");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData = MyWebClient.DownloadData(MainWindow.serverLink +@"/web/supportlist.txt");
                supportlist.Text = Encoding.UTF8.GetString(pageData);
            }
            catch
            {
                supportlist.Text = "获取赞助名单失败！";
            }*/
        }
    }
}
