using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MyWpfLib2.Extend
{
    public interface INotify : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string name);
    }

    /// <summary>
    /// 拡張メソッドを使うためのインターフェース
    /// </summary>
    public interface IDisposableContainer:IDisposable
    {
        List<IDisposable> DisposableList { get; set; }
    }
    /// <summary>
    /// IDisposableを保存と一斉開放を行う拡張メソッド
    /// </summary>
    public static class IDisposableContainerExtend
    {
        public static void RegistDisposableList(this IDisposable iDisposable,IDisposableContainer container)
        {
            if (container.DisposableList == null) container.DisposableList = new List<IDisposable>();
            container.DisposableList.Add(iDisposable);
        }

        public static void DisposableListClear(this IDisposableContainer container)
        {
            foreach (var item in container.DisposableList)
            {
                item.Dispose();
            }
            container.DisposableList.Clear();
        }
    }


    /// <summary>
    /// INotifyPropertyChanged用の拡張メソッド。Raiseを使う。
    /// </summary>
    public static class INotifyPropertyChangedExtend
    {
        static Dictionary<string, PropertyChangedEventArgs> PropertyChangedEventArgsDic = new Dictionary<string, PropertyChangedEventArgs>(255);
        static PropertyChangedEventArgs GetPropertyChangedEventArgs(string name)
        {
            if(PropertyChangedEventArgsDic.ContainsKey(name))
            {
                return PropertyChangedEventArgsDic[name];
            }
            else
            {
                PropertyChangedEventArgsDic.Add(name, new PropertyChangedEventArgs(name));
                return PropertyChangedEventArgsDic[name];
            }
        }

        public static void Raise(this PropertyChangedEventHandler propertyEvent, object sender, string name)
        {
            if (propertyEvent != null)
            {
                propertyEvent.Invoke(sender, GetPropertyChangedEventArgs(name));
            }
        }
    }

    /// <summary>
    /// INotifyPropertyChangedを実装したValueを持つクラス。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    
    public class NotifyProperty<T> : INotifyPropertyChanged
    {
        static TaskScheduler uiScheduler;

        protected TaskScheduler UIScheduler
        {
            get
            {                
                if (uiScheduler ==null)
                {
                    uiScheduler = TaskScheduler.Default;
                //    uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                }
                return uiScheduler;
            }
        }

        public NotifyProperty()
            {
            }
        public NotifyProperty(T item)
        {
            this.Value = item;
        }

        /// <summary>
        /// Subcribeのほうを使ってください。
        /// </summary>
        internal event Action<T> ValueChange;

        public event PropertyChangedEventHandler PropertyChanged;
        T _value;
        
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == null || _value.Equals(value) == false)
                {
                    _value = value;
                    RaisePropertyChanged();
                }
            }
        }

       
        /// <summary>
        /// 値が更新されたことにする。
        /// </summary>
        public void RaisePropertyChanged()
        {
            Task.Factory.StartNew(() =>
            {
                PropertyChanged.Raise(this, nameof(Value));
            },System.Threading.CancellationToken.None, TaskCreationOptions.None, UIScheduler);
            ValueChange?.Invoke(this.Value);
        }

        /// <summary>
        /// 更新時実行するアクションを購読する。
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IDisposable Subcribe(Action<T> action)
        {
            ValueChange += action;
            
            return new DisposableNotifyPropertyEvent<T>(this, action);
        }
 
    }

    /// <summary>
    /// 登録したイベントをDispose時削除するためのクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisposableNotifyPropertyEvent<T>  : IDisposable
    {
        Action<T> _action { get; set; }
        WeakReference instance = null;

        public DisposableNotifyPropertyEvent(NotifyProperty<T> obj, Action<T> action)
        {
            instance = new WeakReference(obj);
            _action = action;         
        }

        public void Dispose()
        {
            if(instance !=null && instance.IsAlive)
            {
                var notify = instance.Target as NotifyProperty<T>;
                if (notify != null)
                {
                    notify.ValueChange -= _action;
                }
            }
        }
    }


    public class Sample:IDisposableContainer
    {
        NotifyProperty<string> Text { get; } = new NotifyProperty<string>("Test");
        NotifyProperty<string> Text2 { get; } = new NotifyProperty<string>();

        public List<IDisposable> DisposableList
        {
            get; set;
        }

        public Sample()
        {           
            Text.Subcribe(n => Text2.Value = n + "_text2").RegistDisposableList(this);
            Text.RaisePropertyChanged();            
        }

        public void Dispose()
        {
            this.DisposableListClear();
        }
    }


}
