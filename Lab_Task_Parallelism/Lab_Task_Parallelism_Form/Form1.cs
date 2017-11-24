using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_Task_Parallelism_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var task = await GetData();
            this.textBox1.Text = task;
        }

        private async Task<string> GetData()
        {
            var client = new HttpClient();
            var task = client.GetStringAsync("http://msdn.microsoft.com");
            return await task;
        }
    }
}
