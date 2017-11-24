using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace Lab_Task_Parallelism_Form
{
    public partial class frmGetData : Form
    {
        private CancellationTokenSource _CTS;

        public frmGetData()
        {
            InitializeComponent();
            _CTS = new CancellationTokenSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var TaskGetPerson = GetPerson_async();
            textBox1.Text += string.Format("Thread:{0}  button1_click_start\r\n", Thread.CurrentThread.ManagedThreadId);
            TaskGetPerson.ContinueWith(t =>
            {
                switch (TaskGetPerson.Status)
                {
                    case TaskStatus.RanToCompletion:
                        dataGridView1.DataSource = t.Result;
                        break;
                    case TaskStatus.Canceled:
                        break;
                    case TaskStatus.Faulted:
                        break;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            button1.Enabled = true;
        }

        private async Task<List<Person>> GetPerson_async()
        {
            var TaskGetPerson= Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                throw new Exception("GetPerson_async error");
                
                return GetData.GetPerson();
            });
            var result = await TaskGetPerson;
            return result;
        }

        //子thread掛載到父thread後，會等待裡面的子thread執行完成後再往下跑
        private Task<List<Person>> GetPerson_async_with_child_task()
        {
            var TaskGetPerson = Task.Factory.StartNew(() =>
            {
                textBox1.Invoke(new Action(() =>textBox1.Text+= string.Format("Thread:{0}  GetPerson_async_parent_start\r\n",
                     Thread.CurrentThread.ManagedThreadId)));

                var TaskChild = Task.Factory.StartNew(() =>
                {
                    textBox1.Invoke(new Action(() => textBox1.Text += string.Format("Thread:{0}  GetPerson_async_Child_start\r\n",
                        Thread.CurrentThread.ManagedThreadId)));
                    Thread.Sleep(5000);
                    textBox1.Invoke(new Action(() => textBox1.Text += string.Format("Thread:{0}  GetPerson_async_Child_completed\r\n",
                        Thread.CurrentThread.ManagedThreadId)));
                },TaskCreationOptions.AttachedToParent);

                Thread.Sleep(2000);
                //throw new Exception("GetPerson_async error");

                textBox1.Invoke(new Action(() => textBox1.Text += string.Format("Thread:{0}  GetPerson_async_parent_completed\r\n",
                     Thread.CurrentThread.ManagedThreadId)));
                return GetData.GetPerson();
            });

            
            return TaskGetPerson;
        }

        //dealing with error by continuation
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            var TaskGetPerson = GetPerson_async();
            textBox1.Text += string.Format("Thread:{0}  button2_click_start\r\n", Thread.CurrentThread.ManagedThreadId);
            TaskGetPerson.ContinueWith(t =>
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.DataSource = t.Result;
                }));
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            button2.Enabled = true;
        }

        //to stop task by CancellationTokenSource
        private void button3_Click(object sender, EventArgs e)
        {
            var TaskCounting = CountingAsync(_CTS.Token);
            
        }

        private Task CountingAsync(CancellationToken v_CT)
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    textBox1.Invoke(new Action(() =>
                    {
                        textBox1.Text += ".";
                    }));

                    if (v_CT.IsCancellationRequested)
                        return;
                }
            },v_CT);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _CTS.Cancel();
        }
    }
}
