using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using MyLib.UI;
using FukaboriCore.ViewModel;

namespace FukaboriCore.Model
{

    public class GroupQuestionSum : GalaSoft.MvvmLight.ObservableObject
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
            foreach (var item in countDic.OrderBy(n => n.Key.Order))
            {
                AnswerCell.Add(new Cell() { Count = item.Value, 横Rate = item.Value * 100 / (double)this.Count });
            }
        }

        public bool Visibility { get { return _Visibility; } set { Set(ref _Visibility, value); } }
        private bool _Visibility = default(bool);

        public bool CheckVisibility()
        {
            Visibility = Parent.MinCount < this.Count;
            return Visibility;
        }

        public int Count { get { return data.Count; } }
        public GroupQuestionSumViewModel Parent { get; set; }

        public void ToTsv(MyLib.IO.TsvBuilder tsv)
        {
            if (Visibility == false) return;

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
    }

}
