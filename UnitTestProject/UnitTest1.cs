using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FukaboriCore.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Enqueite enqueite = new Enqueite();
            System.Console.WriteLine( enqueite.ToJson());

           

        }

        [TestMethod]
        public async Task MyTestMethod2()
        {
            Enqueite enqueite = new Enqueite();
            enqueite.QuestionLoad(new System.IO.StreamReader(@"C:\Users\kiich\Downloads\918270_rawdata\質問項目.txt"));
            await enqueite.DataLoad(new System.IO.StreamReader(@"C:\Users\kiich\Downloads\918270_rawdata\918270_rawdata.tsv"));
            System.Console.WriteLine(enqueite.ToJson());
        }

        [TestMethod]
        public void MyTestMethod3()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine("Key\tText\tType\tAnswers\t");
            foreach(var line in System.IO.File.ReadAllLines(@"C:\Users\kiich\OneDrive\Fukabori\しせいどう？\質問文.tsv").Skip(1))
            {
                var d = line.Split('\t');

                if (d[1] != "MA")
                {
                    stringBuilder.Append(d[0]);
                    stringBuilder.Append("\t");
                    stringBuilder.Append($"{d[6]} {d[5]}");
                    stringBuilder.Append("\t");
                    stringBuilder.Append("離散");
                    stringBuilder.Append("\t");

                    stringBuilder.Append(string.Join(",", d.Skip(7).Where(n => n != string.Empty).Select((n, index) => $"{index + 1}:{n}")));
                    stringBuilder.AppendLine();
                }
                else
                {
                    foreach(var item in d.Skip(7).Where(n => n != string.Empty).Select((text, index) => (text,index)))
                    {
                        stringBuilder.Append($"{d[0]}_{item.index+1}");

                        stringBuilder.Append("\t");
                        stringBuilder.Append($"{item.text} [{d[6]}]");
                        stringBuilder.Append("\t");
                        stringBuilder.Append("離散");
                        stringBuilder.Append("\t");

                        stringBuilder.Append("0:いいえ,1:はい");
                        stringBuilder.AppendLine();

                    }

                }
            }

            System.Console.WriteLine(stringBuilder.ToString());

        }


        [TestMethod]
        public void MyTestMethod4()
        {
            var file = @"D:\work\コロナアンケート.tsv";

            var text = System.IO.File.ReadAllText(file).Replace("\r\n","\n");

            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;

            foreach (var item in text)
            {
                if(item == '\"')
                {
                    flag = !flag;
                }
                else if(item == '\n')
                {
                    if(flag)
                    {
                        stringBuilder.Append("<br/>");
                    }
                    else
                    {
                        stringBuilder.Append("\n");
                    }
                }
                else
                {
                    stringBuilder.Append(item);
                }
            }
//            System.Console.WriteLine(stringBuilder.ToString());

            System.IO.File.WriteAllText(@"D:\work\コロナアンケート2.tsv", stringBuilder.ToString().Replace("\n", "\r\n"));
        }
    }

    public static class ListExnted
    {
        public static List<T> AddItem<T>(this List<T> list,T item)
        {
            list.Add(item);
            return list;
        }
    }



}
