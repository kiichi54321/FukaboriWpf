
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyLib.Statistics
{
    /// <summary>
    /// 平均と分散を求めてくれるクラスです。
    /// </summary>
    public class AvgStd
    {
        List<double> dataList = new List<double>();
        public AvgStd()
        {
        }

        public AvgStd(IEnumerable<double> collection)
        {
            dataList = new List<double>(collection);
            flag = false;
        }

        public void Clear()
        {
            dataList.Clear();
            flag = false;
        }

        public void Add(double d)
        {
            if (double.IsNaN(d) == false)
            {
                dataList.Add(d);
                flag = false;
            }
        }

        public double GetAvg()
        {
            double sum = 0;
            foreach (double d in dataList)
            {
                sum += d;
            }
            return sum / (double)dataList.Count;
        }

        public double GetStd()
        {
            double sum = 0;
            avg = GetAvg();

            foreach (double d in dataList)
            {
                sum += Math.Pow(d - avg, 2);
            }

            std = Math.Sqrt(sum / (double)dataList.Count);
            flag = true;
            return std;
        }

        public void GetAvgStd(out double avg, out double std)
        {
            double sum = 0;
            avg = GetAvg();

            foreach (double d in dataList)
            {
                sum += Math.Pow(d - avg, 2);
            }

            std = Math.Sqrt(sum / (double)dataList.Count);

        }

        public double 標準化(double value)
        {
            if (flag == false) GetStd();
            return (value - avg) / std;
        }

        public double Get偏差値(double value)
        {
            if (flag == false) GetStd();
            return (value - avg) / std * 10 + 50;
        }

        bool flag = false;
        double avg = 0;
        double std = 0;
        public double Avg
        {
            get
            {
                if (flag)
                {
                    return avg;
                }
                else
                {
                    GetStd();
                    return avg;
                }
            }
        }
        public double Std
        {
            get
            {
                if (flag)
                {
                    return std;
                }
                else
                {
                    GetStd();
                    return std;
                }
            }
        }

        public int Count
        {
            get
            {
                return dataList.Count;
            }
        }

        public double GetMin()
        {
            double min = double.MaxValue;
            foreach (var item in dataList)
            {
                min = Math.Min(item, min);
            }
            return min;
        }

        public double GetMax()
        {
            double max = double.MinValue;
            foreach (var item in dataList)
            {
                max = Math.Max(item, max);
            }
            return max;
        }

        public double Quartile(int num)
        {
            var sorted_list = this.dataList.OrderBy(n => n).ToArray();
            if (sorted_list.Length > 0)
            {
                return sorted_list[sorted_list.Length * num / 4];
            }
            return double.NaN;
        }

        public double ジニ係数
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < dataList.Count; i++)
                {
                    for (int k = 0; k < dataList.Count; k++)
                    {
                        if (i != k)
                        {
                            sum += Math.Abs(dataList[i] - dataList[k]);
                        }
                    }
                }
                return sum / (GetAvg() * 2);
            }
        }

        /// <summary>
        /// ジニ係数を返す。ジニ係数を出せない時は、0を返す。
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static double Getジニ係数(double[] dataList)
        {
            if (dataList.Length == 0) return 0;
            double sum = 0;
            double count = 0;
            for (int i = 0; i < dataList.Length; i++)
            {
                for (int k = 0; k < dataList.Length; k++)
                {
                    if (i != k)
                    {
                        sum += Math.Abs(dataList[i] - dataList[k]);
                        count++;
                    }
                }
            }
            return (sum/count) / (dataList.Average() * 2);
        }
    }
}


