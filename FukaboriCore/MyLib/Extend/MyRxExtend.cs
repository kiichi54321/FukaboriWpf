using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib.RxExtend
{

    public static class ObservableCollectionExtensions
    {
        public static IDisposable ConnectToReadOnlyCollection<TType, TResult>(
            this ObservableCollection<TType> self,
            out ReadOnlyObservableCollection<TResult> target,
            Func<TType, TResult> converter)
        {
            var d = new CompositeDisposable();

            ObservableCollection<TResult> proxy = new ObservableCollection<TResult>(self.Select(t => converter(t)).ToArray());
            target = new ReadOnlyObservableCollection<TResult>(proxy);

            var collectionChanged = self.CollectionChangedAsObservable();
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Add)
                .Select(e => new { Index = e.NewStartingIndex, Value = converter(e.NewItems.Cast<TType>().First()) })
                .Subscribe(v => proxy.Insert(v.Index, v.Value))
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Move)
                .Select(e => new { OldIndex = e.OldStartingIndex, NewIndex = e.NewStartingIndex })
                .Subscribe(v =>
                {
                    var item = proxy[v.OldIndex];
                    proxy.RemoveAt(v.OldIndex);
                    proxy.Insert(v.NewIndex, item);
                })
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Remove)
                .Select(e => new { Index = e.OldStartingIndex })
                .Subscribe(v => proxy.RemoveAt(v.Index))
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Replace)
                .Select(e => new { Index = e.NewStartingIndex, Item = converter(e.NewItems.Cast<TType>().First()) })
                .Subscribe(v => proxy[v.Index] = v.Item)
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Reset)
                .Subscribe(_ =>
                {
                    proxy.Clear();
                    foreach (var item in self.ToArray())
                    {
                        proxy.Add(converter(item));
                    }
                })
                .AddTo(d);

            return d;
        }

        /// <summary>
        /// 二つのコレクションを同期させる。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IDisposable Bind<T>(
           this ObservableCollection<T> self,
            ObservableCollection<T> target
           )
        {
            var d = new CompositeDisposable();
            var proxy = target;

            proxy.Clear();
            foreach (var item in self.ToArray())
            {
                proxy.Add(item);
            }

            var collectionChanged = self.CollectionChangedAsObservable();
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Add)
                .Select(e => new { Index = e.NewStartingIndex, Value = e.NewItems.Cast<T>().First() })
                .Subscribe(v => proxy.Insert(v.Index, v.Value))
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Move)
                .Select(e => new { OldIndex = e.OldStartingIndex, NewIndex = e.NewStartingIndex })
                .Subscribe(v =>
                {
                    var item = proxy[v.OldIndex];
                    proxy.RemoveAt(v.OldIndex);
                    proxy.Insert(v.NewIndex, item);
                })
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Remove)
                .Select(e => new { Index = e.OldStartingIndex })
                .Subscribe(v => proxy.RemoveAt(v.Index))
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Replace)
                .Select(e => new { Index = e.NewStartingIndex, Item = e.NewItems.Cast<T>().First() })
                .Subscribe(v => proxy[v.Index] = v.Item)
                .AddTo(d);
            collectionChanged.Where(e => e.Action == NotifyCollectionChangedAction.Reset)
                .Subscribe(_ =>
                {
                    proxy.Clear();
                    foreach (var item in self.ToArray())
                    {
                        proxy.Add(item);
                    }
                })
                .AddTo(d);

            return d;
        }
    }
}
