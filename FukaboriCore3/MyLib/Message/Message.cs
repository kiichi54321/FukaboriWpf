using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// https://code.msdn.microsoft.com/windowsdesktop/MVVM-Light-toolkitMessenger-140cc342 
/// から拝借
/// </summary>
namespace MyLib.Message
{
    
    /// <summary> 
    /// メッセージの結果を非同期で待つようにIMessengerを拡張する 
    /// </summary> 
    public static class MessengerExtensions
    {
        /// <summary> 
        /// メッセージを送信して結果を非同期で返します。 
        /// </summary> 
        /// <typeparam name="TResult"></typeparam> 
        /// <typeparam name="TMessage"></typeparam> 
        /// <param name="self"></param> 
        /// <param name="message"></param> 
        /// <returns>メッセージの処理結果</returns> 
        public static Task<TResult> SendAsync<TResult, TMessage>(this IMessenger self, TMessage message)
            where TMessage : MessageBase
        {
            var asyncMessage = new AsyncMessage<TResult, TMessage>(message);
            self.Send(asyncMessage);
            return asyncMessage.Task;
        }

        /// <summary> 
        /// メッセージを送信して結果を非同期で返します。 
        /// </summary> 
        /// <typeparam name="TResult"></typeparam> 
        /// <typeparam name="TMessage"></typeparam> 
        /// <param name="self"></param> 
        /// <param name="message"></param> 
        /// <param name="token"></param> 
        /// <returns>メッセージの処理結果</returns> 
        public static Task<TResult> SendAsync<TResult, TMessage>(this IMessenger self, TMessage message, object token)
            where TMessage : MessageBase
        {
            var asyncMessage = new AsyncMessage<TResult, TMessage>(message);
            self.Send(asyncMessage, token);
            return asyncMessage.Task;
        }

        /// <summary> 
        /// メッセージを受信して結果を返す処理の登録を行います。 
        /// 戻り値のインスタンスの参照がGCに回収されるか、Disposeメソッドを呼び出すことで処理の登録を解除できます。 
        /// </summary> 
        /// <typeparam name="TResult"></typeparam> 
        /// <typeparam name="TMessage"></typeparam> 
        /// <param name="self"></param> 
        /// <param name="callback"></param> 
        /// <returns>メッセージの登録解除を行うためのIDisposable型のインスタンス。</returns> 
        public static IDisposable RegisterAsyncMessage<TResult, TMessage>(
            this IMessenger self,
            Func<TMessage, Task<TResult>> callback)
            where TMessage : MessageBase
        {
            return self.RegisterAsyncMessage<TResult, TMessage>(
                null,
                callback);
        }

        /// <summary> 
        /// メッセージを受信して結果を返す処理の登録を行います。 
        /// 戻り値のインスタンスの参照がGCに回収されるか、Disposeメソッドを呼び出すことで処理の登録を解除できます。 
        /// </summary> 
        /// <typeparam name="TResult"></typeparam> 
        /// <typeparam name="TMessage"></typeparam> 
        /// <param name="self"></param> 
        /// <param name="token"></param> 
        /// <param name="callback"></param> 
        /// <returns>メッセージの登録解除を行うためのIDisposable型のインスタンス。</returns> 
        public static IDisposable RegisterAsyncMessage<TResult, TMessage>(
            this IMessenger self,
            object token,
            Func<TMessage, Task<TResult>> callback)
            where TMessage : MessageBase
        {
            return self.RegisterAsyncMessage<TResult, TMessage>(
                token,
                false,
                callback);
        }

        /// <summary> 
        /// メッセージを受信して結果を返す処理の登録を行います。 
        /// 戻り値のインスタンスの参照がGCに回収されるか、Disposeメソッドを呼び出すことで処理の登録を解除できます。 
        /// </summary> 
        /// <typeparam name="TResult"></typeparam> 
        /// <typeparam name="TMessage"></typeparam> 
        /// <param name="self"></param> 
        /// <param name="receiveDerivedMessageToo"></param> 
        /// <param name="callback"></param> 
        /// <returns>メッセージの登録解除を行うためのIDisposable型のインスタンス。</returns> 
        public static IDisposable RegisterAsyneMessage<TResult, TMessage>(
            this IMessenger self,
            bool receiveDerivedMessageToo,
            Func<TMessage, Task<TResult>> callback)
            where TMessage : MessageBase
        {
            return self.RegisterAsyncMessage<TResult, TMessage>(
                null,
                receiveDerivedMessageToo,
                callback);
        }

