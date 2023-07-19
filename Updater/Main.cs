using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SevenZip;

namespace Updater
{
    public class UpdaterInit
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                // 主类，一般用户直接运行的话报错就行
                Console.WriteLine("请不要尝试双击本程序");
            }
            else
            {
                Url = args[0];
                FileName = Path.GetTempPath() + "\\" + args[1];
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new UpdatingProgress());
            }
        }
        /// <summary>
        /// 必须是直连下载的URL
        /// </summary>
        public static string Url { get; set; }
        /// <summary>
        /// 用于缓存文件的文件名，一般填写自己软件名字和压缩格式就可以，例如AutoUpdater.zip
        /// </summary>
        public static string FileName { get; set; }

    }
}
