using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab_Task_Parallelism
{
    public class TaskFactory
    {
        //StartNew 會自動將 thread start
        public int UseFactory()
        {
            var task = Task<int>.Factory.StartNew(() =>
             {
                 return 2 * 2;
             });
            Console.WriteLine(task.Status);
            var nResult = task.Result;
            Console.WriteLine(task.Status);
            return nResult;
        }

        //StartNew 需手動將 thread start
        public int NotUseFactory()
        {
            var task = new Task<int>(() =>
             {
                 return 2 * 2;
             });
            Console.WriteLine(task.Status);
            task.Start();
            var nResult = task.Result;
            Console.WriteLine(task.Status);
            return nResult;
        }


        //implement TaskFactory.FromAsync function
        public byte[] ReadText()
        {
            var file = File.OpenRead(@"C:\temp\DemoText.txt");
            var buff = new byte[1024];
            var task = Task.Factory.FromAsync<byte[], int, int, int>(
                file.BeginRead, file.EndRead, buff, 0, buff.Length, null);
            task.Wait();
            return buff;
        }

        //當底下的 child task 附加到 parent task 時，
        //parent task 會等到底下的 child task 都執行完才會繼續
        public void AttachedToParent()
        {
            var task = Task.Factory.StartNew(() =>
             {
                 var subTask = Task.Factory.StartNew(() =>
                  {
                      Console.ReadLine();
                  }, TaskCreationOptions.AttachedToParent);
                 Console.WriteLine("in maintask");
             });
            task.Wait();  //要等到子 task 都執行完，父 task 才算是執行完畢
            Console.WriteLine("out of maintask");
        }

        //可在廻圈內藉由 Task.wait 的 timeout 設定間隔看是否執行完畢
        public void TaskWaitWithTimeout()
        {
            var task = Task.Factory.StartNew(() =>
            {
                int n = 0;
                for (decimal i = 0; i < 10000000000; i++)
                {
                    n++;
                }
            });

            //the alternative is to use task.IsCompleted
            while (!task.Wait(500))
            {
                Console.Write('.');
            }
            Console.WriteLine("\nComplete");
        }

        private void Sum(int a, int b, Action callback)
        {
            for (decimal i = 0; i < 10000000; i++)
            {

            }
            if (callback != null)
                callback();
        }


        public void InvokeIsAnotherThread()
        {
            var act = new Action(() =>
             {
                 Console.WriteLine("callback function {0}", Thread.CurrentThread.ManagedThreadId);
             });

            var func = new Func<int, int, int>((a, b) =>
             {
                 for (int i = 0; i < 500000000; i++)
                 {

                 }
                 Console.WriteLine("in func BeginInvoke");
                 return a + b;
             });

            func.BeginInvoke(1, 2, x =>
            {
                int c= func.EndInvoke(x);
                Console.WriteLine("in callback {0}", c);
                Console.WriteLine("in callback thread : {0}", Thread.CurrentThread.ManagedThreadId);
            }, null);
        }

        //在ThreadPool內即是使用ConcurrentQueue,huge amount of enqueue and dequeue won't cause error
        public void ThreadPoolWithConcurrentQueue()
        {
            object locker = new object();
            var Tasks = new List<Task>();
            decimal nSum = 0;
            for (int i = 0; i < 10; i++)
            {
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (decimal j = 0; j < 1000000; j++)
                    {
                        lock (locker)
                        {
                            nSum++;  
                        }
                    }
                }));
            }

            Task.WaitAll(Tasks.ToArray());
            Console.WriteLine(string.Format("ThreadPoolWithConcurrentQueue:{0}",nSum));
        }

        public void ThreadWithNoLock()
        {
            object locker = new object();
            var Threads = new List<Thread>();
            decimal nSum = 0;
            for (int i = 0; i < 10; i++)
            {
                var T = new Thread(() =>
                 {
                     for (decimal j = 0; j < 1000000; j++)
                         lock (locker)
                         {
                             nSum++;  
                         }
                 });
                T.Start();
                Threads.Add(T);
            }
            Thread.Sleep(10000);
            
            Console.WriteLine(string.Format("ThreadWithNoLock:{0}", nSum));
        }

        public Task CountingAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine(string.Format("Thread:{0}__CountingAsync__Start",
                   Thread.CurrentThread.ManagedThreadId));
                Thread.Sleep(3000);
                Console.WriteLine(string.Format("Thread:{0}__CountingAsync__End",
                   Thread.CurrentThread.ManagedThreadId));
            });
        }

        public async void CallCountingAsync()
        {
            int nSum = 0;
            Console.WriteLine("CallCountingAsync Start");
            await CountingAsync().ConfigureAwait(false);
            while(true)
            {
                Thread.Sleep(1000);
                nSum++;
                Console.WriteLine(string.Format("Thread:{0}__CallCountingAsync:{1}", 
                    Thread.CurrentThread.ManagedThreadId, nSum));
            }
        }

        public async Task<int> GetAnswer()
        {
            await Task.Delay(3000);
            int answer = 2 * 3;
            return answer;
        }

        public void StillReadInLockMode()
        {
            decimal temp = 0;
            object locker = new object();
            var t1 = Task.Run(() =>
             {
                 lock (locker)
                 {
                     for (int i = 0; i < 10000001; i++)
                     {

                         temp += i;
                     }
                     //temp = i;
                 }
                 Console.WriteLine($"by t1 {temp}");
             });
            var t2 = Task.Run(() =>
             {
                 Interlocked.
                 //Task.Delay(1000);
                 //for (int i = 1000000; i > 1; i--)
                 //{

                 //    lock (locker)
                 //    {
                 //        temp = i;
                 //    }
                 //}
                 Console.WriteLine(temp);

             });
        }
    }
}