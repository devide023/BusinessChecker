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
    public class WeekCheck : IClock
    {
        private DateTime _now;
        public WeekCheck()
        {
            _now = OperateIniFile.ReadIniData("week", "time", DateTime.Now.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath).ToDateTime();
            Checker_Timer.Week_Time = _now;
        }
        public double Time_Span_Check()
        {
            TimeSpan ts = DateTime.Now - Checker_Timer.Week_Time;
            if (ts.TotalDays >= Checker_Config.Week_Interval)
            {
                Checker_Timer.Week_Time = DateTime.Now.ToString(Checker_Config.DateFormat).ToDateTime();
                OperateIniFile.WriteIniData("week", "time", Checker_Timer.Week_Time.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath);
                string path = Checker_Config.TaskPath + "\\week";
                
                Utility.WriteFile("weekcheck  " + ts.TotalDays.ToString() + "天");
            }
            return ts.TotalDays;
        }

    }
}
