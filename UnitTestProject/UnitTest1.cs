using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FukaboriCore.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

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
