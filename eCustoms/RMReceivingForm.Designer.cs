namespace eCustoms
{
    partial class RMReceivingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMReceivingForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bnDownloadToEXCEL = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.cbColumnName = new System.Windows.Forms.CheckBox();
            this.cbCreationDates = new System.Windows.Forms.CheckBox();
            this.btnAdjust = new System.Windows.Forms.Button();
            this.cmbAdjustment = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtFieldName = new System.Windows.Forms.TextBox();
            this.cmbFieldName = new System.Windows.Forms.ComboBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvRMReceiving = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClickLink = new System.Windows.Forms.DataGridViewLinkColumn();
            this.fakeLabel2 = new UserControls.FakeLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvRMAdjustment = new System.Windows.Forms.DataGridView();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.llinkPrompt = new System.Windows.Forms.LinkLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSearchAndUpload = new System.Windows.Forms.Button();
            this.cmbSelectItem = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMReceiving)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMAdjustment)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.bnDownloadToEXCEL);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.cbColumnName);
            this.panel1.Controls.Add(this.cbCreationDates);
            this.panel1.Controls.Add(this.btnAdjust);
            this.panel1.Controls.Add(this.cmbAdjustment);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.txtFieldName);
            this.panel1.Controls.Add(this.cmbFieldName);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Location = new System.Drawing.Point(2, 111);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1846, 90);
            this.panel1.TabIndex = 23;
            // 
            // bnDownloadToEXCEL
            // 
            this.bnDownloadToEXCEL.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnDownloadToEXCEL.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.bnDownloadToEXCEL.Image = ((System.Drawing.Image)(resources.GetObject("bnDownloadToEXCEL.Image")));
            this.bnDownloadToEXCEL.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bnDownloadToEXCEL.Location = new System.Drawing.Point(1173, 14);
            this.bnDownloadToEXCEL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bnDownloadToEXCEL.Name = "bnDownloadToEXCEL";
            this.bnDownloadToEXCEL.Size = new System.Drawing.Size(153, 51);
            this.bnDownloadToEXCEL.TabIndex = 9;
            this.bnDownloadToEXCEL.Text = " Download";
            this.bnDownloadToEXCEL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bnDownloadToEXCEL.UseVisualStyleBackColor = true;
            this.bnDownloadToEXCEL.Click += new System.EventHandler(this.bnDownloadToEXCEL_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowse.Location = new System.Drawing.Point(1339, 14);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(147, 51);
            this.btnBrowse.TabIndex = 18;
            this.btnBrowse.Text = "  Preview";
            this.btnBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // cbColumnName
            // 
            this.cbColumnName.AutoSize = true;
            this.cbColumnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.cbColumnName.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.cbColumnName.Location = new System.Drawing.Point(8, 25);
            this.cbColumnName.Name = "cbColumnName";
            this.cbColumnName.Size = new System.Drawing.Size(160, 26);
            this.cbColumnName.TabIndex = 24;
            this.cbColumnName.Text = "Column Name";
            this.cbColumnName.UseVisualStyleBackColor = true;
            this.cbColumnName.CheckedChanged += new System.EventHandler(this.cbColumnName_CheckedChanged);
            // 
            // cbCreationDates
            // 
            this.cbCreationDates.AutoSize = true;
            this.cbCreationDates.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.cbCreationDates.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.cbCreationDates.Location = new System.Drawing.Point(597, 25);
            this.cbCreationDates.Name = "cbCreationDates";
            this.cbCreationDates.Size = new System.Drawing.Size(177, 26);
            this.cbCreationDates.TabIndex = 23;
            this.cbCreationDates.Text = "Create between";
            this.cbCreationDates.UseVisualStyleBackColor = true;
            this.cbCreationDates.CheckedChanged += new System.EventHandler(this.cbCreationDates_CheckedChanged);
            // 
            // btnAdjust
            // 
            this.btnAdjust.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdjust.ForeColor = System.Drawing.Color.Blue;
            this.btnAdjust.Image = ((System.Drawing.Image)(resources.GetObject("btnAdjust.Image")));
            this.btnAdjust.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdjust.Location = new System.Drawing.Point(1680, 17);
            this.btnAdjust.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdjust.Name = "btnAdjust";
            this.btnAdjust.Size = new System.Drawing.Size(135, 51);
            this.btnAdjust.TabIndex = 22;
            this.btnAdjust.Text = "   Adjust";
            this.btnAdjust.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdjust.UseVisualStyleBackColor = true;
            this.btnAdjust.Click += new System.EventHandler(this.btnAdjust_Click);
            // 
            // cmbAdjustment
            // 
            this.cmbAdjustment.AutoCompleteCustomSource.AddRange(new string[] {
            "",
            "Edition",
            "Deletion"});
            this.cmbAdjustment.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.cmbAdjustment.FormattingEnabled = true;
            this.cmbAdjustment.Items.AddRange(new object[] {
            "Edition",
            "Deletion"});
            this.cmbAdjustment.Location = new System.Drawing.Point(1514, 23);
            this.cmbAdjustment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAdjustment.Name = "cmbAdjustment";
            this.cmbAdjustment.Size = new System.Drawing.Size(148, 29);
            this.cmbAdjustment.TabIndex = 21;
            this.cmbAdjustment.SelectedIndexChanged += new System.EventHandler(this.cmbAdjustment_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(1326, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 25);
            this.label3.TabIndex = 20;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(1500, -9);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Size = new System.Drawing.Size(3, 98);
            this.groupBox6.TabIndex = 19;
            this.groupBox6.TabStop = false;
            // 
            // txtFieldName
            // 
            this.txtFieldName.Enabled = false;
            this.txtFieldName.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtFieldName.Location = new System.Drawing.Point(345, 22);
            this.txtFieldName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFieldName.Name = "txtFieldName";
            this.txtFieldName.Size = new System.Drawing.Size(238, 29);
            this.txtFieldName.TabIndex = 13;
            // 
            // cmbFieldName
            // 
            this.cmbFieldName.Enabled = false;
            this.cmbFieldName.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.cmbFieldName.FormattingEnabled = true;
            this.cmbFieldName.Items.AddRange(new object[] {
            "Item No",
            "Lot No",
            "BGD No"});
            this.cmbFieldName.Location = new System.Drawing.Point(189, 22);
            this.cmbFieldName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbFieldName.Name = "cmbFieldName";
            this.cmbFieldName.Size = new System.Drawing.Size(148, 29);
            this.cmbFieldName.TabIndex = 12;
            // 
            // dtpTo
            // 
            this.dtpTo.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Enabled = false;
            this.dtpTo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(1001, 22);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(163, 31);
            this.dtpTo.TabIndex = 17;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            this.dtpTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpTo_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(949, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 25);
            this.label2.TabIndex = 16;
            this.label2.Text = "and";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CalendarFont = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.Enabled = false;
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(781, 22);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(163, 31);
            this.dtpFrom.TabIndex = 15;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvRMReceiving);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(2, 203);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1848, 646);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            // 
            // dgvRMReceiving
            // 
            this.dgvRMReceiving.AllowUserToAddRows = false;
            this.dgvRMReceiving.AllowUserToDeleteRows = false;
            this.dgvRMReceiving.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRMReceiving.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvRMReceiving.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRMReceiving.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRMReceiving.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.ClickLink});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRMReceiving.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRMReceiving.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRMReceiving.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvRMReceiving.Location = new System.Drawing.Point(4, 29);
            this.dgvRMReceiving.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvRMReceiving.Name = "dgvRMReceiving";
            this.dgvRMReceiving.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRMReceiving.RowTemplate.Height = 23;
            this.dgvRMReceiving.Size = new System.Drawing.Size(1840, 612);
            this.dgvRMReceiving.TabIndex = 24;
            this.dgvRMReceiving.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRMReceiving_CellClick);
            this.dgvRMReceiving.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRMReceiving_ColumnHeaderMouseClick);
            this.dgvRMReceiving.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvRMReceiving_MouseUp);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "Select";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 67;
            // 
            // ClickLink
            // 
            this.ClickLink.HeaderText = "";
            this.ClickLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ClickLink.Name = "ClickLink";
            this.ClickLink.Text = "Click";
            this.ClickLink.UseColumnTextForLinkValue = true;
            this.ClickLink.Width = 5;
            // 
            // fakeLabel2
            // 
            this.fakeLabel2.BackColor = System.Drawing.SystemColors.Control;
            this.fakeLabel2.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.fakeLabel2.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel2.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel2.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel2.CImage = null;
            this.fakeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.fakeLabel2.LabelText = "";
            this.fakeLabel2.Location = new System.Drawing.Point(2, 94);
            this.fakeLabel2.Margin = new System.Windows.Forms.Padding(6);
            this.fakeLabel2.Name = "fakeLabel2";
            this.fakeLabel2.Size = new System.Drawing.Size(1848, 17);
            this.fakeLabel2.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvRMAdjustment);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(2, 857);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1848, 158);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            // 
            // dgvRMAdjustment
            // 
            this.dgvRMAdjustment.AllowUserToAddRows = false;
            this.dgvRMAdjustment.AllowUserToDeleteRows = false;
            this.dgvRMAdjustment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRMAdjustment.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvRMAdjustment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRMAdjustment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRMAdjustment.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRMAdjustment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRMAdjustment.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvRMAdjustment.Location = new System.Drawing.Point(4, 29);
            this.dgvRMAdjustment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvRMAdjustment.Name = "dgvRMAdjustment";
            this.dgvRMAdjustment.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRMAdjustment.RowTemplate.Height = 23;
            this.dgvRMAdjustment.Size = new System.Drawing.Size(1840, 124);
            this.dgvRMAdjustment.TabIndex = 27;
            // 
            // fakeLabel1
            // 
            this.fakeLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.fakeLabel1.CFaceColorDark = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.fakeLabel1.CFaceColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(207)))));
            this.fakeLabel1.CFontAlignment = UserControls.Alignment.Left;
            this.fakeLabel1.CFontColor = System.Drawing.Color.DarkSlateBlue;
            this.fakeLabel1.CImage = null;
            this.fakeLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.fakeLabel1.LabelText = "";
            this.fakeLabel1.Location = new System.Drawing.Point(2, 851);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(6);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1848, 17);
            this.fakeLabel1.TabIndex = 26;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.llinkPrompt);
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Controls.Add(this.btnDownload);
            this.panel3.Controls.Add(this.txtPath);
            this.panel3.Controls.Add(this.btnSearchAndUpload);
            this.panel3.Controls.Add(this.cmbSelectItem);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Location = new System.Drawing.Point(2, 2);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1846, 90);
            this.panel3.TabIndex = 9;
            // 
            // llinkPrompt
            // 
            this.llinkPrompt.AutoSize = true;
            this.llinkPrompt.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.llinkPrompt.LinkColor = System.Drawing.Color.Navy;
            this.llinkPrompt.Location = new System.Drawing.Point(1497, 31);
            this.llinkPrompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llinkPrompt.Name = "llinkPrompt";
            this.llinkPrompt.Size = new System.Drawing.Size(126, 25);
            this.llinkPrompt.TabIndex = 6;
            this.llinkPrompt.TabStop = true;
            this.llinkPrompt.Text = "Specification";
            this.llinkPrompt.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llinkPrompt_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(1634, -11);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(3, 98);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDownload.Location = new System.Drawing.Point(1647, 5);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(170, 83);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = " Download\r\n All Records";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtPath.Location = new System.Drawing.Point(908, 26);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(582, 29);
            this.txtPath.TabIndex = 4;
            // 
            // btnSearchAndUpload
            // 
            this.btnSearchAndUpload.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearchAndUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchAndUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearchAndUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchAndUpload.Image")));
            this.btnSearchAndUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchAndUpload.Location = new System.Drawing.Point(680, 22);
            this.btnSearchAndUpload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearchAndUpload.Name = "btnSearchAndUpload";
            this.btnSearchAndUpload.Size = new System.Drawing.Size(218, 38);
            this.btnSearchAndUpload.TabIndex = 3;
            this.btnSearchAndUpload.Text = " Search and Upload";
            this.btnSearchAndUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearchAndUpload.UseVisualStyleBackColor = true;
            this.btnSearchAndUpload.Click += new System.EventHandler(this.btnSearchAndUpload_Click);
            // 
            // cmbSelectItem
            // 
            this.cmbSelectItem.Enabled = false;
            this.cmbSelectItem.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.cmbSelectItem.FormattingEnabled = true;
            this.cmbSelectItem.Items.AddRange(new object[] {
            "1-Import New Or Update existing records",
            "2-Batch Update Customs Rcvd Date",
            "3-Batch Update Receipts ID, Status And Gate In Date"});
            this.cmbSelectItem.Location = new System.Drawing.Point(196, 23);
            this.cmbSelectItem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSelectItem.Name = "cmbSelectItem";
            this.cmbSelectItem.Size = new System.Drawing.Size(463, 29);
            this.cmbSelectItem.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label10.Location = new System.Drawing.Point(33, 29);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(148, 25);
            this.label10.TabIndex = 1;
            this.label10.Text = "Selection Items";
            // 
            // RMReceivingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1833, 738);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.fakeLabel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RMReceivingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RM Receiving Operation Interface ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RMReceivingForm_FormClosing);
            this.Load += new System.EventHandler(this.RMReceivingForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMReceiving)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRMAdjustment)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvRMReceiving;
        private UserControls.FakeLabel fakeLabel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvRMAdjustment;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbSelectItem;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSearchAndUpload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbFieldName;
        private System.Windows.Forms.TextBox txtFieldName;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.ComboBox cmbAdjustment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel llinkPrompt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewLinkColumn ClickLink;
        private System.Windows.Forms.CheckBox cbCreationDates;
        private System.Windows.Forms.CheckBox cbColumnName;
        private System.Windows.Forms.Button bnDownloadToEXCEL;
    }
}