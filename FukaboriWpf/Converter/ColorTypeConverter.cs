using FukaboriCore.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FukaboriWpf.Converter
{
    public class ColorTypeConverter : IValueConverter
    {
        public static Dictionary<FukaboriCore.Model.ColorType, Brush> colorDic = new Dictionary<FukaboriCore.Model.ColorType, Brush>()
        {
            { ColorType.BackColor , new SolidColorBrush(Colors.White) },
            { ColorType.DangerColor , new SolidColorBrush(Colors.Red) },
            { ColorType.InfoColor , new SolidColorBrush(Colors.Blue) },
            { ColorType.PrimaryColor , new SolidColorBrush(Colors.Black) },
            { ColorType.RelationColor , new SolidColorBrush(Colors.Orange.SetA(150)) },
            { ColorType.RelationColor2 , new SolidColorBrush(Colors.Orange.SetA(50)) },
            { ColorType.SeletedColor , new SolidColorBrush(Colors.Cyan) },
            { ColorType.SuccessColor , new SolidColorBrush(Colors.BurlyWood) },
            { ColorType.WarningColor , new SolidColorBrush(Colors.Yellow) },
        };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(colorDic.TryGetValue((ColorType)value,out var brush))
            {
                return brush;
            }
            return colorDic.First().Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class ColorExtend
    {
        public static Color SetA(this Color color, byte a)
        {
            color.A = a;
            return color;
        }
    }
}
