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
    public partial class hMessage : Form
    {
        public hMessage()
        {
            InitializeComponent();
        }

        public void Show_string(string title)
        {
            label1.Text = title;
            this.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hMessage_Load(object sender, EventArgs e)
        {

        }
    }
}
