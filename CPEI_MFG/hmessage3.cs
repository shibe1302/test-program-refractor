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
    public partial class hmessage3 : Form
    {
        private bool CResult;
        public hmessage3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CResult = true;
            this.Close();
        }
        public bool Show_string(string title, string PicPath)
        {
            //pictureBox1.ImageLocation =PictureBox.Con
            pictureBox1.Image = Image.FromFile(PicPath);
            label1.Text = title;
            this.ShowDialog();
            return CResult;
        }
    }
}
