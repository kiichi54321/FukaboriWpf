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
using GalaSoft.MvvmLight.Command;

namespace FukaboriWpf.Command
{
    public static class ListBoxCommand
    {
        static RelayCommand<ListBox> selectAll;
        public static RelayCommand<ListBox> SelectAll
        {
            get
            {
                if(selectAll == null)
                {
                    selectAll = new RelayCommand<ListBox>((n) => { n.SelectAll(); });
                }
                return selectAll;
            }
        }
    }
}
