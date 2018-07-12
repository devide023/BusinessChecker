using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Model;
using Tool;
using System.ComponentModel.Composition;
using CheckerInterface;
using System.Configuration;
using System.ComponentModel.Composition.Hosting;
using CheckerImp;
using CheckerImp.Services;
using System.Threading;
namespace BusinessChecker
{    
    public partial class CheckerService : ServiceBase
    {
        [ImportMany(typeof(IClock))]
        private IEnumerable<IClock> _Clocks { get; set; }
        private System.Timers.Timer time_count = new System.Timers.Timer();
        public CheckerService()
        {
            InitializeComponent();
            //
            string base_path = AppDomain.CurrentDomain.BaseDirectory;
            Checker_Config.BasePath = base_path;
            Checker_Config.TimerPath = base_path + @"\timer.ini";
            Checker_Config.TaskPath = base_path + @"\task";
        }

        private void Read_Config()
        {
            Checker_Config.Check_Interval = (ConfigurationManager.AppSettings["interval"] != null ? ConfigurationManager.AppSettings["interval"] : "30000").ToDouble();
            Checker_Config.DateFormat = ConfigurationManager.AppSettings["dateformat"] != null ? ConfigurationManager.AppSettings["dateformat"] : "yyyy-MM-dd HH:mm:ss";
            Checker_Config.Check_Depth = (ConfigurationManager.AppSettings["check_depth"] != null ? ConfigurationManager.AppSettings["check_depth"] : "4").ToInt();
            Checker_Config.Minute_Interval = (ConfigurationManager.AppSettings["minute_interval"] != null ? ConfigurationManager.AppSettings["minute_interval"] : "1").ToDouble();
            Checker_Config.Hour_Interval = (ConfigurationManager.AppSettings["hour_interval"] != null ? ConfigurationManager.AppSettings["hour_interval"] : "1").ToDouble();
            Checker_Config.Day_Interval = (ConfigurationManager.AppSettings["day_interval"] != null ? ConfigurationManager.AppSettings["day_interval"] : "1").ToDouble();
            Checker_Config.Week_Interval = (ConfigurationManager.AppSettings["week_interval"] != null ? ConfigurationManager.AppSettings["week_interval"] : "1").ToDouble();
            Checker_Config.Month_Interval = (ConfigurationManager.AppSettings["month_interval"] != null ? ConfigurationManager.AppSettings["month_interval"] : "1").ToDouble();
            Checker_Config.Year_Interval = (ConfigurationManager.AppSettings["year_interval"] != null ? ConfigurationManager.AppSettings["year_interval"] : "1").ToDouble();
        }
        private void InitConifg()
        {
            Read_Config();
            var catlog = new DirectoryCatalog(Checker_Config.BasePath);
            CompositionContainer container = new CompositionContainer(catlog);
            container.ComposeParts(this);
            time_count.Interval = Checker_Config.Check_Interval;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                InitConifg();
                //定时任务检查
                time_count.Elapsed += Time_count_Elapsed;
                time_count.Enabled = true;
                time_count.Start();
                Utility.WriteFile("启动");
                //网站关键字检查
                Thread check_thread = new Thread(()=> {
                    WebCheck_Service webchecker = new WebCheck_Service();
                    webchecker.Start_Check();
                });
                check_thread.Start();
            }
            catch (Exception e)
            {
                Utility.WriteFile(e.Message);
            }
        }

        private void Time_count_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Read_Config();
            foreach (IClock clock in _Clocks)
            {                
                double ts = clock.Time_Span_Check();
            }
        }

        protected override void OnContinue()
        {
            InitConifg();
            time_count.Start();
            Utility.WriteFile("重启");
        }

        protected override void OnShutdown()
        {
            time_count.Stop();
            Utility.WriteFile("关闭");
        }

        protected override void OnPause()
        {
            time_count.Stop();
            Utility.WriteFile("暂停");
        }

        protected override void OnStop()
        {
            time_count.Stop();
            Utility.WriteFile("停止");
        }
    }
}
