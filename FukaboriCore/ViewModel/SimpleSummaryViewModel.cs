using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using MyLib.UI;
using System.Collections.ObjectModel;
using FukaboriCore.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using FukaboriCore.Service;

namespace FukaboriCore.ViewModel
{
    public class SimpleSummaryViewModel:GalaSoft.MvvmLight.ViewModelBase, MyLib.Interface.ITsv
    {
        public SimpleSummaryViewModel()
        {
        }

        public List<PropertyData> DataList { get { return _DataList; } set { Set(ref _DataList, value); } }
        private List<PropertyData> _DataList = default(List<PropertyData>);

        private void Submit(IEnumerable<Question> questions)
        {
            if (TokkakeisuFlag)
            {
                var  target = PropertyData.CreatePropertyData(questions, Enqueite.Current.AnswerLines);
                var all = PropertyData.CreatePropertyData(questions, Enqueite.Current.AllAnswerLine);
                var all_dic = all.SelectMany(n => n.KeyCountDic).ToDictionary(n => n.Key, n => n.Value);
                foreach (var item in target.SelectMany(n=>n.KeyCountDic))
                {
                    item.Value.計算Tokka(all_dic[item.Key]);
                }
                DataList = target.ToList();
            }
            else
            {
                DataList = PropertyData.CreatePropertyData(questions, Enqueite.Current.AnswerLines).ToList();
            }
        }
        #region Submit Command
        /// <summary>
        /// Gets the Submit.
        /// </summary>
        public RelayCommand<IEnumerable<Question>> SubmitCommand
        {
            get { return _SubmitCommand ?? (_SubmitCommand = new RelayCommand<IEnumerable<Question>>((n) => { Submit(n); })); }
        }
        private RelayCommand<IEnumerable<Question>> _SubmitCommand;
        #endregion

        public bool TokkakeisuFlag { get { return _TokkakeisuFlag; } set { Set(ref _TokkakeisuFlag, value); } }
        private bool _TokkakeisuFlag = false;

        public void CreatePropertyData(IEnumerable<Question> question)
        {
            DataList.Clear();
            imageVisibility = false;
            foreach (var item in PropertyData.CreatePropertyData(question, Enqueite.Current.AnswerLines))
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

        bool sortFlag = true;

        List<PropertyData> SortDataList<T>(Func<PropertyData, T> func)
        {
            List<PropertyData> list;
            if (sortFlag)
            {
                list = DataList.OrderBy(func).ToList();
            }
            else
            {
                list = DataList.OrderByDescending(func).ToList();
            }
            sortFlag = !sortFlag;
            return list;
        }

        private void NameSort()
        {
            DataList = SortDataList(n => n.Name);
        }
        #region NameSort Command
        /// <summary>
        /// Gets the NameSort.
        /// </summary>
        public RelayCommand NameSortCommand
        {
            get { return _NameSortCommand ?? (_NameSortCommand = new RelayCommand(() => { NameSort(); })); }
        }
        private RelayCommand _NameSortCommand;
        #endregion

        private void AvgSort()
        {
            DataList = SortDataList(n => n.Average);
        }
        #region AvgSort Command
        /// <summary>
        /// Gets the AvgSort.
        /// </summary>
        public RelayCommand AvgSortCommand
        {
            get { return _AvgSortCommand ?? (_AvgSortCommand = new RelayCommand(() => { AvgSort(); })); }
        }
        private RelayCommand _AvgSortCommand;
        #endregion

        private void StdSort()
        {
            DataList = SortDataList(n => n.Std);
        }
        #region StdSort Command
        /// <summary>
        /// Gets the StdSort.
        /// </summary>
        public RelayCommand StdSortCommand
        {
            get { return _StdSortCommand ?? (_StdSortCommand = new RelayCommand(() => { StdSort(); })); }
        }
        private RelayCommand _StdSortCommand;
        #endregion

        private void ClipTsv()
        {
            MyLib.IO.TsvBuilder tsvBuilder = new MyLib.IO.TsvBuilder();
            foreach (var item in DataList)
            {
                item.ToTsv(tsvBuilder);
                tsvBuilder.NextLine();
            }
            SimpleIoc.Default.GetInstance<ISetClipBoardService>().SetTextWithMessage(tsvBuilder.ToString());
        }
        #region ClipTsv Command
        /// <summary>
        /// Gets the ClipTsv.
        /// </summary>
        public RelayCommand ClipTsvCommand
        {
            get { return _ClipTsvCommand ?? (_ClipTsvCommand = new RelayCommand(() => { ClipTsv(); })); }
        }
        private RelayCommand _ClipTsvCommand;
        #endregion

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
