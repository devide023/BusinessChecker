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
    class MonthCheck : IClock
    {
        private DateTime _now;
        public MonthCheck()
        {
            _now = OperateIniFile.ReadIniData("month", "time", DateTime.Now.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath).ToDateTime();
            Checker_Timer.Month_Time = _now;
        }
        public double Time_Span_Check()
        {
            TimeSpan ts = DateTime.Now - Checker_Timer.Month_Time;
            if (ts.TotalDays >= Checker_Config.Month_Interval)
            {
                Checker_Timer.Month_Time = DateTime.Now.ToString(Checker_Config.DateFormat).ToDateTime();
                OperateIniFile.WriteIniData("month", "time", Checker_Timer.Month_Time.ToString(Checker_Config.DateFormat), Checker_Config.TimerPath);
                string path = Checker_Config.TaskPath + "\\month";
                
                Utility.WriteFile("monthcheck  " + ts.TotalDays.ToString() + "天");
            }
            return ts.TotalDays;
        }

    }
}
