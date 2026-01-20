using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPEI_MFG
{
    public partial class hmessageWorkFind : Form
    {
        public bool csResult;
        public hmessageWorkFind()
        {
            InitializeComponent();
        }
        public bool Show_string(string title, string PicPath)
        {
            //pictureBox1.ImageLocation =PictureBox.Con
            pictureBox1.Image = Image.FromFile(PicPath);
            label1.Text = title;
            this.ShowDialog();
            return csResult;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            csResult = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            csResult = true;
            this.Close();
        }
    }
}
