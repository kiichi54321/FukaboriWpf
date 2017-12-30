using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace MyWpfLib.Controls
{
    public class ImageData : INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string Name { get; set; }
        public string Text { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }

        public Uri Source { get; set; }
        public BitmapImage ImageSource { get; set; }
        public dynamic Parent { get; set; }

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
                NotifyPropertyChanged("Opacity");
            }
        }
        public decimal Id { get; set; }
        public object Tag { get; set; }
        public dynamic Extend { get; set; }
    }
}
