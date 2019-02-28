using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight.Command;

namespace FukaboriCore.Model
{
    /// <summary>
    /// Question関係の拡張メソッド
    /// </summary>
    public static class QuestionHelper
    {
        public static double GetValue(this QuestionAnswer a)
        {
            if(a !=null ) return a.Value;
            else { return double.NaN; }
        }
    }

    public class AnswerGroup : QuestionAnswer
    {        
        public System.Collections.ObjectModel.ObservableCollection<QuestionAnswer> Answeres { get; set; }
        private bool isActive = true;
        
        public bool IsActive
        {
            get {
                if (this.Answeres.Any())
                {
                    return isActive;
                }
                return false;
            }
            set { isActive = value; }
        }

        public AnswerGroup()
        { }

        public void SetQuestion(Question question)
        {
            this.Question = question;
            foreach (var item in Answeres)
            {
                item.Question = question;
            }
        }

        public AnswerGroup(QuestionAnswer qa)
        {
            Answeres = new System.Collections.ObjectModel.ObservableCollection<QuestionAnswer>() { qa };
            this.Value = qa.GroupValue;
            this.TextValue = qa.TextValue;
            this.Question = qa.Question;
            this.AnswerType = qa.AnswerType;
            this.AnswerType2 = qa.AnswerType2;
        }
        //    public Question Question { get; set; }



        public override bool Check(MyLib.IO.TSVLine line)
        {
            var val = line.GetValue(this.Question.Key, null);
            if (val == null)
            {
                return false;
            }

            if (this.AnswerType == AnswerType.タグ)
            {
                foreach(var item in val.Split(',').Select(n=>n.Trim()))
                {
                    var answer = this.Question.GetOriginalValue(val);
                    if (Answeres.Any(n => n.TextValue == answer.TextValue))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                var answer = this.Question.GetOriginalValue(val);
                if (Answeres.Any(n => n.TextValue == answer.TextValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Move(QuestionAnswer qa)
        {
            foreach (var item in this.Question.AnswerGroup)
            {
                if (item.Answeres.Contains(qa))
                {
                    item.Answeres.Remove(qa);
                }
            }
            this.Answeres.Add(qa);
            this.Question.AnswerChange();

        }

        public void Add(QuestionAnswer qa)
        {
            this.Answeres.Add(qa);
        }

        public QuestionAnswer GetQuestionAnswer(double value)
        {
            if (this.Answeres.Where(n => n.Value == value).Any())
            {
                return this.Answeres.Where(n => n.Value == value).First();
            }
            else
            {
                return null;
            }
        }
    }
}