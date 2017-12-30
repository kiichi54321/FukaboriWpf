using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyWpfLib.Graph
{
    /// <summary>
    /// Linkの描画なしの基本形
    /// </summary>
    public class BaseLink : ILink
    {
        public BaseLink(INode nodeA,INode nodeB)
        {
            this.node_a = nodeA;
            this.node_b = nodeB;
        }


        #region ILink
        protected INode node_a;
        protected INode node_b;

        public INode Node_a
        {
            get
            {
                return node_a;
            }
            set
            {
                node_a = value;
            }
        }

        public INode Node_b
        {
            get
            {
                return node_b;
            }
            set
            {
                node_b = value;
            }
        }

        int count = 1;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        private Sort.Type sortType = Sort.Type.Jaccard;
        public Sort.Type SortType
        {
            get
            {
                return sortType;
            }
            set
            {
                sortType = value;

            }
        }

        public bool ContainNode(INode node)
        {
            if (node == node_a || node == node_b)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public INode LinkNode(INode node)
        {
            if (node == node_a)
            {
                return node_b;
            }
            if (node == node_b)
            {
                return node_a;
            }
            return null;
        }

        private double sortKey = 0;
        public double SortKey
        {
            get
            {
                if (sortType != Sort.Type.Custom)
                {
                    return Sort.SortKey(count, node_a.Count, node_b.Count, sortType);
                }
                else
                {
                    return sortKey;
                }
            }
            set
            {
                sortKey = value;
                sortType = Sort.Type.Custom;
            }
        }


        /// <summary>
        /// ItemName \t ItemName の形式で返す。
        /// </summary>
        /// <returns></returns>
        public string GetNames()
        {
            if (node_a.NodeName.CompareTo(node_b.NodeName) > 0)
            {
                return node_a.NodeName + "\t" + node_b.NodeName;
            }
            else
            {
                return node_b.NodeName + "\t" + node_a.NodeName;
            }
        }

        public void AddCount()
        {
            count++;
        }

        public void AddCount(int c)
        {
            count = count + c;
        }

        public object Tag
        {
            get;
            set;
        }

        public int CompareTo(ILink other)
        {
            return this.SortKey.CompareTo(other.SortKey);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
        public System.Windows.Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
            }
        }

        #endregion



        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void SetPoint()
        {
            throw new NotImplementedException();
        }
    }
}
