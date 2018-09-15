
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FukaboriCore.Behaviour
{
    public class ListBoxBehaviour
    {
        public static readonly DependencyProperty SeletedItemsProperty =
            DependencyProperty.RegisterAttached("SeletedItems", typeof(IList), typeof(ListBoxBehaviour), new PropertyMetadata(new PropertyChangedCallback(SeletedItemsChanged)));
        static void SeletedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox element = (ListBox)d;
            element.SelectionChanged += Element_SelectionChanged;
        }

        static void Element_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox element = (ListBox)sender;
            element.SetValue(SeletedItemsProperty, element.SelectedItems);
        }

        public static void SetSeletedItems(UIElement element, IList value)
        {
            element.SetValue(SeletedItemsProperty, value);
        }

        public static IList GetSeletedItems(UIElement element)
        {
            return (IList)element.GetValue(SeletedItemsProperty);
        }
    }

}