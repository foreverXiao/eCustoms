﻿namespace eCustoms
{
    partial class GetBomDataForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetBomDataForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.llblMessage = new System.Windows.Forms.LinkLabel();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSearchAndUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.gBoxShow = new System.Windows.Forms.GroupBox();
            this.FindAndUploadExcelReport = new System.Windows.Forms.Button();
            this.txtPathRpt = new System.Windows.Forms.TextBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.dgvBOM = new System.Windows.Forms.DataGridView();
            this.dgvCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cmsFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChooseFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcludeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefreshFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRecordFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBlankFieldFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.fakeLabel1 = new UserControls.FakeLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPathBom = new System.Windows.Forms.TextBox();
            this.btnSearchBom = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.gBoxShow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOM)).BeginInit();
            this.cmsFilter.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.llblMessage);
            this.panel1.Controls.Add(this.txtPath);
            this.panel1.Controls.Add(this.btnSearchAndUpload);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 76);
            this.panel1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "- Breakdown FG compoent into more detail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(5, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "- Stop the update if BOM exists in history record";
            // 
            // llblMessage
            // 
            this.llblMessage.AutoSize = true;
            this.llblMessage.Font = new System.Drawing.Font("Microsoft YaHei", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llblMessage.ForeColor = System.Drawing.Color.Navy;
            this.llblMessage.LinkColor = System.Drawing.Color.Navy;
            this.llblMessage.Location = new System.Drawing.Point(591, 41);
            this.llblMessage.Name = "llblMessage";
            this.llblMessage.Size = new System.Drawing.Size(87, 17);
            this.llblMessage.TabIndex = 6;
            this.llblMessage.TabStop = true;
            this.llblMessage.Text = "Specification";
            this.llblMessage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblMessage_LinkClicked);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtPath.Location = new System.Drawing.Point(266, 8);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(413, 22);
            this.txtPath.TabIndex = 4;
            // 
            // btnSearchAndUpload
            // 
            this.btnSearchAndUpload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchAndUpload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnSearchAndUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchAndUpload.Image")));
            this.btnSearchAndUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchAndUpload.Location = new System.Drawing.Point(265, 34);
            this.btnSearchAndUpload.Name = "btnSearchAndUpload";
            this.btnSearchAndUpload.Size = new System.Drawing.Size(263, 33);
            this.btnSearchAndUpload.TabIndex = 5;
            this.btnSearchAndUpload.Text = "Search and Upload Excel Draft BOM";
            this.btnSearchAndUpload.UseVisualStyleBackColor = true;
            this.btnSearchAndUpload.Click += new System.EventHandler(this.btnSearchAndUploadDraftBOM_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Batch Upload New BOM";
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.gBoxShow);
            this.groupBox.Controls.Add(this.btnShow);
            this.groupBox.Controls.Add(this.dgvBOM);
            this.groupBox.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox.ForeColor = System.Drawing.Color.Navy;
            this.groupBox.Location = new System.Drawing.Point(1, 82);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(1231, 582);
            this.groupBox.TabIndex = 22;
            this.groupBox.TabStop = false;
            // 
            // gBoxShow
            // 
            this.gBoxShow.Controls.Add(this.FindAndUploadExcelReport);
            this.gBoxShow.Controls.Add(this.txtPathRpt);
            this.gBoxShow.ForeColor = System.Drawing.Color.Navy;
            this.gBoxShow.Location = new System.Drawing.Point(190, 20);
            this.gBoxShow.Name = "gBoxShow";
            this.gBoxShow.Size = new System.Drawing.Size(808, 60);
            this.gBoxShow.TabIndex = 35;
            this.gBoxShow.TabStop = false;
            this.gBoxShow.Text = "Transform SAP COOISPI report to draft BOM";
            this.gBoxShow.Visible = false;
            // 
            // FindAndUploadExcelReport
            // 
            this.FindAndUploadExcelReport.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FindAndUploadExcelReport.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.FindAndUploadExcelReport.Image = ((System.Drawing.Image)(resources.GetObject("FindAndUploadExcelReport.Image")));
            this.FindAndUploadExcelReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.FindAndUploadExcelReport.Location = new System.Drawing.Point(7, 21);
            this.FindAndUploadExcelReport.Name = "FindAndUploadExcelReport";
            this.FindAndUploadExcelReport.Size = new System.Drawing.Size(253, 30);
            this.FindAndUploadExcelReport.TabIndex = 35;
            this.FindAndUploadExcelReport.Text = "Find and Upload SAP Excel Report ";
            this.FindAndUploadExcelReport.UseVisualStyleBackColor = true;
            this.FindAndUploadExcelReport.Click += new System.EventHandler(this.FindAndUploadExcelReport_Click);
            // 
            // txtPathRpt
            // 
            this.txtPathRpt.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtPathRpt.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPathRpt.Location = new System.Drawing.Point(266, 27);
            this.txtPathRpt.Name = "txtPathRpt";
            this.txtPathRpt.Size = new System.Drawing.Size(531, 22);
            this.txtPathRpt.TabIndex = 36;
            // 
            // btnShow
            // 
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShow.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShow.Location = new System.Drawing.Point(3, 19);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(41, 23);
            this.btnShow.TabIndex = 33;
            this.btnShow.Text = "*";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // dgvBOM
            // 
            this.dgvBOM.AllowUserToAddRows = false;
            this.dgvBOM.AllowUserToDeleteRows = false;
            this.dgvBOM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBOM.BackgroundColor = System.Drawing.Color.LightGray;
            this.dgvBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvBOM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCheck,
            this.dgvDelete});
            this.dgvBOM.ContextMenuStrip = this.cmsFilter;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBOM.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBOM.GridColor = System.Drawing.Color.YellowGreen;
            this.dgvBOM.Location = new System.Drawing.Point(3, 19);
            this.dgvBOM.Name = "dgvBOM";
            this.dgvBOM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBOM.RowTemplate.Height = 23;
            this.dgvBOM.Size = new System.Drawing.Size(1225, 560);
            this.dgvBOM.TabIndex = 23;
            this.dgvBOM.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBOM_CellClick);
            this.dgvBOM.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBOM_ColumnHeaderMouseClick);
            this.dgvBOM.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvBOM_MouseUp);
            // 
            // dgvCheck
            // 
            this.dgvCheck.HeaderText = "Select";
            this.dgvCheck.Name = "dgvCheck";
            this.dgvCheck.Width = 48;
            // 
            // dgvDelete
            // 
            this.dgvDelete.HeaderText = "";
            this.dgvDelete.Name = "dgvDelete";
            this.dgvDelete.Text = "Delete";
            this.dgvDelete.UseColumnTextForButtonValue = true;
            this.dgvDelete.Width = 5;
            // 
            // cmsFilter
            // 
            this.cmsFilter.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseFilter,
            this.tsmiExcludeFilter,
            this.tsmiRefreshFilter,
            this.tsmiRecordFilter,
            this.tsmiBlankFieldFilter});
            this.cmsFilter.Name = "cmsFilter";
            this.cmsFilter.Size = new System.Drawing.Size(166, 154);
            // 
            // tsmiChooseFilter
            // 
            this.tsmiChooseFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiChooseFilter.Image")));
            this.tsmiChooseFilter.Name = "tsmiChooseFilter";
            this.tsmiChooseFilter.Size = new System.Drawing.Size(165, 30);
            this.tsmiChooseFilter.Text = "Choose Filter";
            this.tsmiChooseFilter.Click += new System.EventHandler(this.tsmiChooseFilter_Click);
            // 
            // tsmiExcludeFilter
            // 
            this.tsmiExcludeFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExcludeFilter.Image")));
            this.tsmiExcludeFilter.Name = "tsmiExcludeFilter";
            this.tsmiExcludeFilter.Size = new System.Drawing.Size(165, 30);
            this.tsmiExcludeFilter.Text = "Exclude Filter";
            this.tsmiExcludeFilter.Click += new System.EventHandler(this.tsmiExcludeFilter_Click);
            // 
            // tsmiRefreshFilter
            // 
            this.tsmiRefreshFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRefreshFilter.Image")));
            this.tsmiRefreshFilter.Name = "tsmiRefreshFilter";
            this.tsmiRefreshFilter.Size = new System.Drawing.Size(165, 30);
            this.tsmiRefreshFilter.Text = "Refresh Filter";
            this.tsmiRefreshFilter.Click += new System.EventHandler(this.tsmiRefreshFilter_Click);
            // 
            // tsmiRecordFilter
            // 
            this.tsmiRecordFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRecordFilter.Image")));
            this.tsmiRecordFilter.Name = "tsmiRecordFilter";
            this.tsmiRecordFilter.Size = new System.Drawing.Size(165, 30);
            this.tsmiRecordFilter.Text = "Record Filter";
            this.tsmiRecordFilter.Click += new System.EventHandler(this.tsmiRecordFilter_Click);
            // 
            // tsmiBlankFieldFilter
            // 
            this.tsmiBlankFieldFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiBlankFieldFilter.Image")));
            this.tsmiBlankFieldFilter.Name = "tsmiBlankFieldFilter";
            this.tsmiBlankFieldFilter.Size = new System.Drawing.Size(165, 30);
            this.tsmiBlankFieldFilter.Text = "BlankField Filter";
            this.tsmiBlankFieldFilter.Click += new System.EventHandler(this.tsmiBlankFieldFilter_Click);
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
            this.fakeLabel1.Location = new System.Drawing.Point(1, 79);
            this.fakeLabel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fakeLabel1.Name = "fakeLabel1";
            this.fakeLabel1.Size = new System.Drawing.Size(1232, 11);
            this.fakeLabel1.TabIndex = 21;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnPreview);
            this.panel3.Controls.Add(this.btnDownload);
            this.panel3.Location = new System.Drawing.Point(695, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(214, 76);
            this.panel3.TabIndex = 13;
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreview.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnPreview.Location = new System.Drawing.Point(107, 7);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(90, 60);
            this.btnPreview.TabIndex = 12;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDownload.Location = new System.Drawing.Point(10, 8);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(90, 60);
            this.btnDownload.TabIndex = 11;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(235)))), ((int)(((byte)(140)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.txtPathBom);
            this.panel4.Controls.Add(this.btnSearchBom);
            this.panel4.Location = new System.Drawing.Point(905, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(328, 76);
            this.panel4.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(8, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "only redo one.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(8, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "- Every time ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(8, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "Freeze BOM";
            // 
            // txtPathBom
            // 
            this.txtPathBom.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.txtPathBom.Location = new System.Drawing.Point(102, 7);
            this.txtPathBom.Name = "txtPathBom";
            this.txtPathBom.Size = new System.Drawing.Size(210, 22);
            this.txtPathBom.TabIndex = 17;
            // 
            // btnSearchBom
            // 
            this.btnSearchBom.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchBom.ForeColor = System.Drawing.Color.Blue;
            this.btnSearchBom.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchBom.Image")));
            this.btnSearchBom.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchBom.Location = new System.Drawing.Point(102, 33);
            this.btnSearchBom.Name = "btnSearchBom";
            this.btnSearchBom.Size = new System.Drawing.Size(177, 33);
            this.btnSearchBom.TabIndex = 18;
            this.btnSearchBom.Text = "Search And Freeze Bom  ";
            this.btnSearchBom.UseVisualStyleBackColor = true;
            this.btnSearchBom.Click += new System.EventHandler(this.btnSearchBom_Click);
            // 
            // GetBomDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 665);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.fakeLabel1);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GetBomDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get BOM Data Operation Interface";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetBomDataForm_FormClosing);
            this.Load += new System.EventHandler(this.GetBomDataForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.gBoxShow.ResumeLayout(false);
            this.gBoxShow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOM)).EndInit();
            this.cmsFilter.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel llblMessage;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSearchAndUpload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox;
        private UserControls.FakeLabel fakeLabel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvBOM;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCheck;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDelete;
        private System.Windows.Forms.ContextMenuStrip cmsFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiChooseFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcludeFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefreshFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiRecordFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiBlankFieldFilter;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.TextBox txtPathBom;
        private System.Windows.Forms.Button btnSearchBom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox gBoxShow;
        private System.Windows.Forms.Button FindAndUploadExcelReport;
        private System.Windows.Forms.TextBox txtPathRpt;
    }
}