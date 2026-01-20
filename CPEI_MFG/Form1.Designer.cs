namespace CPEI_MFG
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "111",
            "333",
            "444"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("222");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.com_SFIS = new System.IO.Ports.SerialPort(this.components);
            this.com_DUT = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Label_Pass = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Label_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Label_Retest = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Label_Time = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label_Ver = new System.Windows.Forms.ToolStripStatusLabel();
            this.rb_SfisON = new System.Windows.Forms.RadioButton();
            this.rb_SfisOff = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_Result = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label_Error = new System.Windows.Forms.Label();
            this.label_scan = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer_CheckFinish = new System.Windows.Forms.Timer(this.components);
            this.timer_Count = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.rb_Golden = new System.Windows.Forms.RadioButton();
            this.btn_Laser = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label_Model = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label_StationNO = new System.Windows.Forms.Label();
            this.label_Station = new System.Windows.Forms.Label();
            this.com_fixture = new System.IO.Ports.SerialPort(this.components);
            this.com_fixtureUSB = new System.IO.Ports.SerialPort(this.components);
            this.timer_checkSFC = new System.Windows.Forms.Timer(this.components);
            this.comm_SoftLED = new System.IO.Ports.SerialPort(this.components);
            this.com_Laser = new System.IO.Ports.SerialPort(this.components);
            this.Com_LedTest = new System.IO.Ports.SerialPort(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txLoopNum = new System.Windows.Forms.TextBox();
            this.loop_Retest = new System.Windows.Forms.Label();
            this.loop_Fail = new System.Windows.Forms.Label();
            this.loop_Pass = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEnableLoop = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.S_ServerMOCPath = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.S_Cancel = new System.Windows.Forms.Button();
            this.S_OK = new System.Windows.Forms.Button();
            this.S_BOMVersion = new System.Windows.Forms.TextBox();
            this.S_FCDVersion = new System.Windows.Forms.TextBox();
            this.S_FWVersion = new System.Windows.Forms.TextBox();
            this.S_FIXCom = new System.Windows.Forms.TextBox();
            this.S_DUTCom = new System.Windows.Forms.TextBox();
            this.S_SFISCom = new System.Windows.Forms.TextBox();
            this.S_LocalLogPath = new System.Windows.Forms.TextBox();
            this.S_ServerLogPath = new System.Windows.Forms.TextBox();
            this.S_ServerProgramPath = new System.Windows.Forms.TextBox();
            this.S_StationNO = new System.Windows.Forms.TextBox();
            this.S_Station = new System.Windows.Forms.TextBox();
            this.S_JavaPath = new System.Windows.Forms.TextBox();
            this.S_JavaWindows = new System.Windows.Forms.TextBox();
            this.S_Model = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.S_PWD = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.timer_autoScan = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // com_SFIS
            // 
            this.com_SFIS.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.com_SFIS_DataReceived);
            // 
            // com_DUT
            // 
            this.com_DUT.BaudRate = 115200;
            this.com_DUT.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.com_DUT_DataReceived);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.Label_Pass,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.Label_Fail,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.Label_Retest,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel8,
            this.Label_Time,
            this.toolStripStatusLabel9,
            this.toolStripStatusLabel10,
            this.toolStripStatusLabel11,
            this.label_Ver});
            this.statusStrip1.Location = new System.Drawing.Point(0, 658);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1333, 25);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(86, 20);
            this.toolStripStatusLabel1.Text = "Total PASS :";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // Label_Pass
            // 
            this.Label_Pass.Name = "Label_Pass";
            this.Label_Pass.Size = new System.Drawing.Size(17, 20);
            this.Label_Pass.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(33, 20);
            this.toolStripStatusLabel2.Text = "      ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(75, 20);
            this.toolStripStatusLabel3.Text = "Total Fail :";
            // 
            // Label_Fail
            // 
            this.Label_Fail.Name = "Label_Fail";
            this.Label_Fail.Size = new System.Drawing.Size(17, 20);
            this.Label_Fail.Text = "0";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(33, 20);
            this.toolStripStatusLabel4.Text = "      ";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(91, 20);
            this.toolStripStatusLabel5.Text = "Retest Rate :";
            // 
            // Label_Retest
            // 
            this.Label_Retest.Name = "Label_Retest";
            this.Label_Retest.Size = new System.Drawing.Size(48, 20);
            this.Label_Retest.Text = "0.00%";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(33, 20);
            this.toolStripStatusLabel6.Text = "      ";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(33, 20);
            this.toolStripStatusLabel7.Text = "      ";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(79, 20);
            this.toolStripStatusLabel8.Text = "Test Time :";
            // 
            // Label_Time
            // 
            this.Label_Time.Name = "Label_Time";
            this.Label_Time.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(33, 20);
            this.toolStripStatusLabel9.Text = "      ";
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(33, 20);
            this.toolStripStatusLabel10.Text = "      ";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(64, 20);
            this.toolStripStatusLabel11.Text = "Version: ";
            // 
            // label_Ver
            // 
            this.label_Ver.Name = "label_Ver";
            this.label_Ver.Size = new System.Drawing.Size(88, 20);
            this.label_Ver.Text = "V2025.12.02";
            this.label_Ver.Click += new System.EventHandler(this.label_Ver_Click);
            // 
            // rb_SfisON
            // 
            this.rb_SfisON.AutoSize = true;
            this.rb_SfisON.Checked = true;
            this.rb_SfisON.Location = new System.Drawing.Point(12, 85);
            this.rb_SfisON.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rb_SfisON.Name = "rb_SfisON";
            this.rb_SfisON.Size = new System.Drawing.Size(83, 21);
            this.rb_SfisON.TabIndex = 8;
            this.rb_SfisON.TabStop = true;
            this.rb_SfisON.Text = "SFIS ON";
            this.rb_SfisON.UseVisualStyleBackColor = true;
            this.rb_SfisON.CheckedChanged += new System.EventHandler(this.rb_SfisON_CheckedChanged);
            // 
            // rb_SfisOff
            // 
            this.rb_SfisOff.AutoSize = true;
            this.rb_SfisOff.Location = new System.Drawing.Point(124, 84);
            this.rb_SfisOff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rb_SfisOff.Name = "rb_SfisOff";
            this.rb_SfisOff.Size = new System.Drawing.Size(89, 21);
            this.rb_SfisOff.TabIndex = 9;
            this.rb_SfisOff.Text = "SFIS OFF";
            this.rb_SfisOff.UseVisualStyleBackColor = true;
            this.rb_SfisOff.CheckedChanged += new System.EventHandler(this.rb_SfisON_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Location = new System.Drawing.Point(16, 385);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(376, 265);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listView1.Location = new System.Drawing.Point(8, 18);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(359, 237);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ColumnHeader1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_Result);
            this.groupBox2.Location = new System.Drawing.Point(16, 17);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(376, 91);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // label_Result
            // 
            this.label_Result.AutoSize = true;
            this.label_Result.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Result.ForeColor = System.Drawing.Color.Blue;
            this.label_Result.Location = new System.Drawing.Point(68, 18);
            this.label_Result.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Result.Name = "label_Result";
            this.label_Result.Size = new System.Drawing.Size(202, 54);
            this.label_Result.TabIndex = 0;
            this.label_Result.Text = "Standby";
            this.label_Result.Click += new System.EventHandler(this.label_Result_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(223, 78);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 28);
            this.button1.TabIndex = 13;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_Error
            // 
            this.label_Error.AutoSize = true;
            this.label_Error.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Error.ForeColor = System.Drawing.Color.Red;
            this.label_Error.Location = new System.Drawing.Point(4, 22);
            this.label_Error.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Error.Name = "label_Error";
            this.label_Error.Size = new System.Drawing.Size(77, 17);
            this.label_Error.TabIndex = 15;
            this.label_Error.Text = "Error Code:";
            this.label_Error.Visible = false;
            this.label_Error.Click += new System.EventHandler(this.label_Error_Click);
            // 
            // label_scan
            // 
            this.label_scan.AutoSize = true;
            this.label_scan.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_scan.Location = new System.Drawing.Point(8, 34);
            this.label_scan.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_scan.Name = "label_scan";
            this.label_scan.Size = new System.Drawing.Size(58, 24);
            this.label_scan.TabIndex = 17;
            this.label_scan.Text = "Scan:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(67, 33);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(288, 22);
            this.textBox1.TabIndex = 18;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMacInput_01);
            // 
            // timer_CheckFinish
            // 
            this.timer_CheckFinish.Enabled = true;
            this.timer_CheckFinish.Interval = 1000;
            this.timer_CheckFinish.Tick += new System.EventHandler(this.timer_CheckFinish_Tick);
            // 
            // timer_Count
            // 
            this.timer_Count.Enabled = true;
            this.timer_Count.Interval = 1000;
            this.timer_Count.Tick += new System.EventHandler(this.timer_Count_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.rb_Golden);
            this.groupBox3.Controls.Add(this.btn_Laser);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.label_scan);
            this.groupBox3.Controls.Add(this.rb_SfisOff);
            this.groupBox3.Controls.Add(this.rb_SfisON);
            this.groupBox3.Location = new System.Drawing.Point(16, 223);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(376, 155);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(221, 118);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 28);
            this.button2.TabIndex = 23;
            this.button2.Text = "Clear\r\n";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // rb_Golden
            // 
            this.rb_Golden.AutoSize = true;
            this.rb_Golden.Location = new System.Drawing.Point(12, 123);
            this.rb_Golden.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rb_Golden.Name = "rb_Golden";
            this.rb_Golden.Size = new System.Drawing.Size(107, 21);
            this.rb_Golden.TabIndex = 22;
            this.rb_Golden.Text = "Golden Test\r\n";
            this.rb_Golden.UseVisualStyleBackColor = true;
            this.rb_Golden.Visible = false;
            this.rb_Golden.CheckedChanged += new System.EventHandler(this.rb_Golden_CheckedChanged);
            // 
            // btn_Laser
            // 
            this.btn_Laser.Location = new System.Drawing.Point(289, 121);
            this.btn_Laser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Laser.Name = "btn_Laser";
            this.btn_Laser.Size = new System.Drawing.Size(67, 28);
            this.btn_Laser.TabIndex = 21;
            this.btn_Laser.Text = "Log";
            this.btn_Laser.UseVisualStyleBackColor = true;
            this.btn_Laser.Click += new System.EventHandler(this.btn_Laser_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label_Error);
            this.groupBox4.Location = new System.Drawing.Point(16, 110);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(376, 112);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label_Model);
            this.groupBox5.Location = new System.Drawing.Point(405, 17);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(476, 91);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Mode Name";
            // 
            // label_Model
            // 
            this.label_Model.AutoSize = true;
            this.label_Model.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Model.ForeColor = System.Drawing.Color.Blue;
            this.label_Model.Location = new System.Drawing.Point(88, 26);
            this.label_Model.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Model.Name = "label_Model";
            this.label_Model.Size = new System.Drawing.Size(244, 46);
            this.label_Model.TabIndex = 0;
            this.label_Model.Text = "ModelName";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label_StationNO);
            this.groupBox6.Controls.Add(this.label_Station);
            this.groupBox6.Location = new System.Drawing.Point(889, 17);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Size = new System.Drawing.Size(428, 91);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Station";
            // 
            // label_StationNO
            // 
            this.label_StationNO.AutoSize = true;
            this.label_StationNO.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_StationNO.ForeColor = System.Drawing.Color.Blue;
            this.label_StationNO.Location = new System.Drawing.Point(227, 61);
            this.label_StationNO.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_StationNO.Name = "label_StationNO";
            this.label_StationNO.Size = new System.Drawing.Size(64, 17);
            this.label_StationNO.TabIndex = 16;
            this.label_StationNO.Text = "L2-FT-01";
            // 
            // label_Station
            // 
            this.label_Station.AutoSize = true;
            this.label_Station.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Station.ForeColor = System.Drawing.Color.Blue;
            this.label_Station.Location = new System.Drawing.Point(55, 32);
            this.label_Station.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Station.Name = "label_Station";
            this.label_Station.Size = new System.Drawing.Size(70, 46);
            this.label_Station.TabIndex = 0;
            this.label_Station.Text = "FT";
            this.label_Station.Click += new System.EventHandler(this.label_Station_Click);
            // 
            // com_fixture
            // 
            this.com_fixture.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.com_fixture_DataReceived);
            // 
            // com_fixtureUSB
            // 
            this.com_fixtureUSB.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.com_fixtureUSB_DataReceived);
            // 
            // timer_checkSFC
            // 
            this.timer_checkSFC.Interval = 200;
            this.timer_checkSFC.Tick += new System.EventHandler(this.timer_checkSFC_Tick);
            // 
            // comm_SoftLED
            // 
            this.comm_SoftLED.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.comm_SoftLED_DataReceived);
            // 
            // com_Laser
            // 
            this.com_Laser.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.com_Laser_DataReceived);
            // 
            // Com_LedTest
            // 
            this.Com_LedTest.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.Com_LedTestRecData);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox7);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(904, 504);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "LoopTestInfo";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txLoopNum);
            this.groupBox7.Controls.Add(this.loop_Retest);
            this.groupBox7.Controls.Add(this.loop_Fail);
            this.groupBox7.Controls.Add(this.loop_Pass);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.cbEnableLoop);
            this.groupBox7.Location = new System.Drawing.Point(3, 1);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox7.Size = new System.Drawing.Size(895, 495);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "groupBox7";
            // 
            // txLoopNum
            // 
            this.txLoopNum.Location = new System.Drawing.Point(149, 102);
            this.txLoopNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txLoopNum.Name = "txLoopNum";
            this.txLoopNum.Size = new System.Drawing.Size(132, 22);
            this.txLoopNum.TabIndex = 8;
            // 
            // loop_Retest
            // 
            this.loop_Retest.Location = new System.Drawing.Point(149, 268);
            this.loop_Retest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loop_Retest.Name = "loop_Retest";
            this.loop_Retest.Size = new System.Drawing.Size(111, 22);
            this.loop_Retest.TabIndex = 7;
            this.loop_Retest.Text = "0%";
            // 
            // loop_Fail
            // 
            this.loop_Fail.Location = new System.Drawing.Point(149, 224);
            this.loop_Fail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loop_Fail.Name = "loop_Fail";
            this.loop_Fail.Size = new System.Drawing.Size(111, 22);
            this.loop_Fail.TabIndex = 6;
            this.loop_Fail.Text = "0";
            // 
            // loop_Pass
            // 
            this.loop_Pass.Location = new System.Drawing.Point(149, 162);
            this.loop_Pass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loop_Pass.Name = "loop_Pass";
            this.loop_Pass.Size = new System.Drawing.Size(111, 22);
            this.loop_Pass.TabIndex = 5;
            this.loop_Pass.Text = "0";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(15, 268);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 31);
            this.label5.TabIndex = 4;
            this.label5.Text = "Retest Rate : ";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(15, 217);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 31);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fail Num : ";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 165);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 31);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pass Num :";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 111);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "LoopNum : ";
            // 
            // cbEnableLoop
            // 
            this.cbEnableLoop.Location = new System.Drawing.Point(17, 54);
            this.cbEnableLoop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableLoop.Name = "cbEnableLoop";
            this.cbEnableLoop.Size = new System.Drawing.Size(167, 32);
            this.cbEnableLoop.TabIndex = 0;
            this.cbEnableLoop.Text = "Enable Loop Test";
            this.cbEnableLoop.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage1.Controls.Add(this.listView2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(904, 504);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Test Summary";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(0, 1);
            this.listView2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(893, 500);
            this.listView2.TabIndex = 1;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(904, 504);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Test Message";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(897, 489);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(900, 496);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(405, 116);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 3;
            this.tabControl1.Size = new System.Drawing.Size(912, 533);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.S_ServerMOCPath);
            this.tabPage4.Controls.Add(this.label19);
            this.tabPage4.Controls.Add(this.S_Cancel);
            this.tabPage4.Controls.Add(this.S_OK);
            this.tabPage4.Controls.Add(this.S_BOMVersion);
            this.tabPage4.Controls.Add(this.S_FCDVersion);
            this.tabPage4.Controls.Add(this.S_FWVersion);
            this.tabPage4.Controls.Add(this.S_FIXCom);
            this.tabPage4.Controls.Add(this.S_DUTCom);
            this.tabPage4.Controls.Add(this.S_SFISCom);
            this.tabPage4.Controls.Add(this.S_LocalLogPath);
            this.tabPage4.Controls.Add(this.S_ServerLogPath);
            this.tabPage4.Controls.Add(this.S_ServerProgramPath);
            this.tabPage4.Controls.Add(this.S_StationNO);
            this.tabPage4.Controls.Add(this.S_Station);
            this.tabPage4.Controls.Add(this.S_JavaPath);
            this.tabPage4.Controls.Add(this.S_JavaWindows);
            this.tabPage4.Controls.Add(this.S_Model);
            this.tabPage4.Controls.Add(this.label18);
            this.tabPage4.Controls.Add(this.label17);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.label13);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.S_PWD);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Size = new System.Drawing.Size(904, 504);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Setting";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // S_ServerMOCPath
            // 
            this.S_ServerMOCPath.Location = new System.Drawing.Point(167, 204);
            this.S_ServerMOCPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_ServerMOCPath.Name = "S_ServerMOCPath";
            this.S_ServerMOCPath.Size = new System.Drawing.Size(591, 22);
            this.S_ServerMOCPath.TabIndex = 29;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(28, 209);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(114, 17);
            this.label19.TabIndex = 28;
            this.label19.Text = "ServerMOCPath:";
            // 
            // S_Cancel
            // 
            this.S_Cancel.Location = new System.Drawing.Point(659, 380);
            this.S_Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_Cancel.Name = "S_Cancel";
            this.S_Cancel.Size = new System.Drawing.Size(100, 31);
            this.S_Cancel.TabIndex = 27;
            this.S_Cancel.Text = "Cancel";
            this.S_Cancel.UseVisualStyleBackColor = true;
            this.S_Cancel.Click += new System.EventHandler(this.S_Cancel_Click);
            // 
            // S_OK
            // 
            this.S_OK.Location = new System.Drawing.Point(517, 380);
            this.S_OK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_OK.Name = "S_OK";
            this.S_OK.Size = new System.Drawing.Size(100, 31);
            this.S_OK.TabIndex = 26;
            this.S_OK.Text = "OK";
            this.S_OK.UseVisualStyleBackColor = true;
            this.S_OK.Click += new System.EventHandler(this.S_OK_Click);
            // 
            // S_BOMVersion
            // 
            this.S_BOMVersion.Location = new System.Drawing.Point(625, 309);
            this.S_BOMVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_BOMVersion.Name = "S_BOMVersion";
            this.S_BOMVersion.Size = new System.Drawing.Size(132, 22);
            this.S_BOMVersion.TabIndex = 25;
            // 
            // S_FCDVersion
            // 
            this.S_FCDVersion.Location = new System.Drawing.Point(625, 274);
            this.S_FCDVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_FCDVersion.Name = "S_FCDVersion";
            this.S_FCDVersion.Size = new System.Drawing.Size(132, 22);
            this.S_FCDVersion.TabIndex = 24;
            // 
            // S_FWVersion
            // 
            this.S_FWVersion.Location = new System.Drawing.Point(625, 240);
            this.S_FWVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_FWVersion.Name = "S_FWVersion";
            this.S_FWVersion.Size = new System.Drawing.Size(132, 22);
            this.S_FWVersion.TabIndex = 23;
            // 
            // S_FIXCom
            // 
            this.S_FIXCom.Location = new System.Drawing.Point(167, 309);
            this.S_FIXCom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_FIXCom.Name = "S_FIXCom";
            this.S_FIXCom.Size = new System.Drawing.Size(132, 22);
            this.S_FIXCom.TabIndex = 22;
            // 
            // S_DUTCom
            // 
            this.S_DUTCom.Location = new System.Drawing.Point(167, 274);
            this.S_DUTCom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_DUTCom.Name = "S_DUTCom";
            this.S_DUTCom.Size = new System.Drawing.Size(132, 22);
            this.S_DUTCom.TabIndex = 21;
            // 
            // S_SFISCom
            // 
            this.S_SFISCom.Location = new System.Drawing.Point(167, 240);
            this.S_SFISCom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_SFISCom.Name = "S_SFISCom";
            this.S_SFISCom.Size = new System.Drawing.Size(132, 22);
            this.S_SFISCom.TabIndex = 20;
            // 
            // S_LocalLogPath
            // 
            this.S_LocalLogPath.Location = new System.Drawing.Point(167, 95);
            this.S_LocalLogPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_LocalLogPath.Name = "S_LocalLogPath";
            this.S_LocalLogPath.Size = new System.Drawing.Size(591, 22);
            this.S_LocalLogPath.TabIndex = 19;
            // 
            // S_ServerLogPath
            // 
            this.S_ServerLogPath.Location = new System.Drawing.Point(167, 130);
            this.S_ServerLogPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_ServerLogPath.Name = "S_ServerLogPath";
            this.S_ServerLogPath.Size = new System.Drawing.Size(591, 22);
            this.S_ServerLogPath.TabIndex = 18;
            // 
            // S_ServerProgramPath
            // 
            this.S_ServerProgramPath.Location = new System.Drawing.Point(167, 166);
            this.S_ServerProgramPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_ServerProgramPath.Name = "S_ServerProgramPath";
            this.S_ServerProgramPath.Size = new System.Drawing.Size(591, 22);
            this.S_ServerProgramPath.TabIndex = 17;
            // 
            // S_StationNO
            // 
            this.S_StationNO.Location = new System.Drawing.Point(625, 58);
            this.S_StationNO.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_StationNO.Name = "S_StationNO";
            this.S_StationNO.Size = new System.Drawing.Size(132, 22);
            this.S_StationNO.TabIndex = 16;
            // 
            // S_Station
            // 
            this.S_Station.Location = new System.Drawing.Point(376, 58);
            this.S_Station.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_Station.Name = "S_Station";
            this.S_Station.Size = new System.Drawing.Size(132, 22);
            this.S_Station.TabIndex = 15;
            // 
            // S_JavaPath
            // 
            this.S_JavaPath.Location = new System.Drawing.Point(0, 0);
            this.S_JavaPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_JavaPath.Name = "S_JavaPath";
            this.S_JavaPath.Size = new System.Drawing.Size(132, 22);
            this.S_JavaPath.TabIndex = 30;
            // 
            // S_JavaWindows
            // 
            this.S_JavaWindows.Location = new System.Drawing.Point(0, 0);
            this.S_JavaWindows.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_JavaWindows.Name = "S_JavaWindows";
            this.S_JavaWindows.Size = new System.Drawing.Size(132, 22);
            this.S_JavaWindows.TabIndex = 31;
            // 
            // S_Model
            // 
            this.S_Model.Location = new System.Drawing.Point(167, 58);
            this.S_Model.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_Model.Name = "S_Model";
            this.S_Model.Size = new System.Drawing.Size(132, 22);
            this.S_Model.TabIndex = 14;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label18.Location = new System.Drawing.Point(28, 62);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 17);
            this.label18.TabIndex = 13;
            this.label18.Text = "Model:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 102);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 17);
            this.label17.TabIndex = 12;
            this.label17.Text = "LocalLogPath:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(28, 138);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 17);
            this.label16.TabIndex = 11;
            this.label16.Text = "ServerLogPath:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(28, 172);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(137, 17);
            this.label15.TabIndex = 10;
            this.label15.Text = "ServerProgramPath:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(28, 244);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 17);
            this.label14.TabIndex = 9;
            this.label14.Text = "COM_SFIS:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 278);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 17);
            this.label13.TabIndex = 8;
            this.label13.Text = "COM_DUT:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(28, 314);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 17);
            this.label12.TabIndex = 7;
            this.label12.Text = "COM_FIX:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(487, 244);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 17);
            this.label11.TabIndex = 6;
            this.label11.Text = "FWVersion:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(487, 278);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 17);
            this.label10.TabIndex = 5;
            this.label10.Text = "FCDVersion:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(487, 314);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "BOMVersion:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Location = new System.Drawing.Point(539, 62);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "Station NO:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Location = new System.Drawing.Point(315, 62);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Station:";
            // 
            // S_PWD
            // 
            this.S_PWD.Location = new System.Drawing.Point(167, 18);
            this.S_PWD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.S_PWD.Name = "S_PWD";
            this.S_PWD.PasswordChar = '*';
            this.S_PWD.Size = new System.Drawing.Size(132, 22);
            this.S_PWD.TabIndex = 1;
            this.S_PWD.TextChanged += new System.EventHandler(this.S_PWD_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Password:";
            // 
            // timer_autoScan
            // 
            this.timer_autoScan.Interval = 2000;
            this.timer_autoScan.Tick += new System.EventHandler(this.timer_autoScan_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(1333, 683);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.IO.Ports.SerialPort com_SFIS;
        private System.IO.Ports.SerialPort com_DUT;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel Label_Pass;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel Label_Fail;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel Label_Retest;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel Label_Time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb_SfisON;
        private System.Windows.Forms.RadioButton rb_SfisOff;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_Result;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_Error;
        private System.Windows.Forms.Label label_scan;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripStatusLabel label_Ver;
        private System.Windows.Forms.Timer timer_CheckFinish;
        private System.Windows.Forms.Timer timer_Count;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label_Model;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label_Station;
        private System.IO.Ports.SerialPort com_fixture;
        private System.IO.Ports.SerialPort com_fixtureUSB;
        private System.Windows.Forms.Timer timer_checkSFC;
        private System.IO.Ports.SerialPort comm_SoftLED;
        private System.IO.Ports.SerialPort com_Laser;
        private System.Windows.Forms.Button btn_Laser;
        private System.IO.Ports.SerialPort Com_LedTest;
        private System.Windows.Forms.RadioButton rb_Golden;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txLoopNum;
        private System.Windows.Forms.Label loop_Retest;
        private System.Windows.Forms.Label loop_Fail;
        private System.Windows.Forms.Label loop_Pass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbEnableLoop;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Timer timer_autoScan;
        private System.Windows.Forms.Label label_StationNO;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox S_PWD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox S_BOMVersion;
        private System.Windows.Forms.TextBox S_FCDVersion;
        private System.Windows.Forms.TextBox S_FWVersion;
        private System.Windows.Forms.TextBox S_FIXCom;
        private System.Windows.Forms.TextBox S_DUTCom;
        private System.Windows.Forms.TextBox S_SFISCom;
        private System.Windows.Forms.TextBox S_LocalLogPath;
        private System.Windows.Forms.TextBox S_ServerLogPath;
        private System.Windows.Forms.TextBox S_ServerProgramPath;
        private System.Windows.Forms.TextBox S_StationNO;
        private System.Windows.Forms.TextBox S_Station;
        private System.Windows.Forms.TextBox S_Model;
        private System.Windows.Forms.TextBox S_JavaPath;
        private System.Windows.Forms.TextBox S_JavaWindows;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button S_Cancel;
        private System.Windows.Forms.Button S_OK;
        private System.Windows.Forms.TextBox S_ServerMOCPath;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
    }
}

