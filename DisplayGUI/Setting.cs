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
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Global.ColorSelected = "Color.Gold";
            this.label6.BackColor = System.Drawing.Color.Gold;
            this.label6.Text = "金色";
            Global.ColorChanged = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Global.ColorSelected = "Color.Silver";
            this.label6.BackColor = System.Drawing.Color.Silver;
            this.label6.Text = "银色";
            Global.ColorChanged = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Global.ColorSelected = "Color.Gray";
            this.label6.BackColor = System.Drawing.Color.Gray;
            this.label6.Text = "灰色";
            Global.ColorChanged = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveText();
            Global.COMSeleted = comboBox24.Text;
            switch (Global.ColorSelected)
            {
                case "Color.Gold":
                    Console.WriteLine("Gold");
                    var psigold = new System.Diagnostics.ProcessStartInfo(Global.goldphoto);
                    psigold.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
                    var processgold = System.Diagnostics.Process.Start(psigold);
                    break;
                case "Color.Silver":
                    Console.WriteLine("Silver");
                    var psisilver = new System.Diagnostics.ProcessStartInfo(Global.silverphoto);
                    psisilver.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
                    var processsilver = System.Diagnostics.Process.Start(psisilver);
                    break;
                case "Color.Gray":
                    Console.WriteLine("Gray");
                    var psigray = new System.Diagnostics.ProcessStartInfo(Global.grayphoto);
                    psigray.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
                    var processgray = System.Diagnostics.Process.Start(psigray);
                    break;
            }
            DisplayGUI ShowForm = new DisplayGUI();
            this.Hide();
            if (ShowForm.ShowDialog() == DialogResult.OK)
                this.Show();
        }

        private void SaveText()
        {
            Global.PosPrecsion1 = this.comboBox1.Text;
            Global.PosPrecsion2 = this.comboBox2.Text;
            Global.PosPrecsion3 = this.comboBox3.Text;
            Global.PosPrecsion4 = this.comboBox4.Text;
            Global.PosPrecsion5 = this.comboBox5.Text;
            Global.PosPrecsion6 = this.comboBox6.Text;
            Global.PosPrecsion7 = this.comboBox7.Text;
            Global.PosPrecsion8 = this.comboBox8.Text;
            Global.PosPrecsion9 = this.comboBox9.Text;
            Global.PosPrecsion10 = this.comboBox10.Text;
            Global.PosPrecsion11 = this.comboBox11.Text;
            Global.PosPrecsion12 = this.comboBox12.Text;
            Global.PosPrecsion13 = this.comboBox3.Text;
            Global.PosPrecsion14 = this.comboBox14.Text;
            Global.PosPrecsion15 = this.comboBox15.Text;
            Global.PosPrecsion16 = this.comboBox16.Text;
            Global.PosPrecsion17 = this.comboBox17.Text;
            Global.PosPrecsion18 = this.comboBox18.Text;
            Global.PosPrecsion19 = this.comboBox19.Text;
            Global.PosPrecsion20 = this.comboBox20.Text;
            Global.PosPrecsion21 = this.comboBox21.Text;
            Global.PosPrecsion22 = this.comboBox22.Text;
            Global.PosPrecsion23 = this.comboBox23.Text;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Global.precisiontxt, false))
            {
                file.WriteLine("1 " + Global.PosPrecsion1);
                file.WriteLine("2 " + Global.PosPrecsion2);
                file.WriteLine("3 " + Global.PosPrecsion3);
                file.WriteLine("4 " + Global.PosPrecsion4);
                file.WriteLine("5 " + Global.PosPrecsion5);
                file.WriteLine("6 " + Global.PosPrecsion6);
                file.WriteLine("7 " + Global.PosPrecsion7);
                file.WriteLine("8 " + Global.PosPrecsion8);
                file.WriteLine("9 " + Global.PosPrecsion9);
                file.WriteLine("10 " + Global.PosPrecsion10);
                file.WriteLine("11 " + Global.PosPrecsion11);
                file.WriteLine("12 " + Global.PosPrecsion12);
                file.WriteLine("13 " + Global.PosPrecsion13);
                file.WriteLine("14 " + Global.PosPrecsion14);
                file.WriteLine("15 " + Global.PosPrecsion15);
                file.WriteLine("16 " + Global.PosPrecsion16);
                file.WriteLine("17 " + Global.PosPrecsion17);
                file.WriteLine("18 " + Global.PosPrecsion18);
                file.WriteLine("19 " + Global.PosPrecsion19);
                file.WriteLine("20 " + Global.PosPrecsion20);
                file.WriteLine("21 " + Global.PosPrecsion21);
                file.WriteLine("22 " + Global.PosPrecsion22);
                file.WriteLine("23 " + Global.PosPrecsion23);
            }
        }
        private void Setting_Load(object sender, EventArgs e)
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
            if (System.IO.File.Exists(Global.precisiontxt))
            {
                var file = System.IO.File.Open(Global.precisiontxt, System.IO.FileMode.Open);
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
                    txt[num] = txt[num].Substring(txt[num].IndexOf(" ") + 1);
                }
                this.comboBox1.Text = txt[0];
                this.comboBox2.Text = txt[1];
                this.comboBox3.Text = txt[2];
                this.comboBox4.Text = txt[3];
                this.comboBox5.Text = txt[4];
                this.comboBox6.Text = txt[5];
                this.comboBox7.Text = txt[6];
                this.comboBox8.Text = txt[7];
                this.comboBox9.Text = txt[8];
                this.comboBox10.Text = txt[9];
                this.comboBox11.Text = txt[10];
                this.comboBox12.Text = txt[11];
                this.comboBox13.Text = txt[12];
                this.comboBox14.Text = txt[13];
                this.comboBox15.Text = txt[14];
                this.comboBox16.Text = txt[15];
                this.comboBox17.Text = txt[16];
                this.comboBox18.Text = txt[17];
                this.comboBox19.Text = txt[18];
                this.comboBox20.Text = txt[19];
                this.comboBox21.Text = txt[20];
                this.comboBox22.Text = txt[21];
                this.comboBox23.Text = txt[22];
            }
            else
            {
                this.comboBox1.Text = Global.PosPrecsion1;
                this.comboBox2.Text = Global.PosPrecsion2;
                this.comboBox3.Text = Global.PosPrecsion3;
                this.comboBox4.Text = Global.PosPrecsion4;
                this.comboBox5.Text = Global.PosPrecsion5;
                this.comboBox6.Text = Global.PosPrecsion6;
                this.comboBox7.Text = Global.PosPrecsion7;
                this.comboBox8.Text = Global.PosPrecsion8;
                this.comboBox9.Text = Global.PosPrecsion9;
                this.comboBox10.Text = Global.PosPrecsion10;
                this.comboBox11.Text = Global.PosPrecsion11;
                this.comboBox12.Text = Global.PosPrecsion12;
                this.comboBox13.Text = Global.PosPrecsion13;
                this.comboBox14.Text = Global.PosPrecsion14;
                this.comboBox15.Text = Global.PosPrecsion15;
                this.comboBox16.Text = Global.PosPrecsion16;
                this.comboBox17.Text = Global.PosPrecsion17;
                this.comboBox18.Text = Global.PosPrecsion18;
                this.comboBox19.Text = Global.PosPrecsion19;
                this.comboBox20.Text = Global.PosPrecsion20;
                this.comboBox21.Text = Global.PosPrecsion21;
                this.comboBox22.Text = Global.PosPrecsion22;
                this.comboBox23.Text = Global.PosPrecsion23;
            }
            this.groupBox1.Visible = false;
            this.groupBox2.Visible = false;
            this.groupBox3.Visible = false;
            this.groupBox4.Visible = false;
            string[] SerialPortstr = System.IO.Ports.SerialPort.GetPortNames();
            if (SerialPortstr == null)
                MessageBox.Show("没有找到串口");
            else
            {
                Array.Sort(SerialPortstr, new CustomComparer());
                foreach (string s in SerialPortstr)
                {
                    comboBox24.Items.Add(s);
                }
                comboBox24.SelectedIndex = 0;
            }
        }
        public class CustomComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                string s1 = (string)x;
                string s2 = (string)y;
                if (s1.Length > s2.Length) return 1;
                if (s1.Length < s2.Length) return -1;
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] > s2[i]) return 1;
                    if (s1[i] < s2[i]) return -1;
                }
                return 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Global.ButtonOK = true;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.comboBox1.Text = @"0.02";
            this.comboBox2.Text = @"0.02";
            this.comboBox3.Text = @"0.02";
            this.comboBox4.Text = @"0.02";
            this.comboBox5.Text = @"0.02";
            this.comboBox6.Text = @"0.02";
            this.comboBox7.Text = @"0.02";
            this.comboBox8.Text = @"0.02";
            this.comboBox9.Text = @"0.02";
            this.comboBox10.Text = @"0.02";
            this.comboBox11.Text = @"0.02";
            this.comboBox12.Text = @"0.02";
            this.comboBox13.Text = @"0.02";
            this.comboBox14.Text = @"0.02";
            this.comboBox15.Text = @"0.02";
            this.comboBox16.Text = @"0.02";
            this.comboBox17.Text = @"0.02";
            this.comboBox18.Text = @"0.02";
            this.comboBox19.Text = @"0.02";
            this.comboBox20.Text = @"0.02";
            this.comboBox21.Text = @"0.02";
            this.comboBox22.Text = @"0.02";
            this.comboBox23.Text = @"0.02";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.groupBox1.Visible = true;
            this.groupBox2.Visible = true;
            this.groupBox3.Visible = true;
            this.groupBox4.Visible = true;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            sptest();

        }

        private void sptest()
        {
            try
            {
                sp.PortName = comboBox24.SelectedItem.ToString();
                sp.BaudRate = 9600;
                sp.DataBits = 7;
                sp.StopBits = StopBits.One;
                sp.Parity = Parity.Even;
                sp.Open();
                byte[] SendBuf = new byte[11] { 0x02, 0x30, 0x30, 0x31, 0x30, 0x30, 0x30, 0x31, 0x03, 0x35, 0x35 };
                sp.Write(SendBuf, 0, SendBuf.Length);
                System.Threading.Thread.Sleep(50);
                int n = sp.BytesToRead; //记录下缓冲区的字节个数
                byte[] buf = new byte[n]; //声明一个临时数组存储当前来的串口数据
                sp.Read(buf, 0, n);
                if (n == 6)
                {
                    if ((buf[2] & 1) == 1)
                        MessageBox.Show("测试成功！传感器信号为ON。");
                    else if ((buf[2] & 1) == 0)
                        MessageBox.Show("测试成功！传感器信号为OFF。");
                    else
                        MessageBox.Show("测试失败！接收到异常数据。Err" + n);
                }
                else if (n == 0)
                    MessageBox.Show("测试失败！未接收到数据。Err" + n);
                else
                    MessageBox.Show("测试失败！接收到异常数据。Err" + n);
                System.Threading.Thread.Sleep(10);
                sp.Close();
                

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error");
                return;
            }
        }
    }

}
