using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_Task_Parallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            int nSum = 0;
            Console.WriteLine("main thread {0}", Thread.CurrentThread.ManagedThreadId);
            var t = new TaskFactory();
            //t.ActionIsAnotherThread();
            //Console.WriteLine(convert);
            t.CallCountingAsync();
            while (true)
            {
                Thread.Sleep(1000);
                nSum++;
                Console.WriteLine(string.Format("Thread:{0}__Main:{1}",
                    Thread.CurrentThread.ManagedThreadId, nSum));
            }
            Console.ReadLine();
        }
    }
}
