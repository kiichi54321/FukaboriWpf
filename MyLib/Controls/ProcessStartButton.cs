using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MyWpfLib.Controls
{
    public class ProcessStartButton:System.Windows.Controls.Button
    {
        public ProcessStartButton()
           : base()
        {
            this.Click += ShowProgressButton_Click;
        }

        void ShowProgressButton_Click(object sender, RoutedEventArgs e)
        {
            if (TargetText != null)
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(TargetText);
            }
        }

        public string TargetText
        {
            get { return (string)GetValue(TargetTextProperty); }
            set { SetValue(TargetTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetTextProperty =
            DependencyProperty.Register("TargetText", typeof(string), typeof(ProcessStartButton), new PropertyMetadata(null));

        
    }
}
