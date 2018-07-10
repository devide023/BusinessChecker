using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Model;
using Tool;

namespace KeyWords.BLL
{
    public class Finder
    {
        private int row_id = 1;
        public Finder()
        {

        }

        public string Get_Html(string url)
        {
            string html = string.Empty;
            Stream responseStream;
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    request.Method = "GET";
                    responseStream = request.GetResponse().GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                    {
                        html = reader.ReadToEnd();
                        responseStream.Close();
                    }
                }
                catch (Exception e)
                {
                    html = e.Message;
                }
            }
            return html;
        }

        public List<sys_url> Get_Link_List(sys_url url_entity)
        {
            List<sys_url> list = new List<sys_url>();
            if (!string.IsNullOrEmpty(url_entity.html))
            {
                StringBuilder reg = new StringBuilder();
                reg.Append("<a.*?href=['\"](?<url>.*?)['\"].*?</a>");
                MatchCollection matchs = Regex.Matches(url_entity.html, reg.ToString(), RegexOptions.IgnoreCase);
                foreach (Match match in matchs)
                {
                    string okurl = "";
                    string url = match.Groups["url"].ToString().ToLower().Trim();
                    if (url.Contains("http") && url.Contains(url_entity.domain))
                    {
                        if (list.Where(t => t.url == url).Count() == 0)
                        {
                            okurl = url;
                            list.Add(new sys_url { domain = url_entity.domain, url = okurl, id = row_id, pid = url_entity.id });
                            row_id++;
                        }
                    }
                    if (url.StartsWith("/"))
                    {
                        okurl = url_entity.domain + url;
                        if (list.Where(t => t.url == okurl).Count() == 0)
                        {
                            list.Add(new sys_url { domain = url_entity.domain, url = okurl, id = row_id, pid = url_entity.id });
                            row_id++;
                        }
                    }
                }
            }
            return list;
        }
    }
}
