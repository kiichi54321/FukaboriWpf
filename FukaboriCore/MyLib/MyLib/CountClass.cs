using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib
{
    /// <summary>
    /// �����グ�̎��̃N���X�B
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
        /// �P���₷
        /// </summary>
        public int Add()
        {
            count++;
            return count;
        }
        /// <summary>
        /// �w�萔������
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
        /// �J�E���g���̕������Ԃ��B
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return count.ToString();
        }



    
        #region IComparable<CountClass> �����o


        public int  CompareTo(CountClass other)
        {
            return this.count.CompareTo(other.count);
        }

        #endregion
}
}
