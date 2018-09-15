using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace MyWpfLib.Draw
{
        public class TextOnLine : UserControl
        {
            public static readonly DependencyProperty X1Property =
    DependencyProperty.Register("X1", typeof(double), typeof(TextOnLine), new PropertyMetadata(OnPathPropertyChanged));
            public static readonly DependencyProperty Y1Property =
                DependencyProperty.Register("Y1", typeof(double), typeof(TextOnLine), new PropertyMetadata(OnPathPropertyChanged));
            public static readonly DependencyProperty X2Property =
                DependencyProperty.Register("X2", typeof(double), typeof(TextOnLine), new PropertyMetadata(OnPathPropertyChanged));
            public static readonly DependencyProperty Y2Property =
                DependencyProperty.Register("Y2", typeof(double), typeof(TextOnLine), new PropertyMetadata(OnPathPropertyChanged));

            public static readonly DependencyProperty TextMarginProperty =
                DependencyProperty.Register("TextMargin", typeof(double), typeof(TextOnLine), new PropertyMetadata(OnPathPropertyChanged));

            static void OnPathPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            {
                (obj as TextOnLine).OnPathPropertyChanged(args);
            }


            public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("Text", typeof(string), typeof(TextOnLine), new PropertyMetadata(OnTextPropertyChanged));
             

            // Properties
            //public FontFamily FontFamily
            //{
            //    set { SetValue(FontFamilyProperty, value); }
            //    get { return (FontFamily)GetValue(FontFamilyProperty); }
            //}

            //public FontStyle FontStyle
            //{
            //    set { SetValue(FontStyleProperty, value); }
            //    get { return (FontStyle)GetValue(FontStyleProperty); }
            //}

            //public FontWeight FontWeight
            //{
            //    set { SetValue(FontWeightProperty, value); }
            //    get { return (FontWeight)GetValue(FontWeightProperty); }
            //}

            //public FontStretch FontStretch
            //{
            //    set { SetValue(FontStretchProperty, value); }
            //    get { return (FontStretch)GetValue(FontStretchProperty); }
            //}

            //public Brush Foreground
            //{
            //    set { SetValue(ForegroundProperty, value); }
            //    get { return (Brush)GetValue(ForegroundProperty); }
            //}

            public string Text
            {
                set { SetValue(TextProperty, value); }
                get { return (string)GetValue(TextProperty); }
            }

            public double X1
            {
                set { SetValue(X1Property, value); }
                get { return (double)GetValue(X1Property); }
            }
            public double X2
            {
                set { SetValue(X2Property, value); }
                get { return (double)GetValue(X2Property); }
            }
            public double Y1
            {
                set { SetValue(Y1Property, value); }
                get { return (double)GetValue(Y1Property); }
            }
            public double Y2
            {
                set { SetValue(Y2Property, value); }
                get { return (double)GetValue(Y2Property); }
            }

            public double TextMargn
            {
                set { SetValue(TextMarginProperty, value); }
                get { return (double)GetValue(TextMarginProperty); }
            }



            // Property changed handlers
            static void OnFontPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            {
                (obj as TextOnLine).OnFontPropertyChanged(args);
            }

            protected void OnFontPropertyChanged(DependencyPropertyChangedEventArgs args)
            {
                textBlock.FontFamily = this.FontFamily;
                textBlock.FontStretch = this.FontStretch;
                textBlock.FontSize = this.FontSize;
                textBlock.FontStyle = this.FontStyle;
                textBlock.FontWeight = this.FontWeight;
                textBlock.Foreground = this.Foreground;

                Layout();
            }

            public void Layout()
            {
               // if (this.Visibility == System.Windows.Visibility.Visible)
                {
                    var c = Math.Sqrt(Math.Pow((X2 - X1), 2) + Math.Pow((Y2 - Y1), 2));
                    if (c > 0)
                    {
                        double angle = 0;
                        if ((X2 - X1) * (Y2 - Y1) > 0)
                        {
                            angle = Math.Acos(Math.Abs(X2 - X1) / c) * 180 / Math.PI;
                        }
                        else
                        {
                            angle = Math.Acos(Math.Abs(Y2 - Y1) / c) * 180 / Math.PI-90;
                        }
                        var angle2 = (angle + 90) * Math.PI / 180;
                        Canvas.SetLeft(this, (X2 - X1) / 2 + X1 - textBlock.ActualWidth / 2 + Math.Cos(angle2) * TextMargn);
                        Canvas.SetTop(this, (Y2 - Y1) / 2 + Y1 - textBlock.ActualHeight / 2 + Math.Sin(angle2) * TextMargn);
                        textBlock.RenderTransform = new RotateTransform() { Angle = angle, CenterX = this.ActualWidth / 2, CenterY = this.ActualHeight / 2 };

                    //    MySilverlightLibrary.LogManage.WriteLine("X1:" + X1.ToString("F3") + ",X2:" + X2.ToString("F3") + ",Y2:" + Y1.ToString("F3") + ",Y2:" + Y2.ToString("F3") + ",angle:" + angle.ToString("F3") + ",angle2:" + angle2.ToString("F3"));
                    }
                }
            }

            static void OnForegroundPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            {
                
                (obj as TextOnLine).OnForegroundPropertyChanged(args);
            }

            protected void OnForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
            {
                textBlock.Foreground = this.Foreground;
            }

            static void OnTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            {
                (obj as TextOnLine).OnTextPropertyChanged(args);
            }

            TextBlock textBlock = new TextBlock();
            public TextOnLine()
            {
                this.Content = textBlock;
            }

            

            protected void OnTextPropertyChanged(DependencyPropertyChangedEventArgs args)
            {
                textBlock.Text = this.Text;
                Layout();
            }

            protected  void OnPathPropertyChanged(DependencyPropertyChangedEventArgs args)
            {
                Layout();
            }


 
        }

}
