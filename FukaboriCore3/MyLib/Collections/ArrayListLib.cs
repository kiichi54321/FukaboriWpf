using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLib
{
	/// <summary>
	/// Class1 の概要の説明です。
	/// </summary>
	public class ArrayListLib
	{
		public ArrayListLib()
		{

		}

        /// <summary>
        /// 重複を削除したリストを返す。リストの中身がソート可能である必要があります。(ジェネリック対応版）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> OverlapToSingle<T>(List<T> list)
        where T:IComparable<T>
        {
            if (list == null)
            {
                return new List<T>();
            }
            List<T> tmpList = new List<T>(list);
            tmpList.Sort();

            List<T> newList = new List<T>();

            if (tmpList.Count > 0)
            {
                T tmpT = tmpList[0];
                newList.Add(tmpT);
                foreach (T t in tmpList)
                {
                    if (t.Equals(tmpT) == false)
                    {
                        newList.Add(t);
                        tmpT = t;
                    }
                    
                }
            }
            return newList;
        }

        /// <summary>
        /// 重複を削除したリストを返す。リストの中身がソート可能である必要があります。(ジェネリック対応版）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> OverlapToSingle<T>(ICollection<T> list)
        where T : IComparable<T>
        {
            if (list == null)
            {
                return new List<T>();
            }
            List<T> tmpList = new List<T>(list);
            tmpList.Sort();

            List<T> newList = new List<T>();

            if (tmpList.Count > 0)
            {
                T tmpT = tmpList[0];
                newList.Add(tmpT);
                foreach (T t in tmpList)
                {
                    if (t.Equals(tmpT) == false)
                    {
                        newList.Add(t);
                        tmpT = t;
                    }

                }
            }
            return newList;
        }

        /// <summary>
        /// 重複を削除したリストを返します。中身がソート可能である必要はありません。
        /// ですが、長く、項目数の多いリストになると遅くなる傾向があります。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> OverlabToSingleNotComparable<T>(List<T> list)
        {
            List<T> newlist = new List<T>();
            foreach (T t in list)
            {
                if (newlist.Contains(t) == false)
                {
                    newlist.Add(t);
                }
            }
            return newlist;
            
        }
        /// <summary>
        /// 重複を削除したリストを返す。リストの中身がソート可能である必要があります。(ジェネリック対応版）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //public static List<T> OverlapToSingle(List<T> list)
        //{
        //    list.Sort();
        //    List<T> newList = new List<T>();
            
        //    T tmpObj = null;
        //    foreach (T obj in list)
        //    {
        //        if (obj.Equals(tmpObj) == false)
        //        {
        //            newList.Add(obj);
        //            tmpObj = obj;
        //        }
        //    }
        //    return newList;
        //}


        /// <summary>
        /// 二つのリストを足したものを返す（ジェネリック版）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> MixList<T>(List<T> list1, List<T> list2)
        {
            List<T> newList = new List<T>();
            newList.AddRange(list1);
            newList.AddRange(list2);

            return newList;
           
        }


        /// <summary>
        /// 二つのリストの同じ要素の数を数える。
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static int MatchCount<T>(List<T> list1, List<T> list2)
            where T:IComparable<T>
        {
            List<T> list1Tmp = OverlapToSingle<T>(list1);
            List<T> list2Tmp = OverlapToSingle<T>(list2);

            List<T> list = MixList(list1Tmp, list2Tmp);
            list.Sort();
            int count = -1;

            if (list.Count > 0)
            {
                T tmpObj = list[0];
                foreach (T obj in list)
                {
                    if (obj.Equals(tmpObj) == false)
                    {
                        tmpObj = obj;
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 二つのリストの同じ要素を返す。
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> MatchList<T>(List<T> list1, List<T> list2)
            where T : IComparable<T>
        {
            List<T> list1Tmp = OverlapToSingle<T>(list1);
            List<T> list2Tmp = OverlapToSingle<T>(list2);

            List<T> list = MixList(list1Tmp, list2Tmp);
            list.Sort();
//            int count = -1;

            List<T> listTmp = new List<T>();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count-1; i++)
                {
                    T tmpObj1 = list[i];
                    T tmpObj2 = list[i+1];
                    if (tmpObj1.Equals(tmpObj2))
                    {
                        listTmp.Add(tmpObj2);
                    }
                }
                listTmp = OverlapToSingle<T>(listTmp);
            }
            return listTmp;
        }

        /// <summary>
        /// 差分を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> DiffList<T>(List<T> list1, List<T> list2,out List<T> matchList)
            where T : IComparable<T>
        {
            List<T> listTmp = OverlapToSingle<T>(list1);
            matchList = MatchList<T>(listTmp, list2);

            foreach (T var in matchList)
            {
                listTmp.Remove(var);
            }

            return listTmp;
        }

        /// <summary>
        /// 差分を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static List<T> DiffList<T>(List<T> list1, List<T> list2)
    where T : IComparable<T>
        {
            List<T> match = new List<T>();
            return DiffList<T>(list1, list2, out match);
        }


        /// <summary>
        /// リストをシャッフルする。ランダムを指定する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ShuffleList<T>(List<T> list,Random r)
        {
            MyLib.ListWithSortKey<double, T> randomList = new ListWithSortKey<double, T>();
            if (r == null)
            {
                r = new Random();
            }
            for (int i = 0; i < list.Count; i++)
            {
                randomList.Add(r.NextDouble(), list[i]);
            }
            randomList.Sort();
            
            return new List<T>(randomList.Values);
        }

        public static List<T> ShuffleList<T>(List<T> list)
        {
            return ShuffleList<T>(list, null);
        }
	}
}
