using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace MyLib.IO
{
    public class TsvBuilder
    {
        List<TsvLine> line = new List<TsvLine>();
        System.Collections.Generic.HashSet<string> columList = new HashSet<string>();

        TsvLine current = new TsvLine();

        public void NextLine()
        {
            line.Add(current);
            current = new TsvLine();
        }

        public void Add(string key, string val)
        {
            if (val != null)
            {
                current.Add(key, val.Replace("\n", " ").Replace("\r", "").Replace("\t", " "));
                columList.Add(key);
            }
        }

        public string DefaultValue { get; set; } = string.Empty;

        public void Add(string key, int val)
        {
            Add(key, val.ToString());
        }
        public void Add(string key, double val)
        {
            Add(key, val.ToString());
        }
        public class TsvLine
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            public Dictionary<string, string> Dic
            {
                get { return dic; }
                set { dic = value; }
            }

            public void Add(string key, string value)
            {
                if (dic.ContainsKey(key))
                {
                    dic[key] = value;
                }
                else
                {
                    dic.Add(key, value);
                }

            }

            public string GetValue(string key,string defaultValue)
            {
                if (dic.ContainsKey(key))
                {
                    return dic[key];
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in columList)
            {
                sb.Append(item).Append("\t");
            }
            sb.AppendLine();
            foreach (var item in line)
            {
                foreach (var col in columList)
                {
                    sb.Append(item.GetValue(col,DefaultValue)).Append("\t");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

    }

}
