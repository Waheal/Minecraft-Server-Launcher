using Gac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MSL
{
    /// <summary>
    /// DownloadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadWindow : Window
    {
        DownLoadFile dlf = new DownLoadFile();
        public static int downloadthread = 32;
        public static string downloadinfo;
        public static string downloadPath;
        public static string filename;
        public static string downloadurl;
        DispatcherTimer timer1 = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        //static Thread thread;
        public DownloadWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dlf.DownLoadThreadNum = downloadthread;//下载线程数，不设置默认为3
            dlf.doSendMsg += SendMsgHander;//下载过程处理事件
            dlf.AddDown(downloadurl, downloadPath, 0, filename);
            dlf.StartDown();
            taskinfo.Content = downloadinfo;
            infolabel.Text = downloadinfo;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Start();
            /*
            thread = new Thread(Downloader);
            thread.Start();*/
        }

        private void SendMsgHander(DownMsg msg)
        {
            switch (msg.Tag)
            {
                case DownStatus.Start:
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        infolabel.Text = "获取下载地址……大小：" + msg.LengthInfo;
                    });
                    break;
                case DownStatus.End:
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        pbar.Value = 100;
                        infolabel.Text = "下载完成！";
                    });
                    break;
                case DownStatus.DownLoad:
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        if (infolabel.Text != "停止下载中，请耐心等待……双击取消按钮可强制关闭此窗口")
                        {
                            infolabel.Text = "已下载：" + msg.SizeInfo + " 进度：" + msg.Progress.ToString() + "%" + " 速度：" + msg.SpeedInfo + " 剩余时间：" + msg.SurplusInfo;
                            /*
                            if (msg.Progress < 0)
                            {
                                System.Windows.MessageBox.Show("err0");
                                dlf.StopDown();
                                Thread thread = new Thread(Downloadfile);
                                thread.Start();
                            }
                            */
                        }
                        pbar.Value = msg.Progress;
                    });
                    System.Windows.Forms.Application.DoEvents();
                    break;
                case DownStatus.Error:
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        infolabel.Text = "失败：" + msg.ErrMessage;
                        System.Windows.Forms.Application.DoEvents();
                        //thread = new Thread(Downloadfile);
                        //thread.Start();
                    });
                    break;
            }
        }

        public static class DispatcherHelper
        {
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static void DoEvents()
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
                try { Dispatcher.PushFrame(frame); }
                catch (InvalidOperationException) { }
            }
            private static object ExitFrames(object frame)
            {
                ((DispatcherFrame)frame).Continue = false;
                return null;
            }
        }
        void timer1_Tick(object sender, EventArgs e)
        {
            if (infolabel.Text == "下载完成！")
            {
                downloadinfo = null;
                downloadurl = null;
                timer1.Stop();
                Close();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            dlf.StopDown();

            infolabel.Text = "停止下载中，请耐心等待……双击取消按钮可强制关闭此窗口";
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = TimeSpan.FromSeconds(1);
            timer2.Start();
        }

        void timer2_Tick(object sender, EventArgs e)
        {
            if (File.Exists(downloadPath + @"\" + filename))
            {
                try
                {
                    infolabel.Text = "停止下载中，请耐心等待……双击取消按钮可强制关闭此窗口";
                    File.Delete(downloadPath + @"\" + filename);
                }
                catch
                { }
            }
            else
            {
                downloadinfo = null;
                downloadurl = null;
                timer1.Stop();
                timer2.Stop();
                Close();
            }
        }

        private void mainBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
    /*
    private void Downloadfile()
    {
        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        {
            infolabel.Text = "连接下载地址中...";
        });
        //try
        //{
        HttpWebRequest Myrq = (HttpWebRequest)HttpWebRequest.Create(downloadurl);
        HttpWebResponse myrp;
        myrp = (HttpWebResponse)Myrq.GetResponse();
        long totalBytes = myrp.ContentLength;
        Stream st = myrp.GetResponseStream();
        FileStream so;
        System.Windows.MessageBox.Show(downloadPath);
        if (downloadPath.Substring(downloadPath.Length - 2, 1) == "\\")
        {
            so = new FileStream(downloadPath + filename, FileMode.Create);
        }
        else
        {
            so = new FileStream(downloadPath + "\\" + filename, FileMode.Create);
        }
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
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                infolabel.Text = "下载中：" + percent.ToString("f2") + "%";
                pbar.Value = percent;
            });
            DispatcherHelper.DoEvents();
        }
        so.Close();
        st.Close();
        System.Windows.Forms.MessageBox.Show("ok");
        this.Close();

        //}
        //catch (Exception aaa)
        //{
        //   this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
        //   {
        //      infolabel.Text = "发生错误" + aaa;
        //   });
        //}
    }*/
}
