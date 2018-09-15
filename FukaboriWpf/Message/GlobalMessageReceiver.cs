using GalaSoft.MvvmLight.Messaging;
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

namespace FukaboriWpf.Message
{
    public class GlobalMessageReceiver
    {
        // メッセージの受信処理がMessengerから削除されないように参照としてとっておくためのフィールド 
        private IDisposable confirmReceiver;

        public GlobalMessageReceiver()
        {
            // 確認ダイアログメッセージを受信 
        }
    }
}
