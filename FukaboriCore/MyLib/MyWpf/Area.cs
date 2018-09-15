using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MyWpfLib.Draw
{
    public class Area
    {
        Point location = new Point();
        public Point Location {
            get { return location; }
            set { location = value; }
        }
        Size size = new Size();
        public Size Size { get { return size; } set { size = value; } }

        public Area()
        {
        }

        public Area(Point p, Size s)
        {
            this.Location = p;
            this.Size = s;
        }

        public Area(Size s)
        {
            this.location = new Point(0, 0);
            this.size = s;
        }

        public double Top
        {
            get
            {
                return this.Location.Y;
            }
        }
        public double Bottom
        {
            get
            {
                return this.Location.Y + size.Height;
            }            
        }
        public double Left
        {
            get
            {
                return this.location.X;
            }
        }

        public double Right
        {
            get
            {
                return this.location.X + size.Width;
            }
        }

        public bool Contains(Point point)
        {
            if (this.Top < point.Y && this.Bottom > point.Y && this.Left < point.X && this.Right > point.X)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
