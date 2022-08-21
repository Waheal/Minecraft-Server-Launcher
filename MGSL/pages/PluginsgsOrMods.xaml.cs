using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    /// PluginsgsOrMods.xaml 的交互逻辑
    /// </summary>
    public partial class PluginsgsOrMods : Page
    {
        public PluginsgsOrMods()
        {
            InitializeComponent();
        }

        private void grid6_Loaded(object sender, RoutedEventArgs e)
        {
            ReFresh();
        }
        void ReFresh()
        {
            if (Directory.Exists(MainWindow.serverbase + @"\plugins"))
            {
                pluginslist.Items.Clear();
                DirectoryInfo directoryInfo = new DirectoryInfo(MainWindow.serverbase + @"\plugins");
                FileInfo[] file = directoryInfo.GetFiles("*.jar");
                foreach (FileInfo f in file)
                {
                    pluginslist.Items.Add(f.Name);
                }
                if (Directory.Exists(MainWindow.serverbase + @"\mods"))
                {
                    lab001.Content = "已检测到plugins和mods文件夹，以下为您的插件及模组";
                    openpluginsDir.IsEnabled = true;
                    openmodsDir.IsEnabled = true;
                    addPlugin.IsEnabled = true;
                    addMod.IsEnabled = true;
                    delPlugin.IsEnabled = true;
                    delMod.IsEnabled = true;
                    modslist.Items.Clear();
                    DirectoryInfo directoryInfo1 = new DirectoryInfo(MainWindow.serverbase + @"\mods");
                    FileInfo[] file1 = directoryInfo1.GetFiles("*.jar");
                    foreach (FileInfo f1 in file1)
                    {
                        modslist.Items.Add(f1.Name);
                    }
                }
                else
                {
                    modslist.Items.Clear();
                    lab001.Content = "已检测到plugins文件夹，以下为您的插件";
                    openpluginsDir.IsEnabled = true;
                    openmodsDir.IsEnabled = false;
                    addPlugin.IsEnabled = true;
                    addMod.IsEnabled = false;
                    delPlugin.IsEnabled = true;
                    delMod.IsEnabled = false;
                }
            }
            else
            {
                if (Directory.Exists(MainWindow.serverbase + @"\mods"))
                {
                    pluginslist.Items.Clear();
                    lab001.Content = "已检测到mods文件夹，以下为您的模组";
                    openmodsDir.IsEnabled = true;
                    addMod.IsEnabled = true;
                    modslist.Items.Clear();
                    DirectoryInfo directoryInfo = new DirectoryInfo(MainWindow.serverbase + @"\mods");
                    FileInfo[] file = directoryInfo.GetFiles("*.jar");
                    foreach (FileInfo f in file)
                    {
                        modslist.Items.Add(f.Name);
                    }
                    openpluginsDir.IsEnabled = false;
                    addPlugin.IsEnabled = false;
                }
                else
                {
                    lab001.Content = "未检测到plugins文件夹及mods文件夹，请重启服务器或检查服务端是否支持插件模组";
                    openpluginsDir.IsEnabled = false;
                    openmodsDir.IsEnabled = false;
                    addPlugin.IsEnabled = false;
                    addMod.IsEnabled = false;
                    delPlugin.IsEnabled = false;
                    delMod.IsEnabled = false;
                }
            }
        }
        private void openpluginsDir_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "explorer.exe";
            p.StartInfo.Arguments = MainWindow.serverbase + @"\plugins";
            p.Start();
        }

        private void openmodsDir_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "explorer.exe";
            p.StartInfo.Arguments = MainWindow.serverbase + @"\mods";
            p.Start();
        }
        private void reFresh_Click(object sender, RoutedEventArgs e)
        {
            ReFresh();
        }

        private void addPlugin_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件";
            openfile.Filter = "JAR文件|*.jar|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                File.Copy(openfile.FileName, MainWindow.serverbase + @"\plugins");
                ReFresh();
            }
        }

        private void addMod_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件";
            openfile.Filter = "JAR文件|*.jar|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                File.Copy(openfile.FileName, MainWindow.serverbase + @"\mods");
                ReFresh();
            }
        }
        private void delPlugin_Click(object sender, RoutedEventArgs e)
        {
            if (pluginslist.SelectedIndex!= -1)
            {
                File.Delete(MainWindow.serverbase + @"\plugins\" + pluginslist.SelectedItem.ToString());
                ReFresh();
            }
        }

        private void delMod_Click(object sender, RoutedEventArgs e)
        {
            if (modslist.SelectedIndex != -1)
            {
                File.Delete(MainWindow.serverbase + @"\mods\" + modslist.SelectedItem.ToString());
                ReFresh();
            }
        }
        private void openspigot_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.spigotmc.org/");
        }

        private void opencurseforge_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("https://www.curseforge.com/");
            DownloadPorM downloadPorM = new DownloadPorM();
            downloadPorM.ShowDialog();
        }
    }
}
