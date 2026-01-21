using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FT1_UDR
{
    public partial class hmessage2 : Form
    {
        private bool CResult;
        public hmessage2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CResult = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CResult = false;
            this.Close();
        }
        public bool Show_string(string title)
        {
            label1.Text = title;
            this.ShowDialog();
            return CResult;
        }
    }
}
