using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace MySilverlightLibrary.Graph
{
    public partial class TriangleChartControl : UserControl
    {
        public TriangleChartControl()
        {
            InitializeComponent();

            CreateBase();

            dataList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(dataList_CollectionChanged);
        }

        void dataList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetDataPoint();
        }


        public ContentControl Point1Content { get; set; }
        public ContentControl Point2Content { get; set; }
        public ContentControl Point3Content { get; set; }


        private Point centerPoint = new Point();
        private int centerLength = 100;
        private List<Point> threePointList = new List<Point>();
        private System.Collections.ObjectModel.ObservableCollection<TriangleChartData> dataList = new System.Collections.ObjectModel.ObservableCollection<TriangleChartData>();

        public System.Collections.ObjectModel.ObservableCollection<TriangleChartData> DataList
        {
            get { return dataList; }
            set { dataList = value; }
        }

        bool visibleName = true;

        public bool VisibleName
        {
            get { return visibleName; }
            set { visibleName = value; }
        }

        public void CreateBase()
        {
            CreateThreePoint();
        }



        public int SubLineCount
        {
            get { return (int)GetValue(SubLineCountProperty); }
            set { SetValue(SubLineCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubLineCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubLineCountProperty =
            DependencyProperty.Register("SubLineCount", typeof(int), typeof(TriangleChartControl), new PropertyMetadata(4));

        public Brush TraiangleFill
        {
            get { return (Brush)GetValue(TraiangleFillProperty); }
            set { SetValue(TraiangleFillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TraiangleFillProperty =
            DependencyProperty.Register("TraiangleFill", typeof(Brush), typeof(TriangleChartControl), new PropertyMetadata(new SolidColorBrush(Colors.White)));


        private void CreateThreePoint()
        {
            this.Measure(this.RenderSize);

            centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight * 2 / 3);
            centerLength = (int)(this.ActualHeight * 0.8 * 2 / 3);
            double kakudo = Math.PI * 2 / 3;
            double startKakudo = Math.PI;
            threePointList.Clear();

            Polygon polygon = new Polygon();
            polygon.Fill = this.TraiangleFill;

            for (int i = 0; i < 3; i++)
            {
                Point p = new Point(centerPoint.X + (int)(Math.Sin(startKakudo + kakudo * i) * centerLength), centerPoint.Y + (int)(Math.Cos(startKakudo + kakudo * i) * centerLength));
                threePointList.Add(p);
                polygon.Points.Add(p);
            }
            baseCanvas.Children.Clear();
            baseCanvas.Children.Add(polygon);
            for (int i = 0; i < 3; i++)
            {
                List<Point> tmpList = new List<Point>();
                for (int k = 0; k < 3; k++)
                {
                    if (i != k)
                    {
                        tmpList.Add(threePointList[k]);
                    }
                }

                for (int l = 1; l < SubLineCount; l++)
                {
                    float r = l / (float)SubLineCount;
                    var p1 = MyWpfLib.Mathematics.Geometry.GetInternalPoint(threePointList[i], tmpList[0], r);
                    var p2 = MyWpfLib.Mathematics.Geometry.GetInternalPoint(threePointList[i], tmpList[1], r);
                    baseCanvas.Children.Add(new Line()
                    {
                        X1 = p1.X,
                        Y1 = p1.Y,
                        X2 = p2.X,
                        Y2 = p2.Y,
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.LightGray),
                        StrokeDashOffset = 1
                    });

                }
            }
            baseCanvas.Children.Add(new Line()
            {
                X1 = threePointList[0].X,
                Y1 = threePointList[0].Y,
                X2 = threePointList[1].X,
                Y2 = threePointList[1].Y,
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Colors.Black)
            });
            baseCanvas.Children.Add(new Line()
            {
                X1 = threePointList[0].X,
                Y1 = threePointList[0].Y,
                X2 = threePointList[2].X,
                Y2 = threePointList[2].Y,
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Colors.Black)
            });
            baseCanvas.Children.Add(new Line()
            {
                X1 = threePointList[1].X,
                Y1 = threePointList[1].Y,
                X2 = threePointList[2].X,
                Y2 = threePointList[2].Y,
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Colors.Black)
            });
            LabelSetPoint();
            //     LabelPaint();
        }

        public DataTemplate DataTemplete { get; set; }
        private int labelMargn = 10;

        public int LabelMargn
        {
            get { return labelMargn; }
            set { labelMargn = value; }
        }

        private void LabelPaint()
        {
            if (Point1Content != null)
            {
                if (labelCanvas.Children.Contains(Point1Content) == false)
                {
                    labelCanvas.Children.Add(Point1Content);
                    Point1Content.SizeChanged += (o, e) =>
                    {
                        if (threePointList.Count > 0)
                        {
                            Canvas.SetLeft(Point1Content, threePointList[0].X - Point1Content.ActualWidth / 2);
                            Canvas.SetTop(Point1Content, threePointList[0].Y - LabelMargn - Point1Content.ActualHeight / 2);
                        }
                    };
                }
            }
            if (Point2Content != null)
            {
                if (labelCanvas.Children.Contains(Point2Content) == false)
                {
                    labelCanvas.Children.Add(Point2Content);
                    Point2Content.SizeChanged += (o, e) =>
                        {
                            if (threePointList.Count > 0)
                            {
                                Canvas.SetLeft(Point2Content, threePointList[1].X - Point2Content.ActualWidth / 2 - Math.Sqrt(LabelMargn));
                                Canvas.SetTop(Point2Content, threePointList[1].Y + Math.Sqrt(LabelMargn));
                            }
                        };
                }
            }
            if (Point3Content != null)
            {
                if (labelCanvas.Children.Contains(Point3Content) == false)
                {
                    labelCanvas.Children.Add(Point3Content);
                    Point3Content.SizeChanged += (o, e) =>
                        {
                            if (threePointList.Count > 0)
                            {
                                Canvas.SetLeft(Point3Content, threePointList[2].X - Point3Content.ActualWidth / 2 + Math.Sqrt(LabelMargn));
                                Canvas.SetTop(Point3Content, threePointList[2].Y + Math.Sqrt(LabelMargn));
                            }
                        };
                }
            }
        }

        private void LabelSetPoint()
        {
            if (Point1Content != null)
            {
                Canvas.SetLeft(Point1Content, threePointList[0].X - Point1Content.ActualWidth / 2);
                Canvas.SetTop(Point1Content, threePointList[0].Y - LabelMargn - Point1Content.ActualHeight / 2);
            }
            if (Point2Content != null)
            {
                Canvas.SetLeft(Point2Content, threePointList[1].X - Point2Content.ActualWidth / 2 - Math.Sqrt(LabelMargn));
                Canvas.SetTop(Point2Content, threePointList[1].Y + Math.Sqrt(LabelMargn));
            }
            if (Point3Content != null)
            {
                Canvas.SetLeft(Point3Content, threePointList[2].X - Point3Content.ActualWidth / 2 + Math.Sqrt(LabelMargn));
                Canvas.SetTop(Point3Content, threePointList[2].Y + Math.Sqrt(LabelMargn));
            }
        }
        public void SetDataPoint()
        {
            canvas2.Children.Clear();
            foreach (TriangleChartData data in dataList)
            {
                GetPoint(data);
                FrameworkElement d;
                if (DataTemplete != null)
                {
                    d = DataTemplete.LoadContent() as FrameworkElement;
                }
                else
                {
                    d = new Button() { Width = 10, Height = 10 };
                }
                d.DataContext = data;
                canvas2.Children.Add(d);
                d.Tag = data;
                if (double.IsNaN(data.Point.X) == false && double.IsNaN(data.Point.Y) == false)
                {
                    Canvas.SetTop(d, data.Point.Y - d.ActualHeight / 2);
                    Canvas.SetLeft(d, data.Point.X - d.ActualWidth / 2);
                }
                d.SizeChanged += (o, e) =>
                {
                    var oo = o as FrameworkElement;
                    var dd = oo.Tag as TriangleChartData;
                    if (double.IsNaN(dd.Point.X) == false && double.IsNaN(dd.Point.Y) == false)
                    {
                        Canvas.SetTop(oo, dd.Point.Y - d.ActualHeight / 2);
                        Canvas.SetLeft(oo, dd.Point.X - d.ActualWidth / 2);
                    }
                };
            }
        }

        private void GetPoint(TriangleChartData data)
        {
            if ((data.D1 + data.D2) > 0)
            {
                double r1 = (data.D2) / (data.D1 + data.D2);
                Point p1 = MyWpfLib.Mathematics.Geometry.GetInternalPoint(threePointList[0], threePointList[1], (float)r1);
                MyWpfLib.Mathematics.Geometry.Line line1 = new MyWpfLib.Mathematics.Geometry.Line(p1, threePointList[2]);
                if ((data.D3 + data.D2) > 0 && data.D2 > 0)
                {
                    double r2 = (data.D3) / (data.D3 + data.D2);
                    Point p2 = MyWpfLib.Mathematics.Geometry.GetInternalPoint(threePointList[1], threePointList[2], (float)r2);
                    MyWpfLib.Mathematics.Geometry.Line line2 = new MyWpfLib.Mathematics.Geometry.Line(p2, threePointList[0]);
                    data.Point = MyWpfLib.Mathematics.Geometry.GetIntersection2(line1, line2);
                }
                else if (data.D2 == 0)
                {
                    double r3 = (data.D3) / (data.D3 + data.D1);
                    Point p3 = MyWpfLib.Mathematics.Geometry.GetInternalPoint(threePointList[0], threePointList[2], (float)r3);
                    MyWpfLib.Mathematics.Geometry.Line line2 = new MyWpfLib.Mathematics.Geometry.Line(p3, threePointList[1]);
                    data.Point = MyWpfLib.Mathematics.Geometry.GetIntersection2(line1, line2);

                }
                else
                {
                    data.Point = new Point(threePointList[1].X, threePointList[1].Y);
                }
            }
            else
            {
                data.Point = new Point(threePointList[2].X, threePointList[2].Y);
            }
        }




        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateThreePoint();
            LabelPaint();
            SetDataPoint();
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            //   CreateThreePoint();
        }

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateThreePoint();
            SetDataPoint();

        }
    }

    [ContentProperty("ChildData")]
    public class TriangleChartData : DependencyObject, IDisposable
    {
        public TriangleChartData()
        {
        }

        public TriangleChartData(string name, double d1, double d2, double d3)
        {
            this.Text = name;
            this.D1 = d1;
            this.D2 = d2;
            this.D3 = d3;
        }


        bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public override string ToString()
        {
            return Text + "(" + D1.ToString() + ":" + D2.ToString() + ":" + D3.ToString() + ")";
        }



        public Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Visibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("Visibility", typeof(Visibility), typeof(TriangleChartData), new PropertyMetadata(Visibility.Visible));




        public Color DataColor
        {
            get { return (Color)GetValue(DataColorProperty); }
            set { SetValue(DataColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataColorProperty =
            DependencyProperty.Register("DataColor", typeof(Color), typeof(TriangleChartData), new PropertyMetadata(Colors.Black));



        public object ChildData { get; set; }

        Color normalColor = Colors.Black;
        Color selectColor = Colors.Red;

        Size baseSize = new Size(10, 10);

        public Size BaseSize
        {
            get { return baseSize; }
            set { baseSize = value; }
        }

        private Point point = new Point();

        public Point Point
        {
            get { return point; }
            set { point = value; }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TriangleChartData), new PropertyMetadata(string.Empty));




        public double D1 { get; set; }
        public double D2 { get; set; }
        public double D3 { get; set; }



        public string DataCsv
        {
            get { return (string)GetValue(DataCsvProperty); }
            set
            {
                SetValue(DataCsvProperty, value);
                var d = this.DataCsv.Split(',');
                if (d.Length > 2)
                {
                    this.D1 = double.Parse(d[0]);
                    this.D2 = double.Parse(d[1]);
                    this.D3 = double.Parse(d[2]);
                }
            }
        }

        // Using a DependencyProperty as the backing store for DataCsv.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataCsvProperty =
            DependencyProperty.Register("DataCsv", typeof(string), typeof(TriangleChartData), new PropertyMetadata(string.Empty));




        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(string), typeof(TriangleChartData), new PropertyMetadata(string.Empty));
       
        public object Tag { get; set; }
        #region IDisposable メンバ

        public void Dispose()
        {

        }

        #endregion
    }

    public class KeyValuePair : DependencyObject
    {

        public KeyValuePair()
        {
        }



        public string First
        {
            get { return (string)GetValue(FirstProperty); }
            set { SetValue(FirstProperty, value); }
        }

        // Using a DependencyProperty as the backing store for First.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstProperty =
            DependencyProperty.Register("First", typeof(string), typeof(KeyValuePair), new PropertyMetadata(string.Empty));




        public double Second
        {
            get { return (double)GetValue(SecondProperty); }
            set { SetValue(SecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Second.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.Register("Second", typeof(double), typeof(KeyValuePair), new PropertyMetadata(0));

        




        
        
    }

    public class DataBox
    {
        public List<string> TextList { get; set; }

        string textCsv = string.Empty;

        public string TextCsv
        {
            get { return textCsv; }
            set { textCsv = value;
            this.TextList = this.textCsv.Split(',').ToList();
            }
        }

        public List<string> Text1List { get; set; }

        string text1Csv = string.Empty;

        public string Text1Csv
        {
            get { return textCsv; }
            set
            {
                text1Csv = value;
                this.Text1List = this.textCsv.Split(',').ToList();
            }
        }

        public List<string> Text2List { get; set; }

        string text2Csv = string.Empty;

        public string Text2Csv
        {
            get { return textCsv; }
            set
            {
                text2Csv = value;
                this.Text2List = this.text2Csv.Split(',').ToList();
            }
        }
        public List<string> Text3List { get; set; }

        string text3Csv = string.Empty;

        public string Text3Csv
        {
            get { return textCsv; }
            set
            {
                text3Csv = value;
                this.Text3List = this.text3Csv.Split(',').ToList();
            }
        }

        public List<double> DoubleList { get; set; }

        string doubleCsv = string.Empty;
        public string DoubleCsv
        {
            get { return doubleCsv; }
            set
            {
                doubleCsv = value;
                List<double> list = new List<double>();
                foreach (var item in this.doubleCsv.Split(','))
                {
                    double d;
                    if (double.TryParse(item, out d))
                    {
                        list.Add(d);
                    }
                }
                this.DoubleList = list;
            }
        }

        public string Text4 { get; set; }
        public string Text5 { get; set; }
    }
}
