using System;
using System.Net;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace FukaboriCore.Model
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
