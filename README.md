# Updater



## 一个可以让你更新软件的程序（支持UI界面）

# 使用方法：
            

```c#
namespace FormExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 检查更新
            UpdateCheck();
        }

        // 检查Update
        private void UpdateCheck()
        {
            Version now = new Version("1.1.1.1");
            // web这一步可以视情况，可以选择正则表达式或者json从服务器获取最新版本号
            Version web = new Version("1.2.2.2");
            // 如果有新版本
            if (web > now)
            {
                StartProcess(@".\Updater.exe", "https://ghproxy.com/https://github.com/xiaohuihui1022/TheWorstEngine/releases/download/1.4/TWEngine.zip",  "FormExample.zip");
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
```

## TODOs:

- [x] 异步下载
- [x] UI绘制
- [x] 压缩包解压
- [x] 替换文件
- [ ] 自身更新(预计需要跑脚本)