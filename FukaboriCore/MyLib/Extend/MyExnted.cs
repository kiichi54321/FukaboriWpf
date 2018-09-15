using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace MyLib.Extend
{
    public static class StringExtensions
    {

        /// <summary>
        /// IsNullOrEmpty　ならTrue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return System.String.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 改行コードを削除します
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string ChopReturnCode(this string txt)
        {
            return txt.Replace("\r", "").Replace("\n", "");
        }

        /// <summary>
        /// 配列を指定した文字列で区切った文字列を返す。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separetor"></param>
        /// <returns></returns>
        public static string JoinString(this IEnumerable<string> array, string separetor)
        {
            StringBuilder strBuider = new StringBuilder();
            foreach (string str in array)
            {
                strBuider.Append(str);
                strBuider.Append(separetor);
            }
            if (strBuider.Length > separetor.Length)
            {
                strBuider.Length = strBuider.Length - separetor.Length;
            }
            return strBuider.ToString();
        }

        /// <summary>
        /// 改行くぎりの配列に変換する。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> TextReadLines(this string text)
        {
            System.IO.StringReader reader = new System.IO.StringReader(text);
            while (reader.Peek() > -1)
            {
                yield return reader.ReadLine();
            }

        }



    }

    public static class DictionaryExtensions
    {
        /// <summary>
        /// Dicに対して、TryGetValueをして、返り値が値。失敗した時はデフォルト値。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(
          this IDictionary<TKey, TValue> source, TKey key
        )
        {
            if (source == null || source.Count == 0)
                return default(TValue);

            TValue result;
            if (source.TryGetValue(key, out result))
                return result;
            return default(TValue);
        }

        /// <summary>
        /// Dicに対して、値の追加。同じ値があったら、上書きをする。
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="Tvalue"></typeparam>
        /// <param name="sorce"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<Tkey,Tvalue> AddKeyValue<Tkey,Tvalue>( this IDictionary<Tkey,Tvalue> sorce,Tkey key,Tvalue value)
        {
            if(sorce == null)
            {
                sorce = default(Dictionary<Tkey, Tvalue>);
            }
            if(sorce.ContainsKey(key))
            {
                sorce[key] = value;
            }
            else
            {
                sorce.Add(key, value);
            }
            return sorce;
        }
    }
}
