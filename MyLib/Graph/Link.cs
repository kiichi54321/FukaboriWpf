﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace MyWpfLib.Graph
{
    public class Link:BaseLink, ILink,IDisposable
    {
        Canvas canvas;
        Line line;

        public Line Line
        {
            get { return line; }
        }
        Line clickLine;
        public Line ClickLine
        {
            get { return clickLine; }
        }


        public Link(INode nodeA,INode nodeB, Canvas canvas)
            :base(nodeA,nodeB)
        {
            this.canvas = canvas;
            Init();
            
        }

        private void Init()
        {
            line = new Line();
            clickLine = new Line();
            line.StrokeThickness = 2;
            line.Stroke = new SolidColorBrush(Colors.Black);
            clickLine.StrokeThickness = 5;
            clickLine.Stroke = new SolidColorBrush(Colors.Transparent);
          //  clickLine.Cursor = Cursors.Hand;
            clickLine.MouseDown += new MouseButtonEventHandler(clickLine_MouseDown);
            clickLine.MouseEnter += new MouseEventHandler(clickLine_MouseEnter);
            clickLine.MouseLeave += new MouseEventHandler(clickLine_MouseLeave);

            Binding x1binding = new Binding("NodeX");
            x1binding.Source = node_a;
            x1binding.Mode = BindingMode.TwoWay;
            line.SetBinding(Line.X1Property, x1binding);
            clickLine.SetBinding(Line.X1Property, x1binding);
            

            Binding y1binding = new Binding("NodeY");
            y1binding.Source = node_a;
            y1binding.Mode = BindingMode.TwoWay;
            line.SetBinding(Line.Y1Property, y1binding);
            clickLine.SetBinding(Line.Y1Property, y1binding);

            Binding x2binding = new Binding("NodeX");
            x2binding.Source = node_b;
            x2binding.Mode = BindingMode.TwoWay;
            line.SetBinding(Line.X2Property, x2binding);
            clickLine.SetBinding(Line.X2Property, x2binding);

            Binding y2binding = new Binding("NodeY");
            y2binding.Source = node_b;
            y2binding.Mode = BindingMode.TwoWay;
            line.SetBinding(Line.Y2Property, y2binding);
            clickLine.SetBinding(Line.Y2Property, y2binding);

            
            canvas.Children.Add(line);
            canvas.Children.Add(clickLine);
        }


        public event MouseEventHandler MouseLeave;
        void clickLine_MouseLeave(object sender, MouseEventArgs e)
        {
            clickLine.Stroke = new SolidColorBrush(Colors.Transparent);
            if (MouseLeave != null)
            {
                MouseLeave(this, e);
            }
        }

        public MyLib.Manage.StateInfo State { get; set; }

        public event MouseEventHandler MouseEnter;
        void clickLine_MouseEnter(object sender, MouseEventArgs e)
        {
            clickLine.Stroke = new SolidColorBrush(Color.FromArgb(100,lineColor.R,lineColor.G,lineColor.B));
            if (MouseEnter != null)
            {
                MouseEnter(this, e);
            }
            
        }

        private bool arrowA = false;
        private bool arrowB = false;
        private int arrowWidth = 10;

        




        public event MouseButtonEventHandler MouseDown;
        void clickLine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            this.X1 = node_a.NodeX;
            this.Y1 = node_a.NodeY;
            this.X2 = node_b.NodeX;
            this.Y2 = node_b.NodeY;
        }


        private Color lineColor = Colors.Black;

        public Color LineColor
        {
            get { return lineColor; }
            set
            {
                lineColor = value;
                line.Stroke = new SolidColorBrush(value);
            }
        }

        private double strokeThickeness = 2;
       
        public double StrokeThickeness
        {
            get { return strokeThickeness; }
            set {
                strokeThickeness = value;
                line.StrokeThickness = value;
                clickLine.StrokeThickness = value + 3;

            }
        }

        public event EventHandler RemoveEvent;
        public new void Remove()
        {
            canvas.Children.Remove(line);
            canvas.Children.Remove(clickLine);
            node_a.RemoveLink(this);
            node_b.RemoveLink(this);


            BindingOperations.ClearBinding(line, Line.X1Property);
            BindingOperations.ClearBinding(clickLine, Line.X1Property);
            BindingOperations.ClearBinding(line, Line.Y1Property);
            BindingOperations.ClearBinding(clickLine, Line.Y1Property);

            if (RemoveEvent != null)
            {
                RemoveEvent(this, new EventArgs());
            }

            isDisposed = true;

        }

        public void SetPoint(Point p1, Point p2)
        {
            this.X1 = p1.X;
            this.X2 = p2.X;
            this.Y1 = p1.Y;
            this.Y2 = p2.Y;
        }

        public new void SetPoint()
        {
            this.X1 = node_a.NodeX;
            this.Y1 = node_a.NodeY;
            this.X2 = node_b.NodeX;
            this.Y2 = node_b.NodeY;
        }


        public double X1
        {
            get
            {
                return line.X1;
            }
            set
            {
                line.X1 = value;
                clickLine.X1 = value;
            }
        }

        public double Y1
        {
            get
            {
                return line.Y1;
            }
            set
            {
                line.Y1 = value;
                clickLine.Y1 = value;
            }
        }
        public double X2
        {
            get
            {
                return line.X2;
            }
            set
            {
                line.X2 = value;
                clickLine.X2 = value;
            }
        }

        public double Y2
        {
            get
            {
                return line.Y2;
            }
            set
            {
                line.Y2 = value;
                clickLine.Y2 = value;
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
                line.Opacity = opacity;
                clickLine.Opacity = opacity;
                NotifyPropertyChanged("Opacity");
            }
        }

        public bool CanSelect
        {
            get
            {
                if (clickLine.Visibility == System.Windows.Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    clickLine.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    clickLine.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        #region ILink
        //public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged(string name)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(name));
        //    }
        //}

        System.Windows.Visibility visibility = System.Windows.Visibility.Visible;
        public new System.Windows.Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                line.Visibility = value;
                clickLine.Visibility = value;
            }
        }

        #endregion

        private bool isDisposed = false;

        public bool IsDisposed
        {
            get { return isDisposed; }
        }
        public void Dispose()
        {
            Remove();
        }


    }
}