        /// <summary> 
        /// メッセージを受信して結果を返す処理の登録を行います。 
        /// 戻り値のインスタンスの参照がGCに回収されるか、Disposeメソッドを呼び出すことで処理の登録を解除できます。 
        /// </summary> 
        /// <typeparam name="TResult"></typeparam> 
        /// <typeparam name="TMessage"></typeparam> 
        /// <param name="self"></param> 
        /// <param name="token"></param> 
        /// <param name="receiveDerivedMessageToo"></param> 
        /// <param name="callback"></param> 
        /// <returns>メッセージの登録解除を行うためのIDisposable型のインスタンス。</returns> 
        public static IDisposable RegisterAsyncMessage<TResult, TMessage>(
            this IMessenger self,
            object token,
            bool receiveDerivedMessageToo,
            Func<TMessage, Task<TResult>> callback)
            where TMessage : MessageBase
        {
            return new AsyncMessageReceiver<TResult, TMessage>(
                self,
                token,
                receiveDerivedMessageToo,
                callback);
        }
    }
    /// <summary> 
    /// 非同期処理用メッセージ 
    /// </summary> 
    /// <typeparam name="TResult">戻り値の型</typeparam> 
    /// <typeparam name="TMessage">非同期メッセージがラップするメッセージの型</typeparam> 
    sealed class AsyncMessage<TResult, TMessage> : MessageBase
        where TMessage : MessageBase
    {
        /// <summary> 
        /// ラップしているメッセージ 
        /// </summary> 
        public TMessage InnerMessage { get; private set; }

        /// <summary> 
        /// 処理の完了通知用TaskCompletionSource 
        /// </summary> 
        private TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();

        /// <summary> 
        /// メッセージをラップして非同期処理用メッセージを作成 
        /// </summary> 
        /// <param name="innerMessage">ラップするメッセージ</param> 
        public AsyncMessage(TMessage innerMessage)
        {
            this.InnerMessage = innerMessage;
        }

        /// <summary> 
        /// 処理を完了して結果を設定する 
        /// </summary> 
        /// <param name="result">処理の結果</param> 
        public void SetResult(TResult result)
        {
            this.completionSource.SetResult(result);
        }

        /// <summary> 
        /// 処理の結果の例外を設定する。 
        /// </summary> 
        /// <param name="ex">例外</param> 
        public void SetException(Exception ex)
        {
            this.completionSource.SetException(ex);
        }

        /// <summary> 
        /// タスクを取得 
        /// </summary> 
        public Task<TResult> Task
        {
            get { return completionSource.Task; }
        }
    }

    /// <summary> 
    /// 特定のメッセージの受信を行うクラス 
    /// </summary> 
    /// <typeparam name="TResult">メッセージを受信して行う処理の結果の型</typeparam> 
    /// <typeparam name="TMessage">受信するメッセージの型</typeparam> 
    sealed class AsyncMessageReceiver<TResult, TMessage> : IDisposable
        where TMessage : MessageBase
    {
        /// <summary> 
        /// メッセージ受信時に行う処理 
        /// </summary> 
        private Func<TMessage, Task<TResult>> callback;

        /// <summary> 
        /// メッセージの受信の登録解除処理 
        /// </summary> 
        private Action unregisterAction;

        /// <summary> 
        /// インスタンスを作成します 
        /// </summary> 
        /// <param name="messenger">メッセージを発行するメッセンジャーのインスタンス</param> 
        /// <param name="token">メッセージ受信の識別に使用するトークン</param> 
        /// <param name="receiveDerivedMessagesToo">???</param> 
        /// <param name="callback">メッセージ受信時の処理</param> 
        public AsyncMessageReceiver(
            IMessenger messenger,
            object token,
            bool receiveDerivedMessagesToo,
            Func<TMessage, Task<TResult>> callback)
        {
            this.callback = callback;
            messenger.Register<AsyncMessage<TResult, TMessage>>(this, token, receiveDerivedMessagesToo, this.AsyncMessageCallback);
            this.unregisterAction = () => messenger.Unregister<AsyncMessage<TResult, TMessage>>(this, token, this.AsyncMessageCallback);
        }

        /// <summary> 
        /// 非同期メッセージ受信処理 
        /// </summary> 
        /// <param name="message"></param> 
        private async void AsyncMessageCallback(AsyncMessage<TResult, TMessage> message)
        {
            try
            {
                var result = await this.callback(message.InnerMessage);
                message.SetResult(result);
            }
            catch (Exception ex)
            {
                message.SetException(ex);
            }
        }

        /// <summary> 
        /// メッセージの受信の解除を行います 
        /// </summary> 
        public void Dispose()
        {
            this.unregisterAction();
            this.callback = null;
            this.unregisterAction = null;
        }
    }
}

