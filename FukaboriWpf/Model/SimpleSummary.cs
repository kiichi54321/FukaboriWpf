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
using System.Collections.Generic;
using System.Linq;

namespace FukaboriWpf.Model
{
    public class SimpleSummary:MyLib.Interface.ITsv
    {
        public SimpleSummary()
        {
            DataList = new System.Collections.ObjectModel.ObservableCollection<PropertyData>();
        }
        public System.Collections.ObjectModel.ObservableCollection<PropertyData> DataList { get; set; }
        bool avgSort = false;
        bool stdSort = false;
        bool nameSort = false;

        public void CreatePropertyData(IEnumerable<Model.Question> question)
        {
            DataList.Clear();
            imageVisibility = Visibility.Collapsed;
            foreach (var item in Model.PropertyData.CreatePropertyData(question, EnqueiteData.Current.Value.AnswerLines))
            {
                if (item.Question.ImageVisibility == Visibility.Visible)
                {
                    imageVisibility = Visibility.Visible;
                }
                DataList.Add(item);
            }
        }
        Visibility imageVisibility = Visibility.Collapsed;
        public Visibility ImageVisibility
        {
            get
            {
                return imageVisibility;
            }
        }

        public string ToTsv()
        {
            MyLib.IO.TsvBuilder tsv = new MyLib.IO.TsvBuilder();
            foreach (var item in DataList)
            {
                item.ToTsv(tsv);
            }
            return tsv.ToString();
        }

        public void SortName()
        {
            if (nameSort)
            {
                var tmp = DataList.ToList();
                DataList.Clear();
                foreach (var item in tmp.OrderBy(n => n.Name))
                {
                    DataList.Add(item);
                }
            }
            else
            {
                var tmp = DataList.ToList();
                DataList.Clear();
                foreach (var item in tmp.OrderByDescending(n => n.Name))
                {
                    DataList.Add(item);
                }
            }
            nameSort = !nameSort;
        }

        public void SortAvg()
        {
            if (avgSort)
            {
                var tmp = DataList.ToList();
                DataList.Clear();
                foreach (var item in tmp.OrderBy(n => n.Average))
                {
                    DataList.Add(item);
                }
            }
            else
            {
                var tmp = DataList.ToList();
                DataList.Clear();
                foreach (var item in tmp.OrderByDescending(n => n.Average))
                {
                    DataList.Add(item);
                }
            }
            avgSort = !avgSort;
        }

        public void SortStd()
        {
            if (stdSort)
            {
                var tmp = DataList.ToList();
                DataList.Clear();
                foreach (var item in tmp.OrderBy(n => n.Std))
                {
                    DataList.Add(item);
                }
            }
            else
            {
                var tmp = DataList.ToList();
                DataList.Clear();
                foreach (var item in tmp.OrderByDescending(n => n.Std))
                {
                    DataList.Add(item);
                }
            }
            stdSort= !stdSort;
        }

        public int ImageCount { get; set; }
    }
}
