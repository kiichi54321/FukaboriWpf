using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FukaboriCore.Service
{
    public interface IShowMessageService
    {
        void Show(string message);
        void Show(string message, string caption);
        Task<bool> ShowAsync(string message, string caption);
        Task<bool> ShowAsync(string message, string caption, MessageBoxType messageBoxType);
    }
    public class ShowMessageService : IShowMessageService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public void Show(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }

        public Task<bool> ShowAsync(string message, string caption)
        {
            return Task.Run<bool>(() => {
                var r = MessageBox.Show(message, caption);
                if (r == MessageBoxResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public Task<bool> ShowAsync(string message, string caption, MessageBoxType messageBoxType)
        {
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            switch (messageBoxType)
            {
                case MessageBoxType.OK:
                    messageBoxButton = MessageBoxButton.OK;
                    break;
                case MessageBoxType.OKCancel:
                    messageBoxButton = MessageBoxButton.OKCancel;
                    break;
                case MessageBoxType.YesNo:
                    messageBoxButton = MessageBoxButton.YesNo;
                    break;
                case MessageBoxType.YesNoCancel:
                    messageBoxButton = MessageBoxButton.YesNoCancel;
                    break;
                default:
                    break;
            }


            return Task.Run<bool>(() => {
                var r = MessageBox.Show(message, caption,messageBoxButton);
                if (r == MessageBoxResult.OK || r == MessageBoxResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

    }

    public enum MessageBoxType
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel,
    }
}
