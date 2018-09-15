using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLib
{
	/// <summary>
	/// List の概要の説明です。
	/// </summary>
	public class List
	{
		public List()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}

		/// <summary>
		/// リストの内容をカンマ区切りの文字列にします。
		/// </summary>
		/// <param name="list"></param>
		public static string ListToString(IList list)
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
			foreach(object obj in list)
			{
				strBuilder.Append(obj.ToString());
				strBuilder.Append(",");
			}

			return strBuilder.ToString();
		}

        public static string ListToString(List<string> list)
        {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
            foreach (string obj in list)
            {
                strBuilder.Append(obj);
                strBuilder.Append(",");
            }
            return strBuilder.ToString();
        }

    }
}
