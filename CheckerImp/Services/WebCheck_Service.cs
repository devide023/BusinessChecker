using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using CheckerImp;
using CheckerInterface;
using Model;
namespace CheckerImp.Services
{
    public class WebCheck_Service
    {
        private KeyWord_WebChecker _web_checker = new KeyWord_WebChecker();
        private List<string> _check_url_list = new List<string>();
        private List<string> _checked_list = new List<string>();
        private string _url_list_path = Checker_Config.TaskPath + @"\urllist\urls.txt";
        private string _checked_url_path = Checker_Config.TaskPath + @"\urllist\checked_urls.txt";
        public WebCheck_Service()
        {
            _check_url_list = Utility.Read_File_Line(_url_list_path).ToList();
            _checked_list =  Utility.Read_File_Line(_checked_url_path).ToList();
            _web_checker.Complete_Check += _web_checker_Complete_Check;
            var list = _check_url_list.Where(t => !_checked_list.Contains(t));
            if (list.Count() > 0)
            {
                string init_url = list.FirstOrDefault();
                _web_checker.Checking(init_url);
            }
        }
        /// <summary>
        /// 检查完成时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void _web_checker_Complete_Check(object sender, CheckArgs args)
        {
            Utility.WriteFile("---------结束检查" + args.web_entity.url + "-----------");
            Utility.Write_File_Line( _checked_url_path, args.web_entity.url);
            string path = Checker_Config.TaskPath + "\\hour";
            string url_log_path = path + @"\url_log\";
            string timename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            string log2 = "finded_url" + timename;
            foreach (var item in args.finded_key_urls)
            {
                Utility.WriteLog(url_log_path, log2, item.id.ToString() + "\t" + item.pid.ToString() + "\t" + item.url + "\t" + item.find_key);
            }
            _checked_list.Add(args.web_entity.url);
            var list = _check_url_list.Where(t => !_checked_list.Contains(t));
            if (list.Count() > 0)
            {
                string next_url = list.FirstOrDefault();
                _web_checker.Checking(next_url);
            }
        }
    }
}
