using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib
{
    /// <summary>
    /// 数え上げの時のクラス。
    /// </summary>
    //[Serializable]
    public class CountClass:IComparable<CountClass>
    {

        private object tag; 
        private int count=0;
        public CountClass()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">tag</param>
        public CountClass(object obj)
        {
            tag = obj;
        }

        /// <summary>
        /// １増やす
        /// </summary>
        public int Add()
        {
            count++;
            return count;
        }
        /// <summary>
        /// 指定数分足す
        /// </summary>
        /// <param name="i"></param>
        public int Add(int i)
        {
            count = count + i;
            return count;
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public void Clear()
        {
            count = 0;
        }

        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        /// <summary>
        /// カウント数の文字列を返す。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return count.ToString();
        }



    
        #region IComparable<CountClass> メンバ


        public int  CompareTo(CountClass other)
        {
            return this.count.CompareTo(other.count);
        }

        #endregion
}
}
