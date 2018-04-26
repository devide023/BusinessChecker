using CheckerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Tool;
using System.IO;
using Aspose.Cells;
using Model;

namespace CheckerImp
{
    public class KeyWordFinder : IKeyWordFinder
    {
        private DataTable _source = new DataTable();
        private SqlHelper SH = null;
        private string export_xlsx_path = string.Empty;//导出excel路径
        public string Export_Xlsx_Path
        {
            get
            {
                return export_xlsx_path;
            }
            set
            {
                export_xlsx_path = value;
            }
        }
        public DataTable Find_Source(string fllpath)
        {
            try
            {
                string sql = "";
                using (StreamReader sr = new StreamReader(fllpath, Encoding.UTF8))
                {
                    sql = sr.ReadToEnd();
                    _source = SH.Get_Data(sql);
                    return _source;
                }
            }
            catch (Exception e)
            {
                Utility.WriteFile(e.Message);
                return _source;
            }
        }
        public KeyWordFinder()
        {
            SH = new SqlHelper();
        }
        public KeyWordFinder(string connstr)
        {
            SH = new SqlHelper(connstr);
        }

        public string[] Read_Keywords(string path)
        {
            try
            {
                List<string> keys = new List<string>();
                using (StreamReader sr = new StreamReader(path))
                {
                    string content = string.Empty;
                    content = sr.ReadLine();
                    while (null != content)
                    {
                        keys.Add(content);
                        content = sr.ReadLine();
                    }
                }
                return keys.ToArray();
            }
            catch (Exception e)
            {
                Utility.WriteFile(e.Message);
                return new string[] { };
            }

        }

        public string Find_Keywords(params string[] keywords)
        {
            try
            {
                List<string> list = new List<string>();
                List<ExportItem> export_list = new List<ExportItem>();
                string sql = "";
                StringBuilder sqlfile = new StringBuilder();
                if (_source != null && _source.Rows.Count > 0)
                {
                    foreach (DataRow item in _source.Rows)
                    {
                        string str_where = "";
                        foreach (var t in keywords)
                        {
                            str_where += item["colname"].ToString() + " like  '%" + t + "%' or ";
                        }
                        if (str_where.Length > 0)
                        {
                            str_where = str_where.Substring(0, str_where.Length - 3);
                        }
                        sql = string.Format(" select count(*) from {0} where ( {1} );\n ", item["tablename"], str_where);
                        //查找到关键字
                        if (SH.Get_Data(sql).Rows[0][0].ToInt() > 0)
                        {
                            string query = string.Format("select * from {0} where ( {1} );\n ", item["tablename"], str_where);
                            /*sqlfile.AppendFormat(query);
                            sqlfile.Append("\r\n");
                            sqlfile.AppendFormat("-".PadLeft(120, '-'));
                            sqlfile.AppendFormat("\r\n");*/
                            ExportItem exportitem = new ExportItem();
                            DataTable query_result = SH.Get_Data(query);
                            query_result.TableName = item["tablename"].ToString();
                            exportitem.data = query_result;
                            exportitem.tablename = item["tablename"].ToString();
                            exportitem.colname = item["colname"].ToString();
                            export_list.Add(exportitem);
                            /*for (int i = 0; i < query_result.Rows.Count; i++)
                            {
                                //输出列
                                if (i == 0)
                                {
                                    sqlfile.AppendFormat("{0}\t{1}", query_result.Columns[0].ColumnName, item["colname"]);
                                    sqlfile.AppendFormat("\r\n");
                                    sqlfile.AppendFormat("-".PadLeft(120, '-'));
                                    sqlfile.AppendFormat("\r\n");
                                }
                                //输出内容
                                sqlfile.AppendFormat("{0}\t{1}", query_result.Rows[i][0], query_result.Rows[i][item["colname"].ToString()]);
                                sqlfile.AppendFormat("\r\n");
                                sqlfile.AppendFormat("-".PadLeft(120, '-'));
                                sqlfile.AppendFormat("\r\n");
                            }*/
                        }
                    }
                }
                //输出到excel
                Export_Excel(export_list);
                return sqlfile.ToString();
            }
            catch (Exception e)
            {
                Utility.WriteFile(e.Message);
                return string.Empty;
            }
        }

        public void Export_Excel(List<ExportItem> export_datas)
        {
            try
            {
                if (!string.IsNullOrEmpty(Export_Xlsx_Path))
                {
                    Workbook wookbook = new Workbook();
                    wookbook.Worksheets.Clear();
                    Worksheet sheet = wookbook.Worksheets.Add("Result");
                    sheet.Cells[0, 0].Value = "tablename";
                    sheet.Cells[0, 1].Value = "columnname";
                    sheet.Cells[0, 2].Value = "id";
                    sheet.Cells[0, 3].Value = "value";
                    int index = 1;
                    foreach (var export_data in export_datas)
                    {
                        foreach (DataRow item in export_data.data.Rows)
                        {
                            sheet.Cells[index, 0].Value = export_data.tablename;
                            sheet.Cells[index, 1].Value = export_data.colname;
                            sheet.Cells[index, 2].Value = item[0].ToString();
                            sheet.Cells[index, 3].Value = item[export_data.colname].ToString();
                            index++;
                        }
                        Style objStyle = new CellsFactory().CreateStyle();
                        objStyle.Font.IsBold = true;
                        StyleFlag objStyleFlag = new StyleFlag();
                        objStyleFlag.FontBold = true;
                        sheet.Cells.ApplyRowStyle(0, objStyle, objStyleFlag);
                        // Fit columns width to contents
                        sheet.AutoFitColumns();
                    }
                    int pos = Export_Xlsx_Path.LastIndexOf("\\");
                    string fileName = Export_Xlsx_Path.Substring(0, pos + 1) + System.Guid.NewGuid().ToString() + ".xlsx";
                    //Save workbook as per export type
                    wookbook.Save(fileName, SaveFormat.Xlsx); 
                }

            }
            catch (Exception e)
            {
                Utility.WriteFile(e.Message);
            }
        }
    }
}
