using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using MyLib.UI;
using MyLib.Message;

namespace FukaboriCore.Model
{
    
    public class Question : ObservableObject, System.ComponentModel.INotifyPropertyChanged
    {
        
        public string Key { get; set; }
        private string text = string.Empty;
        
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"\{.*\}");
                var m = r.Match(value);
                if (m.Success)
                {
                    ImageUrl = m.Value.Replace("{", "").Replace("}", "");
                    text = text.Replace(m.Value, string.Empty);
                }

                RaisePropertyChanged(nameof(Text));
            }
        }
        List<string> answer = new List<string>();
        
        public List<string> Answers
        {
            get { return answer; }
            set
            {
                answer = value;
            }
        }
        public string ViewText { get { return Key + ":" + Text; } }
        private string type = string.Empty;
        
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;               
            }
        }

        public void SetType(string type)
        {
            if (type == "離散")
            {
                AnswerType = AnswerType.離散;
                AnswerType2 = AnswerType2.離散;
            }
            else if (type == "順序")
            {
                AnswerType = AnswerType.順序;
                AnswerType2 =AnswerType2.連続値;
            }
            else if (type == "数値")
            {
                AnswerType = AnswerType.数値;
                AnswerType2 = AnswerType2.連続値;
            }
            else if (type == "文字列")
            {
                AnswerType = AnswerType.文字列;
                AnswerType2 = AnswerType2.離散;
            }
            else if (type == "ラベル")
            {
                AnswerType = AnswerType.ラベル;
                AnswerType2 = AnswerType2.離散;
            }
            else
            {
                AnswerType = AnswerType.不明;
                AnswerType2 = AnswerType2.離散;
            }
        }

        
        public AnswerType AnswerType { get; set; }
        
        public AnswerType2 AnswerType2 { get; set; }

        
        public string ImageUrl { get; set; }

        private List<string> children = new List<string>();
        public List<string> Children { get { return children; } set { children = value; } }
        [Newtonsoft.Json.JsonIgnore]
        public QuestionManage QuestionManage { get; set; }

        /// <summary>
        /// これは拡張質問である。
        /// </summary>
        public bool IsExtendQuestion { get { return isExtendQuestion; } set { isExtendQuestion = value; } }
        bool isExtendQuestion = false;

        public bool CanDelete
        {
            get { if (IsExtendQuestion) return true;
                return false;
            }
        }

        async void Delete()
        {
            var result = await Messenger.Default.SendAsync<bool, NotificationMessage<string>>(
       new NotificationMessage<string>("本当に消しますか？", "注意"));
            if (result)
            {
                this.QuestionManage.Delete(this);
            }
        }

        RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null) deleteCommand = new RelayCommand(() => Delete());
                return deleteCommand;
            }
        }

        public bool ImageVisibility
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl) && (Children == null || Children.Any()==false))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //bool reverse = false;
        // 
        //public bool Reverse
        //{
        //    get { return reverse; }
        //    set { reverse = value; }
        //}

        System.Collections.ObjectModel.ObservableCollection<AnswerGroup> answerGroup = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();
        
        public System.Collections.ObjectModel.ObservableCollection<AnswerGroup> AnswerGroup
        {
            get { return answerGroup; }
            set { answerGroup = value; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<AnswerGroup> AnswerGroupIsActive
        {
            get
            {
                return answerGroup.Where(n => n.IsActive == true);
            }
        }

        public void CreateQuestionAnswer(IEnumerable<double> list2)
        {
            int order = 1;
            Dictionary<string, AnswerGroup> dic = new Dictionary<string, AnswerGroup>();
            foreach (var item in list2.Distinct().OrderBy(n => n))
            {
                var qa = new QuestionAnswer()
                {
                    AnswerType = this.AnswerType,
                    AnswerType2 = this.AnswerType2,
                    Value = item,
                    Question = this,
                    TextValue = item.ToString()
                };
                var key = item.ToString("F1");
                if (dic.ContainsKey(key))
                {
                    dic[key].Add(qa);
                }
                else
                {
                    var g = new AnswerGroup(qa);
                    g.TextValue = key;
                    g.Order = order;
                    g.Value = double.Parse(key);
                    this.AnswerGroup.Add(g);
                    dic.Add(key, g);
                    order++;
                }
            }
        }
        


        public void CreateQuestionAnswer(IEnumerable<MyLib.IO.TSVLine> lines)
        {

            List<string> list = new List<string>();
            foreach (var item in lines)
            {
                list.Add(item.GetValue(this.Key, null));
            }
            List<double> list2 = new List<double>();
            foreach (var item in list.Distinct())
            {
                double val;
                if (double.TryParse(item, out val))
                {
                    list2.Add(val);
                }
            }
            CreateQuestionAnswer(list2); 

        //    int order = 1;
        ////    if (this.Answers == null || this.Answers.Count == 0)
        //    {
        //        Dictionary<string, AnswerGroup> dic = new Dictionary<string, Model.AnswerGroup>();
        //        foreach (var item in list2.OrderBy(n => n))
        //        {
        //            var qa = new QuestionAnswer()
        //            {
        //                AnswerType = this.AnswerType,
        //                AnswerType2 = this.AnswerType2,
        //                Value = item,
        //                Question = this,
        //                TextValue = item.ToString()
        //            };
        //            var key = item.ToString("F1");
        //            if (dic.ContainsKey(key))
        //            {
        //                dic[key].Add(qa);
        //            }
        //            else
        //            {
        //                var g = new AnswerGroup(qa);
        //                g.TextValue = key;
        //                g.Order = order;
        //                g.Value = double.Parse(key);
        //                this.AnswerGroup.Add(g);
        //                dic.Add(key, g);
        //                order++;
        //            }
        //        }
        //    }
            //else
            //{
            //    Dictionary<Between, AnswerGroup> dic2 = new Dictionary<Between, Model.AnswerGroup>();
            //    List<double> list3 = new List<double>();
            //    foreach (var item in this.Answers)
            //    {
            //        double v;
            //        if (double.TryParse(item.Split(':').First(), out v))
            //        {
            //            list3.Add(v);

            //        }
            //    }
            //    double tmp = list3.OrderBy(n => n).First();
            //    dic2.Add(new Between() { End = tmp }, new AnswerGroup()
            //    {
            //        AnswerType = this.AnswerType,
            //        AnswerType2 = this.AnswerType2,
            //        Question = this,
            //        Value = tmp,
            //        TextValue = tmp +"未満",
            //        Order = order
            //    });
            //    order++;
            //    foreach (var item in list3.OrderBy(n => n).Skip(1))
            //    {
            //        {
            //            dic2.Add(new Between() { Start = tmp, End = item }, new AnswerGroup()
            //            {
            //                AnswerType = this.AnswerType,
            //                AnswerType2 = this.AnswerType2,
            //                Question = this,
            //                Value = tmp,
            //                TextValue = tmp.ToString()+"以上"+ item.ToString()+"未満",
            //                Order = order
            //            });
            //        }
            //        tmp = item;
            //        order++;
            //    }
            //    tmp =  list3.OrderBy(n=>n).Last();
            //    dic2.Add(new Between() { Start =  tmp}, new AnswerGroup()
            //    {
            //        AnswerType = this.AnswerType,
            //        AnswerType2 = this.AnswerType2,
            //        Question = this,
            //        Value = tmp,
            //        TextValue = tmp + "以上",
            //        Order = order
            //    });
            //    tmp = list3.OrderBy(n => n).First();
            //    foreach (var item in dic2)
            //    {
            //        this.AnswerGroup.Add(item.Value);
            //    }

            //    foreach (var item in list2.OrderBy(n => n))
            //    {
            //        var qa = new QuestionAnswer()
            //        {
            //            AnswerType = this.AnswerType,
            //            AnswerType2 = this.AnswerType2,
            //            Value = item,
            //            Question = this,
            //            TextValue = item.ToString()
            //        };
            //        foreach (var item3 in dic2)
            //        {
            //            if (item3.Key.Check(item))
            //            {
            //                item3.Value.Add(qa);
            //                break;
            //            }
            //        }
            //    }
            //    if (dic2.First().Value.Answeres.Count == 0)
            //    {
            //        this.AnswerGroup.Remove(dic2.First().Value);
            //    }
            //    if (dic2.Last().Value.Answeres.Count == 0)
            //    {
            //        this.AnswerGroup.Remove(dic2.Last().Value);
            //    }
            //}
        }

        public class Between
        {
            public double? Start { get; set; }
            public double? End { get; set; }
            public bool Check(double d)
            {
                if (Start.HasValue == false)
                {
                    Start = double.MinValue;
                }
                if (End.HasValue == false)
                {
                    End = double.MaxValue;
                }
                if (Start == End)
                {
                    if (d == Start)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (d >= Start && d < End)
                {
                    return true;
                }
                return false;
            }
        }


        public QuestionAnswer GetOriginalValue(string value)
        {
            QuestionAnswer qa = new QuestionAnswer() { AnswerType = this.AnswerType };
            qa.Question = this;
            qa.AnswerType2 = this.AnswerType2;

            if (AnswerType == AnswerType.数値)
            {
                if(string.IsNullOrEmpty(value))
                {
                    qa.Value = double.NaN;
                    return qa;
                }

                var chek = this.AnswerGroup.Select(n => n.GetQuestionAnswer(double.Parse(value))).Where(n => n != null).DefaultIfEmpty(null).FirstOrDefault();
                if (chek == null)
                {
                    qa = new QuestionAnswer() { AnswerType = this.AnswerType, AnswerType2 = this.AnswerType2, Value = double.Parse(value), Question = this, TextValue = value };
                    var g = new AnswerGroup(qa);
                    this.AnswerGroup.Add(g);
                    int order = 1;
                    foreach (var item in this.AnswerGroup.OrderBy(n => n.Value))
                    {
                        item.Order = order;
                        order++;
                    }

                    return qa;
                }
                else
                {
                    return chek;
                }
            }
            else if (AnswerType == AnswerType.文字列)
            {
                qa.TextValue = value;
                qa.Value = double.NaN;
                return qa;
            }
            else if(AnswerType ==AnswerType.不明)
            {
                qa.TextValue = value;
                qa.Value = double.NaN;
                return qa;
            }
            
            if (Answers.Count > 0)
            {
                var list = GetOriginalValue().Where(n => n.Value.ToString() == value);
                if (list.Any())
                {
                    return list.First();
                }
                else
                {
                    if (AnswerType !=AnswerType.ラベル)
                    {
                        qa.TextValue = "エラー";
                        qa.Value = double.NaN;
                        return qa;
                    }
                }
            }
            if (AnswerType == AnswerType.ラベル)
            {
                qa.TextValue = value;
                qa.Value = double.NaN;
                return qa;
            }
            //if (AnswerType == Model.AnswerType.離散 || AnswerType == Model.AnswerType.順序)
            //{


            //}
            //else 
            return qa;


        }

        List<QuestionAnswer> GetOriginalValue()
        {
            List<QuestionAnswer> list = new List<QuestionAnswer>();
            int count = 1;
            if (this.AnswerType == AnswerType.文字列 || this.AnswerType == AnswerType.数値)
            {                
                return new List<QuestionAnswer>();
            
            }
            if(this.AnswerType == AnswerType.ラベル)
            {
                foreach (var item in this.Answers)
                {
                    QuestionAnswer qa = new QuestionAnswer() { AnswerType = this.AnswerType };
                    qa.Question = this;
                    qa.TextValue = item;
                    qa.Value = count;
                    qa.GroupValue = count;
                    count++;
                    list.Add(qa);
                }
                return list;
            }
            foreach (var item in this.Answers)
            {
                var d = item.Split(':');
                QuestionAnswer qa = new QuestionAnswer() { AnswerType = this.AnswerType };
                qa.Question = this;
                qa.Order = count;
                if (d.Length > 1)
                {
                    try
                    {
                        var intValue = int.Parse(d[0]);
                        qa.TextValue = d[1];
                      
                        if (d.Length > 2)
                        {
                            qa.Value = int.Parse(d[0]);
                            qa.GroupValue = int.Parse(d[2]);
                            //qa.Value = int.Parse(d[0]);
                            //qa.OrignalValue = int.Parse(d[2]);
                        }
                        else
                        {
                            qa.GroupValue = intValue;
                            qa.Value = intValue;
                        }
                    }
                    catch
                    {
                        qa.TextValue = "エラー";
                        qa.Value = double.NaN;
                    }
                }
                else
                {
                    qa.TextValue = item;
                    qa.Value = count;
                    qa.GroupValue = count;
                }
                count++;
                list.Add(qa);
            }

            return list;
        }

        public List<AnswerGroup> GetValue()
        {
            return AnswerGroup.ToList();
        }

        public AnswerGroup GetValue(MyLib.IO.TSVLine line)
        {
            return GetAnswerGroup(line.GetValue(this.Key, null));
        }

        public void Init()
        {
            answerGroup = new System.Collections.ObjectModel.ObservableCollection<AnswerGroup>();
            Dictionary<string, AnswerGroup> dic = new Dictionary<string, AnswerGroup>();
            int count = 1;
            foreach (var item in GetOriginalValue())
            {
                if (dic.ContainsKey(item.GroupKey))
                {
                    dic[item.GroupKey].Add(item);
                }
                else
                {
                    dic.Add(item.GroupKey, new AnswerGroup(item) { TextValue = item.GroupKey, Order = count });
                    answerGroup.Add(dic[item.GroupKey]);
                    count++;
                }
            }

        }
        public void Init2()
        {
            foreach (var item in answerGroup)
            {
                item.SetQuestion(this);
            }
            AnswerChange();
        }


        public IEnumerable<QuestionAnswer> QuestionAnsweres
        {
            get
            {
                List<QuestionAnswer> list = new List<QuestionAnswer>();
                foreach (var item in answerGroup)
                {
                    list.AddRange(item.Answeres);
                }
                return list;
            }
        }


        public static Question Create(int num, string name)
        {
            Question q = new Question();
            List<string> list = new List<string>();
            for (int i = 0; i < num; i++)
            {
                list.Add((i + 1).ToString() + ":" + name + (i + 1).ToString());
            }
            q.Answers = list;
            q.AnswerType = AnswerType.順序;
            q.AnswerType2 = AnswerType2.連続値;
            q.Init();
            return q;
        }

        public static Question Create(string name,IEnumerable<string> list)
        {
            Question q = new Question();
            q.Key = name;

            if (CheckDouble(list))
            {
                q.AnswerType = AnswerType.数値;
                q.AnswerType2 = AnswerType2.連続値;
            }
            else
            {
                q.Answers = list.ToList();
                q.AnswerType = AnswerType.ラベル;
                q.AnswerType2 = AnswerType2.離散;
            }
            q.Init();
            return q;
        }

        static bool CheckDouble(IEnumerable<string> list)
        {
            bool flag = true;
            double d;
            foreach (var item in list)
            {
                if( double.TryParse(item,out d))
                {

                }
                else
                {
                    flag = false; break;
                }
            }
            return flag;
        }




        Dictionary<string, AnswerGroup> answerGroupDic;
        internal void AnswerChange()
        {
            if (answerGroupDic == null) answerGroupDic = new Dictionary<string,AnswerGroup>();
            answerGroupDic.Clear();
            foreach (var item in this.AnswerGroup)
            {
                foreach (var item2 in item.Answeres.Select(n=>n.KeyValue).Distinct())
                {
                    answerGroupDic.Add(item2, item);
                }
            }
            maxValue = null;
        }

        private AnswerGroup GetAnswerGroup(string text)
        {
            if (text == null)
            {
                return null;
            }
            if (answerGroupDic == null)
            {
                AnswerChange();
            }
            if (answerGroupDic.ContainsKey(text))
            {
                return answerGroupDic[text];
            }
            else if (text.Length == 0)
            {
                answerGroupDic.Add(text, new AnswerGroup(new QuestionAnswer() { Question = this, AnswerType = this.AnswerType, AnswerType2 = this.AnswerType2, TextValue = "(回答なし)" , Value = double.NaN}));
                return answerGroupDic[text];
            }
            else
            {
                if (answerGroupDic.Where(n => n.Value.TextValue == text).Any())
                {
                    var d = answerGroupDic.Where(n => n.Value.TextValue == text).First();
                    answerGroupDic.Add(d.Value.TextValue, d.Value);
                    return d.Value;
                }
                if (this.AnswerType2 == AnswerType2.連続値)
                {
                    answerGroupDic.Add(text, new AnswerGroup(new QuestionAnswer() { Question = this, AnswerType = this.AnswerType, AnswerType2 = this.AnswerType2, TextValue = text, Value = double.Parse( text) }));
                }
                else
                {
                    answerGroupDic.Add(text, new AnswerGroup(new QuestionAnswer() { Question = this, AnswerType = this.AnswerType, AnswerType2 = this.AnswerType2, TextValue = text, }));
                }
                return answerGroupDic[text];
            }
        }

        double? maxValue = null;
        public double MaxValue
        {
            get
            {
                if (maxValue.HasValue)
                {
                    return maxValue.Value;
                }
                else
                {
                    maxValue = double.MinValue;
                    foreach (var item in this.AnswerGroupIsActive.Where(n=>n.Answeres.Count>0))
                    {
                        maxValue = Math.Max(item.Value, maxValue.Value);
                    }
                    return maxValue.Value;
                }
            }
        }

    }


    public enum AnswerType
    {
        離散, 順序, 数値, 文字列, 不明,ラベル
    }
    public enum AnswerType2
    {
        離散, 連続値
    }


    
    public class QuestionManage
    {
        Dictionary<string, Question> dic = new Dictionary<string, Question>();
        
        public Dictionary<string, Question> Dic
        {
            get { return dic; }
            set { dic = value; }
        }
        System.Collections.ObjectModel.ObservableCollection<Question> list = new System.Collections.ObjectModel.ObservableCollection<Question>();
        public void Clear()
        {
            this.dic.Clear();
            this.list.Clear();
        }
        [Newtonsoft.Json.JsonIgnore]
        Enqueite enqueite;

        public void Init( Enqueite e)
        {
            list = new System.Collections.ObjectModel.ObservableCollection<Question>();
            foreach (var item in dic)
            {
                list.Add(item.Value);
                item.Value.Init2();
                item.Value.QuestionManage = this;
            }
            list.CollectionChanged += List_CollectionChanged;
            enqueite = e;
        }

        private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Messenger.Default.Send(new QuestionChangeMessage());
        }

        public void Add(string key, Question q)
        {
            if (dic.ContainsKey(key) == false)
            {
                dic.Add(key, q);
                list.Add(q);
                q.QuestionManage = this;
            }
        }

        public Question GetQuestion(string key)
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            return null;
        }

        public string CreateName(string name)
        {
            int count = 1;
            while (true)
            {
                if (dic.ContainsKey(name + "_" + count.ToString()) == false)
                {
                    break;
                }
                count++;
            }
            return name + "_" + count.ToString();
        }

        public IEnumerable<Question> SearchQuestion(string viewText)
        {
            if(string.IsNullOrEmpty( viewText))
            {
                return List;
            }
            var list = List.Where(n => n.ViewText.Contains(viewText));
            if (list.Any()) return list;
            return List;
        }

        public System.Collections.ObjectModel.ObservableCollection<Question> List { get { return list; } }


        public void AddExtendQuestion(Question q)
        {
            Add(q.Key, q);
            q.Init();
            q.IsExtendQuestion = true;            
        }

        internal void Delete(Question question)
        {            
            dic.Remove(question.Key);
            list.Remove(question);
            foreach (var item in enqueite.AllAnswerLine)
            {
                item.RemoveExtendColumn(question.Key);
            }
        }
    }

    public class QuestionChangeMessage: MessageBase
    {
    }
}
