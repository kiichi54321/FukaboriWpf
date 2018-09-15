using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using MyLib.UI;
using System.Collections.ObjectModel;
using FukaboriCore.Model;

namespace FukaboriCore.ViewModel
{
    public class SimpleSummaryViewModel:GalaSoft.MvvmLight.ViewModelBase, MyLib.Interface.ITsv
    {
        public SimpleSummaryViewModel()
        {
            DataList = new System.Collections.ObjectModel.ObservableCollection<PropertyData>();
        }
        public System.Collections.ObjectModel.ObservableCollection<PropertyData> DataList { get; set; }
        bool avgSort = false;
        bool stdSort = false;
        bool nameSort = false;

        

        public void CreatePropertyData(IEnumerable<Question> question)
        {
            DataList.Clear();
            imageVisibility = false;
            foreach (var item in PropertyData.CreatePropertyData(question, EnqueiteData.Current.Value.AnswerLines))
            {
                if (item.Question.ImageVisibility == true)
                {
                    imageVisibility = true;
                }
                DataList.Add(item);
            }
        }
        bool imageVisibility = false;
        public bool ImageVisibility
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

    public class SimpleSummary2 : ResultData
    {
        public List<Question> SelectedQuestion { get; set; } = new List<Question>();
    }

    /// <summary>
    /// 大元のデータ
    /// </summary>
    public abstract class ResultData:GalaSoft.MvvmLight.ObservableObject
    {
        List<QuestionAnswer> _InQuestion = new List<QuestionAnswer>();
        public List<QuestionAnswer> InQuestion
        {
            get { return _InQuestion; }
            set { Set(nameof(InQuestion), ref _InQuestion, value); }
        }
        List<QuestionAnswer> _OutQuestion = new List<QuestionAnswer>();
        public List<QuestionAnswer> OutQuestion { get { return _OutQuestion; } set { Set(nameof(OutQuestion), ref _OutQuestion, value); } }
        string _Title = string.Empty;
        public string Title { get { return _Title; } set { Set(nameof(Title), ref _Title, value); } }
        public ObservableCollection<ResultData> Children { get; set; } = new ObservableCollection<ResultData>();
        public ResultData Parent { get; set; }

        public ObservableCollection<QuestionAnswer> TmpInQuestion { get; set; } = new ObservableCollection<QuestionAnswer>();
        public ObservableCollection<QuestionAnswer> TmpOutQuesiton { get; set; } = new ObservableCollection<QuestionAnswer>();

        public virtual void SubmitFukabori()
        {

        }

    }
}
