using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace DisplayGUI
{
    public partial class DisplayGUI : Form
    {
        public DisplayGUI()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync(); //流程处理
            backgroundWorker2.RunWorkerAsync(); //串口通讯   
            backgroundWorker3.RunWorkerAsync(); //IO与界面元素变化
        }

        private delegate void ListViewDelegate(string detail);

        private void ListView(string detail)
        {
            if (this.listView1.InvokeRequired == false)
            {
                for (int i = 0; i < Global.TotalNum; i++)
                {
                    this.listView1.Items[i].SubItems[1].Text = Global.Result[i];
                    if (Global.Result[i] == "OK")
                    {
                        this.listView1.Items[i].SubItems[1].BackColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        this.listView1.Items[i].SubItems[1].BackColor = System.Drawing.Color.Red;
                    }
                }
            }
            else
            {
                ListViewDelegate LViewD = new ListViewDelegate(ListView);
                this.listView1.Invoke(LViewD, detail);
            }
            
        }

        private void Thread_ListView()
        {
            for (int i = 0; i < Global.TotalNum; i++)
            {
                ListView(Global.Result[i]);
            }
        }
        
        private delegate void LabelDelegate(string detail);

        private void Label1Change(string detail)
        {
            if (this.label1.InvokeRequired ==false)
                this.label1.Text = detail;
            else
            {
                LabelDelegate LChangeD = new LabelDelegate(Label1Change);
                this.label1.Invoke(LChangeD, detail);
            }
        }
        private void Label2Change(string detail)
        {
            if (this.label2.InvokeRequired == false)
                this.label2.Text = detail;
            else
            {
                LabelDelegate LChangeD = new LabelDelegate(Label2Change);
                this.label2.Invoke(LChangeD, detail);
            }
        }
        private delegate void GroupboxDelegate(bool detail);

        private void Groupbox5Change(bool detail)
        {
            if (this.groupBox5.InvokeRequired == false)
                this.groupBox5.Visible = detail;
            else
            {
                GroupboxDelegate GChangeD = new GroupboxDelegate(Groupbox5Change);
                this.groupBox5.Invoke(GChangeD, detail);
            }
        }
        private void Groupbox4Change(bool detail)
        {
            if (this.groupBox4.InvokeRequired == false)
                this.groupBox4.Visible = detail;
            else
            {
                GroupboxDelegate GChangeD = new GroupboxDelegate(Groupbox4Change);
                this.groupBox4.Invoke(GChangeD, detail);
            }
        }
        private delegate void ButtonDelegate(string detail);

        private void Button1Change(string detail)
        {
            if (this.button1_ImageSwitch.InvokeRequired == false)
                this.button1_ImageSwitch.Text = detail;
            else
            {
                ButtonDelegate BChangeD = new ButtonDelegate(Button1Change);
                this.button1_ImageSwitch.Invoke(BChangeD, detail);
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
                // 显示图像与文本
                if (Global.PhotoSingalLast)
                    Global.TakePhotos = false;
                else if (Global.PhotoSingal && !Global.DisplayResult && !Global.DisplayWait)
                {
                    Global.TakePhotos = true;
                    Global.PhotoSingalLast = true;
                }
                if (!Global.PhotoSingal && !Global.DisplayResult && !Global.DisplayWait)
                    Global.PhotoSingalLast = false;

                if (Global.DisplayWait)
                {
                    this.pictureBox2.Image = Properties.Resources.图像载入;
                }
                /*
                if (Global.DisplayStart && Global.ProgramStep == 1)
                {
                    System.Threading.Thread.Sleep(100);
                    this.pictureBox1.ImageLocation = Global.sourcejpg;
                    //Groupbox5Change(true); //处理前的图像
                    Groupbox4Change(false); //处理后的图像
                    Global.ProgramStep = 2;
                }
                */
                if (Global.DisplayResult)
                {
                    this.pictureBox2.ImageLocation = Global.finalpng;
                    //Groupbox5Change(false); //处理前的图像
                    //Groupbox4Change(true); //处理后的图像
                    Thread_ListView();
                    Global.DisplayResult = false;
                    Global.DisplayStart = false;
                    //Global.ProgramStep = 0;
                    Console.WriteLine("显示图像完成");
                    //正向反向显示
                    if (Global.Pos_Neg)
                        Label2Change(@"正向→");
                    else
                        Label2Change(@"←反向");
                    Button1Change("切换至\r\n处理前图像");
                    BackupFile();
                    
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
                }
                /*
                while (Global.ColorChanged)
                {
                    
                    switch (Global.ColorSelected)
                    {
                        case "Color.Gold":
                            this.label6.BackColor = Color.Gold;
                            LabelChange("金色");
                            break;
                        case "Color.Silver":
                            this.label6.BackColor = Color.Silver;
                            LabelChange("银色");
                            break;
                        case "Color.Gray":
                            this.label6.BackColor = Color.Gray;
                            LabelChange("灰色");
                            break;
                    }
                    Global.ColorChanged = false;
                }
                */
                //产品检测状态显示
                if (!Global.DisplayWait)
                {
                    //this.label1.Text = @"检测完成";
                    Label1Change("检测完成");
                    this.label1.BackColor = Color.Green;
                }
                else if (Global.DisplayWait && !Global.DisplayStart)
                {
                    //this.label1.Text = @"相机拍照";
                    Label1Change("相机拍照");
                    this.label1.BackColor = Color.SaddleBrown;
                }
                else if (Global.DisplayWait && Global.DisplayStart)
                {
                    //this.label1.Text = @"图像处理";
                    Label1Change("图像处理");
                    this.label1.BackColor = Color.DarkMagenta;
                }
                System.Threading.Thread.Sleep(1);
            }
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void BackupFile()
        {
        //将文件备份
        Backup:
            if (!IsFileOpen(Global.sourcejpg) && !IsFileOpen(Global.finalpng) && !IsFileOpen(Global.outputtxt))
            {
                string root = Global.directoryroot + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                string rootOK = root + "_OK";
                string rootNG = root + "_NG";
                if (Global.NG_OK)
                {
                    System.IO.Directory.CreateDirectory(rootOK);
                    System.IO.File.Move(Global.sourcejpg, rootOK + "\\source000.jpg");
                    System.IO.File.Move(Global.finalpng, rootOK + "\\final.png");
                    System.IO.File.Move(Global.outputtxt, rootOK + "\\output.txt");
                }
                else
                {
                    System.IO.Directory.CreateDirectory(rootNG);
                    System.IO.File.Move(Global.sourcejpg, rootNG + "\\source000.jpg");
                    System.IO.File.Move(Global.finalpng, rootNG + "\\final.png");
                    System.IO.File.Move(Global.outputtxt, rootNG + "\\output.txt");
                }
                System.IO.File.Delete(Global.donebin);
            }
            else
                goto Backup;
        }

        private bool IsFileOpen(string filePath)
        {
            bool result = false;
            System.IO.FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenWrite(filePath);
                fs.Close();
            }
            catch (Exception ex)
            {
                result = true;
            }
            return result;//true 打开 false 没有打开
        }

        private void DisplayGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Global.ButtonOK)
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            else
            {
                this.DialogResult = DialogResult.OK;
                Global.ButtonOK = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync(); //流程处理
            backgroundWorker2.CancelAsync(); //串口通讯
            backgroundWorker3.CancelAsync(); //IO与界面元素变化
            //sp.Close();
            Global.ButtonOK = true;
            this.Close();
            
        }

        private void DisplayGUI_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void DisplayGUI_Load(object sender, EventArgs e)
        {
            switch (Global.ColorSelected)
            {
                case "Color.Gold":
                    this.label6.BackColor = Color.Gold;
                    this.label6.Text = "金色";
                    break;
                case "Color.Silver":
                    this.label6.BackColor = Color.Silver;
                    this.label6.Text = "银色";
                    break;
                case "Color.Gray":
                    this.label6.BackColor = Color.Gray;
                    this.label6.Text = "灰色";
                    break;
            }
            this.listView1.Items[0].SubItems[2].Text = Global.PosPrecsion1;
            this.listView1.Items[1].SubItems[2].Text = Global.PosPrecsion2;
            this.listView1.Items[2].SubItems[2].Text = Global.PosPrecsion3;
            this.listView1.Items[3].SubItems[2].Text = Global.PosPrecsion4;
            this.listView1.Items[4].SubItems[2].Text = Global.PosPrecsion5;
            this.listView1.Items[5].SubItems[2].Text = Global.PosPrecsion6;
            this.listView1.Items[6].SubItems[2].Text = Global.PosPrecsion7;
            this.listView1.Items[7].SubItems[2].Text = Global.PosPrecsion8;
            this.listView1.Items[8].SubItems[2].Text = Global.PosPrecsion9;
            this.listView1.Items[9].SubItems[2].Text = Global.PosPrecsion10;
            this.listView1.Items[10].SubItems[2].Text = Global.PosPrecsion11;
            this.listView1.Items[11].SubItems[2].Text = Global.PosPrecsion12;
            this.listView1.Items[12].SubItems[2].Text = Global.PosPrecsion13;
            this.listView1.Items[13].SubItems[2].Text = Global.PosPrecsion14;
            this.listView1.Items[14].SubItems[2].Text = Global.PosPrecsion15;
            this.listView1.Items[15].SubItems[2].Text = Global.PosPrecsion16;
            this.listView1.Items[16].SubItems[2].Text = Global.PosPrecsion17;
            this.listView1.Items[17].SubItems[2].Text = Global.PosPrecsion18;
            this.listView1.Items[18].SubItems[2].Text = Global.PosPrecsion19;
            this.listView1.Items[19].SubItems[2].Text = Global.PosPrecsion20;
            this.listView1.Items[20].SubItems[2].Text = Global.PosPrecsion21;
            this.listView1.Items[21].SubItems[2].Text = Global.PosPrecsion22;
            this.listView1.Items[22].SubItems[2].Text = Global.PosPrecsion23;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //Open SerialPort
            if (!Global.COMOpen)
            {
                sp.PortName = Global.COMSeleted;
                sp.BaudRate = 9600;
                sp.DataBits = 7;
                sp.StopBits = StopBits.One;
                sp.Parity = Parity.Even;
                sp.Open();
                Global.COMOpen = true;
            }
            while (Global.COMOpen && !backgroundWorker2.CancellationPending)
            {
                //SerialPort Send and Recevive automatically
                if (Global.SendInfoSingal)
                {
                    sp.Write(Global.SendInfo, 0, Global.SendInfo.Length);
                    System.Threading.Thread.Sleep(50);
                    int wn = sp.BytesToRead; //记录下缓冲区的字节个数
                    byte[] wbuf = new byte[wn]; //声明一个临时数组存储当前来的串口数据
                    sp.Read(wbuf, 0, wn);
                    Global.SendInfoSingal = false;
                }
                byte[] SendBuf = new byte[11] { 0x02, 0x30, 0x30, 0x31, 0x30, 0x30, 0x30, 0x31, 0x03, 0x35, 0x35 };
                sp.Write(SendBuf, 0, SendBuf.Length);
                System.Threading.Thread.Sleep(50);
                int n = sp.BytesToRead; //记录下缓冲区的字节个数
                byte[] buf = new byte[n]; //声明一个临时数组存储当前来的串口数据
                sp.Read(buf, 0, n);

                if (n == 6)
                {
                    if ((buf[2] & 1) == 1)
                        Global.PhotoSingal = true;
                    else if ((buf[2] & 1) == 0)
                        Global.PhotoSingal = false;
                    else
                        Console.WriteLine("传输失败。Err" + n);
                }
                else
                    Console.WriteLine("传输失败。Err" + n);
            }
            if (backgroundWorker2.CancellationPending)
            {
                sp.Close();
                Global.COMOpen = false;
                e.Cancel = true;
                return;
            }
        }

        private void button1_ImageSwitch_Click(object sender, EventArgs e)
        {
            if (this.label1.Text == @"检测完成")
            {
                //groupBox5.Visible = !groupBox5.Visible; //处理前的图像
                groupBox4.Visible = !groupBox4.Visible; //处理后的图像
                if (button1_ImageSwitch.Text == "切换至\r\n处理前图像")
                    button1_ImageSwitch.Text = "切换至\r\n处理后图像";
                else
                    button1_ImageSwitch.Text = "切换至\r\n处理前图像";
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            //IO与界面元素变化
            while (!backgroundWorker3.CancellationPending)
            {
                if (Global.TakePhotos || Global.DisplayWait)
                {
                    Global.SendInfo[8] = 0x31;
                    Global.SendInfo[9] = 0x30;
                    Global.SendInfoSingal = true;
                }
                else
                {
                    System.Threading.Thread.Sleep(300);
                    if (!Global.NG_OK)
                        Global.SendInfo[9] = 0x31;
                    else
                        Global.SendInfo[9] = 0x32;
                    Global.SendInfo[8] = 0x30;
                    Global.SendInfoSingal = true;
                }
                if (Global.SendInfo[9] + Global.SendInfo[8] == 0x61)
                    Global.SendInfo[12] = 0x38;
                else if (Global.SendInfo[9] + Global.SendInfo[8] == 0x62)
                    Global.SendInfo[12] = 0x39;
                else if (Global.SendInfo[9] + Global.SendInfo[8] == 0x63)
                    Global.SendInfo[12] = 0x41;
                System.Threading.Thread.Sleep(1);
            }
                if (backgroundWorker3.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void pictureBox2_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Global.FinalComplete = true;
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Global.SourceComplete = true;
        }
    }
}
