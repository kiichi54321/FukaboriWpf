
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;

namespace MyWpfLib.Graph
{
    public class GraphManage
    {
        protected List<Link> linkList = new List<Link>();
  //      private List<Node> nodeList = new List<Node>();
        protected Dictionary<Node, NodeControl> nodeDic = new Dictionary<Node, NodeControl>();
        protected Dictionary<string, Node> nodeNameDic = new Dictionary<string, Node>();
        protected MyLib.CountClass linkCount = new MyLib.CountClass();
        protected MyLib.CountClass nodeCount = new MyLib.CountClass();
        protected Canvas canvas;

        public GraphManage(Canvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// 全部消す
        /// </summary>
        public void Clear()
        {
            List<Node> list = new List<Node>(nodeNameDic.Values);
            foreach (var item in list)
            {
                this.Remove(item);
            }
            linkCount.Clear();
            nodeCount.Clear();
            foreach (var item in new List<Link>(linkList))
            {
                this.Remove(item);
            }

        }

        public ICollection<Link> Links
        {
            get {return linkList; }
        }
        public ICollection<Node> Nodes
        {
            get { return nodeDic.Keys; }
        }
        public ICollection<NodeControl> NodeControls
        {
            get
            {
                return nodeDic.Values;
            }
        }

        /// <summary>
        /// ベースGraphManageクラスの　CreateNode
        /// </summary>
        /// <param name="name"></param>
        /// <param name="point"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public NodeControl CreateNode(string name,  Point point,Color color)
        {
            if (nodeNameDic.ContainsKey(name))
            {
                return nodeDic[nodeNameDic[name]];
            }
            Node node = new Node();
            node.NodeName = name;
            if (color != null)
            {
                node.NodeFillColor = color;
                node.NodeStrokeColor = color;
            }
            if (point != null)
            {
                node.CenterPoint = point;
            }
            else
            {
                Random r = new Random();
                node.CenterPoint = new Point(r.NextDouble() * canvas.ActualWidth, r.NextDouble() * canvas.ActualHeight);
            }
            NodeControl nodeControl = new NodeControl();
            nodeControl.SetNode(node);
            nodeControl.RemoveEvent += new EventHandler(nodeControl_RemoveEvent);
            canvas.Children.Add(nodeControl);
            Canvas.SetZIndex(nodeControl, 1);
            nodeDic.Add(node, nodeControl);
            nodeNameDic.Add(name, node);
            return nodeControl;
        }

        void nodeControl_RemoveEvent(object sender, EventArgs e)
        {
            var node = sender as Node;
            Remove(node);
        }

        /// <summary>
        /// ベースGraphManageクラスのCreateNode
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public NodeControl CreateNode(Node node)
        {
            if (nodeDic.ContainsKey(node))
            {
                return nodeDic[node];
            }
            if (nodeNameDic.ContainsKey(node.NodeName))
            {
                var n = nodeNameDic[node.NodeName];
                return nodeDic[n];
            }

            NodeControl nodeControl = new NodeControl();
            nodeControl.SetNode(node);
            nodeControl.RemoveEvent += new EventHandler(nodeControl_RemoveEvent);
            canvas.Children.Add(nodeControl);
            Canvas.SetZIndex(nodeControl, 1);
            nodeDic.Add(node, nodeControl);
            nodeNameDic.Add(node.NodeName, node);
            return nodeControl;

        }

        public void Update()
        {
            foreach (var item in nodeDic.Values)
            {
                item.Update();
            }
            foreach (var item in linkList)
            {
                item.Update();
            }
        }


        public void Remove(Node node)
        {
            if (nodeDic.ContainsKey(node))
            {
                node.RemoveAllLink();
                canvas.Children.Remove(nodeDic[node]);
                nodeDic.Remove(node);
                nodeNameDic.Remove(node.NodeName);
                DisposeLinkList();
            }
            else
            {
            }
        }
        public void Remove(Link link)
        {
            link.Remove();
            DisposeLinkList();
        }

        /// <summary>
        /// Linkの片付けをする
        /// </summary>
        private void DisposeLinkList()
        {
            List<Link> list = new List<Link>();
            foreach (var item in linkList)
            {
                if (item.IsDisposed)
                {
                    list.Add(item);
                }
            }
            foreach (var item in list)
            {
                linkList.Remove(item);
                linkDic.Remove(item.GetNames());
            }
        }

        Dictionary<string, Link> linkDic = new Dictionary<string, Link>();
       /// <summary>
        ///ベースGraphManageクラス の　CreateLink
       /// </summary>
       /// <param name="nodeA"></param>
       /// <param name="nodeB"></param>
       /// <returns></returns>
        public Link CreateLink(Node nodeA, Node nodeB)
        {
            BaseLink l = new BaseLink(nodeA, nodeB);
            if (linkDic.ContainsKey(l.GetNames()))
            {
                return linkDic[l.GetNames()];
            }
            else
            {
                Link link = new Link(nodeA, nodeB, canvas);
                nodeA.AddLink(link);
                nodeB.AddLink(link);
                linkList.Add(link);
                linkDic.Add(link.GetNames(), link);
                return link;
            }
        }

        public Link GetLink(Node nodeA, Node nodeB)
        {
            BaseLink l = new BaseLink(nodeA, nodeB);
            if (linkDic.ContainsKey(l.GetNames()))
            {
                var link = linkDic[l.GetNames()];
                if (link.IsDisposed == false)
                {
                    return link;
                }
                else
                {
                    DisposeLinkList();
                    return null;
                }
            }
            return null;
        }

    }
}
