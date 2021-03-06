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
        ProcessWorker bgWorker;
        Timer timer1;
        public List<Process> targetProcess;
        BindingList<ProcessProfileModel> processProfileList;
        public MainForm()
        {
            InitializeComponent();

            bgWorker = new ProcessWorker();

            BindPriorityList(comboBox3);

            processProfileList = new BindingList<ProcessProfileModel>();
            var processProfileBinding = new BindingSource();
            processProfileBinding.DataSource = processProfileList;
            comboBox2.DataSource = processProfileBinding.DataSource;
            processProfileList.Add(new ProcessProfileModel());
            CheckSelectedProfile();

            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Enabled = false;
        }

        // https://stackoverflow.com/questions/600869/how-to-bind-a-list-to-a-combobox
        private void BindPriorityList(ComboBox comboBox)
        {
            var bindingSource1 = new BindingSource();
            bindingSource1.DataSource = PriorityUtil.priorityList;
            comboBox.DataSource = bindingSource1.DataSource;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //if (CheckProcessName()) bGWorker.RunWorkerAsync(new List<object> { "label", null });
            //CheckAndStartPriorityWorker();
            bgWorker.RunWorker();
        }

        /// <summary>
        /// 选择框更新事件：全局启用
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="e">事件参数</param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                timer1.Enabled = true;
                infoLabel.Text = "全局开启";
            }
            else
            {
                timer1.Enabled = false;
                infoLabel.Text = "全局关闭";
            }
        }

        /// <summary>
        /// 按钮点击事件：添加/设置进程配置
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="e">事件参数</param>
        private void button3_Click(object sender, EventArgs e)
        {
            ProcessProfileModel ppm = (ProcessProfileModel)comboBox2.SelectedItem;
            if (ppm.processName == null)
            {
                Console.WriteLine("--null");
                foreach (ProcessProfileModel pp in processProfileList)
                {
                    if (pp.processName == textBox2.Text)
                    {
                        label1.Text = "进程名重复";
                        return;
                    }
                }
                ProcessProfileModel nppm = new ProcessProfileModel(bgWorker, textBox2.Text, checkBox2.CheckState, (ProcessPriorityClass)comboBox3.SelectedItem);
                processProfileList.Insert(0, nppm);
                comboBox2.SelectedItem = nppm;
            }
            else
            {
                Console.WriteLine((ProcessProfileModel)comboBox2.SelectedItem);
                ppm.processName = textBox2.Text;
                ppm.SetWithCheckState(checkBox2.CheckState);
                ppm.priority = (ProcessPriorityClass)comboBox3.SelectedItem;
                processProfileList.ResetBindings();
            }
        }

        /// <summary>
        /// 选择栏更新事件：进程配置
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="e">事件参数</param>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelectedProfile();
        }

        /// <summary>
        /// 根据选择的进程配置刷新UI
        /// 检测 是否选择了一个有效的进程 并修改UI
        /// </summary>
        private void CheckSelectedProfile()
        {
            ProcessProfileModel ppm = (ProcessProfileModel)comboBox2.SelectedItem;
            if (ppm.processName == null)
            {
                textBox2.Text = "";
                button3.Text = "添加";
                deletePPM.Visible = false;
            }
            else
            {
                textBox2.Text = ppm.processName;
                checkBox2.CheckState = ppm.enableState ? CheckState.Checked : CheckState.Unchecked;
                comboBox3.SelectedItem = ppm.priority;
                button3.Text = "设置";
                deletePPM.Visible = true;
            }
        }

        /// <summary>
        /// 按钮点击事件：删除选择的进程配置
        /// </summary>
        /// <param name="sender">事件参数</param>
        /// <param name="e">事件参数</param>
        private void deletePPM_Click(object sender, EventArgs e)
        {
            ProcessProfileModel ppm = (ProcessProfileModel)comboBox2.SelectedItem;
            ppm.delete();
            processProfileList.Remove(ppm);
            CheckSelectedProfile();
        }
    }
}
