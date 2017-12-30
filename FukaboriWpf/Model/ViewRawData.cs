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

namespace CrossTableSilverlight.Model
{
    public class ViewRawData:GalaSoft.MvvmLight.ViewModelBase
    {
        public Enqueite Enqueite { get; set; }

        public RelayCommand ViewCommand
        {
            get
            {
                if(viewCommand == null)
                {
                    viewCommand = new RelayCommand(() => {

                    });
                }
                return viewCommand;
            }

            set
            {
                viewCommand = value;
            }
        }

        RelayCommand viewCommand;
    }
}
