using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Updater;

namespace DllExample
{
    public class Class1
    {
        public void Main(string[] args)
        {
            // check update
            UpdateCheck();
            // do sth
        }
        private void UpdateCheck()
        {
            Version now = new Version("1.1.1.1");
            // web这一步可以视情况，可以选择正则表达式或者json从服务器获取最新版本号
            Version web = new Version("1.2.2.2");
            // 如果有新版本
            if (web > now)
            {
                StartProcess(@".\Updater.exe", "https://ghproxy.com/https://github.com/xiaohuihui1022/TheWorstEngine/releases/download/1.4/TWEngine.zip", "FormExample.zip");
                // 退出程序，可以让updater覆盖文件
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 启动exe函数
        /// </summary>
        /// <param name="runFilePath"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool StartProcess(string runFilePath, params string[] args)
        {
            string s = "";
            foreach (string arg in args)
            {
                s = s + arg + " ";
            }
            s = s.Trim();
            Process process = new Process();//创建进程对象    
            ProcessStartInfo startInfo = new ProcessStartInfo(runFilePath, s); // 括号里是(程序名,参数)
            process.StartInfo = startInfo;
            process.Start();
            return true;
        }
    }
}
