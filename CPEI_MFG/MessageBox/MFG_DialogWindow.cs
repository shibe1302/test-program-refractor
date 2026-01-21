using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CPEI_MFG
{
    public partial class MFG_DialogWindow : Form
    {
        
        
        private bool bStartInput;
        private string mResult;
        public MFG_DialogWindow()
        {
            InitializeComponent();
            bStartInput = false;
            ActiveControl = textBox1;
            
        }
        public void InitialDialog()
        {
            
            bStartInput = false;
            this.label1.Text = "";
            this.label2.Text = "";
            this.textBox1.Text = "";
            this.textBox1.Visible = false;
            this.textBox1.Focus();
            mResult = "";
            ActiveControl = textBox1;
            
        }
        public bool AskYesNo(string title, string content)
        {
            this.label1.Text = title;           
            this.label2.Text = content;
            this.textBox1.Visible = false;
            DialogResult dRet = DialogResult.Cancel;
            ActiveControl = button2;
            dRet = this.ShowDialog();
            if (dRet == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string AskInput(string title) 
        {
           
            mResult = "";
            this.label1.Text = title;
            this.textBox1.Text = "";
            this.label2.Text = "";
            this.textBox1.Visible = true;
            ActiveControl = textBox1;
            bStartInput = true;
            this.ShowDialog();
            return mResult;
        }
        public string AskPassword(string title)
        {

            mResult = "";
            this.label1.Text = title;
            this.textBox1.Text = "";
            this.label2.Text = "";
            this.textBox1.Visible = true;
            this.textBox1.PasswordChar = '*';
            ActiveControl = textBox1;
            bStartInput = true;
            this.ShowDialog();
            this.textBox1.PasswordChar = '\0';
            return mResult;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!bStartInput)
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Return)
            {
                mResult = textBox1.Text.Trim();
                this.Close();
            }
        }

        public void ShowWarning(string title, string content)
        {
            this.label1.Text = title;
            this.label2.Text = content;
            this.label2.ForeColor = Color.Red;
            this.textBox1.Visible = false;
            ActiveControl = button3;
            this.ShowDialog();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mResult = textBox1.Text.Trim();//add in 20160618 for Button YES
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MFG_DialogWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (this.button1.Focused)
                {
                    this.Close();
                }
                else if (this.button2.Focused)
                {
                    this.Close();
                }
                else if (this.button3.Focused)
                {
                    this.Close();
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MFG_DialogWindow_Load(object sender, EventArgs e)
        {

        }

    }
}
