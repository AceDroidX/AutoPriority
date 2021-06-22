using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPriority
{
    class ProcessProfileModel
    {
        public string processName;
        private bool _enableState;
        private ProcessWorker worker;
        public bool enableState { get => _enableState; set { SetState(value); } }
        public ProcessPriorityClass priority;
        public ProcessProfileModel()
        {
            this.processName = null;
        }
        public ProcessProfileModel(ProcessWorker worker, string processName, bool enableState, ProcessPriorityClass priority)
        {
            this.worker = worker;
            this.processName = processName;
            this.enableState = enableState;
            this.priority = priority;
        }
        public ProcessProfileModel(ProcessWorker worker, string processName, CheckState enableState, ProcessPriorityClass priority)
        {
            this.worker = worker;
            this.processName = processName;
            SetWithCheckState(enableState);
            this.priority = priority;
        }

        public void SetWithCheckState(CheckState checkState)
        {
            enableState = checkState == CheckState.Checked;
        }

        private bool SetState(bool b)
        {
            _enableState = b;
            if (b)
            {
                worker.RegProcess(this);
            }
            else
            {
                worker.UnRegProcess(this);
            }
            return b;
        }

        public void delete()
        {
            enableState = false;
        }

        public override string ToString()
        {
            if (processName == null) return "添加...";
            return enableState ? $"✔️{processName}[{priority}]" : $"❌{processName}[{priority}]";
        }
    }
}
