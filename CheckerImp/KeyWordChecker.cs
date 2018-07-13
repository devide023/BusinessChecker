using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Tool;
using KeyWords.BLL;
using System.Threading;
namespace CheckerImp
{
    public class KeyWord_WebChecker
    {
        private List<sys_url> all_list = new List<sys_url>();
        private List<sys_url> finded_list = new List<sys_url>();
        private Finder web_finder = new Finder();
        private string[] keys = new string[] { };
        private int depth = 0;
        public KeyWord_WebChecker()
        {

        }
        public delegate void WebCheckHandler(object sender, CheckArgs args);
        public event WebCheckHandler Complete_Check;

        public void Checking(string url)
        {
            all_list.Clear();
            finded_list.Clear();
            keys = Tool.Utility.Read_File_Line(Checker_Config.BasePath + @"\task\keywords\keywords.txt");
            Utility.WriteFile("---------开始检查" + url + "-----------");
            Thread.Sleep(5000);
            sys_url entry = new sys_url();
            entry.id = 1;
            entry.pid = 0;
            entry.url = url;
            entry.domain = url;
            entry.html = web_finder.Get_Html(entry.url);
            var find_word_list = keys.Where(t => entry.html.Contains(t));
            if (find_word_list.Count() > 0)
            {
                var _keys = "";
                find_word_list.ToList().ForEach(t => _keys = _keys + t + ",");
                entry.find_key = _keys;
                finded_list.Add(entry);
            }
            List<sys_url> urls_list = web_finder.Get_Link_List(entry);
            urls_list.ForEach(t => all_list.Add(t));
            foreach (var item in urls_list)
            {
                add_sub(item);
            }
            if (Complete_Check != null)
            {
                CheckArgs args = new CheckArgs();
                args.web_entity = entry;
                args.finded_key_urls = finded_list;
                Complete_Check(this, args);
            }
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
