using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyLib.Statistics
{
    /// <summary>
    /// 相関係数を求める
    /// </summary>
    public class Correlation
    {
        List<Pair> list = new List<Pair>();
        public class Pair
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        public void Add(double x, double y)
        {
            list.Add(new Pair() { X = x, Y = y });
        }

        public double Result()
        {
            var x_avg = list.Select(n => n.X).Average();
            var y_avg = list.Select(n => n.Y).Average();

            var x_std = Math.Sqrt(list.Select(n => Math.Pow(n.X - x_avg, 2)).Average());
            var y_std = Math.Sqrt(list.Select(n => Math.Pow(n.Y - y_avg, 2)).Average());

            var xy = list.Select(n => (n.X - x_avg) * (n.Y - y_avg)).Average();

            return xy / (x_std * y_std);
        }


        public static double Get(double[] d1, double[] d2)
        {
            double s = 0;
            double d1_s = 0;
            double d2_s = 0;
            for (int i = 0; i < Math.Min(d1.Length, d2.Length); i++)
            {
                if (double.IsNaN(d1[i]) || double.IsNaN(d2[i]))
                {

                }
                else
                {
                    s = s + d1[i] * d2[i];
                    d1_s = d1_s + Math.Pow(d1[i], 2);
                    d2_s = d2_s + Math.Pow(d2[i], 2);
                }
            }
            return s / (Math.Sqrt(d1_s) * Math.Sqrt(d2_s));
        }
    }
}
