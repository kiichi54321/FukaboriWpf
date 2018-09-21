using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FukaboriWpf.Control
{
    public class ChangeVisibilityButton : Button
    {
        public FrameworkElement TargetElement
        {
            get { return (FrameworkElement)GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register("TargetElement", typeof(FrameworkElement), typeof(ChangeVisibilityButton), new PropertyMetadata(null));


        public Visibility ReverseVisibilityState
        {
            get { return (Visibility)GetValue(ReverseVisibilityStateProperty); }
            set { SetValue(ReverseVisibilityStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReverseVisibilityState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReverseVisibilityStateProperty =
            DependencyProperty.Register("ReverseVisibilityState", typeof(Visibility), typeof(ChangeVisibilityButton), new PropertyMetadata(Visibility.Collapsed));



        public ChangeVisibilityButton()
        {
            this.Click += ChangeVisibilityButton_Click;
        }

        void ChangeVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            if (TargetElement != null)
            {
                if (TargetElement.Visibility == Visibility.Visible)
                {
                    TargetElement.Visibility = Visibility.Collapsed;
                    ReverseVisibilityState = Visibility.Visible;
                }
                else
                {
                    TargetElement.Visibility = Visibility.Visible;
                    ReverseVisibilityState = Visibility.Collapsed;
                }
            }
        }
    }
}
