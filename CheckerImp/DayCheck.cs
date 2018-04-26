using CheckerInterface;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using Tool;

namespace CheckerImp
{
    [Export(typeof(IClock))]
    public class DayCheck : IClock
    {
        private DateTime _now;
        public DayCheck()
        {
            _now = OperateIniFile.ReadIniData("day", "time", DateTime.Now.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath).ToDateTime();
            Checker_Timer.Day_Time = _now;
        }
        public double Time_Span_Check()
        {
            TimeSpan ts = DateTime.Now - Checker_Timer.Day_Time;
            if (ts.TotalDays >= Checker_Config.Day_Interval)
            {
                /*Checker_Timer.Day_Time = DateTime.Now.ToString(Checker_Config.DateFormat).ToDateTime();
                OperateIniFile.WriteIniData("day", "time", Checker_Timer.Day_Time.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath);
                string path = Checker_Config.TaskPath + "\\day";
                List<KeyWordFinder> finder_list = new List<KeyWordFinder>();
                finder_list.Add(new KeyWordFinder("SiteConn"));
                finder_list.Add(new KeyWordFinder("StrDB"));
                foreach (var finder in finder_list)
                {                
                    string[] keys = finder.Read_Keywords(Checker_Config.TaskPath + "\\keywords\\keywords.txt");
                    DataTable result = finder.Find_Source(Checker_Config.TaskPath + "\\keywords\\keyword_source.sql");
                    finder.Export_Xlsx_Path = Checker_Config.TaskPath + "\\keywords\\exportdata.xlsx";
                    finder.Find_Keywords(keys);
                }
                Utility.WriteFile("daycheck  " + ts.TotalDays.ToString() + "天");*/
            }
            return ts.TotalDays;
        }

    }
}
