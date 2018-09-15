using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyWpfLib.Graph
{
    public interface ILink : IComparable<ILink>, INotifyPropertyChanged
    {
        INode Node_a { get; set; }
        INode Node_b { get; set; }
        int Count { get; set; }
        Sort.Type SortType { get; set; }
        //ILink(INode item1, INode item2);
        bool ContainNode(INode node);
        INode LinkNode(INode node);
        double SortKey { get; set; }
        string GetNames();
        void AddCount();
        void AddCount(int c);
        Object Tag { get; set; }
        System.Windows.Visibility Visibility { get; set; }
        void Remove();
        void SetPoint();
    }
}
