using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class commonDB
    {

        protected static OleDbConnection foxProConn = new OleDbConnection();

        private static string CooperFolder = "";

        public static string _connstring()
        {
            // CooperFolder = ConfigurationManager.AppSettings["Cooper"];
            CooperFolder = PublicInfo.Cooper_Path;
            return @"Provider=vfpoledb;Data Source=" + CooperFolder + ";Collating Sequence=machine;";
        }

        protected static string _newLogConnstring()
        {
            CooperFolder = PublicInfo.Cooper_Path + "\\NEWLOG";
            // CooperFolder =CooperFolder+"\\NEWLOG"; //日誌檔的位置
            return @"Provider=vfpoledb;Data Source=" + CooperFolder + ";Collating Sequence=machine;";
        }




        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="strTable">資料表名稱</param>
        /// <param name="liColumnDataObjs">Parameter 參數</param>
        protected static void addWithParameter(string strTable, List<columnsData> liColumnDataObjs)
        {

            try
            {
                foxProConn.ConnectionString = _connstring();
                //if (string.IsNullOrEmpty(foxProConn.ConnectionString))
                //{
                //    foxProConn.ConnectionString = _connstring();
                //}

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();

                OleDbCommand ocmd = foxProConn.CreateCommand();
                List<OleDbParameter> liParameters = new List<OleDbParameter>();

                string strColumn = "INSERT INTO " + strTable + "(";

                string strValue = "";


                foreach (columnsData columnObj in liColumnDataObjs)
                {
                    strColumn += columnObj.strFileName + ",";
                    strValue += "?,";
                    OleDbParameter oleDbParameter = new OleDbParameter("@" + columnObj.strFileName, columnObj.oledbTypeValue);
                    oleDbParameter.Value = columnObj.strValue;
                    liParameters.Add(oleDbParameter);
                }

                strColumn = strColumn.Substring(0, strColumn.Length - 1) + ")"; //去掉逗號
                strValue = strValue.Substring(0, strValue.Length - 1);

                ocmd.CommandText = strColumn + " Value " + " (" + strValue + ") ";

                if (liColumnDataObjs.Count != 0)
                {
                    ocmd.Parameters.AddRange(liParameters.ToArray());
                }
                ocmd.ExecuteNonQuery();
                foxProConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="strTable">資料表名稱</param>
        /// <param name="liColumnDataObjs">Parameter 參數</param>
        protected static void updateWithParameter(string strTable, List<columnsData> liColumnDataObjs)
        {
            try
            {
                foxProConn.ConnectionString = _connstring();
                //if (string.IsNullOrEmpty(foxProConn.ConnectionString))
                //{
                //    foxProConn.ConnectionString = _connstring();
                //}


                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();
                OleDbCommand ocmd = foxProConn.CreateCommand();
                List<OleDbParameter> liParameters = new List<OleDbParameter>();
                string strColumn = "UPDATE " + strTable + " SET ";
                string strWhereCondition = "";
                foreach (columnsData columnObj in liColumnDataObjs)
                {
                    if (columnObj.strFileName.ToUpper() == "IKEY")
                    {
                        strWhereCondition = " WHERE ikey=" + columnObj.strValue;
                    }
                    else
                    {
                        strColumn += columnObj.strFileName + "=?,";
                        OleDbParameter oleDbParameter = new OleDbParameter("@" + columnObj.strFileName, columnObj.oledbTypeValue);
                        oleDbParameter.Value = columnObj.strValue;
                        liParameters.Add(oleDbParameter);
                    }
                }

                //去掉逗號
                ocmd.CommandText = strColumn.Substring(0, strColumn.Length - 1) + " " + strWhereCondition;

                if (liColumnDataObjs.Count != 0)
                {
                    ocmd.Parameters.AddRange(liParameters.ToArray());
                }
                ocmd.ExecuteNonQuery();
                foxProConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="strTable">資料表名稱</param>
        /// <param name="liColumnDataObjs">欄位集合</param>
        protected static void deleteWithParameter(string strTable, List<columnsData> liColumnDataObjs)
        {
            try
            {
                foxProConn.ConnectionString = _connstring();

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();
                OleDbCommand ocmd = foxProConn.CreateCommand();
                List<OleDbParameter> liParameters = new List<OleDbParameter>();
                string strColumn = "DELETE FROM " + strTable;
                string strWhereCondition = " WHERE ";
                foreach (columnsData columnObj in liColumnDataObjs)
                {

                    strWhereCondition += columnObj.strFileName + "=? AND ";
                    OleDbParameter oleDbParameter = new OleDbParameter("@" + columnObj.strFileName, columnObj.oledbTypeValue);
                    oleDbParameter.Value = columnObj.strValue;
                    liParameters.Add(oleDbParameter);
                }

                //去掉最未碼and
                ocmd.CommandText = strColumn + strWhereCondition.Substring(0, strWhereCondition.Length - 4);

                if (liColumnDataObjs.Count != 0)
                {
                    ocmd.Parameters.AddRange(liParameters.ToArray());
                }
                ocmd.ExecuteNonQuery();
                foxProConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="strTable">資料表</param>
        /// <param name="liColumnDataObjs">修改欄位集合</param>
        /// <param name="strCondition">自訂條件</param>
        protected static void updateWithParameter(string strTable, List<columnsData> liColumnDataObjs, string strCondition)
        {
            try
            {
                foxProConn.ConnectionString = _connstring();

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();
                OleDbCommand ocmd = foxProConn.CreateCommand();
                List<OleDbParameter> liParameters = new List<OleDbParameter>();
                string strColumn = "UPDATE " + strTable + " SET ";
                string strWhereCondition = strCondition;
                foreach (columnsData columnObj in liColumnDataObjs)
                {
                    strColumn += columnObj.strFileName + "=?,";
                    OleDbParameter oleDbParameter = new OleDbParameter("@" + columnObj.strFileName, columnObj.oledbTypeValue);
                    oleDbParameter.Value = columnObj.strValue;
                    liParameters.Add(oleDbParameter);
                }

                //去掉逗號
                ocmd.CommandText = strColumn.Substring(0, strColumn.Length - 1) + " WHERE " + strWhereCondition;

                if (liColumnDataObjs.Count != 0)
                {
                    ocmd.Parameters.AddRange(liParameters.ToArray());
                }
                ocmd.ExecuteNonQuery();
                foxProConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 刪除自訂條件資料
        /// </summary>
        /// <param name="strTable">資料表</param>
        /// <param name="strCondition">刪除條件</param>
        protected static void deleteWithParameter(string strTable, string strCondition)
        {
            try
            {
                foxProConn.ConnectionString = _connstring();

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();
                OleDbCommand ocmd = foxProConn.CreateCommand();
                List<OleDbParameter> liParameters = new List<OleDbParameter>();
                string strColumn = "DELETE FROM " + strTable;
                string strWhereCondition = strCondition;

                //去掉最未碼and
                ocmd.CommandText = strColumn + " WHERE " + strWhereCondition;
                ocmd.ExecuteNonQuery();
                foxProConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  日誌檔儲存
        /// </summary>
        /// <param name="strTableName">資料表名稱</param>
        /// <param name="dt">資料集</param>
        public static void addNewLogWithParameter(string strTableName, DataTable dt)
        {
            try
            {

                foxProConn.ConnectionString = _newLogConnstring();
                //if (string.IsNullOrEmpty(foxProConn.ConnectionString))
                //{
                //    foxProConn.ConnectionString = _newLogConnstring();
                //}

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();

                foreach (DataRow dr in dt.Rows)
                {
                    OleDbCommand ocmd = foxProConn.CreateCommand();
                    List<OleDbParameter> liParameters = new List<OleDbParameter>();
                    string strColumn = "INSERT INTO " + strTableName + "(";
                    string strValue = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        strColumn += dc.ColumnName + ",";
                        strValue += "?,";
                        OleDbParameter oleDbParameter = new OleDbParameter("@" + dc.ColumnName, dc.DataType);
                        oleDbParameter.Value = dr[dc.ColumnName];
                        liParameters.Add(oleDbParameter);
                    }

                    strColumn = strColumn.Substring(0, strColumn.Length - 1) + ")"; //去掉逗號
                    strValue = strValue.Substring(0, strValue.Length - 1);

                    ocmd.CommandText = strColumn + " Value " + " (" + strValue + ") ";

                    if (liParameters.Count > 0)
                    {
                        ocmd.Parameters.AddRange(liParameters.ToArray());
                    }

                    ocmd.ExecuteNonQuery();
                }
                foxProConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 查詢資料列
        /// </summary>
        /// <param name="strCommand"></param>
        /// <returns></returns>
        public static DataTable selectQueryWithDataTable(string strCommand)
        {
            return selectQueryWithDataTable(strCommand, false);
        }


        public static DataTable selectQueryWithDataTable(string strCommand, bool isShowDeleted)
        {
            //WriteEvent.WrittingEventLog writeObj = new WriteEvent.WrittingEventLog();

            try
            {
                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 355 foxproSate:"+ foxProConn.State);

                foxProConn.ConnectionString = _connstring();

                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 359 ");

                //if (string.IsNullOrEmpty(foxProConn.ConnectionString))
                //{
                //    foxProConn.ConnectionString = _connstring();
                //}

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();

                    //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 370");
                }

                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 373   before open");

                foxProConn.Open();

                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 377   after open");

                //不需要讀取已刪除的資料
                if (isShowDeleted == true)
                {
                    OleDbCommand oCmdMarkDeleted = new OleDbCommand("SET DELETED OFF", foxProConn);
                    oCmdMarkDeleted.ExecuteNonQuery();
                }
                else
                {
                    OleDbCommand oCmdMarkDeleted = new OleDbCommand("SET DELETED ON", foxProConn);
                    oCmdMarkDeleted.ExecuteNonQuery();
                }

                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 391   after ExecuteNonQuery");

                List<OleDbParameter> liParameters = new List<OleDbParameter>();
                OleDbCommand ocmd = foxProConn.CreateCommand();
                ocmd.CommandText = strCommand;
                DataTable dt = new DataTable();

                using (OleDbDataReader oledbReaderObj = ocmd.ExecuteReader())
                {
                    dt.Load(oledbReaderObj);
                }

                foxProConn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                foxProConn.Close();
                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 408   exception:"+ex.Message);
                throw new Exception(ex.Message);
                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_scheduleError", Directory.GetCurrentDirectory(), "selectQueryWithDataTable 錯誤" + " LINE : 408   foxProConn.State:" + foxProConn.State);
            }
        }

     

        protected static object selectQueryWithExecuteScalar(string strCommand)
        {
            try
            {
                foxProConn.ConnectionString = _connstring();

                //if (string.IsNullOrEmpty(foxProConn.ConnectionString))
                //{
                //    foxProConn.ConnectionString = _connstring();
                //}


                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();

                OleDbCommand oCmdMarkDeleted = new OleDbCommand("SET DELETED ON", foxProConn);
                oCmdMarkDeleted.ExecuteNonQuery();

                List<OleDbParameter> liParameters = new List<OleDbParameter>();
                OleDbCommand ocmd = foxProConn.CreateCommand();
                ocmd.CommandText = strCommand;
                object oledbReaderObj = ocmd.ExecuteScalar();
                foxProConn.Close();
                return oledbReaderObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 可以針對已刪除的資料作異動
        /// </summary>
        /// <param name="strCommand"></param>
        protected static void executeNonQuery(string strCommand)
        {
            try
            {

                foxProConn.ConnectionString = _connstring();
                //if (string.IsNullOrEmpty(foxProConn.ConnectionString))
                //{
                //    foxProConn.ConnectionString = _connstring();
                //}

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();

                OleDbCommand oCmdMarkDeleted = new OleDbCommand("SET DELETED OFF", foxProConn);
                oCmdMarkDeleted.ExecuteNonQuery();

                OleDbCommand ocmd = foxProConn.CreateCommand();
                ocmd.CommandText = strCommand;
                ocmd.ExecuteNonQuery();
                foxProConn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected static object selectQueryWithExecuteScalar(string strCommand, bool isShowDeleted)
        {

            try
            {
                foxProConn.ConnectionString = _connstring();

                //if (string.IsNullOrEmpty(foxProConn.ConnectionString))
                //{
                //    foxProConn.ConnectionString = _connstring();
                //}

                if (foxProConn.State != ConnectionState.Closed)
                {
                    foxProConn.Close();
                }

                foxProConn.Open();


                OleDbCommand oCmdMarkDeleted = new OleDbCommand();

                if (isShowDeleted == true)
                {
                    //顯示所有已刪除資料
                    oCmdMarkDeleted.CommandText = "SET DELETED OFF";
                }
                else
                {
                    //隱藏所有已刪除資料
                    oCmdMarkDeleted.CommandText = "SET DELETED ON";
                }

                oCmdMarkDeleted.Connection = foxProConn;
                oCmdMarkDeleted.ExecuteNonQuery();


                List<OleDbParameter> liParameters = new List<OleDbParameter>();
                OleDbCommand ocmd = foxProConn.CreateCommand();
                ocmd.CommandText = strCommand;
                object oledbReaderObj = ocmd.ExecuteScalar();
                foxProConn.Close();
                return oledbReaderObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int getIkey(string strTable)
        {
            try
            {
                string strCommand = "SELECT MAX(ikey) FROM " + strTable;
                Object objMaxIkey = commonDB.selectQueryWithExecuteScalar(strCommand, true); //ikey要繼續累加，包含已被刪除的ikey
                int intMaxIkey = 0;
                int.TryParse(objMaxIkey.ToString(), out intMaxIkey);
                intMaxIkey += 1;
                return intMaxIkey;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class columnsData
    {
        public string strFileName
        {
            get;
            set;
        }

        public string strValue
        {
            get;
            set;
        }

        public OleDbType oledbTypeValue
        {
            get;
            set;
        }
    }
}
