using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPriority
{
    class MainUIWorker
    {
        MainForm form;
        public BackgroundWorker userWorker;
        public MainUIWorker(MainForm form)
        {
            this.form = form;
            userWorker = new BackgroundWorker();
            userWorker.DoWork += new DoWorkEventHandler(UserWorker_DoWork);
            userWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(UserWorker_RunWorkerCompleted);
        }

        public bool RunUserWorker(List<object> list)
        {
            if (userWorker.IsBusy)
            {
                form.infoLabel.Text = "上一个操作还未结束";
                return false;
            }
            userWorker.RunWorkerAsync(list);
            form.infoLabel.Text = "开始执行:" + list[0];
            return true;
        }

        // https://docs.microsoft.com/zh-cn/dotnet/desktop/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls?view=netframeworkdesktop-4.8
        private void UserWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object type = ((List<object>)e.Argument)[0];
            object data = ((List<object>)e.Argument)[1];
            switch (type.ToString())
            {
                //case "label":
                //    e.Result = GetStat(type, data);
                //    break;
                //case "priority":
                //    e.Result = SetPriority(type, data);
                    //break;
                default:
                    e.Result = new List<object> { type, data, false };
                    break;
            }
        }
        private void UserWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object type = ((List<object>)e.Result)[0];
            object data = ((List<object>)e.Result)[1];
            object result = ((List<object>)e.Result)[2];
            if (result.GetType() == typeof(bool))
            {
                if ((bool)result == false)
                {
                    form.infoLabel.Text = "操作失败:" + type;
                    return;
                }
            }
            switch (type.ToString())
            {
                case "label":
                    form.label1.Text = result.ToString();
                    form.infoLabel.Text = "操作完成:" + type;
                    break;
                case "priority":
                    form.infoLabel.Text = "操作完成:" + type;
                    break;
                default:
                    form.infoLabel.Text = "未知操作完成" + type;
                    break;
            }
        }

        //private List<object> GetStat(object type, object data)
        //{
        //    form.targetProcess = ProcessUtil.GetProcessByName(form.textBox1.Text);
        //    return new List<object> { type, data, ProcessUtil.GetProcessesStat(form.targetProcess) };
        //}

        //private List<object> SetPriority(object type, object data)
        //{
        //    PriorityUtil.SetProcessesPriority(form.targetProcess, (ProcessPriorityClass)data);
        //    return new List<object> { type, data, true };
        //}
    }
}
