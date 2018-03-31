using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_Task_Parallelism
{
    public class ParallelLab
    {
        public IEnumerable<decimal> OrderByPLINQ(int value)
        {
            var Range = new List<decimal>();
            var rand = new Random(DateTime.Now.Millisecond);
            for (decimal i = 0; i < value; i++)
            {
                Range.Add(rand.Next());
            }
            var Result = from x in Range.AsParallel()
                         orderby x descending
                         select x;
            return Result;
        }

        public IEnumerable<decimal> OrderBy(int value)
        {
            var Range = new List<decimal>();
            var rand = new Random(DateTime.Now.Millisecond);
            for (decimal i = 0; i < value; i++)
            {
                Range.Add(rand.Next());
            }
            var Result = from x in Range
                         orderby x descending
                         select x;
            return Result;
        }

        public void Test()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var iEnum = list.GetEnumerator();
            while (iEnum.MoveNext())
                if (iEnum.Current == 7)
                    Test2(iEnum);
        }

        public void Test2(IEnumerator<int> iEnum)
        {
            while (iEnum.MoveNext())
                Console.WriteLine(iEnum.Current);
        }
    }
}
