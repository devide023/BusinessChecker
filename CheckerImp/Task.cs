using CheckerInterface;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tool;

namespace CheckerImp
{
    public class Task : ITask
    {
        public int Do_Task(string path)
        {
            int ok = 0;
            try
            {
                SqlHelper SH = new SqlHelper();
                DirectoryInfo dicinfo = new DirectoryInfo(path);
                FileInfo[] files = dicinfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    using (StreamReader sr = new StreamReader(file.FullName, Encoding.UTF8))
                    {
                        string sql = sr.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(sql.Replace(" ", "")))
                        {
                            ok = SH.Execute_SQL(sql);
                            if (ok > 0)
                            {
                                Utility.WriteFile(file.FullName+"\n"+sql);
                            }
                        }
                        else
                        {
                            Utility.WriteFile(file.FullName+"空文件!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utility.WriteFile(e.Message);
            }
            return ok;
        }
    }
}
