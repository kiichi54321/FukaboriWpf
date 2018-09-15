using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace RawlerLib.MyExtend
{
    public static class Parallel
    {
        /// <summary>
        /// 並列数え上げ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <param name="readRange">それぞれが一度に読み込む量</param>
        /// <param name="ThreadNum">スレッド数</param>
        /// <param name="progressAction">進行状況</param>
        /// <returns></returns>
        public static Dictionary<T, int> ParalellCount<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> func, int readRange, int ThreadNum, Action<int> progressAction)
        {
            Stack<T> stack = new Stack<T>(source);
            object lockobject = new object();
            int count = 0;
            int all = stack.Count;
            List<System.Threading.Tasks.Task<Dictionary<T, int>>> tasks = new List<System.Threading.Tasks.Task<Dictionary<T, int>>>();
            for (int i = 0; i < ThreadNum; i++)
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew<Dictionary<T, int>>((n) =>
                {
                    T[] range = new T[readRange];
                    Dictionary<T, int> cDic = new Dictionary<T, int>();
                    while (true)
                    {
                        T l ;
                        lock(lockobject)
                        {
                            l = stack.Pop();
                        }
                        if (l == null)
                        {
                            break;
                        }
                        l = stack.Pop();
                            foreach (var item2 in func(l))
                            {
                                cDic.AddCount(item2);
                            }
                        if (progressAction != null)
                        {
                            var c1 = System.Threading.Interlocked.Add(ref count, 1);
                            progressAction((c1 * 100 / all).MaxMin(100, 0));
                        }
                    }
                    return cDic;
                }, System.Threading.Tasks.TaskCreationOptions.LongRunning);
                if (task != null) tasks.Add(task);
            }
            System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
            return tasks.Select(n => n.Result).Marge();
        }
    }
}
