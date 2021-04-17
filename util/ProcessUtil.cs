using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace AutoPriority
{
    internal class ProcessUtil
    {

        public static List<Process> GetProcessByName(String name)
        {
            return new List<Process>(Process.GetProcessesByName(name));
        }
        // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.getprocesses?view=net-5.0
        public static string GetProcessesStat(List<Process> ps)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.priorityclass?view=net-5.0
            // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processpriorityclass?view=net-5.0
            string str = "";
            foreach (Process p in ps)
            {
                str += GetProcessStat(p);
            }
            return str;
        }
        public static string GetProcessesStatByName(string name)
        {
            string str = "";
            List<Process> ps = GetProcessByName(name);
            foreach (Process p in ps)
            {
                str += GetProcessStat(p);
            }
            return str;
        }

        static List<int> GetPIDByArgument(String name, String argument = null)
        {
            List<int> pids = new List<int>();
            // https://social.msdn.microsoft.com/Forums/en-US/669eeaeb-e6fa-403b-86fd-302b24c569fb/how-to-get-the-command-line-arguments-of-running-processes?forum=netfxbcl
            ManagementClass mngmtClass = new ManagementClass("Win32_Process");
            foreach (ManagementObject o in mngmtClass.GetInstances())
            {
                // item:https://docs.microsoft.com/zh-cn/windows/win32/cimwin32prov/win32-process
                if (o["Name"].Equals(name) || o["Name"].Equals(name + ".exe"))
                {
                    if (argument != null)
                    {
                        if (!o["CommandLine"].ToString().Contains(argument))
                        {
                            continue;
                        }
                    }
                    //Console.WriteLine(o["Name"] + " [" + o["CommandLine"] + "]");
                    pids.Add(int.Parse(o["ProcessId"].ToString()));
                    //return;
                    //var arg=o["CommandLine"].ToString().Replace(" --", " -").Split(new string[] { " -" }, StringSplitOptions.RemoveEmptyEntries);
                    //Console.WriteLine(string.Join(",",arg));
                    //Console.WriteLine(o);
                }
            }
            //Console.WriteLine(pids.Count);
            //Console.WriteLine(string.Join(",", pids));
            return pids;
        }
        public static List<Process> GetProcessByModel(ProcessProfileModel ppm)
        {
            return GetProcessByName(ppm.processName);
        }

        static List<Process> GetProcessByPID(List<int> pids)
        {
            List<Process> ps = new List<Process>();
            foreach (int pid in pids)
            {
                ps.Add(Process.GetProcessById(pid));
            }
            return ps;
        }

        static string GetProcessStat(Process myProcess)
        {
            // Display current process statistics.
            myProcess.Refresh();
            string str = "";
            str += $"{myProcess} -\n";
            str += "-------------------------------------\n";
            str += $"  Base priority             : {myProcess.BasePriority}\n";
            str += $"  Priority class            : {myProcess.PriorityClass}\n";
            str += $"  Priority Boost Enabled    : {myProcess.PriorityBoostEnabled}\n";
            return str;
        }
        static void PrintProcessStat(Process myProcess)
        {
            Console.Write(GetProcessStat(myProcess));
        }
    }
}