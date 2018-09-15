using System;
using System.Net;
using System.Windows;

using System.Collections.Generic;
using System.Linq;

namespace MyLib.Statistics
{
    public class ChiSquareTest
    {
        public static ChiSquareTestResult Calculation(List<List<double>> matrix)
        {
            List<double> 横sumList = new List<double>();
            List<double> 縦sumList = new List<double>();

            int maxLenght = 0;
            foreach (var item in matrix)
            {
                maxLenght = Math.Max(maxLenght, item.Count());
                横sumList.Add(item.Sum());
            }
            for (int i = 0; i < maxLenght; i++)
            {
                double sum = 0;
                foreach (var item in matrix)
                {
                     sum += item.ElementAtOrDefault(i);
                }
                縦sumList.Add(sum);
            }
            var all = 横sumList.Sum();


            ChiSquareTestResult result = new ChiSquareTestResult();
            result.TestValue = 0;

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int l = 0; l < maxLenght; l++)
                {
                    var 期待値 = 横sumList[i] * 縦sumList[l] / all ;
                    if (期待値 > 0)
                    {
                        try
                        {
                            result.TestValue += Math.Pow((matrix[i][l] - 期待値), 2) / 期待値;
                        }
                        catch
                        {
                            result.TestValue += Math.Pow((0 - 期待値), 2) / 期待値;
                        }
                    }
                }
            }
            result.自由度 = (縦sumList.Where(n => n > 0).Count() - 1) * (横sumList.Where(n => n > 0).Count() - 1);

            return result;
        }
    }

    public class ChiSquareTestResult
    {
        public double TestValue { get; set; }
        public int 自由度 { get; set; }
    }
}
