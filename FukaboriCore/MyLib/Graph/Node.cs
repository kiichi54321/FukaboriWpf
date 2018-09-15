using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MyWpfLib.Graph
{

    public class Node : INode
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
        string subname = "test";
        public string NodeSubText
        {
            get
            {
                return subname;
            }
            set
            {
                subname = value;
                NotifyPropertyChanged("NodeSubText");
            }
        }

        private dynamic extend;

        public dynamic Extend
        {
            get { return extend; }
            set { extend = value; }
        }
        string imageUrl = string.Empty;
        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                NotifyPropertyChanged("ImageUrl");
            }
        }

        public Point CenterPoint
        {
            get
            {
                return new Point(NodeX, NodeY);
            }
            set
            {
                if (lockPosition == false && double.IsNaN( value.X) == false && double.IsNaN( value.Y) == false)
                {

                    NodeX = value.X;
                    NodeY = value.Y;
                    NotifyPropertyChanged("NodeX");
                    NotifyPropertyChanged("NodeY");
                }
            }
        }
        private bool lockPosition = false;

        public bool LockPosition
        {
            get { return lockPosition; }
            set { lockPosition = value; }
        }
        private bool lockSize = false;

        public bool LockSize
        {
            get { return lockSize; }
            set { lockSize = value; }
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
                if (lockPosition == false && double.IsNaN( value) == false)
                {
                    nodeX = value;
                    NotifyPropertyChanged("NodeX");
                }
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
                if (lockPosition == false && double.IsNaN( value)==false)
                {
                    nodeY = value;
                    NotifyPropertyChanged("NodeY");
                }
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
                if (lockSize == false)
                {
                    nodeHeight = value;
                    NotifyPropertyChanged("NodeHeight");
                }
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
                if (lockSize == false)
                {
                    nodeWidth = value;
                    NotifyPropertyChanged("NodeWidth");
                }
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
                if (lockSize == false)
                {
                    this.NodeHeight = value.Height;
                    this.NodeWidth = value.Width;
                }
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
            set
            {
                nodeStrokeColor = value;
                NotifyPropertyChanged("NodeStrokeColor");
            }
        }

        Visibility visibility = Visibility.Visible;

        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
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

        Visibility sublabelVisibility = Visibility.Visible;
        public Visibility SubLabelVisibility
        {
            get
            {
                return sublabelVisibility;
            }
            set
            {
                sublabelVisibility = value;
                NotifyPropertyChanged("SubLabelVisibility");

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
            var list = new List<ILink>(linkNodeDic.Values);
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
            get { return new List<INode>(linkNodeDic.Where(n=>n.Value.Visibility == System.Windows.Visibility.Visible).Select(n=>n.Key)); }
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
            get
            {
                return new List<ILink>(linkNodeDic.Values);
            }
        }

        public int Id { get; set; }


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
