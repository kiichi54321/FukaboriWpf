using System;
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
    /// <summary>
    /// NodeControl.xaml の相互作用ロジック
    /// </summary>
    public partial class NodeControl : UserControl, INotifyPropertyChanged
    {
        public NodeControl()
        {
            InitializeComponent();
            
        }

        private bool _isDrag = false;
        private Point _dragOffset;
        public event MouseButtonEventHandler NodeMouseDown;
        public event MouseButtonEventHandler NodeMouseUp;
        public event EventHandler<MyLib.Event.EventGenericArgs<Vector>> NodeMove;
        //        double nodeX, nodeY;
        private Node node;
        public void SetNode(Node n)
        {
            node = n;
            this.DataContext = node;
            this.NodeDraw.Height = node.NodeHeight;
            this.Measure(new Size());
            this.NodeX = node.CenterPoint.X;
            this.NodeY = node.CenterPoint.Y;

        }
        public Node Node
        {
            get
            {
                return node;
            }
        }

        public double NodeY
        {
            get
            {
                return Canvas.GetTop(this) + this.NodeDraw.Height / 2;
            }
            set
            {
                Canvas.SetTop(this, value - this.NodeDraw.Height / 2);
                //if (Canvas.GetTop(this) < 30)
                //{
                //    Canvas.SetTop(this, 30);
                //}
                //else if (Canvas.GetBottom(this) < 30)
                //{
                //    Canvas.SetBottom(this, 30);
                //}
                NotifyPropertyChanged("NodeY");
                node.NodeY = value;
            }
        }

        public double NodeX
        {
            get
            {
               return Canvas.GetLeft(this) + this.ActualWidth / 2;
   //             return node.NodeX;
            }
            set
            {
                double x;
                if (this.ActualWidth > 0)
                {
                    x = value - this.ActualWidth / 2;
                }
                else
                {
                    this.textBox1.Text = node.NodeName;
                    this.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    x = value - this.DesiredSize.Width / 2;
                }
                //if (x > 30)
                //{
                //    Canvas.SetLeft(this, x);
                //}
                //else
                //{
                //    Canvas.SetLeft(this, 30);
                //}
                //if (Canvas.GetRight(this) < 30)
                //{
                //    Canvas.SetRight(this, 30);
                //}

                Canvas.SetLeft(this, x);
                NotifyPropertyChanged("NodeX");
                node.NodeX = value;

            }
        }

        public Size NodeCenterSize
        {
            get
            {
               this.textBox1.Text = node.NodeName;
               this.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
               return new Size(this.DesiredSize.Width / 2, this.NodeDraw.Height / 2);
            }
        }


        //public static readonly DependencyProperty NodeXXProperty = DependencyProperty.Register("NodeXX", typeof(double), typeof(NodeControl), new PropertyMetadata(false));
        //public static readonly DependencyProperty NodeYYProperty = DependencyProperty.Register("NodeYY", typeof(double), typeof(NodeControl), new PropertyMetadata(false));

        //public double NodeX
        //{
        //    get { return (double)this.GetValue(NodeControl.NodeXProperty); }
        //    set
        //    {
        //        double x = value - this.ActualWidth / 2;
        //        Canvas.SetLeft(this, x);
        //        this.SetValue(NodeControl.NodeXProperty, x);
        //    }
        //}

        //public double NodeY
        //{
        //    get { return (double)this.GetValue(NodeControl.NodeXProperty); }
        //    set
        //    {
        //        double y =  value - this.NodeDraw.Height / 2;
        //        Canvas.SetTop(this,y);
        //        this.SetValue(NodeYProperty, y);
        //    }
        //}

        //public event MouseButtonEventHandler NodeMouseDown;
        //public event MouseEventHandler NodeMouseMove;
        //public event MouseButtonEventHandler NodeMouseUp;

        private void NodeDraw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var el = sender as FrameworkElement;
            if (el != null)
            {
                _isDrag = true;
                _dragOffset = e.GetPosition((IInputElement)el.Parent);
                el.CaptureMouse();
                if (NodeMouseDown != null) NodeMouseDown(this, e);
                e.Handled = true;
            }
        }

        private void NodeDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrag)
            {
                UIElement el = sender as UIElement;
                var pt = Mouse.GetPosition((IInputElement)this.Parent);

                double _x= Canvas.GetLeft(this);
                double _y= Canvas.GetTop(this);
 //               double xxx = pt.X - _dragOffset.X ;
                double xxx;
//                xxx = pt.X - _dragOffset.X - (this.ActualWidth + this.node.NodeWidth) / 2 / 2;
                xxx = pt.X - _dragOffset.X;
                if (xxx < 10)
                {
                    xxx = 10;
                }
                else if ((double)this.Parent.GetValue(Canvas.ActualWidthProperty) - this.ActualWidth*0.75 < xxx)
                {
                    xxx = (double)this.Parent.GetValue(Canvas.ActualWidthProperty) - this.ActualWidth * 0.75;
                }

                double yyy = pt.Y - _dragOffset.Y;
                if (yyy < 30)
                {
                    yyy = 30;
                }
                else if ((double)this.Parent.GetValue(Canvas.ActualHeightProperty) - 30 < yyy)
                {
                    yyy = (double)this.Parent.GetValue(Canvas.ActualHeightProperty) - 30;
                }


                Canvas.SetLeft(this,xxx);
                Canvas.SetTop(this, yyy);
                double __x= Canvas.GetLeft(this);
                double __y= Canvas.GetTop(this);
                node.NodeX = this.NodeX;
                node.NodeY = this.NodeY;

                NotifyPropertyChanged("NodeX");
                NotifyPropertyChanged("NodeY");

                double xx = __x - _x;
                double yy = __y - _y;
             //   System.Diagnostics.Debug.Print("NodeDraw_MouseMove "+xx+":"+yy);
                if (NodeMove != null) NodeMove(this, new MyLib.Event.EventGenericArgs<Vector>(new Vector(__x - _x, __y - _y)));
                e.Handled = true;
            }
        }

        //private void LinkMove()
        //{
        //    foreach (var item in node.Links)
        //    {
        //        item.SetPoint();
        //    }
        //}
        public MyLib.Manage.StateInfo State { get; set; }


        private void NodeDraw_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrag)
            {
                UIElement el = sender as UIElement;
                el.ReleaseMouseCapture();
                _isDrag = false;
                e.Handled = true;
            }
            if (NodeMouseUp != null) NodeMouseUp(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private double MaxSize = 100;
        private double MinSize = 10;

        private void NodeDraw_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sizeChangeMouseWheel)
            {
                if (this.node.NodeHeight + e.Delta / 50 < MinSize)
                {
                    node.NodeHeight = MinSize;
                    node.NodeWidth = MinSize;

                }
                else if (node.NodeHeight + e.Delta / 50 > MaxSize)
                {
                    node.NodeHeight = MaxSize;
                    node.NodeWidth = MaxSize;
                }
                else
                {
                    node.NodeWidth = node.NodeWidth + e.Delta / 50;
                    node.NodeHeight = node.NodeHeight + e.Delta / 50;
                }
                e.Handled = true;
                SizeChange();
            }
        }

        private bool sizeChangeMouseWheel = true;

        /// <summary>
        /// MouseWheel でのサイズ変更をする
        /// </summary>
        public bool SizeChangeMouseWheel
        {
            get { return sizeChangeMouseWheel; }
            set { sizeChangeMouseWheel = value; }
        }

        public void Update()
        {
            this.textBox1.Text = node.NodeName;
            this.ellipse.Fill = new SolidColorBrush(node.NodeFillColor);
            this.ellipse.Stroke = new SolidColorBrush(node.NodeStrokeColor);
            SizeChange();
        }


        public void SizeChange()
        {
            this.Height = node.NodeHeight + textBox1.ActualHeight;
            this.NodeDraw.Height = node.NodeHeight;
            this.NameRow.Height = new GridLength(textBox1.ActualHeight);
            this.nodeRow.Height = new GridLength(node.NodeHeight);
            this.NodeX = node.NodeX;
            this.NodeY = node.NodeY;

        }

        public event EventHandler RemoveEvent;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (RemoveEvent != null)
            {
                RemoveEvent(node, new EventArgs());
            }
        }

        private bool canRemove = true;

        public bool CanRemove
        {
            get { return canRemove; }
            set { canRemove = value; }
        }
        private void NodeDraw_MouseEnter(object sender, MouseEventArgs e)
        {
            if(canRemove)  button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void NodeDraw_MouseLeave(object sender, MouseEventArgs e)
        {
            if(canRemove) button1.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}
