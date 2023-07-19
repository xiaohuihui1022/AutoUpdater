using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;
using static System.Net.Mime.MediaTypeNames;
using System.Net;

namespace Updater
{
    public partial class UpdatingProgress : Form
    {
        public UpdatingProgress()
        {
            InitializeComponent();
        }

        public void SetLable(string msg)
        {
            if (this.label1.InvokeRequired)
                this.label1.Invoke(new Action<string>(SetLable), msg);
            else
                this.label1.Text = msg;
        }

        public void SetProgress(int value)
        {
            if (this.progress.InvokeRequired)
                this.label1.Invoke(new Action<int>(SetProgress), value);
            else
                this.progress.Value = value < 0 ? 0 : value > 100 ? 100 : value;

            // if (value == 100) this.Invoke(new Action(this.Hide));

            this.Refresh();
        }

        public void AddStatus(string text)
        {
            if (status.Text.Equals(""))
            {
                status.Text = text + "\r\n";
            }
            else
            {
                status.Text += text + "\r\n";
            }

        }

        public void ClearStatus()
        {
            status.Text = "";
        }

        // 下载文件
        public void Download(string StrUrl, string StrFileName)
        {
            Console.WriteLine("Downloading");
            AddStatus("正在从: " + StrUrl + " 获取文件");
            FileStream fileStream;
            fileStream = new FileStream(StrFileName, FileMode.Create);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(StrUrl);
                WebResponse response = httpWebRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                long contentLength = response.ContentLength;
                long num2 = 0L;
                byte[] array = new byte[512];
                int i = responseStream.Read(array, 0, array.Length);
                while (i > 0)
                {
                    fileStream.Write(array, 0, i);
                    i = responseStream.Read(array, 0, 512);
                    num2 += i;
                    SetLable("下载进度：" + (num2 * 100.0 / contentLength).ToString("0.00") + "%");
                    SetProgress((int)(num2 * 100.0 / contentLength));
                    System.Windows.Forms.Application.DoEvents();
                }
                fileStream.Close();
                responseStream.Close();
                SetLable("下载完成");
                SetProgress(0);
                UnZipFile(StrFileName);
            }
            catch (Exception ex)
            {
                fileStream.Close();
                AddStatus("下载过程中出现错误:\n" + ex.ToString());
            }
        }

        public void UnZipFile(string FileName)
        {
            // config里修改lib目录后这里也要修改！
            // config里修改lib目录后这里也要修改！
            // config里修改lib目录后这里也要修改！
            if (IntPtr.Size == 4)
            {
                SevenZipBase.SetLibraryPath(AppDomain.CurrentDomain.BaseDirectory + @"Updater_lib\7z.dll");
            }
            else
            {
                SevenZipBase.SetLibraryPath(AppDomain.CurrentDomain.BaseDirectory + @"Updater_lib\7z64.dll");
            }
            // 预处理文件
            var extractor = new SevenZipExtractor(FileName);
            progress.Maximum = extractor.ArchiveFileData.Count;
            extractor.FileExtractionStarted += new EventHandler<FileInfoEventArgs>(extr_FileExtractionStarted);
            // 软件本体目录，一般不需要改，除非你的Updater和软件本体不在同一目录下
            extractor.BeginExtractArchive(".\\");
        }

        private void extr_FileExtractionStarted(object sender, FileInfoEventArgs e)
        {
            SetLable(string.Format("正在更新 {0}", progress.Value));
            AddStatus(string.Format("更新文件: {0}", e.FileInfo.FileName));
            progress.Value += 1;
            if (progress.Maximum == progress.Value) 
            {
                SetLable("更新完成");
                MessageBox.Show("更新完成，请重启软件以应用更新。", "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                File.Delete(e.FileInfo.FileName);
                Environment.Exit(0);
            }
        }

        private void UpdatingProgress_Shown(object sender, EventArgs e)
        {
            Download(UpdaterInit.Url, UpdaterInit.FileName);
        }
    }
}
