using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
namespace CheckerImp.AccessDb
{
    public class AccessDB
    {
        private string strconn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+AppDomain.CurrentDomain.BaseDirectory+ @"\database\check.accdb;Persist Security Info=False;";
        public AccessDB()
        {
        }

        public OleDbConnection GetConn
        {
            get
            {
                return new OleDbConnection(strconn);
            }
        }

        public int Exec_Sql(string sql)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(strconn))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable Exec_DataTable(string sql)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(strconn))
                {
                    conn.Open();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
