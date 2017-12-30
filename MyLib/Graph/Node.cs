using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;

namespace MyWpfLib.Graph
{
    public class Node:INode
    {
        string name;
        public string NodeName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("NodeName");
            }
        }

        private dynamic extend;

        public dynamic Extend
        {
            get { return extend; }
            set { extend = value; }
        }

        public Point CenterPoint
        {
            get
            {
                return new Point(NodeX, NodeY);
            }
            set
            {
                NodeX = value.X;
                NodeY = value.Y;
                NotifyPropertyChanged("NodeX");
                NotifyPropertyChanged("NodeY");
                
            }
        }

        double nodeX = 0;
        public double NodeX
        {
            get
            {
                return nodeX;
            }
            set
            {
                nodeX = value;
                NotifyPropertyChanged("NodeX");
            }
        }
        double nodeY = 0;

        public double NodeY
        {
            get
            {
                return nodeY;
            }
            set
            {
                nodeY = value;
                NotifyPropertyChanged("NodeY");
            }
        }
        double nodeHeight = 20;
        public double NodeHeight
        {
            get
            {
                return nodeHeight;
            }
            set
            {
                nodeHeight = value;
                NotifyPropertyChanged("NodeHeight");

            }
        }

        double nodeWidth = 20;
        public double NodeWidth
        {
            get
            {
                return nodeWidth;
            }
            set
            {
                nodeWidth = value;
                NotifyPropertyChanged("NodeWidth");
            }
        }

        public Size Size
        {
            get
            {
                return new Size(nodeWidth, nodeHeight);
            }
            set
            {
                this.NodeHeight = value.Height;
                this.NodeWidth = value.Width;
            }
        }

        Color nodeFillColor = Colors.Black;
        public Color NodeFillColor
        {
            get { return nodeFillColor; }
            set
            {
                nodeFillColor = value;
                NotifyPropertyChanged("NodeFillColor");
            }
        }
        Color nodeStrokeColor = Colors.Black;

        public Color NodeStrokeColor
        {
            get { return nodeStrokeColor; }
            set { nodeStrokeColor = value;
            NotifyPropertyChanged("NodeStrokeColor");
            }
        }

        Visibility visibility = Visibility.Visible;

        public Visibility Visibility
        {
            get { return visibility; }
            set { visibility = value;
            NotifyPropertyChanged("Visibility");            
            }
        }

        Visibility labelVisibility = Visibility.Visible;
        public Visibility LabelVisibility
        {
            get
            {
                return labelVisibility;
            }
            set
            {
                labelVisibility = value;
                NotifyPropertyChanged("LabelVisibility");

            }
        }

        private double textMaxWidth = 100;
        public double TextMaxWidth
        {
            get { return textMaxWidth; }
            set
            {
                textMaxWidth = value;
                NotifyPropertyChanged("TextMaxWidth");

            }
        }

        private Dictionary<INode, ILink> linkNodeDic = new Dictionary<INode, ILink>();


        public ILink SearchLink(INode node)
        {
            ILink link = null;
            if (linkNodeDic.TryGetValue(node, out link))
            {
                return link;
            }
            else
            {
                return null;
            }
        }

        public void AddLink(ILink link)
        {
            INode node = link.LinkNode(this);

            if (linkNodeDic.ContainsKey(node) == false)
            {
                linkNodeDic.Add(node, link);
            }
        }

        public void RemoveLink(ILink link)
        {
            INode node = link.LinkNode(this);
            if (linkNodeDic.ContainsKey(node))
            {
                linkNodeDic.Remove(node);
            }
        }
        public void RemoveAllLink()
        {
            var list =new List<ILink>( linkNodeDic.Values);
            foreach (var item in list)
            {
                item.Remove();
            }
            linkNodeDic.Clear();
        }

        private int count = 1;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        public void AddCount()
        {
            count++;
        }

        public void AddCount(int c)
        {
            count = count + c;
        }

        public List<INode> LinkedNodes
        {
            get { return new List<INode>(linkNodeDic.Keys); }
        }

        public void Reset()
        {
            linkNodeDic.Clear();
            count = 1;
        }

        public object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// 状態を示すオブジェクトを入れるところ。
        /// </summary>
        public dynamic State
        {
            get;
            set;
        }

        public int CompareTo(INode other)
        {
            return this.name.CompareTo(other.NodeName);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        public List<ILink> Links
        {
            get {
                return new List<ILink>(linkNodeDic.Values);
            }
        }
        double opacity = 1;
        /// <summary>
        /// 透明度　0～1の値
        /// </summary>
        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                if (value > 1)
                {
                    opacity = 1;
                }
                else if (value < 0)
                {
                    opacity = 0;
                }
                else
                {
                    opacity = value;
                }
                NotifyPropertyChanged("Opacity");
            }
        }
    }
}
