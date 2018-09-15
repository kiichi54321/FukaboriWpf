using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.Collections
{
    /// <summary>
    /// 比較的高速に文字列の存在を確かめる
    /// </summary>
  
    public class StringSearchList
    {
//        private List<string> list = new List<string>();
        private int topStringLength = 1;

        public StringSearchList()
        {
        }

        public StringSearchList(int topLength)
        {
            topStringLength = topLength;
        }

        /// <summary>
        /// 先頭の何文字をインデックスに使うか？
        /// </summary>
        public int TopStringLength
        {
            get { return topStringLength; }
            set { topStringLength = value; }
        }
        private ListDictionary<string, string> listDic = new ListDictionary<string, string>();

        public void Add(string str)
        {
            string top = str.Substring(0, topStringLength);

            listDic.Add(top,str);
            listDic[top].Sort();
        }

        public void AddRange(IEnumerable<string> l)
        {
            IEnumerator<string> enumerator = l.GetEnumerator();
            do
            {
                string str = enumerator.Current;
                string top = str.Substring(0, topStringLength);

                listDic.Add(top, str);
            }
            while (enumerator.MoveNext());

            foreach (string key in listDic.Keys)
            {
                listDic[key].Sort();
            }
        }

        public bool Contains(string str)
        {
            string top = str.Substring(0,topStringLength);
            if (listDic.ContainsKey(top))
            {
                int index = listDic[top].BinarySearch(str);
                if (index >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            listDic.Clear();
        }

        public List<string> List
        {
            get
            {
                List<string> list = new List<string>();

                foreach (string key in listDic.Keys)
                {
                    list.AddRange(listDic[key]);
                }
                return list;
            }
        }

    }
}
