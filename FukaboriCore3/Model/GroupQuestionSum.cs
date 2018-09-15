using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using MyLib.UI;


namespace FukaboriCore.Model
{
    public class GroupQuestionSum : System.ComponentModel.INotifyPropertyChanged
    {
        public List<AnswerGroup> QuestionAnswerList { get; set; }
        public double Avg { get; set; }
        public double Std { get; set; }
        public double 偏差値 { get; set; }
        Question question;
        public Question Question
        {
            get
            {
                return question;
            }
            set
            {
                question = value;
                foreach (var item in question.AnswerGroupIsActive)
                {
                    countDic.Add(item, 0);
                }
            }
        }

        public double GetRate(AnswerGroup answer)
        {
            if (countDic.ContainsKey(answer))
            {
                return countDic[answer] / (double)Count;
            }
            else
            {
                return -1;
            }
        }

        public List<Cell> AnswerCell { get; set; }
        Dictionary<AnswerGroup, int> countDic = new Dictionary<AnswerGroup, int>();
        public MyLib.Statistics.AvgStd data = new MyLib.Statistics.AvgStd();
        public void Add(AnswerGroup answer)
        {
            if (double.IsNaN(answer.Value) == false)
            {
                if (countDic.ContainsKey(answer))
                {
                    countDic[answer]++;
                }
                else
                {
                    countDic.Add(answer, 1);
                }
                data.Add(answer.Value);
            }
        }
        public void SetUp()
        {
            this.Avg = data.GetAvg();
            this.Std = data.GetStd();

            AnswerCell = new List<Cell>();
            foreach (var item in countDic.OrderBy(n=>n.Key.Order))
            {
                AnswerCell.Add(new Cell() { Count = item.Value, 横Rate = item.Value*100 / (double)this.Count });
            }
        }
        bool visibility = true;

        public bool Visibility
        {
            get
            {
                bool tmpVisibility = true;
                if (Parent.MinCount > this.Count)
                {
                    tmpVisibility = false;
                }
                visibility = tmpVisibility;
                return tmpVisibility;
            }
            set
            {
                if (visibility != value)
                {
                    visibility = value;
                    OnPropertyChanged("Visiblity");
                }
            }
        }

        
        



        public bool CheckVisibility()
        {
            bool tmpVisibility = true;
            if (Parent.MinCount > this.Count)
            {
                tmpVisibility = false;
            }
            if (visibility != tmpVisibility)
            {
                OnPropertyChanged("Visiblity");
                return true;
            }
            return false;
        }

        public int Count { get { return data.Count; } }
        public GroupQuestionSumModel Parent { get; set; }

        public void ToTsv(MyLib.IO.TsvBuilder tsv)
        {
            foreach (var item in this.QuestionAnswerList)
            {
                tsv.Add(item.QuestionText, item.ViewText2);
            }
            tsv.Add("Std", this.Std);
            tsv.Add("Avg", this.Avg);
            tsv.Add("Count", this.Count);

            var target = this.Parent.TargetAnswer.GetEnumerator();
            foreach (var item in AnswerCell)
            {
                target.MoveNext();
                tsv.Add(target.Current.ViewText2, item.Count);
             
            }

            tsv.NextLine();
        }


        #region INotifyPropertyChanged メンバー

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string text)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(text));
            }
        }
        #endregion
    }

    public class GroupQuestionSumModel : System.ComponentModel.INotifyPropertyChanged ,MyLib.Interface.ITsv
    {
        public GroupQuestionSumModel()
        {
            DataList = new System.Collections.ObjectModel.ObservableCollection<GroupQuestionSum>();
            Answer = new System.Collections.ObjectModel.ObservableCollection<Question>();
            TargetAnswer = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();
        }
        public System.Collections.ObjectModel.ObservableCollection<Question> Answer { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<GroupQuestionSum> DataList { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<AnswerGroup> TargetAnswer { get; set; }
        public Question TargetQuestion { get; set; }
        public string TargetText
        {
            get
            {
                if (TargetQuestion != null)
                {
                    return TargetQuestion.ViewText;
                }
                else
                {
                    return string.Empty;
                }
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

        public void Sort(object obj)
        {
            if (obj is AnswerGroup)
            {
                var answer = obj as AnswerGroup;
                var list = DataList.ToList();
                SortData(list.OrderByDescending(n => n.GetRate(answer)));
            }
            else
            {
                if (obj.ToString() == "平均")
                {
                    var list = DataList.ToList();
                    SortData(list.OrderByDescending(n => n.Avg));
                }
                else if (obj.ToString() == "分散")
                {
                    SortData(DataList.ToList().OrderByDescending(n => n.Std));
                }
                else if (obj.ToString() == "カウント")
                {
                    SortData(DataList.ToList().OrderByDescending(n => n.Count));
                }
            }
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
            TargetQuestion = targetQuestion;
            TargetAnswer.Clear();
            foreach (var item in targetQuestion.AnswerGroupIsActive)
            {
                TargetAnswer.Add(item);
            }


            OnPropertyChanged("TargetText");
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
                    if (flag)
                    {
                        var tmp = DataList.ToArray();
                        DataList.Clear();
                        foreach (var item in tmp)
                        {
                            DataList.Add(item);
                        }
                    }
                    OnPropertyChanged("MinCount");
                    OnPropertyChanged("DataList");
                }
            }
        }

        #region INotifyPropertyChanged メンバー

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string text)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(text));
            }
        }
        #endregion


    }
}
