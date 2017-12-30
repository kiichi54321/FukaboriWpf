using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace CrossTableSilverlight.Model
{
    public class Person
    {
        public string Uid { get; set; }
        private Dictionary<string, string> answerDic = new Dictionary<string, string>();

        public Dictionary<string, string> AnswerDic
        {
            get { return answerDic; }
            set { answerDic = value; }
        }

        public void Add(string name, string value)
        {
            answerDic.Add(name, value);
          
        }

        public string GetData(string name)
        {
            if (answerDic.ContainsKey(name))
            {
                return answerDic[name];
            }
            return null;
        }
    }

    
}
