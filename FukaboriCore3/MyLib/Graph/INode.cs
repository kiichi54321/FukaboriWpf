using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace MyWpfLib.Graph
{
    public interface INode:IComparable<INode>, INotifyPropertyChanged
    {

        string NodeName { get; set; }
        Point CenterPoint { get; set; }
        double NodeX { get; set; }
        double NodeY { get; set; }
        double NodeHeight { get; set; }
        double NodeWidth { get; set; }
  //      Size Size { get; set; }
        System.Windows.Visibility Visibility { get; set; }
        System.Windows.Visibility LabelVisibility { get; set; }


        ILink SearchLink(INode node);
        void AddLink(ILink link);
        void RemoveLink(ILink link);
        List<ILink> Links { get; }
        void Reset();
        Object Tag { get; set; }
        List<INode> LinkedNodes { get; }
        void AddCount();
        void AddCount(int c);
        int Count { get; set; }
        int Id { get; set; }

    }
}
