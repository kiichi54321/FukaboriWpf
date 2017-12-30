using System;
using System.Collections;
using System.Collections.Generic;

namespace MyWpfLib.Graph
{
	/// <summary>
	/// Sort とソートに使う指標に関する静的クラスです。
	/// </summary>
	public class Sort
	{
		private Sort()
		{
		}
		/// <summary>
		/// ソートタイプの列挙子
		/// </summary>
		public enum Type
		{
			Match,
			Jaccard,
			Dice,
			Overlap,
			Cosine,
			Dependent,
            Leverage,
            Custom
		}
		/// <summary>
		/// ソートに使う値です。
		/// </summary>
		public static float SortKey(int match,int freq1,int freq2,Type sorttype)
		{
				float tmp;
				switch(sorttype)
				{
					case Type.Match: 
					{
						tmp = Match(match,freq1,freq2);
						break;
					}
					case Type.Jaccard:
					{
						tmp = Jaccard(match,freq1,freq2);
						break;
					}
					case Type.Dice:
					{
						tmp = Dice(match,freq1,freq2);
						break;
					}
					case Type.Overlap:
					{
						tmp = Overlap(match,freq1,freq2);
						break;
					}
					case Type.Cosine:
					{
						tmp = Cosine(match,freq1,freq2);
						break;
					}
					case Type.Dependent:
					{
						tmp = Dependent(match,freq1,freq2);
						break;
					}
					default:
						tmp = Match(match,freq1,freq2);
						break;
				}
				return tmp;
				
		}

        /// <summary>
        /// ソートに使う値です。
        /// </summary>
        public static float SortKey(int match, int freq1, int freq2, int universalFreq, Type sorttype)
        {
            float tmp;
            switch (sorttype)
            {
                case Type.Match:
                    {
                        tmp = Match(match, freq1, freq2);
                        break;
                    }
                case Type.Jaccard:
                    {
                        tmp = Jaccard(match, freq1, freq2);
                        break;
                    }
                case Type.Dice:
                    {
                        tmp = Dice(match, freq1, freq2);
                        break;
                    }
                case Type.Overlap:
                    {
                        tmp = Overlap(match, freq1, freq2);
                        break;
                    }
                case Type.Cosine:
                    {
                        tmp = Cosine(match, freq1, freq2);
                        break;
                    }
                case Type.Dependent:
                    {
                        tmp = Dependent(match, freq1, freq2);
                        break;
                    }
                case Type.Leverage:
                    {
                        tmp = Leverage(match, freq1, freq2, universalFreq);
                        break;
                    }
                default:
                    tmp = Match(match, freq1, freq2);
                    break;
            }
            return tmp;

        }

		/// <summary>
		/// 文字列からソートの種類の列挙氏を返します。
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Sort.Type SearchType(string str)
		{
			switch(str)
			{
				case "Match":
					return Type.Match;
				case "Jaccard":
					return Type.Jaccard;
				case "Dice":
					return Type.Dice;
				case "Overlap":
					return Type.Overlap;
				case "Cosine":
					return Type.Cosine;
				case "Dependent":
					return Type.Dependent;
                //case "LogJaccard":
                //    return Type.LogJaccard;
                //case "LogDependet":
                //    return Type.LogDependent;
				default:
					return Type.Match;
			}
		}

