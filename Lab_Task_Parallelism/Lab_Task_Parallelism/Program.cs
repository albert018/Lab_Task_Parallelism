using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lab_Task_Parallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            var p = new ParallelLab();
            var fac = new TaskFactory();
            watch.Start();
            //var result = p.OrderByPLINQ(20000000);
            //var result = p.OrderBy(20000000);
            fac.StillReadInLockMode();
            watch.Stop();
            //Console.WriteLine($"time:{watch.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}
