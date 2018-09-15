using System;
using System.Net;
using System.Windows;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MyLib.Task
{
    public static class Parallel
    {
        public static void ForEach<T>(IEnumerable<T> list, Action<T> action)
            where T : new()
        {
            List<System.Threading.Tasks.Task> taskList = new List<System.Threading.Tasks.Task>();       
            Dictionary<int, List<T>> dic = new Dictionary<int, List<T>>();
            int count = 0;
            int core = Environment.ProcessorCount;
            foreach (var item in list)
            {
                if (dic.ContainsKey(count % core) == false)
                {
                    dic.Add(count % core, new List<T>());
                }
                dic[count % core].Add(item);
                count++;
            }
           
            for (int i = 0; i < dic.Count; i++)
            {              
                taskList.Add(System.Threading.Tasks.Task.Factory.StartNew( (obj) =>
                 {
                     var tmpList = obj as List<T>;
                     foreach (var item in tmpList)
                     {
                         action(item);
                     }
                 }
                ,dic[i].ToList()));
            }
            System.Threading.Tasks.Task.WaitAll(taskList.ToArray());
        }

        public static void For(int len,Action<int> action)
        {
            ForEach<int>(Enumerable.Range(0, len), action);
        }

    }

    public static class Utility
    {
        public static TaskScheduler UISyncContext { get; set; }

        public static void UITask(Action action, TaskScheduler UISyncContext)
        {
            System.Threading.Tasks.Task reportProgressTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                action();
            },
                      CancellationToken.None,
                      TaskCreationOptions.None,
                      UISyncContext);
            reportProgressTask.Wait();
        }

        public static void UITask(Action action)
        {
            if (UISyncContext != null)
            {
                System.Threading.Tasks.Task reportProgressTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    action();
                },
                          CancellationToken.None,
                          TaskCreationOptions.None,
                          UISyncContext);
                reportProgressTask.Wait();
            }
            else
            {
                action();
            }
        }
    }
}
