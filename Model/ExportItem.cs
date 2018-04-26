using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Model
{
    public class ExportItem
    {
            public DataTable data { get; set; }        
            public string tablename { get; set; }
            public string colname { get; set; }
    }
}
