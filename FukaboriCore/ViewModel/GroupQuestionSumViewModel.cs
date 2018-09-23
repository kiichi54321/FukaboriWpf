using System.Linq;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Ioc;
using FukaboriCore.Model;

namespace FukaboriCore.ViewModel
{
    public class GroupQuestionSumViewModel : GalaSoft.MvvmLight.ViewModelBase ,MyLib.Interface.ITsv
    {
        public GroupQuestionSumViewModel()
        {
            DataList = new System.Collections.ObjectModel.ObservableCollection<GroupQuestionSum>();
            Answer = new System.Collections.ObjectModel.ObservableCollection<Question>();
            TargetAnswer = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();
        }
        public System.Collections.ObjectModel.ObservableCollection<Question> Answer { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<GroupQuestionSum> DataList { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<AnswerGroup> TargetAnswer { get; set; }

        public List<Question> QuestionList => Enqueite.Current.QuestionList;

        public IList TargetQusetions { get { return _TargetQusetions; } set { Set(ref _TargetQusetions, value); } }
        private IList _TargetQusetions = default(IList);

        public Question GroupKeyQuestion
        {
            get { return _GroupKeyQuestion; }
            set { if( Set(ref _GroupKeyQuestion, value))
                {
                    if (GroupKeyQuestion != null)
                    {
                        TargetText = GroupKeyQuestion.ViewText;
                    }
                    else
                    {
                        TargetText = string.Empty;
                    }
                }
            }
        }
        private Question _GroupKeyQuestion = default(Question);

        private void Reset()
        {
            TargetQusetions = null;
            GroupKeyQuestion = null;
        }
        #region Reset Command
        /// <summary>
        /// Gets the Reset.
        /// </summary>
        public RelayCommand ResetCommand
        {
            get { return _ResetCommand ?? (_ResetCommand = new RelayCommand(() => { Reset(); })); }
        }
        private RelayCommand _ResetCommand;
        #endregion


        private void Submit(IList list)
        {
            Create(GroupKeyQuestion, list.Cast<Question>(), Enqueite.Current.AnswerLines);
        }
        #region Submit Command
        /// <summary>
        /// Gets the Submit.
        /// </summary>
        public RelayCommand<IList> SubmitCommand
        {
            get { return _SubmitCommand ?? (_SubmitCommand = new RelayCommand<IList>((n) => { Submit(n); })); }
        }
        private RelayCommand<IList> _SubmitCommand;
        #endregion


        public string TargetText { get { return _TargetText; } set { Set(ref _TargetText, value); } }
        private string _TargetText = default(string);

        private void Clip()
        {
            SimpleIoc.Default.GetInstance<Service.ISetClipBoardService>().SetTextWithMessage(this.ToTsv());
        }
        #region Clip Command
        /// <summary>
        /// Gets the Clip.
        /// </summary>
        public RelayCommand ClipCommand
        {
            get { return _ClipCommand ?? (_ClipCommand = new RelayCommand(() => { Clip(); })); }
        }
        private RelayCommand _ClipCommand;
        #endregion


        public string ToTsv()
        {
            MyLib.IO.TsvBuilder tsv = new MyLib.IO.TsvBuilder();
            foreach (var item in DataList)
            {
                item.ToTsv(tsv);
            }
            return tsv.ToString();
        }

        #region Sort Command
        /// <summary>
        /// Gets the Sort.
        /// </summary>
        public RelayCommand<object> SortCommand
        {
            get { return _SortCommand ?? (_SortCommand = new RelayCommand<object>((n) => { Sort(n); })); }
        }
        private RelayCommand<object> _SortCommand;
        #endregion

        public void Sort(object obj)
        {
            var list = DataList.ToList();
            if (obj is AnswerGroup)
            {
                var answer = obj as AnswerGroup;
                SortData(SortBy(list, n => n.GetRate(answer)));
            }
            else
            {
                if (obj.ToString() == "平均")
                {
                    SortData(SortBy(list, n => n.Avg));
                }
                else if (obj.ToString() == "分散")
                {
                    SortData(SortBy(list, n => n.Std));
                }
                else if (obj.ToString() == "カウント")
                {
                    SortData(SortBy(list, n => n.Count));
                }
            }
        }

        bool sortFlag = true;
        private IEnumerable<GroupQuestionSum> SortBy<T>(IEnumerable<GroupQuestionSum> list,System.Func<GroupQuestionSum,T> func )
        {
            if(sortFlag)
            {
                list = list.OrderBy(func);
            }
            else
            {
                list = list.OrderByDescending(func);
            }
            sortFlag = !sortFlag;
            return list;
        }

        private void SortData(IEnumerable<GroupQuestionSum> list)
        {          
            DataList.Clear();
            foreach (var item in list)
            {
                DataList.Add(item);
            }
        }

        public void Create(Question targetQuestion, IEnumerable<Question> groupQuestion, IEnumerable<MyLib.IO.TSVLine> lines)
        {
            if (targetQuestion == null || groupQuestion == null)
            {
                return;
            }
            TargetAnswer.Clear();
            foreach (var item in targetQuestion.AnswerGroupIsActive)
            {
                TargetAnswer.Add(item);
            }

            RaisePropertyChanged("TargetText");
            Dictionary<string, GroupQuestionSum> dic = new Dictionary<string, GroupQuestionSum>();
            List<double> targetValueList = new List<double>();
            foreach (var line in lines)
            {
                var target = targetQuestion.GetValue(line);
                List<AnswerGroup> list = new List<AnswerGroup>();
                if (target != null)
                {
                   targetValueList.Add(target.Value);
                    bool flag = false;
                    foreach (var item2 in groupQuestion)
                    {
                        var g = item2.GetValue(line);
                        if (g != null)
                        {
                            list.Add(g);
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (flag) continue;
                    var key = list.Aggregate(new System.Text.StringBuilder(), (n, m) => n.Append(m.ViewText2).Append("_")).ToString();
                    if (dic.ContainsKey(key))
                    {
                        dic[key].Add(target);
                    }
                    else
                    {
                        GroupQuestionSum g = new GroupQuestionSum();
                        g.QuestionAnswerList = list.ToList();
                        g.Question = targetQuestion;
                        dic.Add(key, g);
                        g.Add(target);
                    }
                }
            }
            MyLib.Statistics.AvgStd avg = new MyLib.Statistics.AvgStd(targetValueList);


            DataList.Clear();
            Answer.Clear();
            foreach (var item in groupQuestion)
            {
                Answer.Add(item);
            }
            foreach (var item in dic)
            {
                item.Value.SetUp();
                item.Value.Parent = this;
            }
            foreach (var item in dic.Values.OrderByDescending(n => n.Avg))
            {
                item.偏差値 = avg.Get偏差値(item.Avg);
                item.CheckVisibility();
                DataList.Add(item);
            }
        }
        int minCount = 10;
        public int MinCount
        {
            get
            {
                return minCount;
            }
            set
            {
                if (minCount == value)
                {
                }
                else
                {
                    minCount = value;
                    bool flag = false;
                    foreach (var item in DataList)
                    {
                        flag = item.CheckVisibility() || flag;
                    }
                    RaisePropertyChanged("MinCount");
                    RaisePropertyChanged("DataList");
                }
            }
        }

    }

}
