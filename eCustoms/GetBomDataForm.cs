using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Core;

namespace eCustoms
{
    public partial class GetBomDataForm : Form
    {
        private LoginForm loginFrm = new LoginForm();
        private DataTable dtExRate = new DataTable();
        protected DataView dataViewToFillDataGridView = new DataView();
        protected DataTable dataTableToFillDataGridView = new DataTable();
        protected PopUpFilterForm filterFrm = null;
        string strFilter = null;
        Regex regexLike_00_00_00Pattern = new Regex(@"(-\d\d){3}$"); // Strings look like '%-00-00-00' or '%-12-34-56',  Select method for DataTable does not work well with operator 'LIKE'
        private class returnMessageInTableAndString
        {
            public DataTable messageInDataTable;
            public String messageString;
        }

        DataTable dtRM_ITEM_ListInBOMDetail = new DataTable();//Component item can be found in BOM detail table (only input items for every BOM)
        DataTable FG_and_BatchNoListFromTableOverviewBOM = new DataTable(); //reycle or intermediate FG can be found in FG BOM table (Only output item for every BOM)
        DataTable dtBatchNoListInBOM_but_Different_FG_Item = new DataTable(); //Batch no can be found in FG BOM table but FG item is different because FG item is changed to recycle C-code item

        public GetBomDataForm() { InitializeComponent(); }
        private static GetBomDataForm getBomDataFrm;
        public static GetBomDataForm CreateInstance()
        {
            if (getBomDataFrm == null || getBomDataFrm.IsDisposed) { getBomDataFrm = new GetBomDataForm(); }
            return getBomDataFrm;
        }

        private void GetBomDataForm_Load(object sender, EventArgs e)
        {
            SqlConnection exeConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (exeConn.State == ConnectionState.Closed) { exeConn.Open(); };
            SqlCommand exeComm = new SqlCommand();
            exeComm.Connection = exeConn;
            SqlDataAdapter exeAdapter = new SqlDataAdapter();
            exeComm.CommandText = @"SELECT DISTINCT [FG NO], [Batch No] FROM C_BOM";
            exeAdapter.SelectCommand = exeComm;
            //DataTable FG_and_BatchNoListFromTableOverviewBOM = new DataTable();
            exeAdapter.Fill(FG_and_BatchNoListFromTableOverviewBOM);
            exeComm.CommandText = @"SELECT DISTINCT [Item No] FROM C_BOMDetail";
            exeAdapter.SelectCommand = exeComm;
            //DataTable dtRM_ITEM_ListInBOMDetail = new DataTable();
            exeAdapter.Fill(dtRM_ITEM_ListInBOMDetail);
            exeComm.Dispose();
            exeAdapter.Dispose();
            if (exeConn.State == ConnectionState.Open) { exeConn.Close(); exeConn.Dispose(); };
        }

        private void GetBomDataForm_FormClosing(object sender, FormClosingEventArgs e)
        { dtExRate.Dispose(); dataTableToFillDataGridView.Dispose(); dtRM_ITEM_ListInBOMDetail.Dispose(); FG_and_BatchNoListFromTableOverviewBOM.Dispose(); }

        private void dgvBOM_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string objHeader = dgv.Columns[0].HeaderText.Trim();

