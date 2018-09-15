using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib
{
    /// <summary>
    /// 循環リスト
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CycleList<T>:List<T>
    {
        int current = 0;
        /// <summary>
        /// 次の要素へ
        /// </summary>
        /// <returns></returns>
        public T Next()
        {
            current++;
            if (current == this.Count)
            {
                current = 0;
            }
            T tmp = this[current];

            return tmp;
        }
        /// <summary>
        /// 前の要素へ
        /// </summary>
        /// <returns></returns>
        public T Back()
        {
            current--;
            if (current == -1)
            {
                current = this.Count - 1;
            }
            T tmp = this[current];
            return tmp;
        }
        /// <summary>
        /// 今の要素へ
        /// </summary>
        /// <returns></returns>
        public T Current()
        {
            return this[current];
        }

        /// <summary>
        /// 最後の要素へ移動
        /// </summary>
        public void SetLast()
        {
            current = this.Count - 1;
        }
        /// <summary>
        /// 最初の要素へ移動
        /// </summary>
        public void SetFirst()
        {
            current = 0;
        }

        public int CurrentPosition
        {
            get
            {
                return current;
            }
        }

        public T GetPosition(int i)
        {
            current = (i+current) % this.Count;
            return this[current];
        }
    }
}
