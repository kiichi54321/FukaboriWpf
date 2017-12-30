using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace MyWpfLib.Converter
{
    /// <summary>
    /// Visibility をOpacity　に変換します。
    /// </summary>

    [ValueConversion(typeof(Visibility), typeof(double))]
    public class VisibilityOpacityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double o = 1;
            var type = (Visibility)value;
            switch (type)
            {
                case Visibility.Collapsed:
                    o = 0.0;
                    break;
                case Visibility.Hidden:
                    o = 0.3;
                    break;
                case Visibility.Visible:
                    break;
                default:
                    break;
            }
            return o;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var o = (double)value;
            if (o <= 0)
            {
                return Visibility.Collapsed;
            }
            else if (o <= 0.3)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }

        }
    }
}
