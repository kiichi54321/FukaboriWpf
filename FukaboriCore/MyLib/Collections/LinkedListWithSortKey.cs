using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.Collections
{
    //[Serializable]
    public class LinkedListWithSortKey<Tkey,Tvalue>
        where Tkey:IComparable<Tkey>
    {
        List<LinkedListWithSortKeyNode<Tkey, Tvalue>> list = new List<LinkedListWithSortKeyNode<Tkey, Tvalue>>();
        private LinkedListWithSortKeyNode<Tkey, Tvalue> first;
        private LinkedListWithSortKeyNode<Tkey, Tvalue> last;
        private LinkedListWithSortKeyNode<Tkey, Tvalue> current;

        public LinkedListWithSortKeyNode<Tkey, Tvalue> Current
        {
            get { return current; }
            set { current = value; }
        }

        public LinkedListWithSortKeyNode<Tkey, Tvalue> First
        {
            get { return first; }
            set { first = value; }
        }

        public LinkedListWithSortKeyNode<Tkey, Tvalue> Last
        {
            get { return last; }
            set { last = value; }
        }

        private void Test()
        {
            LinkedList<string> l = new LinkedList<string>();
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
            first = null;
            last = null;
        }

        public void AddLast(Tkey key, Tvalue val)
        {
            if (list.Count > 0)
            {
                LinkedListWithSortKeyNode<Tkey, Tvalue> node = new LinkedListWithSortKeyNode<Tkey, Tvalue>(key, val, this);
                list.Add(node);
                last.Next = node;
                node.Previous = last;
                last = node;
            }
            else
            {
                LinkedListWithSortKeyNode<Tkey, Tvalue> node = new LinkedListWithSortKeyNode<Tkey, Tvalue>(key, val, this);
                list.Add(node);
                last = node;
                first = node;
            }
        }

        public void Find(Tkey key)
        {
            if (list.Count > 0)
            {
                if (first.SortKey.CompareTo(key) <= 0)
                {
                    current = first;
                    return;
                }
                if (last.SortKey.CompareTo(key) < 0)
                {
                    current = last;
                    return;
                }
                LinkedListWithSortKeyNode<Tkey, Tvalue> node = first;
                while (node != null && node.Next != null)
                {
                    if (node.SortKey.CompareTo(key) >= 0 && node.Next.SortKey.CompareTo(key) < 0)
                    {
                        current = node;
                        return;
                    }

                    node = node.Next;
                }
            }
            else
            {
              
            }
        }

        public void Insert(Tkey key, Tvalue val)
        {
            if (list.Count > 0)
            {
                if (first.SortKey.CompareTo(key) >= 0)
                {
                    LinkedListWithSortKeyNode<Tkey, Tvalue> node2 = new LinkedListWithSortKeyNode<Tkey, Tvalue>(key, val, this);
                    first.Previous = node2;
                    node2.Next = first;
                    first = node2;
                    list.Add(node2);
                    return;
                }
                if (last.SortKey.CompareTo(key) < 0)
                {
                    LinkedListWithSortKeyNode<Tkey, Tvalue> node2 = new LinkedListWithSortKeyNode<Tkey, Tvalue>(key, val, this);
                    last.Next = node2;
                    node2.Previous = last;
                    last = node2;
                    list.Add(node2);
                    return;
                }
                LinkedListWithSortKeyNode<Tkey, Tvalue> node = first;
                while (node != null && node.Next != null)
                {
                    if (node.SortKey.CompareTo(key) >= 0 && node.Next.SortKey.CompareTo(key) < 0)
                    {
                        LinkedListWithSortKeyNode<Tkey, Tvalue> node2 = new LinkedListWithSortKeyNode<Tkey, Tvalue>(key, val, this);
                        LinkedListWithSortKeyNode<Tkey, Tvalue> node3 = node2.Next;

                        node.Next = node2;
                        node2.Previous = node;

                        node2.Next = node3;
                        node3.Previous = node2;

                        list.Add(node2);
                        return;
                    }

                    node = node.Next;
                }
            }
            else
            {
                LinkedListWithSortKeyNode<Tkey, Tvalue> node2 = new LinkedListWithSortKeyNode<Tkey, Tvalue>(key, val, this);
                first = node2;
                last = node2;
                list.Add(node2);
                return;

            }
        }


        public List<Tkey> Keys
        {
            get
            {
                List<Tkey> tmplist = new List<Tkey>();
                foreach (LinkedListWithSortKeyNode<Tkey, Tvalue> node in list)
                {
                    tmplist.Add(node.SortKey);
                }
                return tmplist;
            }
        }


        public void Sort()
        {
            list.Sort();
            ChangeLink();
        }

        private void ChangeLink()
        {
            LinkedListWithSortKeyNode<Tkey, Tvalue> tmp = null;
            foreach (LinkedListWithSortKeyNode<Tkey, Tvalue> node in list)
            {
                node.Previous = tmp;
                if (tmp != null)
                {
                    tmp.Next = node;
                }
                node.Next = null;
                tmp = node;
            }
            if (list.Count > 0)
            {
                first = list[0];
                last = list[list.Count - 1];
            }
            else
            {
                first = null;
                last = null;
            }
        }

        public void Reverse()
        {
            list.Reverse();
            ChangeLink();
        }

        public bool Next()
        {
            if (current.Next != null)
            {
                current = current.Next;
                return true;
            }
            else
            {
                current = null;
                return false;
            }
        }

        public bool Previous()
        {
            if (current.Previous != null)
            {
                current = current.Previous;
                return true;
            }
            else
            {
                current = null;
                return false;
            }
        }
    }


    public class LinkedListWithSortKeyNode<Tkey, Tvalue> : IComparable<LinkedListWithSortKeyNode<Tkey, Tvalue>>
        where Tkey : IComparable<Tkey>
    {
        public LinkedListWithSortKeyNode<Tkey, Tvalue> Next;
        public LinkedListWithSortKeyNode<Tkey, Tvalue> Previous;
        public LinkedListWithSortKey<Tkey, Tvalue> List;
        public Tkey SortKey;
        public Tvalue Value;


        public LinkedListWithSortKeyNode(Tkey key,Tvalue val,LinkedListWithSortKey<Tkey,Tvalue> list)
        {
            SortKey = key;
            Value = val;
            List = list;
        }

        #region IComparable<LinkedListWithSortKeyNode<Tkey,Tvalue>> ÉÅÉìÉo

        public int CompareTo(LinkedListWithSortKeyNode<Tkey, Tvalue> other)
        {
            return this.SortKey.CompareTo(other.SortKey);
        }

        #endregion
    }
}
