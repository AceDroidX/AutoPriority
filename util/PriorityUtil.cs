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
        public static List<ProcessPriorityClass> priorityList => Enum.GetValues(typeof(ProcessPriorityClass)).Cast<ProcessPriorityClass>().ToList();
        public static void SetProcessesPriority(List<Process> process, ProcessPriorityClass priority)
        {
            foreach (Process p in process)
            {
                p.PriorityClass = priority;
            }
        }
        public static void SetProcessesPriorityByModel(ProcessProfileModel ppm)
        {
            SetProcessesPriority(ProcessUtil.GetProcessByModel(ppm),ppm.priority);
        }
        public static void SetProcessesPriorityByModels(List<ProcessProfileModel> ppml)
        {
            foreach (ProcessProfileModel ppm in ppml)
            {
                SetProcessesPriorityByModel(ppm);
            }
        }
    }
}
