using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPriority
{
    class ProcessWorker
    {
        private List<ProcessProfileModel> processProfileList;
        public BackgroundWorker bgWorker;
        public ProcessWorker()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
            processProfileList = new List<ProcessProfileModel>();
        }

        public bool RunWorker()
        {
            if (bgWorker.IsBusy)
            {
                //form.infoLabel.Text = "上一个操作还未结束";
                Console.WriteLine("上一个bgWorker还未结束");
                return false;
            }
            bgWorker.RunWorkerAsync();
            Console.WriteLine("开始执行bgWorker");
            //form.infoLabel.Text = "开始执行:" + list[0];
            return true;
        }
        public void RegProcess(ProcessProfileModel ppm)
        {
            processProfileList.Add(ppm);
        }
        public bool UnRegProcess(ProcessProfileModel ppm)
        {
            return processProfileList.Remove(ppm);
        }

        // https://docs.microsoft.com/zh-cn/dotnet/desktop/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls?view=netframeworkdesktop-4.8
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            PriorityUtil.SetProcessesPriorityByModels(processProfileList);
        }
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("bgWorker执行完毕");
        }
    }
}
