using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLib
{
   /// <summary>
    /// ソート可能なキーを持つリスト。そのままのAddではソートされないので注意。
    /// ソートされた状態が欲しいときはソートしないといけない。
    /// ソートKeyは一意でなくてもいい。
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
     public class ListWithSortKey<Tkey, Tvalue> 
        where Tkey:IComparable<Tkey>
    {
        private List<Basket<Tkey,Tvalue>> list;

        public ListWithSortKey()
        {

            list = new List<Basket<Tkey,Tvalue>>();
        }
        public void Add(Tkey key, Tvalue value)
        {
            list.Add(new Basket<Tkey,Tvalue>(key,value));
        }
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Sort()
        {
            list.Sort();
            
        }
        public void SortDescending()
        {
            list.Sort(delegate(Basket<Tkey, Tvalue> b1, Basket<Tkey, Tvalue> b2)
            {
                return b1.CompareTo(b2) * -1;
            });
        }
       

        public void Reverse()
        {
            list.Reverse();
            
        }

        /// <summary>
        /// KeyValuePairのリストで返します。
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<Tkey,Tvalue>> GetKeyValuePairList()
        {
            List<KeyValuePair<Tkey, Tvalue>> keyValuePairList = new List<KeyValuePair<Tkey, Tvalue>>();
            foreach (Basket<Tkey, Tvalue> basket in list)
            {
                keyValuePairList.Add(new KeyValuePair<Tkey, Tvalue>(basket.sortkey, basket.val));
            }
            return keyValuePairList;
        }

        public ICollection<Tkey> Keys
        {
            get
            {
                List<Tkey> keyList = new  List<Tkey>();
                foreach (Basket<Tkey,Tvalue> basket in list)
                {
                    keyList.Add(basket.sortkey);
                }
                return keyList;
            }
        }
        /// <summary>
        /// 重複なしのキーリストを返します。
        /// </summary>
        /// <returns></returns>
        public List<Tkey> GetKeysCutOverLap()
        {
            List<Tkey> keyList = new List<Tkey>(this.Keys);
            return MyLib.ArrayListLib.OverlapToSingle<Tkey>(keyList);
        }
        public ICollection<Tvalue> Values
        {
            get
            {
                List<Tvalue> valuesList = new List<Tvalue>();
                foreach (Basket<Tkey,Tvalue> basket in list)
                {
                    valuesList.Add(basket.val);
                }
                return valuesList;
            }
        }

        public Tvalue[] ValuesToArray
        {
            get
            {
                List<Tvalue> valuesList = new List<Tvalue>();
                foreach (Basket<Tkey, Tvalue> basket in list)
                {
                    valuesList.Add(basket.val);
                }
                return valuesList.ToArray();
            }
        }

        public Tkey GetKey(int num)
        {
            return list[num].sortkey;
        }

        public Tvalue GetValue(int num)
        {
            return list[num].val;
        }

        public ICollection<Tvalue> Search(Tkey key)
        {
            List<Tvalue> searchList = new  List<Tvalue>();
            foreach (Basket<Tkey,Tvalue> basket in list)
            {
                if (basket.sortkey.CompareTo(key) == 0)
                {
                    searchList.Add(basket.val);
                }
            }
            return searchList;
        }

        public ICollection<Tkey> SearchByValue(Tvalue val)
        {
            List<Tkey> searchList = new List<Tkey>();
            foreach (Basket<Tkey, Tvalue> basket in list)
            {
                if (basket.val.Equals(val))
                {
                    searchList.Add(basket.sortkey);
                }
            }
            return searchList;
        }

        /// <summary>
        /// 一つだけとりあえず探す。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TrySearch(Tkey key, out Tvalue value)
        {
            foreach (Basket<Tkey, Tvalue> basket in list)
            {
                if (basket.sortkey.CompareTo(key) == 0)
                {
                    value = basket.val;
                    return true;
                }
            }
            value = default(Tvalue);
            return false;
        }

        private Basket<Tkey, Tvalue> GetBasketByValue(Tvalue val)
        {
            Basket<Tkey, Tvalue> b = null;
            foreach (Basket<Tkey, Tvalue> basket in list)
            {
                if (basket.val.Equals(val))
                {
                    b = basket;
                    break;
                }
            }
            return b;
        }

        private Basket<Tkey, Tvalue> GetBasketByKey(Tkey key)
        {
            Basket<Tkey, Tvalue> b = null;
            foreach (Basket<Tkey, Tvalue> basket in list)
            {
                if (basket.sortkey.CompareTo(key) == 0)
                {
                    b = basket;
                    break;
                }
            }
            return b;
        }

        public int GetRankByKey(Tkey key,out Tvalue value)
        {
            int count = 1;
            foreach (Basket<Tkey, Tvalue> basket in list)
            {
                if (basket.sortkey.CompareTo(key) == 0)
                {
                    value = basket.val;
                    return count;
                }
                count++;
            }
            value = default(Tvalue);
            return -1;
        }

        public int GetRankByKey(Tkey key)
        {
            Tvalue val;
            return GetRankByKey(key, out val);
        }

        public int GetRankByValue(Tvalue value,out Tkey key)
        {
            int count = 1;
            foreach (Basket<Tkey, Tvalue> basket in list)
            {
                if (basket.val.Equals(value))
                {
                    key = basket.sortkey;
                    return count;
                }
                count++;
            }
            key = default(Tkey);
            return -1;
        }

        /// <summary>
        /// 上位のランキングから指定個数取ってくる。同値がある場合は、それも含める。Keyが数値じゃないとだめ。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<Basket<Tkey, Tvalue>> GetRanking(int num)
        {
            List<Basket<Tkey, Tvalue>> list = new List<ListWithSortKey<Tkey, Tvalue>.Basket<Tkey, Tvalue>>();
            List<Tkey> keyList = this.GetKeysCutOverLap();
            keyList.Sort();
            keyList.Reverse();
            int count = 0;
            Tkey min = default(Tkey);
            foreach (Tkey key in keyList)
            {
                count = this.Search(key).Count + count;
                if (num <= count)
                {
                    min = key;
                    break;
                }
                else
                {
                    min = key;
                }
            }
            foreach (Basket<Tkey, Tvalue> b in this.list)
            {
                if (b.sortkey.CompareTo(min) >= 0)
                {
                    list.Add(b);
                }
            }

            return list;


        }


        public int GetRankByValue(Tvalue value)
        {
            Tkey key;
            return GetRankByValue(value, out key);
        }

        public ICollection<Basket<Tkey, Tvalue>> BasketCollection
        {
            get
            {
                return list;
            }
        }

         public class Basket<Tkey1,Tvalue1> : IComparable<Basket<Tkey1,Tvalue1>>
            where Tkey1:IComparable<Tkey1>
        {
            public Tkey1 sortkey;
            public Tvalue1 val;

            public Basket()
            {

            }

            public Basket(Tkey1 key, Tvalue1 obj)
            {
                sortkey = key;
                val = obj;
            }

            public override string ToString()
            {
                return val.ToString();
            }




            #region IComparable<Basket<Tkey,Tvalue>> メンバ

            public int CompareTo(Basket<Tkey1, Tvalue1> other)
            {
                return this.sortkey.CompareTo(other.sortkey);
            }

            #endregion
        }


    }

	
}
