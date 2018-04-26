using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace CheckerInterface
{
    public interface ISql
    {
        int Execute_SQL(string sql);
        DataTable Get_Data(string sql);
    }
}
