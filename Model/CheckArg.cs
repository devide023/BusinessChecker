using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CheckArgs:EventArgs
    {
        public sys_url web_entity { get; set; }
        public List<sys_url> finded_key_urls { get; set; }
    }
}
