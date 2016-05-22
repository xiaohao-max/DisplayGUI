using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace DisplayGUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Parallel.Invoke(MainDisplay, LogicalCtrl);
        }
        /// <summary>
        /// GUI显示程序
        /// </summary>
        static void MainDisplay()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Setting());
        }
        /// <summary>
        /// 逻辑控制程序
        /// </summary>
        static void LogicalCtrl()
        {
            while (true)
            {
                /// 检测到拍照指令，根据颜色调用拍照程序
                if (Global.TakePhotos & !Global.DisplayWait)
                {
                    System.IO.FileStream CreateFile = new System.IO.FileStream(Global.activatebin, System.IO.FileMode.Create);
                    CreateFile.Close();
                    Global.TakePhotos = false;
                    Global.DisplayResult = false;
                    Global.DisplayWait = true;
                    Global.SourceComplete = false;
                    Global.FinalComplete = false;
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
                }
                /// 判断三个文件是否齐全
                if (Global.DisplayWait)
                {
                DetectFiles:
                    if (/*System.IO.File.Exists(Global.sourcejpg) & */(!System.IO.File.Exists(Global.finalpng) | !System.IO.File.Exists(Global.outputtxt)) & !Global.DisplayStart)
                    {
                        Console.WriteLine("开始显示");
                        Global.DisplayStart = true;
                        Global.ProgramStep = 1;
                    }
                    else if (/*System.IO.File.Exists(Global.sourcejpg) & */System.IO.File.Exists(Global.finalpng) & System.IO.File.Exists(Global.outputtxt) & System.IO.File.Exists(Global.donebin))
                    {
                        Console.WriteLine("存在");
                        Judge();
                        Global.DisplayResult = true;
                        Global.DisplayWait = false;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1);
                        goto DetectFiles;
                    }
                }
            }
        }



        static void Judge()
        {
            var file = System.IO.File.Open(Global.outputtxt, System.IO.FileMode.Open);
            List<string> txt = new List<string>();
            using (var stream = new System.IO.StreamReader(file))
            {
                while (!stream.EndOfStream)
                {
                    txt.Add(stream.ReadLine());
                }
            }
            file.Close();
            for (int num = 0; num < Global.TotalNum; num++)
            {
                txt[num] = txt[num].Substring(txt[num].IndexOf("_") + 1);
            }
            if (txt[Global.TotalNum] == "0")
                Global.Pos_Neg = false;
            else if (txt[Global.TotalNum] == "180")
                Global.Pos_Neg = true;
            Global.Result = txt;
            int OKsum = 0;
            foreach (string s in Global.Result)
            {
                if (s == "OK")
                    OKsum += 1;
            }
            if (OKsum == 23)
                Global.NG_OK = true;
            else
                Global.NG_OK = false;
        }
    }


    /// <summary>
    /// 全局变量定义
    /// </summary>
    class Global
    {
        public static bool DisplayResult = false;
        public static bool DisplayWait = false;
        public static bool DisplayStart = false;
        public static bool PhotoOK = false;
        public static bool ColorChanged = true;
        public static bool ButtonOK = false;
        public static bool Firstform = true;
        public static bool COMOpen = false;
        public static bool PhotoSingal = false;
        public static bool PhotoSingalLast = false;
        public static bool TakePhotos;
        public static bool Pos_Neg = false; //Postive = false; Negative = true;
        public static bool SourceComplete = false;
        public static bool FinalComplete = false;
        public static byte[] SendInfo = new byte[13] { 0x02, 0x31, 0x30, 0x31, 0x30, 0x31, 0x30, 0x31, 0x30, 0x30, 0x03, 0x42, 0x37 };
        public static bool SendInfoSingal = true;
        public static bool NG_OK = false; //NG = false; OK = true;
        public static string ColorSelected = @"Color.Gold";
        public static string COMSeleted;
        public static int TotalNum = 23;
        public static int RetryMax = 3;
        public static int ProgramStep = 0;
        public static List<string> Result = new List<string>();
        public static string sourcejpg = @"C:\DisplayGUI\source000.jpg";
        public static string finalpng = @"C:\DisplayGUI\final.png";
        public static string outputtxt = @"C:\DisplayGUI\output.txt";
        public static string precisiontxt = @"C:\DisplayGUI\precision.txt";
        public static string goldphoto = @"C:\DisplayGUI\gold.exe";
        public static string grayphoto = @"C:\DisplayGUI\gray.exe";
        public static string silverphoto = @"C:\DisplayGUI\silver.exe";
        public static string activatebin = @"C:\DisplayGUI\activate.bin";
        public static string donebin = @"C:\DisplayGUI\done.bin";
        public static string directoryroot = @"D:\History\";
        public static string PosPrecsion1 = "0.02";
        public static string PosPrecsion2 = "0.02";
        public static string PosPrecsion3 = "0.02";
        public static string PosPrecsion4 = "0.02";
        public static string PosPrecsion5 = "0.02";
        public static string PosPrecsion6 = "0.02";
        public static string PosPrecsion7 = "0.02";
        public static string PosPrecsion8 = "0.02";
        public static string PosPrecsion9 = "0.02";
        public static string PosPrecsion10 = "0.02";
        public static string PosPrecsion11 = "0.02";
        public static string PosPrecsion12 = "0.02";
        public static string PosPrecsion13 = "0.02";
        public static string PosPrecsion14 = "0.02";
        public static string PosPrecsion15 = "0.02";
        public static string PosPrecsion16 = "0.02";
        public static string PosPrecsion17 = "0.02";
        public static string PosPrecsion18 = "0.02";
        public static string PosPrecsion19 = "0.02";
        public static string PosPrecsion20 = "0.02";
        public static string PosPrecsion21 = "0.02";
        public static string PosPrecsion22 = "0.02";
        public static string PosPrecsion23 = "0.02";
    }
}
