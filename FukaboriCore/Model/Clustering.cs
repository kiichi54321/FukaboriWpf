using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;
using FukaboriCore.ViewModel;

namespace FukaboriCore.Model
{
    public class Clustering:ObservableObject
    {
        MyLib.Analyze.WardMethod wardMethod = new MyLib.Analyze.WardMethod();

        public Clustering()
        {
            Init();
        }

        public void Init()
        {
            MyLib.Analyze.WardMethod wardMethod = new MyLib.Analyze.WardMethod();
            wardMethod.ReportProgress = reportProgress;
        }

        Action<int> reportProgress;
        public Action<int> ReportProgress
        {
            get { return reportProgress; }
            set {
                reportProgress = value;
            }
        }

        IEnumerable<Question> question;

        public IEnumerable<Question> QuestionList
        {
            get { return question; }
            set { question = value; }
        }

        public void SetData(IEnumerable<MyLib.IO.TSVLine> dataLine, IEnumerable<Question> question)
        {
            this.question = question;
            wardMethod.DataClear();
            foreach (var item in dataLine)
            {
                List<double> data = new List<double>();
                bool flag = true;
                foreach (var item2 in question)
                {
                    var a = item2.GetValue(item);
                    if (a != null)
                    {
                        if (a.AnswerType2 == AnswerType2.連続値)
                        {
                            data.Add(a.Value);
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    wardMethod.AddData(new MyLib.Analyze.WardMethod.Data() { Value = data.ToArray(), Tag = item });
                }
            }
        }

        public async void Run()
        {
            await wardMethod.Run();
        }

        public IEnumerable<MyLib.Analyze.WardMethod.Cluster> GetCluster(int num)
        {
            return wardMethod.GetCluster(num);
        }

        public IEnumerable<ClusterViewData> GetClusterView(int num, IEnumerable<Question> question)
        {
            List<ClusterViewData> list = new List<ClusterViewData>();
            foreach (var item in this.wardMethod.GetCluster(num))
            {
                ClusterViewData cvd = new ClusterViewData();
                list.Add(cvd);
                foreach (var line in item.Tags.OfType<MyLib.IO.TSVLine>())
                {
                    foreach (var q in question)
                    {
                        var l = q.GetValue(line);
                        if (l != null)
                        {
                            cvd.AddData(q, l.Value);
                        }
                    }
                }
                cvd.CreateData();
            }
            return list;
        }

    }

    public class ClusteringData : MyLib.Interface.ITsv
    {
        public List<ClusterViewData> ClusterViewDataList { get; set; }
        public int ClusterNum { get; set; }
        public List<string> UsedQuestion { get; set; }
        public int TryCount { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }

        public void Init(MyLib.IO.TSVFileBase file)
        {
            Dictionary<int, ClusterViewData> dic = new Dictionary<int, ClusterViewData>();
            foreach (var item in ClusterViewDataList)
            {
                item.DataLines = new List<MyLib.IO.TSVLine>();
                foreach (var item2 in item.DataLineIdList)
                {
                    dic.Add(item2, item);
                }
            }

            foreach (var item in file.Lines)
            {
                if (dic.ContainsKey(item.Count))
                {
                    dic[item.Count].Add(item, false);
                }
            }
        }

        #region ITsv メンバー

        public string ToTsv()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int count = 1;
            foreach (var item in this.ClusterViewDataList)
            {
                sb.AppendLine("Cluster:" + count);
                sb.AppendLine("Count:" + item.Count);
                MyLib.IO.TsvBuilder tsv = new MyLib.IO.TsvBuilder();
                foreach (var item2 in item.Properties)
                {
                    item2.ToTsv(tsv);
                }
                sb.AppendLine(tsv.ToString());
                sb.AppendLine();
                count++;
            }
            return sb.ToString();
        }

        #endregion
    }


    
    public class ClusterViewData
    {
        
        public IEnumerable<PropertyData> Properties { get; set; }

        public IEnumerable<MySilverlightLibrary.DataVisualization.IData> IDataProperties
        {
            get { return Properties.OfType<MySilverlightLibrary.DataVisualization.IData>().ToList(); }
        }

        Dictionary<Question, List<double>> dic = new Dictionary<Question, List<double>>();
        
        public Dictionary<Question, List<double>> Dic
        {
            get { return dic; }
            set { dic = value; }
        }

        public void AddData(Question key, double value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(value);
            }
            else
            {
                dic.Add(key, new List<double>() { value });
            }
        }

        public void CreateData()
        {
            List<PropertyData> list = new List<PropertyData>();
            foreach (var item in dic)
            {
                MyLib.Statistics.AvgStd avg = new MyLib.Statistics.AvgStd(item.Value);
                list.Add(new PropertyData() { Name = item.Key.ViewText, Average = avg.GetAvg(), Std = avg.GetStd(), Question = item.Key });
            }
            Properties = list;
        }

