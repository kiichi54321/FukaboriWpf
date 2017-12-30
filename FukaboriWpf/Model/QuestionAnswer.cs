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
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace CrossTableSilverlight.Model
{
    [DataContract]
    public class QuestionAnswer : System.ComponentModel.INotifyPropertyChanged
    {
        [DataMember]
        public AnswerType AnswerType { get; set; }
        [DataMember]
        public AnswerType2 AnswerType2 { get; set; }
        [DataMember]
        public string TextValue { get; set; }
        [DataMember]
        public double Value { get; set; }
        private double? groupValue = null;
        [DataMember]
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

        public Question Question { get; set; }
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// 辞書作成時キーにする文字列
        /// </summary>
        public string KeyValue
        {
            get
            {
                if (AnswerType == AnswerType.ラベル || AnswerType == Model.AnswerType.文字列)
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
        public string GroupKey
        {
            get
            {
                return TextValue.Split('-').First();
            }
        }

        public string ViewText
        {
            get
            {
                return Value + ":" + TextValue + " \t" + Question.ViewText;
            }
        }

        public string QuestionText
        {
            get
            {
                return Question.ViewText;
            }
        }

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



        #region INotifyPropertyChanged メンバー

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
 
}
