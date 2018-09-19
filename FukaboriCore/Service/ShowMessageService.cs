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
    }
}