        /// <summary>
        /// 文字列をSort.Typeに変換します。失敗したらJaccardを返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Sort.Type ParseSortType(string str)
        {
            Sort.Type type;
            try
            {
                type = (Sort.Type)System.Enum.Parse(typeof(Sort.Type), str);
            }
            catch
            {
                type = Type.Jaccard;
            }

            return type;
        }

/// <summary>
/// match
/// </summary>
/// <param name="match"></param>
/// <param name="freq1"></param>
/// <param name="freq2"></param>
/// <returns></returns>
		public static float Match(int match,int freq1,int freq2)
		{
				
				return match;
		}
		/// <summary>
		/// (float)match/(float)(freq1+freq2);
		/// </summary>
		public static float Jaccard(int match,int freq1,int freq2)
		{
			{
                if (freq1 + freq2 - match > 0)
                {
                    float tmp = (float)match / (float)(freq1 + freq2 - match);
                    return tmp;
                }
                else
                {
                    return 0;
                }

			}
		}
		/// <summary>
		/// (float)(2*match)/(float)(freq1+freq2)
		/// </summary>
		public static float Dice(int match,int freq1,int freq2)
		{
			{
				float tmp = (float)(2*match)/(float)(freq1+freq2);
				return tmp;
			}
		}
		/// <summary>
		/// (float)match/(float)Math.Min(freq1,freq2)
		/// </summary>
		public static float Overlap(int match,int freq1,int freq2)
		{
			{
				float tmp = (float)match/(float)Math.Min(freq1,freq2);
				return tmp;
			}
		}
		/// <summary>
		/// (float)(match/(Math.Sqrt(freq1)*Math.Sqrt(freq2)))
		/// </summary>
		/// <param name="match"></param>
		/// <param name="freq1"></param>
		/// <param name="freq2"></param>
		/// <returns></returns>
		public static float Cosine(int match,int freq1,int freq2)
		{
			{
				float tmp = (float)(match/(Math.Sqrt(freq1)*Math.Sqrt(freq2)));
				return tmp;

			}
		}
		/// <summary>
		/// (float)match/(float)(freq1*freq2)
		/// </summary>
		/// <param name="match"></param>
		/// <param name="freq1"></param>
		/// <param name="freq2"></param>
		/// <returns></returns>
		public static float Dependent(int match,int freq1,int freq2)
		{
			{
				float tmp = (float)match/(float)(freq1*freq2);
				return tmp;
			}

		}

        public static float Leverage(int match, int freq1, int freq2, int universal)
        {
            float tmp = (float)match / (float)universal - (float)freq1 * (float)freq2 / (float)Math.Pow(universal, 2);
            return tmp;
        }

		public static float LogJaccard(int match,int freq1,int freq2)
		{
			float tmp = (float)(Math.Log(match)/Math.Log(freq1+freq2-match));
			return tmp;
			
		}

		public static float LogDependent(int match,int freq1,int freq2)
		{
			float tmp = (float)(Math.Log(match)/Math.Log(freq1*freq2));
			return tmp;
		}
		/// <summary>
		/// インプット情報から最適な取得インデックスを探し出します。あらかじめのソートが必須
		/// </summary>
		/// <param name="index"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public static int SearchOptimizeNumber<T>(int index,List<T> list)
            where T : IComparable<T>
		{
			int i = index;
			while(true)
			{
				if(i<=0)
				{
					i = -1;
					break;
				}
				if(i>list.Count-2)
				{
					i = list.Count-2;
					break;
				}

				//try
				{
				
					IComparable<T> current = list[i];
                    IComparable<T> next = list[i + 1];
				
					if(current.CompareTo(list[i+1]) ==0)
					{
						i = i-1;
					}
					else
					{
						break;
					}
				}
				//catch
                //{
                //    // TODO: 例外処理をがんがる
                //    i = -1;
                //    break;
                //}
			}
			int index_back = i;
			i = index;
			while(true)
			{
				if(i<1)
				{
					i = 1;
                    break;

				}
				if(i>list.Count-1)
				{
					i = list.Count-1;
					break;
				}

                //try
				{
				
					IComparable<T> current =list[i];
					IComparable<T> next = list[i-1];

                    if (current.CompareTo(list[i - 1]) == 0)
					{
						i = i+1;
					}
					else
					{
						break;
					}
				}
                //catch
                //{
                //    // TODO: 例外処理をがんがる
                //    i = list.Count;
                //    break;
                //}
			}
			int index_forward = i;

			if(Math.Abs(index-index_back) > Math.Abs(index-index_forward))
			{
				return index_forward;
			}
			else
			{
				return index_back;
			}
		}
	}
}
