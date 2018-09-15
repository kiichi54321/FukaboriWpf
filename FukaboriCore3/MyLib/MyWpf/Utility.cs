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

namespace MyWpfLib
{
    public static class Utility
    {
        public static void ChangeVisibility(FrameworkElement fe)
        {
            if (fe.Visibility == System.Windows.Visibility.Visible)
            {
                fe.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                fe.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public static Visibility ChangeVisibility(Visibility v)
        {
            if (v == Visibility.Visible) return Visibility.Collapsed;
            else return Visibility.Visible;
        }
    }
}
