using CheckerInterface;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Tool;

namespace CheckerImp
{
    [Export(typeof(IClock))]
    public class YearCheck : IClock
    {
        private DateTime _now;
        public YearCheck()
        {
            _now = OperateIniFile.ReadIniData("year", "time", DateTime.Now.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath).ToDateTime();
            Checker_Timer.Year_Time = _now;
        }
        public double Time_Span_Check()
        {
            TimeSpan ts = DateTime.Now - Checker_Timer.Year_Time;
            if (ts.TotalDays >= Checker_Config.Year_Interval)
            {
                Checker_Timer.Year_Time = DateTime.Now.ToString(Checker_Config.DateFormat).ToDateTime();
                OperateIniFile.WriteIniData("year", "time", Checker_Timer.Year_Time.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath);
                string path = Checker_Config.TaskPath + "\\year";
                
                Utility.WriteFile("yearcheck  " + ts.TotalDays.ToString() + "天");
            }
            return ts.TotalDays;
        }

    }
}
