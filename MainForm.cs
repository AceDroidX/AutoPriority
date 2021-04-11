using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPriority
{
    public partial class MainForm : Form
    {
        private BackgroundWorker backgroundWorker1;
        List<Process> targetProcess;
        List<ProcessPriorityClass> priorityList = Enum.GetValues(typeof(ProcessPriorityClass)).Cast<ProcessPriorityClass>().ToList();

        public MainForm()
        {
            InitializeComponent();
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker1_RunWorkerCompleted);
            // https://stackoverflow.com/questions/600869/how-to-bind-a-list-to-a-combobox
            var bindingSource1 = new BindingSource();
            bindingSource1.DataSource = priorityList;
            comboBox1.DataSource = bindingSource1.DataSource;
        }

        private bool CheckProcessName()
        {
            if (textBox1.Text == "")
            {
                label1.Text = "please input process name";
                return false;
            }
            else
            {
                return true;
            }
        }

        // https://docs.microsoft.com/zh-cn/dotnet/desktop/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls?view=netframeworkdesktop-4.8
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            object type = ((List<object>)e.Argument)[0];
            object data = ((List<object>)e.Argument)[1];
            if (type.ToString() == "label")
            {
                targetProcess = ProcessUtil.GetProcessByName(textBox1.Text);
                e.Result = new List<object> { type, data, ProcessUtil.GetProcessesStat(targetProcess) };
            }
            else if (type.ToString() == "priority")
            {
                PriorityUtil.SetProcessesPriority(targetProcess, (ProcessPriorityClass)data);
                e.Result = new List<object> { type, data, true };
            }
            else
            {
                e.Result = new List<object> { type, data, false };
            }
        }
        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object type = ((List<object>)e.Result)[0];
            object data = ((List<object>)e.Result)[1];
            object result = ((List<object>)e.Result)[2];
            if (type.ToString() == "label") label1.Text = result.ToString();
            else if (type.ToString() == "priority") label1.Text = "SetProcessesPriority done";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckProcessName()) backgroundWorker1.RunWorkerAsync(new List<object> { "label", null });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!CheckProcessName()) return;
            if (targetProcess == null)
            {
                label1.Text = "GetStat first";
                return;
                //backgroundWorker1.RunWorkerAsync("label");
            }
            backgroundWorker1.RunWorkerAsync(new List<object> { "priority", (ProcessPriorityClass)comboBox1.SelectedItem });
        }
    }
}
