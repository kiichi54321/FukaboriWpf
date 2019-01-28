using FukaboriCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukaboriCore.Model
{
    public class PropertyData : MySilverlightLibrary.DataVisualization.IData
    {
        public string Name { get; set; }
        public double Average { get; set; }
        public double Std { get; set; }
        public double Quantile_25 { get; set; }
        public double Quantile_50 { get; set; }
        public double Quantile_75 { get; set; }

        Dictionary<AnswerGroup, KeyCount> keyCountDic = new Dictionary<AnswerGroup, KeyCount>();
        private Question question;
        public Question Question { get { return question; } set { SetQuestion(value); } }

        public double? MaxValue2 { get; set; }

        public string ImageUrl { get; set; }
        public IEnumerable<ImageData> ImageDataList
        {
            get
            {
                if (Question.Children != null && Question.Children.Any())
                {
                    var qlist = Question.Children.Select(n => Question.QuestionManage.GetQuestion(n));
                    SimpleSummaryViewModel ss = new SimpleSummaryViewModel();
                    ss.CreatePropertyData(qlist);
                    return ss.DataList.Select(n => new ImageData() { Source = n.ImageUrl, Value = n.Value * 100, ValueVisibilty = true }).OrderByDescending(n => n.Value).Take((int)State.MaxImageViewNum.Value);
                }
                else if (Question.ImageUrl != null)
                {
                    return new List<ImageData>() { new ImageData() { Source = Question.ImageUrl, ValueVisibilty = false } };
                }
                else
                {
                    return Enumerable.Empty<ImageData>();
                }
            }

        }
        public bool ImageVisibility
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl) && (Question.Children == null || Question.Children.Any() == false))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public Dictionary<AnswerGroup, KeyCount> KeyCountDic
        {
            get
            {
                return keyCountDic;
            }
            set { keyCountDic = value; }
        }

        public IEnumerable<KeyCount> DataRow
        {
            get
            {
                return keyCountDic.OrderBy(n => n.Key.Order).Select(n => n.Value);
            }
        }
        List<double> data = new List<double>();

        public List<double> Data
        {
            get { return data; }
            set { data = value; }
        }

        public void SetQuestion(Question q)
        {
            this.question = q;
            keyCountDic = new Dictionary<AnswerGroup, KeyCount>();
            foreach (var item in q.AnswerGroupIsActive.Where(n => n.Answeres.Count > 0))
            {
                keyCountDic.Add(item, new KeyCount() { Key = item.TextValue, Value = item.Value, Count = 0, MaxValue = q.MaxValue });
            }
            this.ImageUrl = q.ImageUrl;

        }

        public void Add(AnswerGroup aq)
        {
            if (double.IsNaN(aq.Value) == false) data.Add(aq.Value);
            if (keyCountDic.ContainsKey(aq) == false)
            {
                keyCountDic.Add(aq, new KeyCount() { Key = aq.TextValue, Value = aq.Value, Count = 0, MaxValue = aq.Question.MaxValue });
            }
            keyCountDic[aq].Count++;
        }

        public void 計算()
        {
            MyLib.Statistics.AvgStd avg = new MyLib.Statistics.AvgStd(data.Where(n => n != double.NaN));
            this.Average = avg.Avg;
            this.Std = avg.Std;
            this.Quantile_25 = avg.Quartile(1);
            this.Quantile_50 = avg.Quartile(2);
            this.Quantile_75 = avg.Quartile(3);

            var sum = keyCountDic.Sum(n => n.Value.Count);
            foreach (var item in keyCountDic)
            {
                item.Value.Rate = item.Value.Count * 100 / (double)sum;
            }
        }

        public static IEnumerable<PropertyData> CreatePropertyData(IEnumerable<Question> question, IEnumerable<MyLib.IO.TSVLine> lines)
        {
            List<PropertyData> list = question.Select(n => new PropertyData() { Name = n.ViewText, Question = n }).ToList();

            foreach (var line in lines)
            {
                foreach (var q in list)
                {
                    foreach (var item in q.Question.GetValueList(line))
                    {
                        q.Add(item);
                    }
                }
            }
            foreach (var item in list)
            {
                item.計算();
            }

            return list;
        }

        #region IData メンバー

        public double Value
        {
            get
            {
                return this.Average;
            }
            set
            {
                this.Average = value;
            }
        }
        public double MaxValue
        {
            get
            {
                if (MaxValue2.HasValue == false)
                {
                    if (this.Question != null)
                    {
                        MaxValue2 = this.Question.MaxValue;
                        return this.Question.MaxValue;
                    }
                }
                if (MaxValue2.HasValue)
                {
                    return MaxValue2.Value;
                }
                return 0;

            }
            set { }
        }

        public string Label
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        #endregion

        public void ToTsv(MyLib.IO.TsvBuilder tsv)
        {
            tsv.Add("Name", this.Name);
            tsv.Add("Value", this.Value);
            tsv.Add("Average", this.Average);
            tsv.Add("Std", this.Std);
            tsv.Add("ImageUrl", this.ImageUrl);
            foreach (var item in this.KeyCountDic)
            {
                tsv.Add(item.Key.ViewText2, item.Value.Count);
            }
            tsv.NextLine();
        }
    }

    public class KeyCount : MySilverlightLibrary.DataVisualization.IData
    {
        public string Key { get; set; }
        public double Value { get; set; }
        public int Count { get; set; }
        public double Rate { get; set; }

        public bool ExtendVisibility { get; set; } = false;

        public int AllCount { get; set; }
        public double AllRate { get; set; }
        public double TokkaRate { get; set; }

        public void 計算Tokka(KeyCount keyCount)
        {
            ExtendVisibility = true;
            AllCount = keyCount.Count;
            AllRate = keyCount.Rate;
            if (AllRate > 0)
            {
                TokkaRate = Rate / AllRate;
            }
        }

        #region IData メンバー


        public double MaxValue
        {
            get;
            set;
        }

        public string Label
        {
            get
            {
                return Key;
            }
            set
            {
                Key = value;
            }
        }

        #endregion
    }

    public class ImageData
    {
        public string Source { get; set; }
        public bool ValueVisibilty { get; set; }
        public double Value { get; set; }
    }
}