                if (objHeader == "Select")
                {
                    for (int i = 0; i < this.dgvBOM.RowCount; i++) { this.dgvBOM[0, i].Value = true; }
                    dgv.Columns[0].HeaderText = "Cancel";
                }
                else if (objHeader == "Cancel")
                {
                    for (int i = 0; i < this.dgvBOM.RowCount; i++) { this.dgvBOM[0, i].Value = false; }
                    dgv.Columns[0].HeaderText = "Select";
                }
                else if (objHeader == "Reverse")
                {
                    for (int i = 0; i < this.dgvBOM.RowCount; i++)
                    {
                        if (String.Compare(this.dgvBOM[0, i].EditedFormattedValue.ToString(), "False") == 0) { this.dgvBOM[0, i].Value = true; }

                        else { this.dgvBOM[0, i].Value = false; }
                    }
                    dgv.Columns[0].HeaderText = "Select";
                }
            }
            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void dgvBOM_MouseUp(object sender, MouseEventArgs e)
        {
            int iCount = 0;
            if (this.dgvBOM.RowCount == 0) { return; }
            if (this.dgvBOM.CurrentRow.Index >= 0)
            {
                for (int i = 0; i < this.dgvBOM.RowCount; i++)
                {
                    if (String.Compare(this.dgvBOM[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    { iCount++; }
                }

                if (iCount < this.dgvBOM.RowCount && iCount > 0)
                { this.dgvBOM.Columns[0].HeaderText = "Reverse"; }

                else if (iCount == this.dgvBOM.RowCount)
                { this.dgvBOM.Columns[0].HeaderText = "Cancel"; }

                else if (iCount == 0)
                { this.dgvBOM.Columns[0].HeaderText = "Select"; }
            }
        }

        private void dgvBOM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (MessageBox.Show("Are you sure to delete this BOM?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SqlConnection delConn = new SqlConnection(SqlLib.StrSqlConnection);
                    if (delConn.State == ConnectionState.Closed) { delConn.Open(); }
                    SqlCommand delComm = new SqlCommand();
                    delComm.Connection = delConn;

                    int iRow = this.dgvBOM.CurrentRow.Index;
                    string strBatchNo = this.dgvBOM["Batch No", iRow].Value.ToString().Trim();
                    string strFGNo = this.dgvBOM["FG No", iRow].Value.ToString().Trim();

                    //Remove the selected BOM data in DataGridView
                    DataRow[] dRow = dataViewToFillDataGridView.Table.Select("[Batch No] = '" + strBatchNo + "' AND [FG No] = '" + strFGNo + "'");
                    foreach (DataRow dr in dRow) { dr.Delete(); }
                    dataTableToFillDataGridView.AcceptChanges();

                    //Delete the selected BOM data in DataBase
                    delComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                    delComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                    delComm.CommandText = @"DELETE FROM M_DailyBOM WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                    delComm.ExecuteNonQuery();
                    delComm.Parameters.Clear();
                    delComm.Dispose();
                    if (delConn.State == ConnectionState.Open)
                    {
                        delConn.Close();
                        delConn.Dispose();
                    }
                }
            }
        }

        private void btnSearchAndUploadDraftBOM_Click(object sender, EventArgs e)
        {
            
            String pathAndFileName = funcLib.getExcelFileToBeUploaded(this.txtPath);
            if (!String.IsNullOrEmpty(pathAndFileName))
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ImportExcelData(pathAndFileName);
                ComsumptionRateEuqalTo100Percent();
                Cursor.Current = Cursors.Default;
            };
            
        }


        private void llblMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //show the field name list in spreadsheet file to be used for upload
        {
            MessageBox.Show("When upload BOM data, please follow below fields name and sequence to list out in Excel. {Sheet1 as Excel default name}" +
                            "\n\tProcess Order No \n\tActual Start Date \n\tActual End Date \n\tBatch No \n\tFG No \n\tFG Description \n\tLine No " +
                            "\n\tItem No \n\tItem Description \n\tLot No \n\tInventory Type \n\tFG Qty \n\tRM Qty " + 
                            "\n\tTotal Input Qty \n\tDrools Qty", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ImportExcelData(string strFilePath)
        {
            String strConn = SqlLib.getOleBbConnnectionStringPerSpeadsheetFileExtension(strFilePath);

            OleDbConnection ExcelOleConnection = new OleDbConnection(strConn);
            ExcelOleConnection.Open();
            OleDbDataAdapter ExcelFileDataAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$] WHERE [Batch No] IS NOT NULL AND [Batch No] <> ''", ExcelOleConnection);
            DataTable ComponentsAndOutputPerProcessOrder = new DataTable();
            ExcelFileDataAdapter.Fill(ComponentsAndOutputPerProcessOrder);
            ExcelFileDataAdapter.Dispose();
            ExcelOleConnection.Dispose();
            if (ComponentsAndOutputPerProcessOrder.Rows.Count == 0)
            {
                MessageBox.Show("There is no data. (Wrong Excel Sheet?)", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ComponentsAndOutputPerProcessOrder.Dispose();
                return;
            }
            string[] keyFieldsBatchNo = {"Batch No"};
            SqlLib sqlLib = new SqlLib();
            DataTable BOM_DetailsListForUpload = sqlLib.SelectDistinct(ComponentsAndOutputPerProcessOrder, keyFieldsBatchNo);
            sqlLib.Dispose(0);

            SqlConnection sqlDB_Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlDB_Conn.State == ConnectionState.Closed) { sqlDB_Conn.Open(); }
            SqlCommand sqlCommands = new SqlCommand();
            sqlCommands.Connection = sqlDB_Conn;
            
            //Delete those Batch No. in the temporary table M_DailyBOM if the same Batch No. to be uploaded from Excel file
            SqlDataAdapter BomAdapter = new SqlDataAdapter("SELECT DISTINCT [Batch No] FROM M_DailyBOM", sqlDB_Conn);
            DataTable distinctBatchNoListInTableDailyBOM = new DataTable();
            BomAdapter.Fill(distinctBatchNoListInTableDailyBOM);
            if (distinctBatchNoListInTableDailyBOM.Rows.Count > 0)
            {
                for (int i = 0; i < distinctBatchNoListInTableDailyBOM.Rows.Count; i++)
                {
                    string BatchNo = distinctBatchNoListInTableDailyBOM.Rows[i][0].ToString().Trim();
                    DataRow[] datarow = BOM_DetailsListForUpload.Select("[Batch No]='" + BatchNo + "'");
                    if (datarow.Length > 0)
                    {
                        sqlCommands.CommandText = "DELETE FROM M_DailyBOM WHERE [Batch No] = '" + BatchNo + "'";
                        sqlCommands.ExecuteNonQuery();
                    }
                }
            }
            distinctBatchNoListInTableDailyBOM.Dispose();

            // Check if BOM to be uploaded into system is alreay in BOM history record, if yes, do not upload BOM to system again
            for (int i = 0; i < BOM_DetailsListForUpload.Rows.Count; i++)
            {
                string strBomName = BOM_DetailsListForUpload.Rows[i][0].ToString().Trim();
                DataRow[] datarow = FG_and_BatchNoListFromTableOverviewBOM.Select("[Batch No]='" + strBomName + "'");
                if (datarow.Length > 0)
                {
                    DataRow[] drow = ComponentsAndOutputPerProcessOrder.Select("[Batch No]='" + strBomName + "'");
                    {
                        foreach (DataRow dr in drow)
                        { ComponentsAndOutputPerProcessOrder.Rows.Remove(dr); }
                    }
                    ComponentsAndOutputPerProcessOrder.AcceptChanges();
                }
            }
            BOM_DetailsListForUpload.Dispose();
            
            if (ComponentsAndOutputPerProcessOrder.Rows.Count == 0)
            {
                MessageBox.Show("There is no new BOM to be uploaded. (All BOMs have been registered in BOM history before?)", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ComponentsAndOutputPerProcessOrder.Dispose();
                sqlDB_Conn.Dispose();
                return;
            }

            DataRow[] recycleOrScrapItems = ComponentsAndOutputPerProcessOrder.Select("[Item Description] LIKE '%BP2%'"); //remove it from the component list if it is scrap items such as drools, rejected material
            foreach (DataRow dr in recycleOrScrapItems) { ComponentsAndOutputPerProcessOrder.Rows.Remove(dr); };
            ComponentsAndOutputPerProcessOrder.Columns.Add("Batch Path", typeof(string));
            ComponentsAndOutputPerProcessOrder.AcceptChanges();
            
            DataTable tempComponentsAndOutputPerProcessOrder = ComponentsAndOutputPerProcessOrder.Copy();
            DataRow[] ComponentsNeedFurtherBreakdown1 = tempComponentsAndOutputPerProcessOrder.Select("[Item Description] LIKE '%-ExplosionToNextLevelBOM'");
            if (ComponentsNeedFurtherBreakdown1.Length > 0) //if we have recyles or intermediates as input items in the BOM, or any over-production FG is changed to C-CODE
            {
                DataRow[] ComponentsNeedFurtherBreakdown = ComponentsAndOutputPerProcessOrder.Select("[Item Description] LIKE '%-ExplosionToNextLevelBOM'");
                foreach (DataRow drow in ComponentsNeedFurtherBreakdown)
                {//ComponentsAndOutputPerProcessOrder excludes all the rows with intermediate FG item with batch No found in BOM history (added a sufix like  '-ExplosionToNextLevelBOM')
                    ComponentsAndOutputPerProcessOrder.Rows.Remove(drow);
                }; 
                ComponentsAndOutputPerProcessOrder.AcceptChanges();

                Dictionary<string, DataTable> BatchNumbersAndNextLevelBreakDownBOMs = new Dictionary<string, DataTable>();
                foreach (DataRow dr in ComponentsNeedFurtherBreakdown1)
                {
                    String batchNo = dr["Lot No"].ToString().Trim().ToUpper();
                    if (!BatchNumbersAndNextLevelBreakDownBOMs.ContainsKey(batchNo))
                    {
                        string getDetailedBOM = "SELECT '' AS [Process Order No], '' AS [Actual Start Date], '' AS [Actual End Date], C_BOMDetail.[Batch No], '' AS [FG No], '' AS [FG Description], " +
                                        "[Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [FG Qty], 0.0 AS [RM Qty],  [Total Input Qty], " +
                                        " 0.0 AS [Drools Qty], '' AS [Batch Path], [Consumption] FROM C_BOMDetail LEFT JOIN C_BOM ON C_BOMDetail.[Batch No] = C_BOM.[Batch No] WHERE C_BOMDetail.[Batch No] = '" + batchNo + "' AND [Consumption] > 0";
                        SqlDataAdapter BomAdp1 = new SqlDataAdapter(getDetailedBOM, sqlDB_Conn);
                        DataTable NextLevelBreakDownBOM = new DataTable();
                        BomAdp1.Fill(NextLevelBreakDownBOM);//Get reycle or intermediate BOM from history BOM details
                        BomAdp1.Dispose();
                        BatchNumbersAndNextLevelBreakDownBOMs.Add(batchNo, NextLevelBreakDownBOM);
                    }
                }


                for (int m = 0; m < ComponentsNeedFurtherBreakdown1.Length; m++)
                {

                    string FirstLevelCompoentLotNo = ComponentsNeedFurtherBreakdown1[m]["Lot No"].ToString().Trim().ToUpper();
                    DataTable BOM_DetailsPerBatchNo = BatchNumbersAndNextLevelBreakDownBOMs[FirstLevelCompoentLotNo].Copy();
                    
                    if (BOM_DetailsPerBatchNo.Rows.Count > 0)  // find out the history BOM to calculate recycle
                    {
                        foreach (DataRow dr in BOM_DetailsPerBatchNo.Rows)
                        {
                            dr["Batch No"] = ComponentsNeedFurtherBreakdown1[m]["Batch No"].ToString().Trim();
                            dr["Process Order No"] = ComponentsNeedFurtherBreakdown1[m]["Process Order No"].ToString().Trim().ToUpper();
                            dr["Actual Start Date"] = ComponentsNeedFurtherBreakdown1[m]["Actual Start Date"].ToString().Trim().ToUpper();
                            dr["Actual End Date"] = ComponentsNeedFurtherBreakdown1[m]["Actual End Date"].ToString().Trim().ToUpper();
                            dr["FG No"] = ComponentsNeedFurtherBreakdown1[m]["FG No"].ToString().Trim().ToUpper();
                            dr["FG Description"] = ComponentsNeedFurtherBreakdown1[m]["FG Description"].ToString().Trim().ToUpper();
                            decimal FG_QtyInNextLevelDetailedBOM = Convert.ToDecimal(dr["FG Qty"].ToString().Trim());
                            decimal Total_InputQtyInNextLevelDetailedBOM = Convert.ToDecimal(dr["Total Input Qty"].ToString().Trim());
                            string FG_Qty = ComponentsNeedFurtherBreakdown1[m]["FG Qty"].ToString().Trim();
                            if (!String.IsNullOrEmpty(FG_Qty)) { dr["FG Qty"] = Convert.ToInt32(FG_Qty); }
                            string total_InputQty = ComponentsNeedFurtherBreakdown1[m]["Total Input Qty"].ToString().Trim();
                            if (!String.IsNullOrEmpty(total_InputQty)) { dr["Total Input Qty"] = Math.Round(Convert.ToDecimal(double.Parse(total_InputQty)), 6); }
                            string DroolsQty = ComponentsNeedFurtherBreakdown1[m]["Drools Qty"].ToString().Trim();
                            if (!String.IsNullOrEmpty(DroolsQty)) { dr["Drools Qty"] = Math.Round(Convert.ToDecimal(double.Parse(DroolsQty)), 6); }
                            dr["Batch Path"] = "/" + ComponentsNeedFurtherBreakdown1[m]["Batch No"].ToString().Trim() + "/" + FirstLevelCompoentLotNo;
                            decimal dConsumption = Math.Round(Convert.ToDecimal(double.Parse(dr["Consumption"].ToString().Trim())), 6);
                            decimal FristLevelComponentQty = Math.Round(Convert.ToDecimal(double.Parse(ComponentsNeedFurtherBreakdown1[m]["RM Qty"].ToString().Trim())), 6);
                            dr["RM Qty"] = Math.Round(FristLevelComponentQty * dConsumption * Total_InputQtyInNextLevelDetailedBOM / FG_QtyInNextLevelDetailedBOM, 6);

                            ComponentsAndOutputPerProcessOrder.ImportRow(dr);
                        }

                    }
                    else  // use the BOM in the same FG to directly split the recycle qty into every existing RM
                    {                        
                            String batchNo = ComponentsNeedFurtherBreakdown1[m]["Batch No"].ToString();
                            decimal dTotalInputQty = 0.0M;
                            
                            decimal FristLevelComponentQty = Math.Round(Convert.ToDecimal(double.Parse(ComponentsNeedFurtherBreakdown1[m]["RM Qty"].ToString().Trim())), 6);
                            DataRow[] firstLevelComponentsRecords = ComponentsAndOutputPerProcessOrder.Select("[Batch No]='" + batchNo + "'");
                            foreach (DataRow dr in firstLevelComponentsRecords)
                            {
                                dTotalInputQty += Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6);
                            }
                            foreach (DataRow dr in firstLevelComponentsRecords)
                            {
                                decimal individualRM_Qty = Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6);
                                dr["RM Qty"] = Math.Round(Convert.ToDecimal(individualRM_Qty * (1 + FristLevelComponentQty / dTotalInputQty )), 6);
                                dr["Batch Path"] = "/" + FirstLevelCompoentLotNo + "/FairShareAllocation";
                            }                     
                    }
                }

                ComponentsAndOutputPerProcessOrder.AcceptChanges();
                tempComponentsAndOutputPerProcessOrder.Dispose();
                                

                DataView dvToSortDataTable = ComponentsAndOutputPerProcessOrder.DefaultView;
                dvToSortDataTable.Sort = "Batch No ASC";
                ComponentsAndOutputPerProcessOrder = dvToSortDataTable.ToTable();

                SetOrdinalForEachRecordInDataTableGroupBySortedKeyField(ComponentsAndOutputPerProcessOrder, "Batch No", "Line No");

                foreach (DataRow dr in ComponentsAndOutputPerProcessOrder.Rows)
                {
                    if (String.IsNullOrEmpty(dr["Batch Path"].ToString().Trim()))
                    {
                        dr["Batch Path"] = "/" + dr["Batch No"].ToString().Trim() + "/";
                    }
                }
                ComponentsAndOutputPerProcessOrder.AcceptChanges();
            }

            InsertIntoSQL_DB_TableFromDataTable(ComponentsAndOutputPerProcessOrder);

            this.GetDgvData(true);
        }


        private void SetOrdinalForEachRecordInDataTableGroupBySortedKeyField(DataTable SortedDataTable, string SortColumnName, string sequenceNumberColumnName)
        {
            string PrecedingBatchNo = String.Empty;
            int iLineNo = 0;
            for (int n = 0; n < SortedDataTable.Rows.Count; n++)
            {
                string CurrentBatchNo = SortedDataTable.Rows[n][SortColumnName].ToString().Trim();
                if (String.Compare(CurrentBatchNo, PrecedingBatchNo) != 0)
                {
                    PrecedingBatchNo = CurrentBatchNo;
                    iLineNo = 1;
                }
                else
                { iLineNo += 1; }
                SortedDataTable.Rows[n][sequenceNumberColumnName] = iLineNo;
            }
            SortedDataTable.AcceptChanges();
        }

        private void InsertIntoSQL_DB_TableFromDataTable(DataTable ComponentsAndOutputPerProcessOrder)
        {
            SqlConnection sqlDB_Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (sqlDB_Conn.State == ConnectionState.Closed) { sqlDB_Conn.Open(); }
            SqlCommand sqlCommands = new SqlCommand();
            sqlCommands.Connection = sqlDB_Conn;

            for (int j = 0; j < ComponentsAndOutputPerProcessOrder.Rows.Count; j++)
            {
                sqlCommands.Parameters.Clear();
                sqlCommands.Parameters.Add("@ProcessOrderNo", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Process Order No"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Actual Start Date"].ToString().Trim();
                sqlCommands.Parameters.Add("@ActualEndDate", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Actual End Date"].ToString().Trim();
                string strBatchPath = ComponentsAndOutputPerProcessOrder.Rows[j]["Batch Path"].ToString().Trim().ToUpper();
                if (String.IsNullOrEmpty(strBatchPath)) { strBatchPath = "/" + ComponentsAndOutputPerProcessOrder.Rows[j]["Batch No"].ToString().Trim().ToUpper() + "/"; }
                sqlCommands.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = strBatchPath;
                sqlCommands.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Batch No"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["FG No"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@FgDesc", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["FG Description"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(ComponentsAndOutputPerProcessOrder.Rows[j]["Line No"].ToString().Trim());
                sqlCommands.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Item No"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Item Description"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Lot No"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@InvType", SqlDbType.NVarChar).Value = ComponentsAndOutputPerProcessOrder.Rows[j]["Inventory Type"].ToString().Trim().ToUpper();
                sqlCommands.Parameters.Add("@RmCategory", SqlDbType.NVarChar).Value = string.Empty; // ComponentsAndOutputPerProcessOrder.Rows[j]["RM Category"].ToString().Trim().ToUpper();
                string FG_Qty = ComponentsAndOutputPerProcessOrder.Rows[j]["FG Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(FG_Qty)) { sqlCommands.Parameters.Add("@FgQty", SqlDbType.Int).Value = 0; }
                else { sqlCommands.Parameters.Add("@FgQty", SqlDbType.Int).Value = Convert.ToInt32(FG_Qty); }
                string strRmQty = ComponentsAndOutputPerProcessOrder.Rows[j]["RM Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(strRmQty)) { sqlCommands.Parameters.Add("@RmQty", SqlDbType.Decimal).Value = 0.0; }
                else { sqlCommands.Parameters.Add("@RmQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(strRmQty)), 6); }
                string total_InputQty = ComponentsAndOutputPerProcessOrder.Rows[j]["Total Input Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(total_InputQty)) { sqlCommands.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = 0.0; }
                else { sqlCommands.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(total_InputQty)), 6); }
                string DroolsQty = ComponentsAndOutputPerProcessOrder.Rows[j]["Drools Qty"].ToString().Trim();
                if (String.IsNullOrEmpty(DroolsQty)) { sqlCommands.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = 0.0; }
                else { sqlCommands.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(DroolsQty)), 6); }
                sqlCommands.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = funcLib.getCurrentUserName();
                sqlCommands.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                sqlCommands.CommandText = "INSERT INTO M_DailyBOM([Process Order No], [Actual Start Date], [Actual End Date], [Batch Path], [Batch No], [FG No], " +
                                      "[FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [FG Qty], [RM Qty], " +
                                      "[Total Input Qty], [Drools Qty], [Creater], [Created Date]) VALUES(@ProcessOrderNo, @ActualStartDate, @ActualEndDate, " +
                                      "@BatchPath, @BatchNo, @FgNo, @FgDesc, @LineNo, @ItemNo, @ItemDesc, @LotNo, @InvType, @RmCategory, @FgQty, @RmQty, " +
                                      "@TotalInputQty, @DroolsQty, @Creater, @CreatedDate)";
                sqlCommands.ExecuteNonQuery();
                sqlCommands.Parameters.Clear();
            }
            sqlCommands.Dispose();
            sqlDB_Conn.Dispose();
        }

        private void GetDgvData(bool bJudge)
        {
            strFilter = "";
            dataViewToFillDataGridView.RowFilter = "";
            string strSQL = null;
            if (bJudge == true)
            {
                strSQL = "SELECT [Batch Path], [Batch No], [FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], " +
                         "[RM Category], [FG Qty], [Total Input Qty], [Drools Qty], [RM Qty], [Actual Start Date], [Actual End Date], [Process Order No] " +
                         "FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            }
            else if (bJudge == false)
            {
                strSQL = "SELECT [Batch Path], [Batch No], [FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], " +
                         "[RM Category], [RM EHB], [BGD No], [FG Qty], [Total Input Qty], [Drools Qty], [Order Price(USD)], [Total RM Cost(USD)], [RM Qty], " +
                         "[RM Currency], [RM Price], [Consumption], [Qty Loss Rate], [HS Code], [CHN Name], [Drools EHB], [Process Order No], [Actual Start Date], " + 
                         "[Actual End Date] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            }
            SqlConnection Conn = new SqlConnection(SqlLib.StrSqlConnection);
            if (Conn.State == ConnectionState.Closed) { Conn.Open(); }
            SqlDataAdapter Adapter = new SqlDataAdapter(strSQL, Conn);
            dataTableToFillDataGridView.Clear();
            Adapter.Fill(dataTableToFillDataGridView);
            Adapter.Dispose();
            dataViewToFillDataGridView = dataTableToFillDataGridView.DefaultView;
            if (dataTableToFillDataGridView.Rows.Count == 0)
            {
                dataTableToFillDataGridView.Clear();
                dataTableToFillDataGridView.Dispose();
                this.dgvBOM.DataSource = DBNull.Value;
            }
            else
            {
                this.dgvBOM.DataSource = dataViewToFillDataGridView;
                this.dgvBOM.Columns[0].HeaderText = "Select";
                this.dgvBOM.Columns["FG No"].Frozen = true;
            }
            if (Conn.State == ConnectionState.Open) { Conn.Close(); Conn.Dispose(); }
        }

        private decimal GetExchangeRate(string strExchange)
        {
            decimal dExchangeRate = 0.0M;
            if (dtExRate.Rows.Count == 0)
            {
                SqlConnection ConnExRate = new SqlConnection(SqlLib.StrSqlConnection);
                if (ConnExRate.State == ConnectionState.Closed) { ConnExRate.Open(); }
                SqlDataAdapter AdapterExRate = new SqlDataAdapter(@"SELECT [ObjectName], [ObjectValue] FROM B_SysInfo WHERE [ObjectName] LIKE 'ExchangeRate%'", ConnExRate);
                AdapterExRate.Fill(dtExRate);
                AdapterExRate.Dispose();
                if (ConnExRate.State == ConnectionState.Open) { ConnExRate.Close(); ConnExRate.Dispose(); }
            }
            DataRow[] dr = dtExRate.Select("[ObjectName]= 'ExchangeRate:" + strExchange.Trim() + "'");
            if (dr.Length > 0) { dExchangeRate = Convert.ToDecimal(dr[0][1].ToString().Trim()); }
            return dExchangeRate;
        }

        private void Update_RMCurrency_RMPrice_BGDNO_RMEHB(SqlConnection exeConn, SqlCommand exeComm)
        {
            exeComm.CommandText = "SELECT [Batch No], [FG No], [Line No], [Item No], [Lot No], [RM EHB], [BGD No], [RM Price], [RM Currency] FROM M_DailyBOM";
            SqlDataAdapter exeAdap1 = new SqlDataAdapter();
            exeAdap1.SelectCommand = exeComm;
            DataTable dtRmEhb = new DataTable();
            exeAdap1.Fill(dtRmEhb);
            exeComm.CommandText = "SELECT DISTINCT [Item No], [Lot No], [RM EHB], [BGD No], [PO Currency] AS [RM Currency], [PO Unit Price] AS [RM Price] " + 
                                  "FROM C_RMReceiving ORDER BY [PO Currency]";
            exeAdap1 = new SqlDataAdapter();
            exeAdap1.SelectCommand = exeComm;
            DataTable dtRmRcvd = new DataTable();
            exeAdap1.Fill(dtRmRcvd);
            exeAdap1.Dispose();
            foreach (DataRow dr in dtRmEhb.Rows)
            {
                string strItemNo = dr["Item No"].ToString().Trim().ToUpper();
                string strLotNo = dr["Lot No"].ToString().Trim().ToUpper();
                DataRow[] drRmEhb = dtRmRcvd.Select("[Item No]='" + strItemNo + "' AND [Lot No]='" + strLotNo + "'");
                if (drRmEhb.Length > 0)
                {
                    string strRmCurr = drRmEhb[0]["RM Currency"].ToString().Trim().ToUpper();
                    string strRmCategory = "USD";
                    if (string.Compare(strRmCurr, "RMB") == 0 || string.Compare(strRmCurr, "CNY") == 0) { strRmCategory = "RMB"; }
                    exeComm.Parameters.Clear();
                    exeComm.Parameters.Add("@RmEhb", SqlDbType.NVarChar).Value = drRmEhb[0]["RM EHB"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@BgdNo", SqlDbType.NVarChar).Value = drRmEhb[0]["BGD No"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@RmCurr", SqlDbType.NVarChar).Value = strRmCurr;
                    exeComm.Parameters.Add("@RmPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(drRmEhb[0]["RM Price"].ToString().Trim());
                    exeComm.Parameters.Add("@RmCategory", SqlDbType.NVarChar).Value = strRmCategory;
                    exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                    exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                    exeComm.CommandText = "UPDATE M_DailyBOM SET [RM EHB] = @RmEhb, [BGD No] = @BgdNo, [RM Currency] = @RmCurr, [RM Price] = @RmPrice, " +
                                          "[RM Category] = @RmCategory WHERE [Batch No] = @BatchNo AND [FG No] = @FgNo AND [Line No] = @LineNo";
                    exeComm.ExecuteNonQuery();
                }
            }
            exeComm.Parameters.Clear();
            dtRmEhb.Dispose();
            dtRmRcvd.Dispose();
        }

        private void Update_TotalRMCost_OrderPrice_QtyLossRate(SqlConnection exeConn, SqlCommand exeComm)
        {
            exeComm.CommandText = "SELECT [Batch No], [FG No], [Line No], [FG Qty], [Total Input Qty], [RM Qty], [RM Currency], [RM Price], [Total RM Cost(USD)] " + 
                                  "FROM M_DailyBOM ORDER BY [Batch No]";
            SqlDataAdapter exeAdap2 = new SqlDataAdapter();
            exeAdap2.SelectCommand = exeComm;
            DataTable dtRmCost = new DataTable();
            exeAdap2.Fill(dtRmCost);

            foreach (DataRow dr in dtRmCost.Rows)
            {               
                string strRmCurr = dr["RM Currency"].ToString().Trim();
                decimal dRmPrice = Convert.ToDecimal(dr["RM Price"].ToString().Trim());
                if (String.Compare(strRmCurr, "USD") != 0) { dRmPrice = Math.Round(dRmPrice * this.GetExchangeRate(strRmCurr), 3); }
                dr["Total RM Cost(USD)"] = Math.Round(dRmPrice * Convert.ToDecimal(dr["RM Qty"].ToString().Trim()), 3);
            }
            dtRmCost.AcceptChanges();

            exeComm.CommandText = "SELECT DISTINCT [Batch No], [FG No] FROM M_DailyBOM";
            exeAdap2 = new SqlDataAdapter();
            exeAdap2.SelectCommand = exeComm;
            DataTable dtBatchList = new DataTable();
            exeAdap2.Fill(dtBatchList);
            exeAdap2.Dispose();
            foreach (DataRow dRow in dtBatchList.Rows)
            {
                string strBatchNo = dRow["Batch No"].ToString().Trim();
                string strFGNo = dRow["FG No"].ToString().Trim();
                decimal dTotalRMCost = (decimal)dtRmCost.Compute("SUM([Total RM Cost(USD)])", "[Batch No]='" + strBatchNo + "' AND [FG No]='" + strFGNo + "'");
                DataRow dr =  dtRmCost.Select("[Batch No]='" + strBatchNo + "' AND [FG No]='" + strFGNo + "'")[0];
                decimal dFgQty = Convert.ToDecimal(dr["FG Qty"].ToString().Trim());
                //decimal dTotalInputQty = Math.Round(Convert.ToDecimal(double.Parse(dr["Total Input Qty"].ToString().Trim())), 6);
                decimal dTotalInputQty = (decimal)dtRmCost.Compute("SUM([RM Qty])", "[Batch No]='" + strBatchNo + "' AND [FG No]='" + strFGNo + "'");
  
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@OrderPrice", SqlDbType.Decimal).Value = Math.Round(dTotalRMCost / dFgQty, 2);
                exeComm.Parameters.Add("@TotalRMCost", SqlDbType.Decimal).Value = Math.Round(dTotalRMCost, 2);
                exeComm.Parameters.Add("@QtyLossRate", SqlDbType.Decimal).Value = Math.Round((1 - dFgQty / dTotalInputQty) * 100.0M, 6);
                exeComm.Parameters.Add("@droolsQty", SqlDbType.Decimal).Value = dTotalInputQty - dFgQty;
                exeComm.Parameters.Add("@totalInputQty", SqlDbType.Decimal).Value = dTotalInputQty;
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = strBatchNo;
                exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = strFGNo;
                exeComm.CommandText = "UPDATE M_DailyBOM SET [Order Price(USD)] = @OrderPrice, [Total RM Cost(USD)] = @TotalRMCost, [Qty Loss Rate] = @QtyLossRate " +
                                      ", [Drools Qty] = @droolsQty , [Total Input Qty] = @totalInputQty WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo";
                exeComm.ExecuteNonQuery();               
            }
            exeComm.Parameters.Clear();
            dtRmCost.Dispose();
            dtBatchList.Dispose();
        }

        private void Update_HSCode_ChnName_DroolsEHB_Consumption(SqlConnection exeConn, SqlCommand exeComm)
        {
            exeComm.CommandText = "SELECT DISTINCT M.[Batch No], M.[FG No], B.[HS Code], B.[FG CHN Name] FROM M_DailyBOM AS M LEFT JOIN (SELECT DISTINCT [Grade], " + 
                                  "[HS Code], [FG CHN Name] FROM B_HsCode) AS B ON SUBSTRING(M.[FG Description], 0, CHARINDEX('-', M.[FG Description], 0)) = B.[Grade]";
            SqlDataAdapter exeAdap3 = new SqlDataAdapter();
            exeAdap3.SelectCommand = exeComm;
            DataTable dtHsCode = new DataTable();
            exeAdap3.Fill(dtHsCode);
            foreach (DataRow dr in dtHsCode.Rows)
            {
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@HsCode", SqlDbType.NVarChar).Value = dr["HS Code"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@ChnName", SqlDbType.NVarChar).Value = dr["FG CHN Name"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                exeComm.CommandText = "UPDATE M_DailyBOM SET [HS Code] = @HsCode, [CHN Name] = @ChnName WHERE [Batch No] = @BatchNo AND [FG No] = @FgNo";
                exeComm.ExecuteNonQuery();
            }
            dtHsCode.Dispose();
            exeComm.CommandText = "SELECT M.[Batch No], M.[FG No], M.[Line No], M.[Total Input Qty], M.[RM Qty], B.[Drools EHB] FROM M_DailyBOM AS M " +
                                  "LEFT JOIN B_Drools AS B ON M.[RM Category] = B.[RM Category] AND M.[CHN Name] = B.[FG CHN Name]";
            exeAdap3.SelectCommand = exeComm;
            DataTable dtDrools = new DataTable();
            exeAdap3.Fill(dtDrools);
            exeAdap3.Dispose();
            foreach (DataRow dr in dtDrools.Rows)
            {
                decimal dTotalInputQty = Math.Round(Convert.ToDecimal(double.Parse(dr["Total Input Qty"].ToString().Trim())), 6);
                decimal individualRM_Qty = Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6);
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = Math.Round(individualRM_Qty / dTotalInputQty, 6);
                exeComm.Parameters.Add("@DroolsEHB", SqlDbType.NVarChar).Value = dr["Drools EHB"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                exeComm.CommandText = "UPDATE M_DailyBOM SET [Consumption] = @Consumption, [Drools EHB] = @DroolsEHB WHERE [Batch No] = @BatchNo " + 
                                      "AND [FG No] = @FgNo AND [Line No] = @LineNo";
                exeComm.ExecuteNonQuery();
            }
            exeComm.Parameters.Clear();
            dtDrools.Dispose();
        }

        //Make sure all the consumption rate adds up to 1.0
        private void ComsumptionRateEuqalTo100Percent()
        {
            if (this.dgvBOM.RowCount == 0) { return; }
            SqlConnection exeConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (exeConn.State == ConnectionState.Closed) { exeConn.Open(); }
            SqlCommand exeComm = new SqlCommand();
            exeComm.Connection = exeConn;

            this.Update_RMCurrency_RMPrice_BGDNO_RMEHB(exeConn, exeComm);
            this.Update_TotalRMCost_OrderPrice_QtyLossRate(exeConn, exeComm);
            this.Update_HSCode_ChnName_DroolsEHB_Consumption(exeConn, exeComm);

            #region //Optimize Consumption, ensure this column's value equal to 1.0
            SqlDataAdapter exeAdapter = new SqlDataAdapter();
            exeComm.CommandText = @"SELECT * FROM V_QueryConsumption";
            exeAdapter.SelectCommand = exeComm;
            DataTable dtConsumption = new DataTable();
            exeAdapter.Fill(dtConsumption);
            exeAdapter.Dispose();
            for (int x = 0; x < dtConsumption.Rows.Count; x++)
            {
                decimal dSumConsump = Math.Round(Convert.ToDecimal(double.Parse(dtConsumption.Rows[x]["SUMConsumption"].ToString().Trim())), 6);
                decimal dConsump = Math.Round(Convert.ToDecimal(double.Parse(dtConsumption.Rows[x]["Consump"].ToString().Trim())), 6);
                exeComm.Parameters.Clear();
                exeComm.Parameters.Add("@Consumption", SqlDbType.Decimal).Value = dConsump + 1.0M - dSumConsump;
                exeComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dtConsumption.Rows[x]["Batch No"].ToString().Trim();
                exeComm.Parameters.Add("@FGNo", SqlDbType.NVarChar).Value = dtConsumption.Rows[x]["FG No"].ToString().Trim();
                exeComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dtConsumption.Rows[x]["Line"].ToString().Trim());
                exeComm.CommandText = "UPDATE M_DailyBOM SET [Consumption] = @Consumption WHERE [Batch No] = @BatchNo AND [FG No] = @FGNo AND [Line No] = @LineNo";
                exeComm.ExecuteNonQuery();
            }
            dtConsumption.Dispose();
            #endregion

            exeComm.Parameters.Clear();
            exeComm.Dispose();
            if (exeConn.State == ConnectionState.Open)
            {
                exeConn.Close();
                exeConn.Dispose();
            }
            if (MessageBox.Show("Successfully extract data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK) { this.GetDgvData(false); }
       
        }


        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.dgvBOM.RowCount == 0) { MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("Are you sure to download the data?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Application.Workbooks.Add(true);

            if (this.dgvBOM.Columns[0].HeaderText != "Select")
            {
                bool bJudge = false;
                int iRow = 2;
                for (int i = 0; i < this.dgvBOM.RowCount; i++)
                {
                    if (String.Compare(this.dgvBOM[0, i].EditedFormattedValue.ToString(), "True") == 0)
                    {
                        bJudge = true;
                        for (int j = 3; j < this.dgvBOM.ColumnCount; j++)
                        { excel.Cells[iRow, j - 2] = "'" + this.dgvBOM[j, i].Value.ToString().Trim(); }
                        iRow++;
                    }
                }
                if (bJudge)
                {
                    for (int k = 3; k < this.dgvBOM.ColumnCount; k++)
                    { excel.Cells[1, k - 2] = this.dgvBOM.Columns[k].HeaderText.ToString(); }
                    excel.Cells.EntireColumn.AutoFit();
                    excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                    excel.Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < this.dgvBOM.RowCount; i++)
                {
                    for (int j = 3; j < this.dgvBOM.ColumnCount; j++)
                    { excel.Cells[i + 2, j - 2] = "'" + this.dgvBOM[j, i].Value.ToString().Trim(); }
                }

                for (int k = 3; k < this.dgvBOM.ColumnCount; k++)
                { excel.Cells[1, k - 2] = this.dgvBOM.Columns[k].HeaderText.ToString(); }

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                excel.Visible = true;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dataViewToFillDataGridView.RowFilter = "";
            SqlConnection BrowseConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BrowseConn.State == ConnectionState.Closed) { BrowseConn.Open(); }

            string strBOMField = "SELECT [Batch Path], [Batch No], [FG No], [FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], " +
                                 "[RM Category], [RM EHB], [BGD No], [FG Qty], [Total Input Qty], [Drools Qty], [Order Price(USD)], [Total RM Cost(USD)], [RM Qty], " +
                                 "[RM Currency], [RM Price], [Consumption], [Qty Loss Rate], [HS Code], [CHN Name], [Drools EHB], [Process Order No], [Actual Start Date], " +
                                 "[Actual End Date], [Creater], [Created Date] FROM M_DailyBOM ORDER BY [Batch No], [FG No], [Line No]";
            SqlDataAdapter BrowseAdapter = new SqlDataAdapter(strBOMField, BrowseConn);
            dataTableToFillDataGridView.Clear();
            BrowseAdapter.Fill(dataTableToFillDataGridView);
            dataViewToFillDataGridView = dataTableToFillDataGridView.DefaultView;
            if (dataTableToFillDataGridView.Rows.Count == 0)
            {
                dataTableToFillDataGridView.Clear();
                dataTableToFillDataGridView.Dispose();
                this.dgvBOM.DataSource = DBNull.Value;
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.dgvBOM.DataSource = dataViewToFillDataGridView;
                this.dgvBOM.Columns[0].HeaderText = "Select";
                this.dgvBOM.Columns["FG No"].Frozen = true;
            }

            BrowseAdapter.Dispose();
            if (BrowseConn.State == ConnectionState.Open)
            {
                BrowseConn.Close();
                BrowseConn.Dispose();
            }
        }

        private void tsmiChooseFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBOM.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBOM.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBOM.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBOM[strColumnName, this.dgvBOM.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] = " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] = '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] = " + strColumnText; }
                    }
                }
                dataViewToFillDataGridView.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiExcludeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBOM.CurrentCell != null)
                {
                    int iColumnIndex = this.dgvBOM.CurrentCell.ColumnIndex;
                    string strColumnName = this.dgvBOM.Columns[iColumnIndex].Name;
                    string strColumnText = this.dgvBOM[strColumnName, this.dgvBOM.CurrentCell.RowIndex].Value.ToString();

                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter += " AND [" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter += " AND [" + strColumnName + "] <> " + strColumnText; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(String))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else if (this.dgvBOM.Columns[iColumnIndex].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] <> '" + strColumnText + "'"; }

                        else { strFilter = "[" + strColumnName + "] <> " + strColumnText; }
                    }
                }
                dataViewToFillDataGridView.RowFilter = strFilter;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiRefreshFilter_Click(object sender, EventArgs e)
        {
            strFilter = "";
            dataViewToFillDataGridView.RowFilter = "";
            if (this.dgvBOM.ColumnCount < 20) { this.GetDgvData(true); }
            else { this.GetDgvData(false); }
        }

        private void tsmiRecordFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvBOM.CurrentCell != null)
            {
                string strColumnName = this.dgvBOM.Columns[this.dgvBOM.CurrentCell.ColumnIndex].Name;
                filterFrm = new PopUpFilterForm(this.funfilter);
                filterFrm.lblFilterColumn.Text = strColumnName;
                filterFrm.cmbFilterContent.DataSource = new DataTable();
                filterFrm.cmbFilterContent.DataSource = dataViewToFillDataGridView.ToTable(true, new string[] { strColumnName });
                filterFrm.cmbFilterContent.DisplayMember = strColumnName;
                filterFrm.ShowDialog();
                fundeleFilter delefilter = new fundeleFilter(funfilter);
            }
        }

        private void funfilter(string strColumnName, string strCondition)
        {
            try
            {
                string strSymbol = filterFrm.cmbFilterSymbol.Text.ToString().Trim().ToUpper();
                if (strFilter.Trim() == "")
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = "[" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = "[" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                else
                {
                    if (String.Compare(strSymbol, "LIKE") == 0 || String.Compare(strSymbol, "NOT LIKE") == 0)
                    {
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '%" + strCondition + "%'"; }
                    }
                    else
                    {
                        if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(string))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else if (this.dgvBOM.Columns[strColumnName].ValueType == typeof(DateTime))
                        { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " '" + strCondition + "'"; }
                        else { strFilter = strFilter + " AND [" + strColumnName + "] " + strSymbol + " " + strCondition; }
                    }
                }
                dataViewToFillDataGridView.RowFilter = strFilter;
                filterFrm.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void tsmiBlankFieldFilter_Click(object sender, EventArgs e)
        {
            if (this.dgvBOM.ColumnCount > 19 && this.dgvBOM.Columns[0].Visible == true)
            {
                try
                {
                    if (!String.IsNullOrEmpty(strFilter.Trim()))
                    { strFilter += @" AND ([RM EHB] = '' OR [RM EHB] IS NULL OR [HS Code] = '' OR [HS Code] IS NULL OR [Drools EHB] = '' OR [Drools EHB] IS NULL)"; }
                    else { strFilter = @"([RM EHB] = '' OR [RM EHB] IS NULL OR [HS Code] = '' OR [HS Code] IS NULL OR [Drools EHB] = '' OR [Drools EHB] IS NULL)"; }
                    dataViewToFillDataGridView.RowFilter = strFilter;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void btnSearchBom_Click(object sender, EventArgs e)
        {
            String pathAndFileName = funcLib.getExcelFileToBeUploaded(this.txtPathBom);
            if (!String.IsNullOrEmpty(pathAndFileName))
            {
                this.ImportBom(pathAndFileName);
                ComsumptionRateEuqalTo100Percent();// Attach this function here to make sure operator will not miss this step  on July.17.2017
            }
        }


        public void ImportBom(string strFilePath)
        {
            string strConn = SqlLib.getOleBbConnnectionStringPerSpeadsheetFileExtension(strFilePath);

            OleDbConnection fzConn = new OleDbConnection(strConn);
            fzConn.Open();
            OleDbDataAdapter fzAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", fzConn);
            DataTable dtBomFz = new DataTable();
            fzAdapter.Fill(dtBomFz);
            fzConn.Dispose();
            if (dtBomFz.Rows.Count == 0)
            {
                MessageBox.Show("There is no data.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtBomFz.Dispose();
                fzConn.Dispose();
                fzAdapter.Dispose();
                return;
            }
            
            SqlConnection BomFzConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (BomFzConn.State == ConnectionState.Closed) { BomFzConn.Open(); }
            SqlCommand BomFzComm = new SqlCommand();
            BomFzComm.Connection = BomFzConn;
            BomFzComm.CommandText = "SELECT COUNT(*) FROM M_DailyBOM";
            int iCount = Convert.ToInt32(BomFzComm.ExecuteScalar());
            if (iCount > 0)
            {
                MessageBox.Show("Make sure all temporary BOMs have been registered in historical BOM list before freeze any BOMs.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BomFzComm.Dispose();
                BomFzConn.Dispose();
                dtBomFz.Dispose();
                return;
            }
            BomFzComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dtBomFz.Rows[0]["Batch No"].ToString().Trim() + "%";
            BomFzComm.CommandText = "SELECT COUNT([Batch No]) FROM C_BOM WHERE [Batch No] LIKE @BatchNo";
            iCount = Convert.ToInt32(BomFzComm.ExecuteScalar());
            if (iCount >= 2)
            {
                MessageBox.Show("System already regenerated the forzen BOM, so refuse to do again.\nPlease note : allow to have a chance.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                BomFzComm.Dispose();
                BomFzConn.Dispose();
                dtBomFz.Dispose();
                return;
            }
            BomFzComm.Parameters.Clear();
            dtBomFz.Columns.Add("Batch Path", typeof(string));
            foreach (DataRow dr in dtBomFz.Rows)
            {               
                BomFzComm.Parameters.Add("@ProcessOrderNo", SqlDbType.NVarChar).Value = dr["Process Order No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@ActualStartDate", SqlDbType.NVarChar).Value = dr["Actual Start Date"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@ActualEndDate", SqlDbType.NVarChar).Value = dr["Actual End Date"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@BatchPath", SqlDbType.NVarChar).Value = "/" + dr["Batch No"].ToString().Trim().ToUpper() + "/FROZEN";
                BomFzComm.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = dr["Batch No"].ToString().Trim().ToUpper() + "R";
                BomFzComm.Parameters.Add("@FgNo", SqlDbType.NVarChar).Value = dr["FG No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@FgDesc", SqlDbType.NVarChar).Value = dr["FG Description"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@LineNo", SqlDbType.Int).Value = Convert.ToInt32(dr["Line No"].ToString().Trim());
                BomFzComm.Parameters.Add("@ItemNo", SqlDbType.NVarChar).Value = dr["Item No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@ItemDesc", SqlDbType.NVarChar).Value = dr["Item Description"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@LotNo", SqlDbType.NVarChar).Value = dr["Lot No"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@InvType", SqlDbType.NVarChar).Value = dr["Inventory Type"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@RmCategory", SqlDbType.NVarChar).Value = dr["RM Category"].ToString().Trim().ToUpper();
                BomFzComm.Parameters.Add("@FgQty", SqlDbType.Int).Value = Convert.ToInt32(dr["FG Qty"].ToString().Trim()); 
                BomFzComm.Parameters.Add("@RmQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())), 6); 
                BomFzComm.Parameters.Add("@TotalInputQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(dr["Total Input Qty"].ToString().Trim())), 6); 
                BomFzComm.Parameters.Add("@DroolsQty", SqlDbType.Decimal).Value = Math.Round(Convert.ToDecimal(double.Parse(dr["Drools Qty"].ToString().Trim())), 6); 
                BomFzComm.Parameters.Add("@Creater", SqlDbType.NVarChar).Value = funcLib.getCurrentUserName();
                BomFzComm.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(System.DateTime.Now.ToString("M/d/yyyy"));

                BomFzComm.CommandText = "INSERT INTO M_DailyBOM([Process Order No], [Actual Start Date], [Actual End Date], [Batch Path], [Batch No], [FG No], " +
                                        "[FG Description], [Line No], [Item No], [Item Description], [Lot No], [Inventory Type], [RM Category], [FG Qty], [RM Qty], " +
                                        "[Total Input Qty], [Drools Qty], [Creater], [Created Date]) VALUES(@ProcessOrderNo, @ActualStartDate, @ActualEndDate, " +
                                        "@BatchPath, @BatchNo, @FgNo, @FgDesc, @LineNo, @ItemNo, @ItemDesc, @LotNo, @InvType, @RmCategory, @FgQty, @RmQty, " +
                                        "@TotalInputQty, @DroolsQty, @Creater, @CreatedDate)";
                BomFzComm.ExecuteNonQuery();
                BomFzComm.Parameters.Clear();
            }
            BomFzComm.Dispose();
            BomFzConn.Dispose();
            dtBomFz.Dispose();
            this.GetDgvData(true);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (this.gBoxShow.Visible == false) { this.gBoxShow.Visible = true; }
            else { this.gBoxShow.Visible = false; this.txtPathRpt.Text = string.Empty; }
        }

        private void FindAndUploadExcelReport_Click(object sender, EventArgs e)
        {
            String pathAndFileName = funcLib.getExcelFileToBeUploaded(this.txtPathRpt);
            if (!String.IsNullOrEmpty(pathAndFileName))
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    String messageF = this.TransformRawDataOfProdutionInputAndOutputToStandardFormat(pathAndFileName);
                    if (!string.IsNullOrEmpty(messageF))
                    {
                        MessageBox.Show(messageF, "Data Issues", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    };
                }
                catch (Exception) { throw; }
                Cursor.Current = Cursors.WaitCursor;
            };
            
        }


        public String TransformRawDataOfProdutionInputAndOutputToStandardFormat(string strFilePath) 
        {
            String messageToBeReturned = String.Empty;

            const String FG_Quantiy_Increased_MT = "101";
            const String FG_Quantiy_Decreased_MT = "102";
            const String RM_Quantiy_Increased_MT = "261";
            const String RM_Quantiy_Decreased_MT = "262";
            const String ByProduct_Quantiy_Increased_MT = "531";

            string strConn = SqlLib.getOleBbConnnectionStringPerSpeadsheetFileExtension(strFilePath);
            OleDbConnection SpreadsheetReportConn = new OleDbConnection(strConn);
            SpreadsheetReportConn.Open();

            String selectionSQL = "SELECT [Order], [Batch] AS [Batch No], [Material] AS [FG No], [Material description] AS [FG Description], [Unit of Entry (=ERFME)] AS [UOM], " +
                      "[Quantity in unit of entry (ERFME)] AS [FG Qty], 'Raw Material' AS [Inventory Type], [Movement Type], [Actual start time] AS [Actual Start Date], " +
                      "[Actual finish date] AS [Actual End Date] FROM [Sheet1$] WHERE ([Movement Type] IN ('" + FG_Quantiy_Increased_MT + "', '" + FG_Quantiy_Decreased_MT + "') AND [Unit of Entry (=ERFME)] IN ('G', 'KG')) " +
                      " OR (([Movement Type] = '"+ ByProduct_Quantiy_Increased_MT + "') AND ([Material description] LIKE '%-COLOR-%'))"; 
            OleDbDataAdapter SpreadsheetReportAdapter = new OleDbDataAdapter(selectionSQL, SpreadsheetReportConn);
            DataTable dtProductionOutputByProcessOrder = new DataTable();
            dtProductionOutputByProcessOrder.Columns.Add("FG Qty", typeof(Int32));
            SpreadsheetReportAdapter.Fill(dtProductionOutputByProcessOrder);
            dtProductionOutputByProcessOrder.Columns["FG Qty"].SetOrdinal(4);
            selectionSQL = "SELECT [Order], [Material] AS [Item No], [Material description] AS [Item Description], [Batch] AS [Lot No], [Movement Type], [Unit of Entry (=ERFME)] AS [UOM], " +
                      "[Quantity in unit of entry (ERFME)] AS [RM Qty] FROM [Sheet1$] WHERE [Movement Type] IN ('" + RM_Quantiy_Increased_MT + "', '" + RM_Quantiy_Decreased_MT +"') AND [Unit of Entry (=ERFME)] IN ('G', 'KG')";
            SpreadsheetReportAdapter = new OleDbDataAdapter(selectionSQL, SpreadsheetReportConn);
            DataTable dtProductionInputByProcessOrder = new DataTable();
            dtProductionInputByProcessOrder.Columns.Add("RM Qty", typeof(decimal));
            SpreadsheetReportAdapter.Fill(dtProductionInputByProcessOrder);
            dtProductionInputByProcessOrder.Columns["RM Qty"].SetOrdinal(4);
       
            SpreadsheetReportAdapter.Dispose();
            SpreadsheetReportConn.Dispose();

            if (dtProductionOutputByProcessOrder.Rows.Count == 0 || dtProductionInputByProcessOrder.Rows.Count == 0)
            {
                MessageBox.Show("There is no data for either FG output or RM input.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtProductionOutputByProcessOrder.Dispose();
                dtProductionInputByProcessOrder.Dispose();
                return messageToBeReturned;
            };

            DataRow[]  RecordsWithMovementType102TobeChangedToNegativeQuantity = dtProductionOutputByProcessOrder.Select("[Movement Type] = '" + FG_Quantiy_Decreased_MT + "'");// FG qty decrease in case there is wrong FG output which needs to be corrected
            foreach (DataRow dr in RecordsWithMovementType102TobeChangedToNegativeQuantity)  //Apr.8.2017
            {
                dr["FG Qty"] = -Convert.ToInt32(dr["FG Qty"].ToString().Trim());
            };

            DataRow[] ByProductRecordsToBeChangedToPrimeProductRecord = dtProductionOutputByProcessOrder.Select("[Movement Type] = '" + ByProduct_Quantiy_Increased_MT + "'");
            foreach (DataRow dr in ByProductRecordsToBeChangedToPrimeProductRecord)  //Mar.11.2019
            {
                DataRow[] PrimeProductsRecordPerProcessOrders = dtProductionOutputByProcessOrder.Select("[Movement Type] = '" + FG_Quantiy_Increased_MT + "' AND [Order] = '" + dr["Order"] + "'");
                if (PrimeProductsRecordPerProcessOrders.Length > 0)
                {
                    dr["FG No"] = PrimeProductsRecordPerProcessOrders[0]["FG No"];
                    dr["FG Description"] = PrimeProductsRecordPerProcessOrders[0]["FG Description"];
                    dr["Movement Type"] = PrimeProductsRecordPerProcessOrders[0]["Movement Type"];
                }
            };



            dtProductionOutputByProcessOrder.Columns.Remove("UOM");
            dtProductionOutputByProcessOrder.Columns.Remove("Movement Type");
            SqlLib lib = new SqlLib();
            string[] strFieldNameM = { "Order", "Batch No", "FG No", "FG Description", "Inventory Type", "Actual Start Date", "Actual End Date" };
            DataTable productionOutputWithoutDuplicatePO_Batch_InvType_Date = lib.SelectDistinct(dtProductionOutputByProcessOrder, strFieldNameM);
            productionOutputWithoutDuplicatePO_Batch_InvType_Date.Columns.Add("FG Qty", typeof(Int32));
            
            String Temp1 = "";
            foreach (DataRow dr in productionOutputWithoutDuplicatePO_Batch_InvType_Date.Rows)
            {
                object totalOutputQuantityByProcessOrder = dtProductionOutputByProcessOrder.Compute("SUM([FG Qty])", "[Order]='" + dr["Order"].ToString().Trim() + "'");
                dr["FG Qty"] = Convert.ToInt32(totalOutputQuantityByProcessOrder.ToString().Trim());
                Temp1 = dr["FG Description"].ToString().Trim();
                if (!regexLike_00_00_00Pattern.IsMatch(Temp1)) { dr["FG Description"] = Temp1  + "-BAG-99-99-99"; };
            }
            dtProductionOutputByProcessOrder.Dispose();

            DataRow[] drow = productionOutputWithoutDuplicatePO_Batch_InvType_Date.Select("[FG Qty] <= 0");
            foreach (DataRow dr in drow) { productionOutputWithoutDuplicatePO_Batch_InvType_Date.Rows.Remove(dr); };
            productionOutputWithoutDuplicatePO_Batch_InvType_Date.AcceptChanges();

            DataRow[] RecordsWithUOM_G_ToBeChangedToKG = dtProductionInputByProcessOrder.Select("[UOM] = 'G'");
            foreach (DataRow dr in RecordsWithUOM_G_ToBeChangedToKG)
            {
                decimal individualRM_Qty = Convert.ToDecimal(dr["RM Qty"].ToString().Trim());
                dr["RM Qty"] = Math.Round(individualRM_Qty / 1000, 6);
                dr["UOM"] = "KG";
            }

            DataRow[] RecordsWithMovementType262TobeChangedToNegativeQuantity = dtProductionInputByProcessOrder.Select("[Movement Type] = '" + RM_Quantiy_Decreased_MT + "'");
            foreach (DataRow dr in RecordsWithMovementType262TobeChangedToNegativeQuantity)  
            {
                dr["RM Qty"] = - Math.Round(Convert.ToDecimal(double.Parse(dr["RM Qty"].ToString().Trim())),6);
            };


            dtProductionInputByProcessOrder.Columns.Remove("UOM");
            dtProductionInputByProcessOrder.Columns.Remove("Movement Type");
            string[] strFieldNameD = { "Order", "Item No", "Item Description", "Lot No" };
            DataTable productionIntputWithoutDuplicatePO_RM_LotNo = lib.SelectDistinct(dtProductionInputByProcessOrder, strFieldNameD);
            productionIntputWithoutDuplicatePO_RM_LotNo.Columns.Add("RM Qty", typeof(decimal));
            foreach (DataRow dr in productionIntputWithoutDuplicatePO_RM_LotNo.Rows)
            {
                object totalRMquantityByProcessOrder = dtProductionInputByProcessOrder.Compute("SUM([RM Qty])", "[Order]='" + dr["Order"].ToString().Trim() + "' AND [Item No]='" + dr["Item No"].ToString().Trim() + "' AND [Lot No]='" + dr["Lot No"].ToString().Trim() + "'");
                dr["RM Qty"] = Math.Round(Convert.ToDecimal(double.Parse(totalRMquantityByProcessOrder.ToString().Trim())), 6);
            }
            dtProductionInputByProcessOrder.Dispose();
            drow = productionIntputWithoutDuplicatePO_RM_LotNo.Select("[RM Qty] <= 0");
            foreach (DataRow dr in drow) { productionIntputWithoutDuplicatePO_RM_LotNo.Rows.Remove(dr);};           
            productionIntputWithoutDuplicatePO_RM_LotNo.Columns.Add("Line No", typeof(Int32));

            SetOrdinalForEachRecordInDataTableGroupBySortedKeyField(productionIntputWithoutDuplicatePO_RM_LotNo, "Order", "Line No");

            productionOutputWithoutDuplicatePO_Batch_InvType_Date.Columns.Add("Total Input Qty", typeof(decimal));
            productionOutputWithoutDuplicatePO_Batch_InvType_Date.Columns.Add("Drools Qty", typeof(decimal));
            foreach (DataRow dr in productionOutputWithoutDuplicatePO_Batch_InvType_Date.Rows)
            {
                decimal FinishedGoodsQuantity = Math.Round(Convert.ToDecimal(double.Parse(dr["FG Qty"].ToString().Trim())), 6);
                DataRow[] datarow = productionIntputWithoutDuplicatePO_RM_LotNo.Select("[Order]='" + dr["Order"].ToString().Trim() + "'");
                if (datarow.Length > 0)
                {
                    object totalRMquantity = productionIntputWithoutDuplicatePO_RM_LotNo.Compute("SUM([RM Qty])", "[Order]='" + dr["Order"].ToString().Trim() + "'");
                    decimal totalRMqtyInDecimal = Math.Round(Convert.ToDecimal(double.Parse(totalRMquantity.ToString().Trim())), 6);
                    dr["Total Input Qty"] = totalRMqtyInDecimal;
                    dr["Drools Qty"] = totalRMqtyInDecimal - FinishedGoodsQuantity;
                }
                else
                {
                    dr["Total Input Qty"] = 0;
                    dr["Drools Qty"] = 0;
                }
            }
            //Check if there is any wrong cases that Total input Qty is less than output FG Qty
            drow = productionOutputWithoutDuplicatePO_Batch_InvType_Date.Select("[Total Input Qty] <= [FG Qty]");
            if (drow.Length > 0)
            {
                foreach (DataRow dr1 in drow)
                {
                    messageToBeReturned += dr1["Order"].ToString() + ";";
                };
                messageToBeReturned = "\nIssues: Total input RM qty <= output FG qty for below process orders.\n" + messageToBeReturned;
            };

            drow = productionOutputWithoutDuplicatePO_Batch_InvType_Date.Select("[Total Input Qty] = 0 OR [Drools Qty] <= 0");
            foreach (DataRow dr in drow) { dr.Delete(); };
            productionOutputWithoutDuplicatePO_Batch_InvType_Date.AcceptChanges();

            DataTable productionInputAndOutputDetailsPerProessOrder = lib.leftJoinDatatablesOnKeyColumn(productionIntputWithoutDuplicatePO_RM_LotNo, productionOutputWithoutDuplicatePO_Batch_InvType_Date, "Order");
            lib.Dispose(0);
            productionOutputWithoutDuplicatePO_Batch_InvType_Date.Dispose();
            productionIntputWithoutDuplicatePO_RM_LotNo.Dispose();

            productionInputAndOutputDetailsPerProessOrder.Columns["Order"].ColumnName = "Process Order No";
            productionInputAndOutputDetailsPerProessOrder.Columns["Actual Start Date"].SetOrdinal(1);
            productionInputAndOutputDetailsPerProessOrder.Columns["Actual End Date"].SetOrdinal(2);
            productionInputAndOutputDetailsPerProessOrder.Columns["Batch No"].SetOrdinal(3);
            productionInputAndOutputDetailsPerProessOrder.Columns["FG No"].SetOrdinal(4);
            productionInputAndOutputDetailsPerProessOrder.Columns["FG Description"].SetOrdinal(5);
            productionInputAndOutputDetailsPerProessOrder.Columns["Line No"].SetOrdinal(6);
            productionInputAndOutputDetailsPerProessOrder.Columns["Item No"].SetOrdinal(7);
            productionInputAndOutputDetailsPerProessOrder.Columns["Item Description"].SetOrdinal(8);
            productionInputAndOutputDetailsPerProessOrder.Columns["Lot No"].SetOrdinal(9);
            productionInputAndOutputDetailsPerProessOrder.Columns["Inventory Type"].SetOrdinal(10);
            productionInputAndOutputDetailsPerProessOrder.Columns["FG Qty"].SetOrdinal(11);
            productionInputAndOutputDetailsPerProessOrder.Columns["RM Qty"].SetOrdinal(12);
            productionInputAndOutputDetailsPerProessOrder.Columns["Total Input Qty"].SetOrdinal(13);
            drow = productionInputAndOutputDetailsPerProessOrder.Select("([Batch No] IS NULL OR [Batch No] = '') OR ([Item No] IS NULL OR [Item No] = '')");
            foreach (DataRow dr in drow) { productionInputAndOutputDetailsPerProessOrder.Rows.Remove(dr); };

            //if RM item No. (input component) can be found in BOM FG NO list (Table C_BOM), add a suffix to the field of item description for further special process
            DataTable distinctRM_ItemList = productionInputAndOutputDetailsPerProessOrder.DefaultView.ToTable(true, "Item No");
            StringBuilder sqlInListOfFG_UsedAsComponent = new StringBuilder("'',");
            foreach (DataRow RM_Item in distinctRM_ItemList.Rows)
            {
                if (FG_and_BatchNoListFromTableOverviewBOM.Select("[FG No] = '" + RM_Item["Item No"].ToString() + "'").Length > 0)
                {
                    sqlInListOfFG_UsedAsComponent.Append("'" + RM_Item["Item No"].ToString() + "',");
                }
            }
            sqlInListOfFG_UsedAsComponent.Length--;

            foreach (DataRow RM_ItemIsFG_Item in productionInputAndOutputDetailsPerProessOrder.Select("[Item No] in (" + sqlInListOfFG_UsedAsComponent.ToString() + ")"))
            {
                RM_ItemIsFG_Item["Item Description"] = RM_ItemIsFG_Item["Item Description"].ToString().Trim() + "-ExplosionToNextLevelBOM"; //Add this one to make it applicable to both intermediate FG and recycle FG as input;
            };
            productionInputAndOutputDetailsPerProessOrder.AcceptChanges();           
           

            //If item Batch No can be found in table C_BOM (FG BOM list) and relevant FG no. is different (FG item is changed to recycle item number), need special mark for further action like replacing FG No.           
            DataTable distinctListOfRM_LotNo_And_Item = productionInputAndOutputDetailsPerProessOrder.DefaultView.ToTable(true, "Lot No", "Item Description");
            StringBuilder sqlInListOfFG_UsedAsComponentButChangedToRecycleMaterial = new StringBuilder("'',");
            foreach (DataRow RecycleItemHasBatchNoOfFG in productionInputAndOutputDetailsPerProessOrder.Rows)
            {
                if (regexLike_00_00_00Pattern.IsMatch(RecycleItemHasBatchNoOfFG["Item Description"].ToString().Trim()))
                {
                    sqlInListOfFG_UsedAsComponentButChangedToRecycleMaterial.Append("'" + RecycleItemHasBatchNoOfFG["Lot No"].ToString() + "',");
                }
            }
            sqlInListOfFG_UsedAsComponentButChangedToRecycleMaterial.Length--;

            String originalFG_ItemName = string.Empty;
            foreach (DataRow RecycleItemHasBatchNoOfFG in productionInputAndOutputDetailsPerProessOrder.Select("[Lot No] in (" + sqlInListOfFG_UsedAsComponentButChangedToRecycleMaterial.ToString() + ")"))
            {
                if (FG_and_BatchNoListFromTableOverviewBOM.Select("[Batch No] = '" + RecycleItemHasBatchNoOfFG["Lot No"].ToString() + "' And [FG No] <> '" + RecycleItemHasBatchNoOfFG["Item No"].ToString() + "'").Length > 0)
                {
                    originalFG_ItemName = FG_and_BatchNoListFromTableOverviewBOM.Select("[Batch No] = '" + RecycleItemHasBatchNoOfFG["Lot No"].ToString() + "'")[0]["FG No"].ToString();
                    RecycleItemHasBatchNoOfFG["Item No"] = originalFG_ItemName;//Replace item no.
                    RecycleItemHasBatchNoOfFG["Item Description"] = RecycleItemHasBatchNoOfFG["Item Description"].ToString().Trim() + "-C-CODE-ExplosionToNextLevelBOM";
                };
            };
            productionInputAndOutputDetailsPerProessOrder.AcceptChanges();
            


            returnMessageInTableAndString returnExceptionMessageInTableAndString = new returnMessageInTableAndString();
            returnExceptionMessageInTableAndString = getDataErrorListBasedOnBusinessRules(productionInputAndOutputDetailsPerProessOrder);

            Dictionary<string, DataTable> ExcelSheetNamesAndDataTables = new Dictionary<string, DataTable>();
            ExcelSheetNamesAndDataTables.Add("Sheet1", productionInputAndOutputDetailsPerProessOrder);
            DataTable warningMessageDataTable = new DataTable();
            warningMessageDataTable = (DataTable)returnExceptionMessageInTableAndString.messageInDataTable.Copy();
            ExcelSheetNamesAndDataTables.Add("Warning message", warningMessageDataTable);
            funcLib.exportDataTablesToSpreadSheets(ExcelSheetNamesAndDataTables);
            messageToBeReturned += returnExceptionMessageInTableAndString.messageString;
            productionInputAndOutputDetailsPerProessOrder.Dispose(); 

            return messageToBeReturned;
        }

        private returnMessageInTableAndString getDataErrorListBasedOnBusinessRules(DataTable productionInputAndOutputDetailsPerProessOrder1)
        {
            
            DataTable dtMessage = new DataTable();
            dtMessage.Columns.Add("Information", typeof(string));
            dtMessage.Columns.Add("Process order", typeof(string));
            dtMessage.Columns.Add("RM Item No", typeof(string));
            dtMessage.Columns.Add("Lot No", typeof(string));

            String messageToBeReturned = string.Empty;

            //Exceptional issues 1 to be reported out: cannot find Item No and Lot NO. in table C_BOM ([FG NO] and [Batch No])
            foreach (DataRow FG_And_BatchNo_NotInOverviewBOM in productionInputAndOutputDetailsPerProessOrder1.Select("[Item Description] LIKE '%-ExplosionToNextLevelBOM'"))
            {
                if (FG_and_BatchNoListFromTableOverviewBOM.Select("[FG No] = '" + FG_And_BatchNo_NotInOverviewBOM["Item No"].ToString() + "' And [Batch NO] = '" + FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString() + "'").Length == 0)
                {
                    messageToBeReturned += "\nThe following RM Item No and Lot No cannot be found in BOM history: Process order;RM Item No;Lot No\n " + FG_And_BatchNo_NotInOverviewBOM["Process Order No"] + ";" + FG_And_BatchNo_NotInOverviewBOM["Item No"].ToString() + ";" + FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString();
                    DataRow dr = dtMessage.NewRow();
                    dr[0] = "Process order, RM Item No and Lot No cannot be found in BOM history";
                    dr[1] = FG_And_BatchNo_NotInOverviewBOM["Process Order No"].ToString();
                    dr[2] = FG_And_BatchNo_NotInOverviewBOM["Item No"].ToString();
                    dr[3] = FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString();
                    dtMessage.Rows.Add(dr);
                };
                //if we can find a revised batch in BOM history, replace old batch No. with a new one like 'XXXXXXXXXXR'
                if (FG_and_BatchNoListFromTableOverviewBOM.Select("[FG No] = '" + FG_And_BatchNo_NotInOverviewBOM["Item No"].ToString() + "' And [Batch NO] = '" + FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString() + "R'").Length > 0)
                {
                    messageToBeReturned += "\n This batch has a revised on in BOM history with suffix 'R': " + FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString() + "R;" + "\n " + FG_And_BatchNo_NotInOverviewBOM["Process Order No"] + ";" + FG_And_BatchNo_NotInOverviewBOM["Item No"].ToString() + ";" + FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString();
                    DataRow dr = dtMessage.NewRow();
                    dr[0] = "This batch has a revised version in BOM history with suffix 'R': " + FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString() + "R ";
                    dr[1] = FG_And_BatchNo_NotInOverviewBOM["Process Order No"].ToString();
                    dr[2] = FG_And_BatchNo_NotInOverviewBOM["Item No"].ToString();
                    dr[3] = FG_And_BatchNo_NotInOverviewBOM["Lot No"].ToString();
                    dtMessage.Rows.Add(dr);
                };
            };
            //Exceptional issues2 to be reported out: RM or component item has the description like FG item description, but it cannot be found in both FG item list in table C_BOM and RM item list in table C_BOMDetail
            foreach (DataRow ComponetSeemsFG_ButNot in productionInputAndOutputDetailsPerProessOrder1.Select("[Item Description] NOT LIKE '%-ExplosionToNextLevelBOM'"))
            {
                if (regexLike_00_00_00Pattern.IsMatch(ComponetSeemsFG_ButNot["Item Description"].ToString()))
                {
                    if (dtRM_ITEM_ListInBOMDetail.Select("[Item No] = '" + ComponetSeemsFG_ButNot["Item No"].ToString() + "'").Length == 0)
                    {
                        messageToBeReturned += "\nThe following RM Item No looks like finished goods item and cannot be found in both FG and RM item list: Process order;RM Item No;Lot No\n " + ComponetSeemsFG_ButNot["Process Order No"].ToString() + ";" + ComponetSeemsFG_ButNot["Item No"].ToString() + ";" + ComponetSeemsFG_ButNot["Lot No"].ToString();
                        DataRow dr = dtMessage.NewRow();
                        dr[0] = "The following RM Item No looks like finished goods item and cannot be found in both FG and RM item list.";
                        dr[1] = ComponetSeemsFG_ButNot["Process Order No"].ToString();
                        dr[2] = ComponetSeemsFG_ButNot["Item No"].ToString();
                        dr[3] = ComponetSeemsFG_ButNot["Lot No"].ToString();
                        dtMessage.Rows.Add(dr);
                    };
                }
            };

            returnMessageInTableAndString messageTableAndString = new returnMessageInTableAndString();
            messageTableAndString.messageInDataTable = dtMessage;
            messageTableAndString.messageString = messageToBeReturned;
            return messageTableAndString;
        }
        
    }


}