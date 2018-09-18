using System;
using System.Net;
using System.Windows;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace FukaboriCore.Model
{
    
    public class QuestionAnswer : GalaSoft.MvvmLight.ObservableObject
    {
        
        public AnswerType AnswerType { get; set; }
        
        public AnswerType2 AnswerType2 { get; set; }
        
        public string TextValue { get; set; }
        
        public double Value { get; set; }
        private double? groupValue = null;
        
        public double GroupValue
        {
            get
            {
                if (groupValue.HasValue)
                {
                    return groupValue.Value;
                }
                else
                {
                    return Value;
                }
            }
            set
            {
                groupValue = value;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public Question Question { get; set; }
        
        public int Order { get; set; }

        /// <summary>
        /// 辞書作成時キーにする文字列
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string KeyValue
        {
            get
            {
                if (AnswerType == AnswerType.ラベル || AnswerType == AnswerType.文字列)
                {
                    return TextValue;
                }
                else
                {
                    return Value.ToString();
                }
            }
        }

        //System.Collections.ObjectModel.ObservableCollection<QuestionAnswer> children = new System.Collections.ObjectModel.ObservableCollection<QuestionAnswer>();

        //public System.Collections.ObjectModel.ObservableCollection<QuestionAnswer> Children
        //{
        //    get { return children; }
        //    set { children = value; }
        //}
        [Newtonsoft.Json.JsonIgnore]
        public string GroupKey
        {
            get
            {
                return TextValue.Split('-').First();
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string ViewText
        {
            get
            {
                return Value + ":" + TextValue + " \t" + Question.ViewText;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string QuestionText
        {
            get
            {
                return Question.ViewText;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string ViewText2
        {
            get
            {
                return Value + ":" + TextValue;
            }
        }

        public virtual bool Check(MyLib.IO.TSVLine line)
        {
            var val = line.GetValue(this.Question.Key, null);
            if (val == null)
            {
                return false;
            }
            var answer = this.Question.GetOriginalValue(val);
            if (answer.TextValue == this.TextValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




    }
 
}
