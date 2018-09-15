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
using System.Collections.Generic;
using System.Windows.Data;

namespace FukaboriWpf
{
    public static class State
    {
        public static GenericBinding<double> MaxImageViewNum { get; set; }
        public static GenericBinding<string> NameText { get; set; }
        public static GenericBinding<int> WordLenghtNum { get; set; }
    }

    public class State2
    {
        public State2()
        {
            WordLenghtNum = new GenericBinding<int>();
        }
        public  GenericBinding<int> WordLenghtNum { get; set; }
    }


    public class GenericBinding<T> : GalaSoft.MvvmLight.ViewModelBase
    {
        public GenericBinding(FrameworkElement ui, DependencyProperty dp)
        {
            this.Value = (T)ui.GetValue(dp);
            AddBinding(ui, dp);
        }
        public GenericBinding()
        {

        }


        public GenericBinding<T> AddBinding(FrameworkElement ui, DependencyProperty dp)
        {
            var binding = new Binding();
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            binding.Path = new PropertyPath("Value");           
            ui.SetBinding(dp, binding);
            return this;
        }

        public Action<T> ChangeValueAction { get; set; }

        T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == null || _value.Equals(value)==false)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                    if (ChangeValueAction != null) ChangeValueAction(_value);
                }
            }
        }
    }
}
