using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> Scriptures = new Dictionary<string, string>();
        const string Path = "scriptures.txt";

        public Form1()
        {
            InitializeComponent();

            // Read in scriptures.
            try
            {
                string[] lines = File.ReadAllLines(Path);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');
                    string verse = parts[0];
                    string text = parts[1];

                    verse = verse.Trim();
                    text = text.Trim();
                    Scriptures.Add(verse, text);
                }
            }
            catch
            {
                MessageBox.Show("Could not read in scripture list.");
                Application.Exit();
            }

            comboBox1.DataSource = Scriptures.Keys.ToList();
        }

        bool CompareText(bool caseSensative = false, bool punctuationSensative = false)
        {
            string a = richTextBox1.Text;
            string b = richTextBox2.Text;

            if (a.Length == 0 || b.Length == 0) return true;

            int length = (b.Length < a.Length) ? b.Length : a.Length;

            a = a.Substring(0, length);

            if (!caseSensative)
            {
                a = a.ToUpper();
                b = b.ToUpper();
            }

            if (!punctuationSensative)
            {
                a = Regex.Replace(a, @"\W+", string.Empty);
                b = Regex.Replace(b, @"\W+", string.Empty);
            }

            a = a.Trim();
            b = b.Trim();

            if (a == b && richTextBox1.TextLength == richTextBox2.TextLength
                && caseSensative == true && punctuationSensative == true)
            {
                MessageBox.Show("That is correct!");
            }

            return (a == b);
        }

        private void ChangeTextColor()
        {
            bool caseValue = checkBox1.Checked;
            bool puncValue = checkBox2.Checked;

            richTextBox2.ForeColor = CompareText(caseValue, puncValue) ? Color.Green : Color.Red;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            ChangeTextColor();
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTextColor();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTextColor();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Visible = !checkBox3.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            string verse = comboBox1.Text;

            try
            {
                richTextBox1.Text = Scriptures[verse];
            }
            catch
            {
                richTextBox1.Text = string.Format("Could not load {0}", verse);
            }
        }
    }
}
