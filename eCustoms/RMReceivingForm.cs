using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace eCustoms
{
    public partial class RMReceivingForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable dTable = new DataTable();
        SqlLib sqlLib = new SqlLib();
        int iRowIndex = 0;

        private static RMReceivingForm RMReceivingFrm;
        public RMReceivingForm() { InitializeComponent(); }
        public static RMReceivingForm CreateInstance()
        {
            if (RMReceivingFrm == null || RMReceivingFrm.IsDisposed) { RMReceivingFrm = new RMReceivingForm(); }
            return RMReceivingFrm;
        }

        private void RMReceivingForm_Load(object sender, EventArgs e)
        {
            dtpFrom.Text= System.DateTime.Now.AddDays(-7).ToString("M/d/yyyy");
            dtpTo.Text = System.DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            cmbSelectItem.SelectedIndex = 0;
            cmbFieldName.SelectedIndex = 1;
            cmbAdjustment.SelectedIndex = 0;
        }

        private void RMReceivingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dTable.Dispose();
            sqlLib.Dispose(0);
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        { this.dtpFrom.CustomFormat = null; }
        private void dtpFrom_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpFrom.CustomFormat = " "; } }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        { this.dtpTo.CustomFormat = null; }
        private void dtpTo_KeyUp(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { this.dtpTo.CustomFormat = " "; } }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!cbColumnName.Checked && !cbCreationDates.Checked)
            {
                MessageBox.Show("Please make sure you apply the filer on dates and fields before preview the records in RM receiving table.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            String selectRecordsFromTableRMreceiving = " SELECT * FROM C_RMReceiving " + getSQL_WhereConditionsStatement() + " ORDER BY [Created Date] DESC, [Item No], [Lot No]";

            SqlConnection browseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (browseConn.State == ConnectionState.Closed) { browseConn.Open(); }
            SqlDataAdapter browseAdapterR = new SqlDataAdapter(selectRecordsFromTableRMreceiving, browseConn);
            DataTable dtFillRmRcvd = new DataTable();
            browseAdapterR.Fill(dtFillRmRcvd);
            browseAdapterR.Dispose();

            this.dgvRMReceiving.DataSource = dtFillRmRcvd;

            if (dtFillRmRcvd.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {               
                this.dgvRMReceiving.Rows[0].HeaderCell.Value = 1;
                for (int i = 2; i < this.dgvRMReceiving.ColumnCount; i++)
                { this.dgvRMReceiving.Columns[i].ReadOnly = true; }
                this.dgvRMReceiving.Columns[3].Frozen = true;
            }

            browseConn.Dispose();

            Cursor.Current = Cursors.Default;
        }

        private String getSQL_WhereConditionsStatement()
        {
            StringBuilder sqlWhereConditionsStatement = new StringBuilder("");
            try {
                sqlWhereConditionsStatement.Append(" WHERE ");
                if (cbColumnName.Checked)
                {
                    sqlWhereConditionsStatement.Append("[" + cmbFieldName.Text.Trim() + "] = '" + txtFieldName.Text.Trim() + "' ");
                }
                else
                {
                    sqlWhereConditionsStatement.Append(" 1=1 ");
                }

                sqlWhereConditionsStatement.Append(" And ");

                if (cbCreationDates.Checked)
                {
                    sqlWhereConditionsStatement.Append(" [Created Date] between '" + dtpFrom.Value.ToString("M/d/yyyy") + "' and '" + dtpTo.Value.ToString("M/d/yyyy") + "' ");
                }
                else
                {
                    sqlWhereConditionsStatement.Append(" 1=1 ");
                }
            }
            catch
            {
                sqlWhereConditionsStatement.Clear();
            }

            return sqlWhereConditionsStatement.ToString();
        }

        private void dgvRMReceiving_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvRMReceiving.RowCount; i++) { this.dgvRMReceiving[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvRMReceiving.RowCount; i++) { this.dgvRMReceiving[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvRMReceiving.RowCount; i++)
                    {
                        if (String.Compare(this.dgvRMReceiving[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvRMReceiving[0, i].Value = true; }
                        else { this.dgvRMReceiving[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }

            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvRMReceiving_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvRMReceiving.RowCount == 0) { return; }
            if (this.dgvRMReceiving.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvRMReceiving.RowCount; i++)
                {
                    if (String.Compare(this.dgvRMReceiving[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvRMReceiving.RowCount && iCount > 0)
                { this.dgvRMReceiving.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvRMReceiving.RowCount)
                { this.dgvRMReceiving.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvRMReceiving.Columns[0].HeaderText = "Select"; }
            }
        }

        private void dgvRMReceiving_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataTable AdjustTbl = new DataTable();
                for (int i = 2; i < this.dgvRMReceiving.ColumnCount; i++)
                { AdjustTbl.Columns.Add(this.dgvRMReceiving.Columns[i].Name, this.dgvRMReceiving.Columns[i].ValueType); }               

                iRowIndex = this.dgvRMReceiving.CurrentRow.Index;
                string strGoodsRcvdDate = this.dgvRMReceiving["Goods Rcvd Date", iRowIndex].Value.ToString().Trim();
                string strCustomsRcvdDate = this.dgvRMReceiving["Customs Rcvd Date", iRowIndex].Value.ToString().Trim();
                string strGateInDate = this.dgvRMReceiving["Gate In Date", iRowIndex].Value.ToString().Trim();

                DataRow dr = AdjustTbl.NewRow();
                dr["Transaction Type"] = this.dgvRMReceiving["Transaction Type", iRowIndex].Value.ToString().Trim();
                dr["Customs Entry No"] = this.dgvRMReceiving["Customs Entry No", iRowIndex].Value.ToString().Trim();
                dr["Inbound Delivery No"] = this.dgvRMReceiving["Inbound Delivery No", iRowIndex].Value.ToString().Trim();
                dr["Item No"] = this.dgvRMReceiving["Item No", iRowIndex].Value.ToString().Trim();
                dr["Item Description"] = this.dgvRMReceiving["Item Description", iRowIndex].Value.ToString().Trim();
                dr["Lot No"] = this.dgvRMReceiving["Lot No", iRowIndex].Value.ToString().Trim();
                dr["BGD No"] = this.dgvRMReceiving["BGD No", iRowIndex].Value.ToString().Trim();
                dr["RM EHB"] = this.dgvRMReceiving["RM EHB", iRowIndex].Value.ToString().Trim(); 
                dr["RM CHN Name"] = this.dgvRMReceiving["RM CHN Name", iRowIndex].Value.ToString().Trim();                
                dr["PO Invoice Qty"] = Convert.ToDecimal(this.dgvRMReceiving["PO Invoice Qty", iRowIndex].Value.ToString().Trim());
                dr["PO Invoice Amount"] = Convert.ToDecimal(this.dgvRMReceiving["PO Invoice Amount", iRowIndex].Value.ToString().Trim()); 
                dr["PO Unit Price"] = Convert.ToDecimal(this.dgvRMReceiving["PO Unit Price", iRowIndex].Value.ToString().Trim()); 
                dr["PO Currency"] = this.dgvRMReceiving["PO Currency", iRowIndex].Value.ToString().Trim();
                dr["SAP PO No"] = this.dgvRMReceiving["SAP PO No", iRowIndex].Value.ToString().Trim();
                dr["Rcvd SLOC"] = this.dgvRMReceiving["Rcvd SLOC", iRowIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strGoodsRcvdDate)) { dr["Goods Rcvd Date"] = DBNull.Value; }
                else { dr["Goods Rcvd Date"] = Convert.ToDateTime(strGoodsRcvdDate); }
                dr["ShipFrom Country"] = this.dgvRMReceiving["ShipFrom Country", iRowIndex].Value.ToString().Trim();
                dr["Country of Origin"] = this.dgvRMReceiving["Country of Origin", iRowIndex].Value.ToString().Trim();             
                if (String.IsNullOrEmpty(strCustomsRcvdDate)) { dr["Customs Rcvd Date"] = DBNull.Value; }
                else { dr["Customs Rcvd Date"] = Convert.ToDateTime(strCustomsRcvdDate); }                                            
                dr["Receipts ID"] = this.dgvRMReceiving["Receipts ID", iRowIndex].Value.ToString().Trim();
                dr["Receipts Status"] = this.dgvRMReceiving["Receipts Status", iRowIndex].Value.ToString().Trim();
                if (String.IsNullOrEmpty(strGateInDate)) { dr["Gate In Date"] = DBNull.Value; }
                else { dr["Gate In Date"] = Convert.ToDateTime(strGateInDate); }
                dr["Creater"] = this.dgvRMReceiving["Creater", iRowIndex].Value.ToString().Trim();
                dr["Created Date"] = this.dgvRMReceiving["Created Date", iRowIndex].Value.ToString().Trim();
                AdjustTbl.Rows.Add(dr);

                dTable = AdjustTbl.Copy();
                this.dgvRMAdjustment.DataSource = DBNull.Value;
                this.dgvRMAdjustment.DataSource = AdjustTbl;
                this.dgvRMAdjustment.ReadOnly = true;
                this.dgvRMAdjustment.Columns[3].Frozen = true;
                if (this.cmbAdjustment.SelectedIndex > 0) { this.cmbAdjustment.Text = String.Empty; }
            }
        }

        private void cmbAdjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dgvRMAdjustment.Rows.Count == 0) { return; }
            this.dgvRMAdjustment.ReadOnly = true;
            for (int k = 0; k < this.dgvRMAdjustment.ColumnCount; k++)
            { this.dgvRMAdjustment.Columns[k].HeaderCell.Style.BackColor = Color.FromKnownColor(KnownColor.HighlightText); }

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = Color.FromArgb(178, 235, 140);
            this.dgvRMAdjustment.EnableHeadersVisualStyles = false;
            this.dgvRMAdjustment.ReadOnly = false;

            if (this.cmbAdjustment.SelectedIndex == 0) { this.dgvRMAdjustment.ReadOnly = true; }
            this.dgvRMAdjustment.Columns["Item Description"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Invoice Qty"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Invoice Amount"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Unit Price"].Visible = false;
            this.dgvRMAdjustment.Columns["PO Currency"].Visible = false;
            this.dgvRMAdjustment.Columns["SAP PO No"].Visible = false;
            this.dgvRMAdjustment.Columns["Rcvd SLOC"].Visible = false;
            this.dgvRMAdjustment.Columns["Creater"].Visible = false;
            this.dgvRMAdjustment.Columns["Created Date"].Visible = false;

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Edition") == 0)
            {
                this.dgvRMAdjustment.Columns["Inbound Delivery No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["Item No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["Lot No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["BGD No"].ReadOnly = true;
                this.dgvRMAdjustment.Columns["RM EHB"].ReadOnly = true;               

                this.dgvRMAdjustment.Columns["Inbound Delivery No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["Item No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["Lot No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["BGD No"].HeaderCell.Style = cellStyle;
                this.dgvRMAdjustment.Columns["RM EHB"].HeaderCell.Style = cellStyle;              
            }

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Deletion") == 0)
            {
                for (int j = 0; j < this.dgvRMAdjustment.ColumnCount - 1; j++)
                {
                    this.dgvRMAdjustment.Columns[j].ReadOnly = true;
                    this.dgvRMAdjustment.Columns[j].HeaderCell.Style = cellStyle;
                }
            }
        }

        private void btnAdjust_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cmbAdjustment.Text.ToString().Trim()))
            {
                MessageBox.Show("Please select 'Adjustment Items' first.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.cmbAdjustment.Focus();
                return;
            }
            if (this.dgvRMAdjustment.RowCount == 0)
            {
                MessageBox.Show("No data need to adjust.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlCommand Comm = new SqlCommand();
            Comm.Connection = Conn;

            bool bIsBOM = false;
            Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = this.dgvRMReceiving["Item No", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = this.dgvRMReceiving["Lot No", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = this.dgvRMReceiving["RM EHB", iRowIndex].Value.ToString().Trim();
            Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = this.dgvRMReceiving["BGD No", iRowIndex].Value.ToString().Trim();
            Comm.CommandText = @"SELECT COUNT(*) FROM C_BOMDetail WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo AND [RM EHB] = @RMEHB AND [BGD No] = @BGDNo";
            int iRecord1 = Convert.ToInt32(Comm.ExecuteScalar());
            if (iRecord1 > 0) { bIsBOM = true; }
            Comm.Parameters.Clear();
                 
            string strTransactionType = this.dgvRMAdjustment["Transaction Type", 0].Value.ToString().Trim().ToUpper();
            string strCustomsEntryNo = this.dgvRMAdjustment["Customs Entry No", 0].Value.ToString().Trim().ToUpper();
            string strItemNo = this.dgvRMAdjustment["Item No", 0].Value.ToString().Trim().ToUpper();
            string strLotNo = this.dgvRMAdjustment["Lot No", 0].Value.ToString().Trim().ToUpper();
            string strBGDNo = this.dgvRMAdjustment["BGD No", 0].Value.ToString().Trim().ToUpper();                    
            string strRMEHB = this.dgvRMAdjustment["RM EHB", 0].Value.ToString().Trim().ToUpper();
            string strRMCHNName = this.dgvRMAdjustment["RM CHN Name", 0].Value.ToString().Trim().ToUpper();        
            string strShipFromCountry = this.dgvRMAdjustment["ShipFrom Country", 0].Value.ToString().Trim().ToUpper();
            string strCountryofOrigin = this.dgvRMAdjustment["Country of Origin", 0].Value.ToString().Trim().ToUpper();
            string strGoodsRcvdDate = this.dgvRMAdjustment["Goods Rcvd Date", 0].Value.ToString().Trim();
            string strCustomsRcvdDate = this.dgvRMAdjustment["Customs Rcvd Date", 0].Value.ToString().Trim();
            string strReceiptsID = this.dgvRMAdjustment["Receipts ID", 0].Value.ToString().Trim().Trim();
            string strReceiptsStatus = this.dgvRMAdjustment["Receipts Status", 0].Value.ToString().Trim().ToUpper();
            string strGateInDate = this.dgvRMAdjustment["Gate In Date", 0].Value.ToString().Trim().ToUpper();

            if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Edition") == 0)
            {
                string strObj = null;
                string strOldReceiptsID = dTable.Rows[0]["Receipts ID"].ToString().Trim().ToUpper();
                string strOldReceiptsStatus = dTable.Rows[0]["Receipts Status"].ToString().Trim().ToUpper();
                string strOldCutomsRcvdDate = dTable.Rows[0]["Customs Rcvd Date"].ToString().Trim();
                string strOldGateInDate = dTable.Rows[0]["Gate In Date"].ToString().Trim();             
                #region //Update C_RMReceiving table
                Comm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = strTransactionType;
                Comm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = strCustomsEntryNo;
                Comm.Parameters.Add("@RMCHNName", SqlDbType.NVarChar).Value = strRMCHNName;
                Comm.Parameters.Add("@ShipFromCountry", SqlDbType.NVarChar).Value = strShipFromCountry;
                Comm.Parameters.Add("@CountryofOrigin", SqlDbType.NVarChar).Value = strCountryofOrigin;
                if (String.IsNullOrEmpty(strGoodsRcvdDate)) { Comm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = DBNull.Value; }
                else { Comm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGoodsRcvdDate); }
                if (!String.IsNullOrEmpty(strOldCutomsRcvdDate) && !String.IsNullOrEmpty(strCustomsRcvdDate))
                {
                    Comm.Parameters.Add("@CustomsRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strCustomsRcvdDate);
                    strObj += ", [Customs Rcvd Date] = @CustomsRcvdDate";
                }
                if (!String.IsNullOrEmpty(strOldReceiptsID) && !String.IsNullOrEmpty(strReceiptsID))
                {
                    Comm.Parameters.Add("@ReceiptsID", SqlDbType.NVarChar).Value = strReceiptsID;
                    strObj += ", [Receipts ID] = @ReceiptsID";
                }
                if (!String.IsNullOrEmpty(strOldReceiptsStatus) && !String.IsNullOrEmpty(strReceiptsStatus))
                {
                    Comm.Parameters.Add("@ReceiptsStatus", SqlDbType.NVarChar).Value = strReceiptsStatus;
                    strObj += ", [Receipts Status] = @ReceiptsStatus";
                }
                if (!String.IsNullOrEmpty(strOldGateInDate) && !String.IsNullOrEmpty(strGateInDate))
                {
                    Comm.Parameters.Add("@GateInDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGateInDate);
                    strObj += ", [Gate In Date] = @GateInDate ";
                }
                Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;              
                Comm.CommandText = "UPDATE C_RMReceiving SET [Transaction Type] = @TransactionType, [Customs Entry No] = @CustomsEntryNo, [RM CHN Name] = @RMCHNName, " +
                                   "[ShipFrom Country] = @ShipFromCountry, [Country of Origin] = @CountryofOrigin, [Goods Rcvd Date] = @GoodsRcvdDate " + strObj +
                                   " WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                SqlTransaction Trans1 = Conn.BeginTransaction();
                Comm.Transaction = Trans1;
                try
                {
                    Comm.ExecuteNonQuery();
                    Trans1.Commit();
                }
                catch (Exception)
                {
                    Trans1.Rollback();
                    Trans1.Dispose();
                    throw;
                }
                finally
                { Comm.Parameters.Clear(); Comm.Dispose(); }
                #endregion

                this.dgvRMReceiving["Transaction Type", iRowIndex].Value = strTransactionType;
                this.dgvRMReceiving["Customs Entry No", iRowIndex].Value = strCustomsEntryNo;
                this.dgvRMReceiving["RM CHN Name", iRowIndex].Value = strRMCHNName;
                this.dgvRMReceiving["ShipFrom Country", iRowIndex].Value = strShipFromCountry;
                this.dgvRMReceiving["Country of Origin", iRowIndex].Value = strCountryofOrigin;
                if (String.IsNullOrEmpty(strGoodsRcvdDate)) { this.dgvRMReceiving["Goods Rcvd Date", iRowIndex].Value = DBNull.Value; }
                else { this.dgvRMReceiving["Goods Rcvd Date", iRowIndex].Value = Convert.ToDateTime(strGoodsRcvdDate); }
                if (!String.IsNullOrEmpty(strCustomsRcvdDate)) { this.dgvRMReceiving["Customs Rcvd Date", iRowIndex].Value = Convert.ToDateTime(strCustomsRcvdDate); }
                this.dgvRMReceiving["Receipts ID", iRowIndex].Value = strReceiptsID;
                this.dgvRMReceiving["Receipts Status", iRowIndex].Value = strReceiptsStatus;
                if (!String.IsNullOrEmpty(strGateInDate)) { this.dgvRMReceiving["Gate In Date", iRowIndex].Value = Convert.ToDateTime(strGateInDate); }

                if (MessageBox.Show("Successfully edit the relevant data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                { this.dgvRMAdjustment.DataSource = DBNull.Value; }
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            else //if (String.Compare(this.cmbAdjustment.Text.ToString().Trim(), "Deletion") == 0)
            {
                if (bIsBOM == false)
                {
                    #region //Delete data in C_RMReceiving table
                    Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                    Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                    Comm.CommandText = @"DELETE FROM C_RMReceiving WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo";
                    Comm.ExecuteNonQuery();
                    Comm.Parameters.Clear();
                    #endregion

                    #region //Delete data C_RMBalance table
                    if (String.IsNullOrEmpty(strCustomsRcvdDate)) 
                    {
                        Comm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = strItemNo;
                        Comm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = strLotNo;
                        Comm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = strBGDNo;
                        Comm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = strRMEHB;
                        Comm.CommandText = @"DELETE C_RMBalance WHERE [Item No] = @ItemNo AND [Lot No] = @LotNo AND [BGD No] = @BGDNo AND [RM EHB] = @RMEHB";
                        Comm.ExecuteNonQuery();
                        Comm.Parameters.Clear();
                    }
                    #endregion
                    this.cmbAdjustment.Text = string.Empty;
                    this.dgvRMReceiving.Rows.RemoveAt(iRowIndex);
                    Comm.Dispose();
                    if (MessageBox.Show("Successfully delete the relevant data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                    { this.dgvRMAdjustment.DataSource = DBNull.Value; }
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
                else
                { 
                    MessageBox.Show("Already generated BOM, deletion is terminated.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Comm.Dispose();
                    Conn.Close();
                    Conn.Dispose();
                    return;
                }
            }       
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to download all history data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            cbColumnName.Checked = false;
            cbCreationDates.Checked = true;
            dtpFrom.Text = System.DateTime.Now.AddYears(-20).ToString("M/d/yyyy");
            dtpTo.Text = System.DateTime.Now.AddYears(1).ToString("M/d/yyyy");

            bnDownloadToEXCEL_Click(sender, e);

            cbColumnName.Checked = false;
            cbCreationDates.Checked = false;
            dtpFrom.Text = System.DateTime.Now.AddDays(-7).ToString("M/d/yyyy");
            dtpTo.Text = System.DateTime.Now.AddDays(1).ToString("M/d/yyyy");

        }

        private void llinkPrompt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strSelectItem = this.cmbSelectItem.Text.Trim();
            if (String.IsNullOrEmpty(strSelectItem)) {
                MessageBox.Show("Please turn on the Selection Items.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (strSelectItem.Contains("1-"))
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tTransaction Type, \n\tCustoms Entry No, \n\tInbound Delivery No, \n\tItem No, \n\tItem Description, \n\tLot No, " +
                                "\n\tBGD No, \n\tRM EHB, \n\tRM CNH Name, \n\tPO Invoice Qty, \n\tPO Invoice Amount, \n\tPO Currency, \n\tSAP PO No, \n\tRcvd SLOC, " +
                                "\n\tGoods Rcvd Date, \n\tShipFrom Country, \n\tCountry of Origin", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (strSelectItem.Contains("2-"))
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tInbound Delivery No, \n\tItem No, \n\tLot No, \n\tBGD No, \n\tRM EHB, \n\tPO Invoice Qty, \n\tCustoms Rcvd Date", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else //if(strSelectItem.Contains("3-"))
            {
                MessageBox.Show("Please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                                "\n\tInbound Delivery No, \n\tItem No, \n\tLot No, \n\tReceipts ID, \n\tReceipts Status, \n\tGate In Date", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        } 

        private void btnSearchAndUpload_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            String pathAndFileName = funcLib.getExcelFileToBeUploaded(txtPath);
            if (!String.IsNullOrEmpty(pathAndFileName))
            {
                AddNewRecordsOrUpdateExistingOnesFromExcelFile(pathAndFileName);
            }
            Cursor.Current = Cursors.Default;
        }

  
        private void AddNewRecordsOrUpdateExistingOnesFromExcelFile(String strFilePath)
        {
            string strConn = SqlLib.getOleBbConnnectionStringPerSpeadsheetFileExtension(strFilePath);

            OleDbConnection ConnToSpreadsheet = new OleDbConnection(strConn);
            ConnToSpreadsheet.Open();
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", ConnToSpreadsheet);
            DataTable RM_ReceivingDataFromExcelFile = new DataTable();
            oleAdapter.Fill(RM_ReceivingDataFromExcelFile);
            oleAdapter.Dispose();
            ConnToSpreadsheet.Dispose();

            if (foundBadDataIntegrityInUploadExcelFileDataTable(RM_ReceivingDataFromExcelFile))
            { 
                RM_ReceivingDataFromExcelFile.Dispose();
                return;
            }


            funcLib.releaseExclusiveControlOverDataTable();
            if (!funcLib.currentUserToUseDataTableExclusively()) { return; };

            SqlConnection oneConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (oneConn.State == ConnectionState.Closed) { oneConn.Open(); }
            SqlCommand oneComm = new SqlCommand();
            oneComm.Connection = oneConn;

            SqlTransaction Trans1 = oneConn.BeginTransaction();
            oneComm.Transaction = Trans1;

            StringBuilder keyReferenceMessage = new StringBuilder("");
            try
            {
                String InboundDeliveryNo = String.Empty;
                String ItemNo = String.Empty;
                String LotNo = String.Empty;
                for (int i = 0; i < RM_ReceivingDataFromExcelFile.Rows.Count; i++)
                {
                    keyReferenceMessage.Clear();
                    keyReferenceMessage.Append("Record Number is " + (i+1).ToString());

                    InboundDeliveryNo = RM_ReceivingDataFromExcelFile.Rows[i]["Inbound Delivery No"].ToString().Trim().ToUpper();
                    ItemNo = RM_ReceivingDataFromExcelFile.Rows[i]["Item No"].ToString().Trim().ToUpper();
                    LotNo = RM_ReceivingDataFromExcelFile.Rows[i]["Lot No"].ToString().Trim().ToUpper();

                    keyReferenceMessage.Append("\n Inbound Delivery No is " + InboundDeliveryNo);
                    keyReferenceMessage.Append("\n Item No is " + ItemNo);
                    keyReferenceMessage.Append("\n Lot N is " + LotNo);

                    oneComm.CommandText = "SELECT * FROM C_RMReceiving WHERE " + "[Inbound Delivery No]='" + InboundDeliveryNo + "' AND [Item No]='" + ItemNo + "' AND [Lot No]='" + LotNo + "'";
                    Boolean hasRecord = Convert.ToInt32(oneComm.ExecuteScalar()) > 0;

                    Boolean OverwriteOldRecord = true;
                    if (hasRecord)
                    {
                        if (MessageBox.Show("Are you sure to overwrite the old record with the latest uploaded data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { OverwriteOldRecord = false; }
                    }

                    if (OverwriteOldRecord)
                    {
                        oneComm.CommandText = "DELETE FROM C_RMReceiving WHERE " + "[Inbound Delivery No]='" + InboundDeliveryNo + "' AND [Item No]='" + ItemNo + "' AND [Lot No]='" + LotNo + "'";
                        oneComm.ExecuteNonQuery();
                    }
                    else
                    {
                        continue;
                    }

                    oneComm.Parameters.Add("@TransactionType", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["Transaction Type"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@CustomsEntryNo", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["Customs Entry No"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@InboundDeliveryNo", SqlDbType.NVarChar).Value = InboundDeliveryNo;
                    oneComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = ItemNo;
                    oneComm.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["Item Description"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = LotNo;
                    oneComm.Parameters.Add("@BGDNo", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["BGD No"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@RMEHB", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["RM EHB"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@RMChnName", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["RM CHN Name"].ToString().Trim().ToUpper();
                    string strPoInvoiceQty = RM_ReceivingDataFromExcelFile.Rows[i]["PO Invoice Qty"].ToString().Trim();
                    if (String.IsNullOrEmpty(strPoInvoiceQty)) { oneComm.Parameters.Add("@PoInvoiceQty", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@PoInvoiceQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strPoInvoiceQty), 6); }
                    string strPoInvoiceAmt = RM_ReceivingDataFromExcelFile.Rows[i]["PO Invoice Amount"].ToString().Trim();
                    if (String.IsNullOrEmpty(strPoInvoiceAmt)) { oneComm.Parameters.Add("@PoInvoiceAmount", SqlDbType.Decimal).Value = 0.0M; }
                    else { oneComm.Parameters.Add("@PoInvoiceAmount", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(strPoInvoiceAmt), 2); }
                    decimal dPoUnitPrice = 0.0M;
                    if (!String.IsNullOrEmpty(strPoInvoiceQty)) { dPoUnitPrice = Math.Round(Convert.ToDecimal(strPoInvoiceAmt) / Convert.ToDecimal(strPoInvoiceQty), 2); }
                    oneComm.Parameters.Add("@PoUnitPrice", SqlDbType.Decimal).Value = dPoUnitPrice;
                    oneComm.Parameters.Add("@PoCurrency", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["PO Currency"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@SapPoNo", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["SAP PO No"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@RcvdSloc", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["Rcvd SLOC"].ToString().Trim().ToUpper();
                    string strGoodsRcvdDate = RM_ReceivingDataFromExcelFile.Rows[i]["Goods Rcvd Date"].ToString().Trim();
                    if (String.IsNullOrEmpty(strGoodsRcvdDate)) { oneComm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = DBNull.Value; }
                    else { oneComm.Parameters.Add("@GoodsRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(strGoodsRcvdDate); }
                    oneComm.Parameters.Add("@ShipFromCountry", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["ShipFrom Country"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@CountryofOrigin", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["Country of Origin"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = funcLib.getCurrentUserName();
                    oneComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy HH:mm:ss"));
                    string CustomsRcvdDate = RM_ReceivingDataFromExcelFile.Rows[i]["Customs Rcvd Date"].ToString().Trim();
                    if (String.IsNullOrEmpty(CustomsRcvdDate)) { oneComm.Parameters.Add("@CustomsRcvdDate", SqlDbType.DateTime).Value = DBNull.Value; }
                    else { oneComm.Parameters.Add("@CustomsRcvdDate", SqlDbType.DateTime).Value = Convert.ToDateTime(CustomsRcvdDate); }
                    oneComm.Parameters.Add("@ReceiptsID", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["Receipts ID"].ToString().Trim().ToUpper();
                    oneComm.Parameters.Add("@ReceiptsStatus", SqlDbType.NVarChar).Value = RM_ReceivingDataFromExcelFile.Rows[i]["Receipts Status"].ToString().Trim().ToUpper();
                    string GateInDate = RM_ReceivingDataFromExcelFile.Rows[i]["Gate In Date"].ToString().Trim();
                    if (String.IsNullOrEmpty(GateInDate)) { oneComm.Parameters.Add("@GateInDate", SqlDbType.DateTime).Value = DBNull.Value; }
                    else { oneComm.Parameters.Add("@GateInDate", SqlDbType.DateTime).Value = Convert.ToDateTime(GateInDate); }

                    oneComm.CommandText = "INSERT INTO C_RMReceiving([Transaction Type], [Customs Entry No], [Inbound Delivery No], [Item No], [Item Description], " +
                                            "[Lot No], [BGD No], [RM EHB], [RM CHN Name], [PO Invoice Qty], [PO Invoice Amount], [PO Unit Price], [PO Currency], " +
                                            "[SAP PO No], [Rcvd SLOC], [Goods Rcvd Date], [ShipFrom Country], [Country of Origin],[Customs Rcvd Date],[Receipts ID], [Receipts Status],[Gate In Date],[Creater], [Created Date]) " +
                                            "VALUES(@TransactionType, @CustomsEntryNo, @InboundDeliveryNo, @ItemNo, @ItemDescription, @LotNo, @BGDNo, @RMEHB, " +
                                            "@RMChnName, @PoInvoiceQty, @PoInvoiceAmount, @PoUnitPrice, @PoCurrency, @SapPoNo, @RcvdSloc, @GoodsRcvdDate," +
                                            "@ShipFromCountry, @CountryofOrigin, @CustomsRcvdDate, @ReceiptsID, @ReceiptsStatus, @GateInDate, @Creater, @CreatedDate)";
                    oneComm.ExecuteNonQuery();
                    oneComm.Parameters.Clear();                  
                }

                Trans1.Commit();
                MessageBox.Show("The number of records added or updated is " + RM_ReceivingDataFromExcelFile.Rows.Count , "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                Trans1.Rollback();
                MessageBox.Show("Something wrong: " + keyReferenceMessage.ToString() , "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Trans1.Dispose();
                oneComm.Dispose();
            }                    
            
            oneComm.Dispose();
            RM_ReceivingDataFromExcelFile.Dispose();
            if (oneConn.State == ConnectionState.Open) { oneConn.Close(); oneConn.Dispose(); }

            funcLib.releaseExclusiveControlOverDataTable();
        }
        
        private Boolean foundBadDataIntegrityInUploadExcelFileDataTable(DataTable RM_ReceivingDataFromExcelFile)
        {
            Boolean badDataInTable_RM_ReceivingDataFromExcelFile = false;

            if (RM_ReceivingDataFromExcelFile.Rows.Count == 0)
            {
                MessageBox.Show("There is no data in the uploaded spreadsheet file.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                badDataInTable_RM_ReceivingDataFromExcelFile = true;
            }

            try
            {
                for (int i = 0; i< RM_ReceivingDataFromExcelFile.Rows.Count; i++)
                {
                    if (String.IsNullOrEmpty(RM_ReceivingDataFromExcelFile.Rows[i]["Inbound Delivery No"].ToString()) 
                        || String.IsNullOrEmpty(RM_ReceivingDataFromExcelFile.Rows[i]["Item No"].ToString()) 
                        || String.IsNullOrEmpty(RM_ReceivingDataFromExcelFile.Rows[i]["Lot No"].ToString()))
                    {
                        MessageBox.Show("Empty value is forbidden for Inbound delivery no, item no and lot no. \n Please check the data in row " + (i+2) + " of Sheet 'Sheet1'.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        badDataInTable_RM_ReceivingDataFromExcelFile = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong with fields of Inbound delivery no, item no and lot no. ", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                badDataInTable_RM_ReceivingDataFromExcelFile = true;
            }
   
            return badDataInTable_RM_ReceivingDataFromExcelFile;
        }

        private void cbCreationDates_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = cbCreationDates.Checked;
            dtpTo.Enabled = cbCreationDates.Checked;
        }

        private void cbColumnName_CheckedChanged(object sender, EventArgs e)
        {
            cmbFieldName.Enabled = cbColumnName.Checked;
            txtFieldName.Enabled = cbColumnName.Checked;
        }

        private void bnDownloadToEXCEL_Click(object sender, EventArgs e)
        {
            
            if (!cbColumnName.Checked && !cbCreationDates.Checked)
            {
                MessageBox.Show("Please make sure you apply the filer on dates and fields before preview the records in RM receiving table.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            String selectRecordsFromTableRMreceiving = " SELECT * FROM C_RMReceiving " + getSQL_WhereConditionsStatement() + " ORDER BY [Created Date] DESC, [Item No], [Lot No]";

            SqlConnection browseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (browseConn.State == ConnectionState.Closed) { browseConn.Open(); }
            SqlDataAdapter browseAdapterR = new SqlDataAdapter(selectRecordsFromTableRMreceiving, browseConn);
            DataTable dtFillRmRcvd = new DataTable();
            browseAdapterR.Fill(dtFillRmRcvd);
            browseAdapterR.Dispose();
            browseConn.Dispose();

            dtFillRmRcvd.Columns.Remove("Creater");
            dtFillRmRcvd.Columns.Remove("Created Date");
            dtFillRmRcvd.AcceptChanges();

            Dictionary<string, DataTable> ExcelSheetNameAndDataTable = new Dictionary<string, DataTable>();
            ExcelSheetNameAndDataTable.Add("Sheet1", dtFillRmRcvd);

            funcLib.exportDataTablesToSpreadSheets(ExcelSheetNameAndDataTable);

            Cursor.Current = Cursors.Default;
        }
        
    }
}
