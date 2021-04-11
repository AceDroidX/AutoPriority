using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPriority
{
    class PriorityUtil
    {
        public static void SetProcessesPriority(List<Process> process, ProcessPriorityClass priority)
        {
            foreach (Process p in process)
            {
                p.PriorityClass = priority;
            }
        }
    }
}
