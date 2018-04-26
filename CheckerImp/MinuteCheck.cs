using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckerInterface;
using Model;
using Tool;
using System.Configuration;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;

namespace CheckerImp
{
    [Export(typeof(IClock))]
    public class MinuteCheck : IClock
    {
        private DateTime _now;
        public MinuteCheck()
        {
            _now = OperateIniFile.ReadIniData("minute", "time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Checker_Config.TimerPath).ToDateTime();
            Checker_Timer.Minute_Time = _now;
        }
        public double Time_Span_Check()
        {
            TimeSpan ts = DateTime.Now - Checker_Timer.Minute_Time;
            if (ts.TotalMinutes >= Checker_Config.Minute_Interval)
            {
                Checker_Timer.Minute_Time = DateTime.Now;
                OperateIniFile.WriteIniData("minute", "time", Checker_Timer.Minute_Time.ToString("yyyy-MM-dd HH:mm:ss"), Checker_Config.TimerPath);
                string path = Checker_Config.TaskPath + "\\minute";
                
                //Utility.WriteFile("minutecheck  " + ts.TotalMinutes.ToString() + "分钟");
            }
            return ts.TotalMinutes;
        }
        

    }
}
