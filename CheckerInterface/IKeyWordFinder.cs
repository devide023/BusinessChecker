using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CheckerInterface
{
    public interface IKeyWordFinder
    {
        string Find_Keywords(params string[] keywords);
    }
}
