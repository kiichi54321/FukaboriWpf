using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FukaboriCore.Service
{
    public interface ISetClipBoardService
    {
        void SetTextWithMessage(string text);
        void SetText(string text);
    }

    public class SetClipBoardService : ISetClipBoardService
    {
        public void SetText(string text)
        {
            Clipboard.SetText(text);
        }

        public void SetTextWithMessage(string text)
        {
            Clipboard.SetText(text);
            MessageBox.Show("クリップボードに格納しました");
        }
    }
}
