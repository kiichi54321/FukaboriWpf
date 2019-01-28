using FukaboriCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FukaboriWpf.Control
{
    /// <summary>
    /// BoxPlotControl.xaml の相互作用ロジック
    /// </summary>
    public partial class BoxPlotControl : UserControl
    {
        public BoxPlotControl()
        {
            InitializeComponent();
        }

        public PropertyData PropertyData
        {
            get { return (PropertyData)GetValue(PropertyDataProperty); }
            set { SetValue(PropertyDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyDataProperty =
            DependencyProperty.Register("PropertyData", typeof(PropertyData), typeof(BoxPlotControl), new PropertyMetadata(null, PropertyDataCallBack));

        private static void PropertyDataCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BoxPlotControl)d;
            if (control != null)
            {
                control.SetPropertyData((PropertyData)e.NewValue);
            }
        }

        public void SetPropertyData(PropertyData propertyData)
        {
            
        }

        public class BoxPlot
        {
            public double MaxValue { get; set; }
            public double MinValue { get; set; }
            public double Q_1 { get; set; }
            public double Q_2 { get; set; }
            public double Q_3 { get; set; }
            public double Avg { get; set; }

            public bool Avg_only { get; set; }
        }

    }
}
