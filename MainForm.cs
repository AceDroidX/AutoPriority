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
        BGWorker bgWorker;
        Timer timer1;
        public List<Process> targetProcess;
        List<ProcessPriorityClass> priorityList = Enum.GetValues(typeof(ProcessPriorityClass)).Cast<ProcessPriorityClass>().ToList();

        public MainForm()
        {
            InitializeComponent();

            bgWorker = new BGWorker(this);

            // https://stackoverflow.com/questions/600869/how-to-bind-a-list-to-a-combobox
            var bindingSource1 = new BindingSource();
            bindingSource1.DataSource = priorityList;
            comboBox1.DataSource = bindingSource1.DataSource;

            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Enabled = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //if (CheckProcessName()) bGWorker.RunWorkerAsync(new List<object> { "label", null });
            //CheckAndStartPriorityWorker();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckProcessName()) bgWorker.RunUserWorker(new List<object> { "label", null });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CheckAndStartPriorityWorker();
        }

        private void CheckAndStartPriorityWorker()
        {
            if (!CheckProcessName()) return;
            if (targetProcess == null)
            {
                label1.Text = "GetStat first";
                return;
                //backgroundWorker1.RunWorkerAsync("label");
            }
            bgWorker.RunUserWorker(new List<object> { "priority", (ProcessPriorityClass)comboBox1.SelectedItem });
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) timer1.Enabled = true;
            else timer1.Enabled = false;
        }
    }
}
