﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class sys_url
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string domain { get; set; }
        public string url { get; set; }
        public string html { get; set; }
        public int depth { get; set; }

    }
}
