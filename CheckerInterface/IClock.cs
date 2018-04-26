using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckerInterface
{
    public interface IClock
    {
        /// <summary>
        /// 时间间隔执行检查任务
        /// </summary>
        double Time_Span_Check();
    }
}
