using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Model
{
    public static class Checker_Config
    {
        public static string BasePath { get; set; }
        /// <summary>
        /// 日志路径hao
        /// </summary>
        public static string LogPath { get; set; }
        public static double Check_Interval { get; set; }
        /// <summary>
        /// 检查器执行时刻yyyy-MM-dd 08:00:00
        /// </summary>
        public static string DateFormat { get; set; }
        public static string TimerPath {get;set;}
        public static string TaskPath {get;set; }
        /// <summary>
        /// 网页关键字检查深度
        /// </summary>
        public static int Check_Depth { get; set; }
        /// <summary>
        /// 分钟间隔设置
        /// </summary>
        public static double Minute_Interval { get; set; }
        public static double Hour_Interval { get; set; }
        public static double Day_Interval { get; set; }
        public static double Week_Interval { get; set; }
        public static double Month_Interval { get; set; }
        public static double Year_Interval { get; set; }

    }
}