        private List<MyLib.IO.TSVLine> dataLines = new List<MyLib.IO.TSVLine>();
        private List<int> dataLineIdList = new List<int>();
        
        public List<int> DataLineIdList
        {
            get { return dataLineIdList; }
            set { dataLineIdList = value; }
        }

        public List<MyLib.IO.TSVLine> DataLines
        {
            get { return dataLines; }
            set { dataLines = value; }
        }

        public void CreateDataLines(MyLib.IO.TSVFileBase file)
        {
            dataLines = new List<MyLib.IO.TSVLine>();
            foreach (var item in file.Lines)
            {
                if (dataLineIdList.Contains(item.Count))
                {
                    dataLines.Add(item);
                }
            }
        }

        public void Add(MyLib.IO.TSVLine line)
        {
            dataLines.Add(line);
            dataLineIdList.Add(line.Count);
        }

        public void Add(MyLib.IO.TSVLine line, bool addIdList)
        {
            dataLines.Add(line);
            if (addIdList)
            {
                dataLineIdList.Add(line.Count);
            }
        }

        public int Count
        {
            get { return dataLines.Count; }
        }
        
        public int Order { get; set; }

        public IEnumerable<PropertyData> GetPropertyData(IEnumerable<Question> question)
        {
            return PropertyData.CreatePropertyData(question, dataLines);
        }
    }
    
    public class PropertyData : MySilverlightLibrary.DataVisualization.IData
    {


        
        public string Name { get; set; }
        
        public double Average { get; set; }
        
        public double Std { get; set; }
        Dictionary<AnswerGroup, KeyCount> keyCountDic = new Dictionary<AnswerGroup, KeyCount>();
        private Question question;
        public Question Question { get { return question; } set { SetQuestion(value); } }
        
        public double? MaxValue2 { get; set; }


        
        public string ImageUrl { get; set; }
        public IEnumerable<ImageData> ImageDataList
        {
            get
            {
                if(Question.Children !=null && Question.Children.Any())
                {
                    var qlist = Question.Children.Select(n => Question.QuestionManage.GetQuestion(n));
                    SimpleSummaryViewModel ss = new SimpleSummaryViewModel();
                    ss.CreatePropertyData(qlist);
                    return ss.DataList.Select(n => new ImageData() { Source = n.ImageUrl, Value = n.Value*100, ValueVisibilty =true }).OrderByDescending(n=>n.Value).Take((int)State.MaxImageViewNum.Value);
                }
                else
                {
                    return new List<ImageData>(){ new ImageData(){ Source = Question.ImageUrl, ValueVisibilty = false}};
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
                //if (keyCountDic.Where(n => n.Key.TextValue == aq.TextValue).Any())
                //{

                //}
                //else
                {
                    keyCountDic.Add(aq, new KeyCount() { Key = aq.TextValue, Value = aq.Value, Count = 0, MaxValue = aq.Question.MaxValue });
                }
            }
            keyCountDic[aq].Count++;
        }

        public void 計算()
        {
            MyLib.Statistics.AvgStd avg = new MyLib.Statistics.AvgStd(data.Where(n=>n!=double.NaN));
            this.Average = avg.Avg;
            this.Std = avg.Std;

            var sum = keyCountDic.Sum(n=>n.Value.Count);
            foreach (var item in keyCountDic)
            {
                item.Value.Rate = item.Value.Count * 100 / (double)sum;
            }
        }

        public static IEnumerable<PropertyData> CreatePropertyData(IEnumerable<Question> question, IEnumerable<MyLib.IO.TSVLine> lines)
        {
            List<PropertyData> list = question.Select(n=>new PropertyData() { Name = n.ViewText, Question = n }).ToList();

            foreach (var line in lines)
            {
                foreach (var q in list)
                {
                    var a = q.Question.GetValue(line);
                    if (a != null ) q.Add(a);
                }
            }
            foreach (var item in list)
            {
                item.計算();
            }

            //foreach (var item in question)
            //{
            //    PropertyData pd = new PropertyData() { Name = item.ViewText, Question = item };
            //    pd.SetQuestion(item);
            //    list.Add(pd);
            //    foreach (var line in lines)
            //    {
            //        var a = item.GetValue(line);
            //        if (a != null)
            //        {
            //            pd.Add(a);
            //        }
            //    }
            //    pd.計算();
            //}
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

        internal void ToTsv(MyLib.IO.TsvBuilder tsv)
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
        //
        //public string ImageUrl { get; set; }
        //public Visibility ImageVisibility
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(ImageUrl))
        //        {
        //            return Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            return Visibility.Visible;
        //        }
        //    }
        //}


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
        public string Source { get; set;}
        public bool ValueVisibilty { get; set; }
        public double Value { get; set; }
    }
}
