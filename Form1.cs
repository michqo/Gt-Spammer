using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// TODO fix error handling, i was lazy to add it to here, better don't use try and catch, it can slow down this app.

namespace savedatBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public const uint WM_KEYDOWN = 0x100;
        public const uint WM_KEYUP = 0x0101;
        public TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
        ListViewItem lv;

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr Hwnd, int msg, int param, int lparam);

        [DllImport("user32.dll")]
        public static extern int ReleaseCapture();

        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            // Checking if some updates are available
            WebClient webClient = new WebClient();
            int crntVersion = 2988; // Current growtopia version, it's for checking if some updates are available
            string version = webClient.DownloadString("https://gist.githubusercontent.com/MichalUSER/5c323c7d4f60516f1708f0243b4fbc5d/raw/92df077c92241c2f5d40323fcc3fe01c091ef22a/gtVersion.txt");
            string downloadLink = webClient.DownloadString("https://gist.githubusercontent.com/MichalUSER/622cf8db4d11f35e75cac4f48088df37/raw/d72aab3cde51372be3bc1ee6ba6323cd48de5fa6/spammerDownloadlink.txt");
            Int32.TryParse(version, out int intVersion);
            if (crntVersion != intVersion)
            {
                if (MessageBox.Show("Updates are available, do you want to download them? Without update you can't use this application.", "Spammer Updater", MessageBoxButtons.YesNo) == DialogResult.Yes) // Updates are available
                {
                    Process.Start(downloadLink); // Start download link of that update
                    this.Close();
                }
                else // Updates are not available
                {
                    this.Close(); // Exit the app
                }
            }
            else
            {
                // nothing
            }
        }

        // Exit the app
        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Spam function
        public void Spam(IntPtr Handle, string Text)
        {
            PostMessage(Handle, WM_KEYDOWN, (IntPtr)(Keys.Enter), IntPtr.Zero);
            PostMessage(Handle, WM_KEYUP, (IntPtr)(Keys.Enter), IntPtr.Zero);
            for (int i = 0; i < Text.Length; i++)
            {
                Keys keys1;
                switch (Text[i].ToString())
                {
                    case " ":
                        keys1 = Keys.Space;
                        break;
                    case "`":
                        keys1 = (Keys)0xC0;
                        break;
                    case "/":
                        keys1 = (Keys)0xBF;
                        break;
                    case @"":
                        keys1 = (Keys)0xDC;
                        break;
                    case "[":
                        keys1 = (Keys)0xDB;
                        break;
                    case "]":
                        keys1 = (Keys)0xDD;
                        break;
                    case "\t":
                    case "\n":
                        keys1 = Keys.Space;
                        break;
                    default:
                        keys1 = (Keys)converter.ConvertFromString(Text[i].ToString());
                        break;
                }
                PostMessage(Handle, WM_KEYDOWN, (IntPtr)keys1, IntPtr.Zero);
                PostMessage(Handle, WM_KEYUP, (IntPtr)keys1, IntPtr.Zero);
            }
            PostMessage(Handle, WM_KEYDOWN, (IntPtr)Keys.Enter, IntPtr.Zero);
            PostMessage(Handle, WM_KEYUP, (IntPtr)Keys.Enter, IntPtr.Zero);
        }
        private void Timer2_Tick(object sender, EventArgs e)
        {
            string windowTitle1 = textBox2.Text.ToString();
            IntPtr winTitle = Process.GetProcessesByName(windowTitle1)[0].MainWindowHandle;
            string spamText = textBox1.Text.ToString();
            Spam(winTitle, spamText.ToUpper());
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            intervalValue.Text = bunifuSlider1.Value.ToString();
        }

        // Auto interval
        private void Timer3_Tick(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 21)
            {
                intervalValue.Text = "3600";
                timer2.Interval = 3600;
                bunifuSlider1.Value = 3600;
                checkBox1.Checked = true;
            }
            else if (textBox1.Text.Length < 41)
            {
                intervalValue.Text = "6100";
                timer2.Interval = 6100;
                bunifuSlider1.Value = 6100;
                checkBox1.Checked = true;
            }
            else if (textBox1.Text.Length < 71)
            {
                intervalValue.Text = "9700";
                timer2.Interval = 9700;
                bunifuSlider1.Value = 9700;
                checkBox1.Checked = true;
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                timer3.Start();
            }
            else if (checkBox2.Checked == false)
            {
                timer3.Stop();
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                timer1.Stop();
            }
            else if (checkBox1.Checked == false)
            {
                timer1.Start();
            }
        }
    }
}
