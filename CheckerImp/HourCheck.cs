using CheckerInterface;
using KeyWords.BLL;
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
    public class HourCheck : IClock
    {
        private DateTime _now;
        private List<sys_url> all_list = new List<sys_url>();
        private List<sys_url> finded_list = new List<sys_url>();
        private Finder web_finder = new Finder();
        private string[] keys = new string[] { };
        private int depth = 0;
        public HourCheck()
        {
            _now = OperateIniFile.ReadIniData("hour", "time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Checker_Config.TimerPath).ToDateTime();
            Checker_Timer.Hour_Time = _now;
        }

        public double Time_Span_Check()
        {
            TimeSpan ts = DateTime.Now - Checker_Timer.Hour_Time;
            string path = Checker_Config.TaskPath + "\\hour";
            KeyWordFinder finder = new KeyWordFinder();
            keys = finder.Read_Keywords(Checker_Config.TaskPath + "\\keywords\\keywords.txt");
            string[] urllist = finder.Read_Keywords(Checker_Config.TaskPath + "\\urllist\\urls.txt");
            string url_log_path = path + "\\url_log\\";
            string log1 = "", log2 = "";
            if (ts.TotalHours >= Checker_Config.Hour_Interval)
            {
                string timename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                log1 = "list_url" + timename;
                log2 = "finded_url" + timename;
                Checker_Timer.Hour_Time = DateTime.Now;
                OperateIniFile.WriteIniData("hour", "time", Checker_Timer.Hour_Time.ToString("yyyy-MM-dd HH:mm:ss"), Checker_Config.TimerPath);
                //
                List<sys_url> list = new List<sys_url>();
                urllist.ToList().ForEach(t=>list.Add(new sys_url {domain=t,pid=0,url=t,depth=0}));
                foreach (var url in list)
                {
                    Utility.WriteFile("---------开始检查"+url.url+"-----------");
                    all_list.Add(url);
                    url.html = web_finder.Get_Html(url.url);
                    var t_list = keys.Where(t => url.html.Contains(t));
                    if (t_list.Count() > 0)
                    {
                        var _keys = "";
                        t_list.ToList().ForEach(t => _keys = _keys + t + ",");
                        url.find_key = _keys;
                        finded_list.Add(url);
                    }
                    List<sys_url> urls_list = web_finder.Get_Link_List(url);
                    urls_list.ForEach(t => all_list.Add(t));
                    foreach (var item in urls_list)
                    {
                        add_sub(item);
                    }
                    Utility.WriteFile("---------结束检查" + url.url + "-----------");
                }
                /*foreach (var item in all_list)
                {
                    Utility.WriteLog(url_log_path, log1, item.id.ToString() + "\t" + item.pid.ToString() + "\t" + item.url);
                }*/
                foreach (var item in finded_list)
                {
                    Utility.WriteLog(url_log_path, log2, item.id.ToString() + "\t" + item.pid.ToString() + "\t" + item.url+"\t"+item.find_key);
                }
                //
                Utility.WriteFile("hourcheck  " + ts.TotalHours.ToString() + "小时");
            }
            return ts.TotalHours;
        }

        private void add_sub(sys_url entity)
        {
            if (depth <= Checker_Config.Check_Depth)
            {
                entity.html = web_finder.Get_Html(entity.url);
                var t_list = keys.Where(t => entity.html.Contains(t));
                if (t_list.Count() > 0)
                {
                    var _keys = "";
                    t_list.ToList().ForEach(t => _keys = _keys + t + ",");
                    entity.find_key = _keys;
                    finded_list.Add(entity);
                }
                List<sys_url> sublist = web_finder.Get_Link_List(entity);
                //去重
                var okurl = sublist.Where(t => all_list.Where(a => a.url == t.url).Count() == 0).ToList();
                okurl.ForEach(t => all_list.Add(t));
                foreach (var bitem in okurl)
                {
                    depth++;
                    add_sub(bitem);
                    depth--;
                }
            }
        }

    }
}
