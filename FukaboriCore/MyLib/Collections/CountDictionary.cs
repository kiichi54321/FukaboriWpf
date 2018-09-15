using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyLib
{
    /// <summary>
    /// 要素ごとの出現頻度を数え上げます。
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    //[Serializable]
    public class CountDictionary<Tkey>
    {
        private Dictionary<Tkey, MyLib.CountClass> countDic = new Dictionary<Tkey, CountClass>();
        public CountDictionary()
        {

        }

        public List<KeyValuePair<Tkey, int>> GetRankingList()
        {
            List<KeyValuePair<Tkey, int>> list = new List<KeyValuePair<Tkey, int>>();
            foreach (var item in countDic.OrderByDescending(n => n.Value.Count))
            {
                list.Add(new KeyValuePair<Tkey, int>(item.Key, item.Value.Count));
            }
            return list;
        }


        public void Add(Tkey key, int count)
        {
            MyLib.CountClass cc;
            if (countDic.TryGetValue(key, out cc))
            {
                cc.Add(count);
            }
            else
            {
                cc = new CountClass();
                cc.Add(count);
                countDic.Add(key, cc);
            }
        }

        public void Add(Tkey key)
        {
            this.Add(key, 1);
        }

        public void AddRange(ICollection<Tkey> collection)
        {
            if (collection != null)
            {
                foreach (Tkey item in collection)
                {
                    this.Add(item, 1);
                }
            }
        }

        public int GetCount(Tkey key)
        {
            if (countDic.ContainsKey(key))
            {
                return countDic[key].Count;
            }
            return 0;
        }

        public ListWithSortKey<int, Tkey> GetListWithCount()
        {
            ListWithSortKey<int, Tkey> list = new ListWithSortKey<int, Tkey>();
            foreach (Tkey key in countDic.Keys)
            {
                list.Add(GetCount(key), key);
            }
            return list;
        }

        public ListWithSortKey<int, Tkey> GetRanking()
        {
            ListWithSortKey<int, Tkey> list = new ListWithSortKey<int, Tkey>();
            foreach (Tkey key in countDic.Keys)
            {
                list.Add(GetCount(key), key);
            }
            list.SortDescending();
            return list;
        }

        ///// <summary>
        ///// ランキングテーブルを返す
        ///// </summary>
        ///// <returns></returns>
        //public System.Data.DataTable GetRankingTable()
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    DataColumn myDataColumn;

        //    // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = System.Type.GetType("System.Int32");
        //    myDataColumn.ColumnName = "ranking";
        //    myDataColumn.ReadOnly = true;
        //    myDataColumn.Unique = true;
        //    myDataColumn.AutoIncrement = true;
        //    myDataColumn.AutoIncrementSeed = 1;
        //    myDataColumn.AutoIncrementStep = 1;

        //    // Add the Column to the DataColumnCollection.
        //    dt.Columns.Add(myDataColumn);

        //    // Create second column.
        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = System.Type.GetType("System.String");
        //    myDataColumn.ColumnName = "Name";
        //    myDataColumn.AutoIncrement = false;
        //    myDataColumn.Caption = "Name";
        //    myDataColumn.ReadOnly = false;
        //    myDataColumn.Unique = false;
        //    // Add the column to the table.
        //    dt.Columns.Add(myDataColumn);


        //    // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        //    myDataColumn = new DataColumn();
        //    myDataColumn.DataType = System.Type.GetType("System.Int32");
        //    myDataColumn.ColumnName = "Count";
        //    myDataColumn.AutoIncrement = false;
        //    myDataColumn.Caption = "Count";
        //    myDataColumn.ReadOnly = false;
        //    myDataColumn.Unique = false;
        //    // Add the column to the table.
        //    dt.Columns.Add(myDataColumn);

        //    foreach (var item in GetRanking().BasketCollection)
        //    {
        //        var row = dt.NewRow();
        //        row["Name"] = item.val.ToString();
        //        row["Count"] = item.sortkey;
        //        dt.Rows.Add(row);
        //    }
        //    return dt;
        //}

        /// <summary>
        /// 頻度の総計を出します
        /// </summary>
        /// <returns></returns>
        public int GetAllCount()
        {
            int all = 0;
            foreach (CountClass cc in countDic.Values)
            {
                all += cc.Count;
            }
            return all;


        }

        public double GetAvg()
        {
            int all = GetAllCount();
            return (double)all / (double)this.Count; 
        }

        public int GetMax()
        {
            int max = 0;
            foreach (var item in countDic.Values)
            {
                max = Math.Max(max, item.Count);
            }
            return max;
        }


        public ICollection<Tkey> Keys
        {
            get
            {
                return countDic.Keys;
            }
        }
        public int Count
        {
            get
            {
                return countDic.Count;
            }
        }

        public void Clear()
        {
            countDic.Clear();
        }


    }
}
