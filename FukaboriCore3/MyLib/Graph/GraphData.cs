using System;
using System.Net;
using System.Windows;

using System.Collections.Generic;


namespace MyWpfLib.Graph
{
    public class GraphData
    {
        List<Node> nodeDataList = new List<Node>();

        public List<Node> NodeDataList
        {
            get { return nodeDataList; }
            set { nodeDataList = value; }
        }
        List<LinkData> linkDataList = new List<LinkData>();

        public List<LinkData> LinkDataList
        {
            get { return linkDataList; }
            set { linkDataList = value; }
        }

        public string GetXML()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(GraphData));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            serializer.Serialize(ms, this);
            System.IO.StreamReader sr = new System.IO.StreamReader(ms);
            return sr.ReadToEnd();

        }

        public class NodeData
        {
            public NodeData()
            {
            }
            public NodeData(Node node)
            {
                this.NodeX = node.NodeX;
                this.NodeY = node.NodeY;
                this.NodeName = node.NodeName;
                this.NodeHeight = node.NodeHeight;
                this.NodeWidth = node.NodeWidth;
                this.NodeFillColor = node.NodeFillColor;
                this.NodeStrokeColor = node.NodeStrokeColor;
                this.Comment = (string)node.Extend.Comment;
            }



            public int id { get; set; }
            public string NodeName { get; set; }
//            public Point CenterPoint { get; set; }
            public double NodeX { get; set; }
            public double NodeY { get; set; }
            public double NodeHeight { get; set; }
            public double NodeWidth { get; set; }
            //      Size Size { get; set; }
            System.Windows.Visibility Visibility { get; set; }
            System.Windows.Visibility LabelVisibility { get; set; }
            public Color NodeFillColor { get; set; }
            public Color NodeStrokeColor { get; set; }
            public string Comment { get; set; }
        }



    }
}
