using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;

namespace KeySprite
{
    public partial class Form1 : Form
    {
        private CDD dd;
        string DLLPath;
        string selectStr1;
        string selectStr2;
        string KeyInterval;
        string sound;
        Dictionary<string, int> KeyToCode;
        Dictionary<int, string> CodeToKey;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            selectStr1 = ConfigurationManager.AppSettings["presskey"].ToString();
            selectStr2 = ConfigurationManager.AppSettings["start"].ToString();
            KeyInterval = ConfigurationManager.AppSettings["Interval"].ToString();
            DLLPath = ConfigurationManager.AppSettings["DLLPath"].ToString();
            sound = ConfigurationManager.AppSettings["sound"].ToString();
            //reg_hotkey();
            //LoadDllFile(DLLPath);
            //comboBox_Init();
            //Map_Init();
            //dd = new CDD();

        }


        private void comboBox_Init()
        {
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("Q");
            comboBox1.Items.Add("F10");
            comboBox1.Items.Add("F11");
            comboBox1.Items.Add("F12");
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(selectStr1);
            comboBox2.Items.Add("1");
            comboBox2.Items.Add("Q");
            comboBox2.Items.Add("F10");
            comboBox2.Items.Add("F11");
            comboBox2.Items.Add("F12");
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf(selectStr2);
        }
        private void Map_Init()
        {
            KeyToCode = new Dictionary<string, int>();
            CodeToKey = new Dictionary<int, string>();
            KeyToCode.Add("F12", 112);
            KeyToCode.Add("F11", 111);
            KeyToCode.Add("Q", 301);
            KeyToCode.Add("F10", 110);
            KeyToCode.Add("1", 201);
            CodeToKey.Add(112,"F12");
            CodeToKey.Add(111, "F11");
            CodeToKey.Add(110, "F10");
            CodeToKey.Add(201, "1");
            CodeToKey.Add(301, "Q");
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectStr1 = comboBox1.SelectedItem.ToString();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectStr2 = comboBox2.SelectedItem.ToString();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            unreg_hotkey();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            KeyInterval = textBox1.Text;
        }

        private void LoadDllFile(string dllfile)
        {

            int ret = dd.Load(dllfile);
            if (ret != 1) { MessageBox.Show("DD64.DLL Load Error"); return; }


            ret = dd.btn(0); //DD Initialize
            if (ret != 1) { MessageBox.Show("DD64.DLL Initialize Error"); return; }

            return;
        }

        #region "HotKey"
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(
         IntPtr hWnd,
         int id,
         KeyModifiers modkey,
         Keys vk
        );
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(
         IntPtr hWnd,
         int id
        );

        void reg_hotkey()
        {
            RegisterHotKey(this.Handle, 80, 0, Keys.F12);
        }

        void unreg_hotkey()
        {
            UnregisterHotKey(this.Handle, 80);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    ProcessHotkey(m);
                    break;
            }
            base.WndProc(ref m);
        }

        private void ProcessHotkey(Message msg)
        {
            switch (msg.WParam.ToInt32())
            {
                case 80:
                    Fun80();
                    break;
                case 90:
                    Fun90();
                    break;
            }
        }

        private void Fun80()
        {
            dd.str("Keyboard char [A-Za_z] {@$} ");
        }

        private void Fun90()
        {
            if (dd.key != null)
            {
                //CTRL+ALT+DEL
                dd.key(600, 1);                                      //600 == L.CTRL down
                dd.key(602, 1);                                      // ALT   down
                dd.key(706, 1);                                      // DEL   down
                System.Threading.Thread.Sleep(5);
                dd.key(706, 2);                                       //up
                dd.key(602, 2);
                dd.key(600, 2);
            }
        }

        #endregion


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            textBox2.Text = e.KeyData.ToString();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
