using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.Collections
{
    /// <summary>
    /// 拡張リンクつきリスト。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //[Serializable]
    public class LinkedList<T>:System.Collections.Generic.LinkedList<T>
    {
        /// <summary>
        /// 指定したものを最初に持ってくる。
        /// </summary>
        /// <param name="node"></param>
        public void MoveFirst(LinkedListNode<T> node)
        {
            this.Remove(node);
            this.AddFirst(node);
        }

        /// <summary>
        /// 指定したものを最初に持ってくる。
        /// </summary>
        /// <param name="value"></param>
        public void MoveFirst(T value)
        {
            LinkedListNode<T> node = this.Find(value);
            MoveFirst(node);
        }

        public void MoveLast(LinkedListNode<T> node)
        {
            this.Remove(node);
            this.AddLast(node);
        }

        public void MoveLast(T value)
        {
            LinkedListNode<T> node = this.Find(value);
            if (node != null)
            {
                MoveLast(node);
            }
        }

        public LinkedListNode<T> Replace(LinkedListNode<T> node, ICollection<T> list)
        {
            LinkedListNode<T> tmp = null;

            foreach (var item in list)
            {
                tmp = this.AddBefore(node, item);
            }
            this.Remove(node);
            return tmp;
        }



        public new void RemoveLast()
        {
            this.Remove(this.Last);
        }

        public new void RemoveFirst()
        {
            this.Remove(this.First);
        }

        /// <summary>
        /// コレクションを返す。
        /// </summary>
        public ICollection<T> Collection
        {
            get
            {
                List<T> list = new List<T>();
                LinkedListNode<T> next = this.First;
                while (next != null)
                {
                    list.Add(next.Value);
                    next = next.Next;
                }
                return list;
            }
        }

    }
}
