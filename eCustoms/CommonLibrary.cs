using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace eCustoms
{
    public class FUVs  //frequently used values
    {
        
        public readonly string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); // +"\n";
        public const string tableOfUsers = "B_Users";

    }

    public class funcLib
    {
        public static String getExcelFileToBeUploaded(TextBox txtPath)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Excel Files(*.xls;*.xlsx)|*.xls;*.xlsx";
            openDlg.ShowDialog();
            txtPath.Text = openDlg.FileName;
            txtPath.Refresh();
            return txtPath.Text.Trim();
        }

        public static Boolean currentUserToUseDataTableExclusively()
        {
            SqlConnection oneConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (oneConn.State == ConnectionState.Closed) { oneConn.Open(); }
            SqlCommand oneComm = new SqlCommand();
            oneComm.Connection = oneConn;

            String dataTableToControlMultipleUsers = "B_MultiUser";
            oneComm.CommandText = "SELECT * FROM " + dataTableToControlMultipleUsers;
            string strUserName = Convert.ToString(oneComm.ExecuteScalar());
            if (!String.IsNullOrEmpty(strUserName))
            {
                if (String.Compare(strUserName.Trim().ToUpper(), getCurrentUserName()) != 0)
                {
                    MessageBox.Show("User (" + strUserName + ") is controlling the working data table. Please wait till that user release the working data table. \n Or ask him/her to log out and log in again.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    oneComm.Dispose();
                    oneConn.Dispose();
                    return false;
                }
            }
            else
            {
                oneComm.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = getCurrentUserName();
                oneComm.CommandText = "INSERT INTO " + dataTableToControlMultipleUsers + "([UserName]) VALUES(@UserName)";
                oneComm.ExecuteNonQuery();
                oneComm.Parameters.Clear();
            }

            oneComm.Dispose();
            oneConn.Dispose();
            return true;
        }

        public static void releaseExclusiveControlOverDataTable()
        {
            SqlConnection oneConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (oneConn.State == ConnectionState.Closed) { oneConn.Open(); }
            SqlCommand oneComm = new SqlCommand();
            oneComm.Connection = oneConn;

            String dataTableToControlMultipleUsers = "B_MultiUser";
            oneComm.CommandText = "Delete FROM " + dataTableToControlMultipleUsers + " Where [UserName] = '" + getCurrentUserName() + "'";
            oneComm.ExecuteScalar();
            oneComm.Dispose();
            oneConn.Dispose();
   
        }

        public static String getCurrentUserName()
        {
            return LoginForm.PublicUserName.Trim().ToUpper();
        }

        public static void exportDataTablesToSpreadSheets(Dictionary<string, DataTable> SpreadSheetNamesAndDataTables)
        {

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

            int counterOfSpreadSheets = 1;
            foreach (KeyValuePair<string, DataTable> kvp in SpreadSheetNamesAndDataTables)
            {
                DataTable spreadsheetDataSource = kvp.Value;
                if (spreadsheetDataSource.Rows.Count > 0)
                {
                    if (counterOfSpreadSheets > 1)
                    {
                        object missing = System.Reflection.Missing.Value;
                        worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add(missing, worksheet, missing, missing);
                    }
                    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[counterOfSpreadSheets];
                    worksheet.Name = kvp.Key;

                    int NumberOfColumns = spreadsheetDataSource.Columns.Count;
                    int NumberOfRows = spreadsheetDataSource.Rows.Count;
                    String[,] dataArray = new String[NumberOfRows + 1, NumberOfColumns];


                    for (int hearderColumn = 0; hearderColumn < NumberOfColumns; hearderColumn++)
                    {
                        dataArray[0, hearderColumn] = spreadsheetDataSource.Columns[hearderColumn].ColumnName;
                    };

                    DataRow dtRow = null;
                    for (int rowNumber = 0; rowNumber < NumberOfRows; rowNumber++)
                    {
                        dtRow = spreadsheetDataSource.Rows[rowNumber];
                        for (int columnNum = 0; columnNum < NumberOfColumns; columnNum++)
                        { dataArray[rowNumber + 1, columnNum] = "'" + dtRow.ItemArray[columnNum].ToString().Trim(); };
                    };
                    worksheet.Range["A1"].Resize[NumberOfRows + 1, NumberOfColumns].Value = dataArray;

                    worksheet.Cells.EntireColumn.AutoFit();
                };

                counterOfSpreadSheets++;
            }

            workbook.Worksheets[1].Activate();
            excel.Visible = true;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
        }
    }


    public class CommonLibrary
    {
        public string strSqlInfo;
        public string SqlInfo
        {
            get { return strSqlInfo; }
        }

        public void ReadConfigSettings()
        {
            string[] strLines = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\ConfigSettings.txt", System.Text.Encoding.Default);

            for (int i = 0; i < strLines.Length; i++ )
            {
                strLines[i] = RijndaelAlgorithm.Decrypt(strLines[i]);
            };
            strSqlInfo = @"Data Source=" + strLines[0] + 
                ";Initial Catalog=" + strLines[1] + 
                ";Persist Security Info=True;User ID=" + strLines[2] + 
                ";Password=" + strLines[3] + 
                ";connect timeout=30;";
        }
    }

    public class SqlLib : IDisposable
    {
        public string doubleFormat(Double doubleValue)
        {
            try
            {
                string doubleValueStr = doubleValue.ToString();
                int eIndex = doubleValueStr.ToUpper().IndexOf("E");
                String format = "";
                if (eIndex == -1) { return doubleValueStr; }
                else
                {
                    string cardinalNumberStr = doubleValueStr.Substring(0, eIndex);
                    string exponentialStr = doubleValueStr.Substring(eIndex + 1);
                    if (exponentialStr.StartsWith("+")) { exponentialStr = exponentialStr.Substring(1); }
                    int exponential = Int32.Parse(exponentialStr);
                    if (exponential > 0)
                    {
                        if ((cardinalNumberStr.Length - 2 - exponential) > 0)
                        {
                            format = "#.";
                            for (int i = 0; i < (cardinalNumberStr.Length - 2 - exponential); i++)
                            { format += 0; }
                        }
                        else { format = "#.0"; }
                    }
                    else if (exponential < 0)
                    {
                        format = "0.";
                        for (int i = 0; i < (cardinalNumberStr.Substring(cardinalNumberStr.IndexOf(".") + 1).Length - exponential); i++)
                        { format += 0; }
                    }
                    else { format = "#.0"; }
                    if (format.Length == 2)
                    { format += 0; }
                    return doubleValue.ToString(format);
                }
            }
            catch (Exception) { } return "";
        }

        public static String getOleBbConnnectionStringPerSpeadsheetFileExtension(String pathAndFileName)
        {
            if (pathAndFileName.ToLower().Contains(".xlsx"))
            { return @"Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + pathAndFileName + "; Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'"; }
            else
            { return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathAndFileName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'"; }
        }

        public static String StrSqlConnection
        {
            get
            {
                CommonLibrary cs = new CommonLibrary();
                cs.ReadConfigSettings();
                return cs.SqlInfo;
            }
        }

        bool disposed;
        private DataTable myDataTable;
        public DataTable GetDataTable(string strSql, string strTable)
        {
            SqlConnection myConnection = new SqlConnection(StrSqlConnection);
            myDataTable = new DataTable();
            myDataTable.Clear();
            try
            {
                if (myConnection.State == ConnectionState.Closed)
                { myConnection.Open(); }
                SqlCommand cmd = new SqlCommand(strSql, myConnection);
                cmd.CommandTimeout = 8000;
                SqlDataAdapter myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = cmd;
                myDataAdapter.Fill(myDataTable);
                myDataTable.TableName = strTable;
                myDataAdapter.Dispose();
            }
            catch (Exception)
            { throw; }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                    myConnection.Dispose();
                }
            }
            return myDataTable;
        }

        public DataTable GetDataTable(string strSql)
        {
            SqlConnection myConnection = new SqlConnection(StrSqlConnection);
            myDataTable = new DataTable();
            myDataTable.Clear();
            try
            {
                if (myConnection.State == ConnectionState.Closed)
                { myConnection.Open(); }
                SqlCommand cmd = new SqlCommand(strSql, myConnection);
                cmd.CommandTimeout = 8000;
                SqlDataAdapter myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = cmd;
                myDataAdapter.Fill(myDataTable);
                myDataAdapter.Dispose();
            }
            catch (Exception)
            { throw; }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                    myConnection.Dispose();
                }
            }
            return myDataTable;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(myDataTable);
        }

        public void Dispose(Int32 iCount)
        {
            if (!this.disposed)
            {
                if (iCount == 0)
                { disposed = true; }
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                { }
                myDataTable.Dispose();
                disposed = true;
            }
        }

        ~SqlLib()
        {
            Dispose(true);
        }

        public string GetData(string strSql)
        {
            SqlConnection myConnection = new SqlConnection(StrSqlConnection);
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand(strSql, myConnection);
                cmd.CommandTimeout = 8000;
                string strReturn = cmd.ExecuteScalar().ToString().Trim();
                cmd.Dispose();
                return strReturn;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
                myConnection.Dispose();
            }
        }

        public DataTable leftJoinDatatablesOnKeyColumn(DataTable leftMainTable, DataTable supplementaryTable, String KeyColName)
        {
            DataTable joinedFinalTable = new DataTable();
            int i = 0;
            int j = 0;
            int k = 0;
            int keyColumnInleftMainTable = 0;
            int keyColumnIn_supplementaryTable = 0;

            joinedFinalTable.TableName = leftMainTable.TableName;
            for (i = 0; i < leftMainTable.Columns.Count; i++)
            {
                if (leftMainTable.Columns[i].ColumnName == KeyColName) { keyColumnInleftMainTable = i; }
                joinedFinalTable.Columns.Add(leftMainTable.Columns[i].ColumnName);
            }

            for (j = 0; j < supplementaryTable.Columns.Count; j++)
            {
                if (supplementaryTable.Columns[j].ColumnName == KeyColName)
                {
                    keyColumnIn_supplementaryTable = j;
                    continue;
                }
                joinedFinalTable.Columns.Add(supplementaryTable.Columns[j].ColumnName);
            }

            for (i = 0; i < leftMainTable.Rows.Count; i++)
            {
                DataRow dr;
                dr = joinedFinalTable.NewRow();
                joinedFinalTable.Rows.Add(dr);
            }

            for (i = 0; i < leftMainTable.Rows.Count; i++)
            {
                for (j = 0; j < leftMainTable.Columns.Count; j++)
                { joinedFinalTable.Rows[i][j] = leftMainTable.Rows[i][j].ToString(); }

                DataRow[] drow = supplementaryTable.Select(KeyColName + " = '" + leftMainTable.Rows[i][keyColumnInleftMainTable].ToString() + "'");
                if (drow.Length > 0)
                {
                    for (k = 0; k < supplementaryTable.Columns.Count; k++)
                    {
                        if (k == keyColumnIn_supplementaryTable) { continue; }
                        joinedFinalTable.Rows[i][j] = drow[0][k].ToString();
                        j++;
                    }
                }
            }

            return joinedFinalTable;
        }


        public DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

            orderedRows = SourceTable.Select("", string.Join(",", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if (!fieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

                    setLastValues(lastValues, row, FieldNames);
                }
            }

            return newTable;
        }

        private bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        private DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }

        public DataTable UniteDataTable(DataTable dt1, DataTable dt2)
        {
            DataTable dt3 = dt1.Clone();
            for (int i = 0; i < dt2.Columns.Count; i++)
            { dt3.Columns.Add(dt2.Columns[i].ColumnName); }
            object[] obj = new object[dt3.Columns.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt3.Rows.Add(obj);
            }

            if (dt1.Rows.Count >= dt2.Rows.Count)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            else
            {
                DataRow dr3;
                for (int i = 0; i < dt2.Rows.Count - dt1.Rows.Count; i++)
                {
                    dr3 = dt3.NewRow();
                    dt3.Rows.Add(dr3);
                }
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            return dt3;
        }
    }
}
