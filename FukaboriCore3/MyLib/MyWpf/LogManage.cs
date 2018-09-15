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

namespace MySilverlightLibrary
{
    public static class LogManage
    {
        public static System.Threading.Tasks.TaskScheduler UIScheduler { get; set; }
        public static TextBox TextBox { get; set; }

        public static void WriteLine(string text)
        {
            if (TextBox != null && UIScheduler != null)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    TextBox.Text += text + "\n";
                }, new System.Threading.CancellationToken(), System.Threading.Tasks.TaskCreationOptions.None, UIScheduler);
            }
        }
    }
}
