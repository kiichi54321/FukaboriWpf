using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MyWpfLib.Graph
{
    public class LinkData:BaseLink
    {
        public LinkData(INode nodeA, INode nodeB)
            : base(nodeA, nodeB)
        {
        }

        public LinkData()
        {
        }

        protected Color lineColor = Colors.Black;

        public virtual Color LineColor
        {
            get { return lineColor; }
            set
            {
                lineColor = value;
               
            }
        }

        protected double strokeThickeness = 2;

        public virtual double StrokeThickeness
        {
            get { return strokeThickeness; }
            set
            {
                strokeThickeness = value;


            }
        }

        protected string comment = string.Empty;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        protected string name = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
