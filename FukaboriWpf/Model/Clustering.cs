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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CrossTableSilverlight.Model
{
    public class Clustering
    {
        MyLib.Analyze.WardMethod wardMethod = new MyLib.Analyze.WardMethod();

        public Clustering()
        {
            Init();
        }

        public void Init()
        {
            MyLib.Analyze.WardMethod wardMethod = new MyLib.Analyze.WardMethod();
            wardMethod.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(wardMethod_RunWorkerCompleted);
            wardMethod.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(wardMethod_ProgressChanged);
        }
        public event System.ComponentModel.ProgressChangedEventHandler ProgressChanged;
        public event System.ComponentModel.RunWorkerCompletedEventHandler RunWorkerCompleted;

        void wardMethod_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (this.RunWorkerCompleted != null)
            {
                this.RunWorkerCompleted(sender, e);
            }
        }

        void wardMethod_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(sender, e);
            }
        }

        public bool IsBusy
        {
            get
            {
                return wardMethod.IsBusy;
            }
        }

        IEnumerable<Model.Question> question;

        public IEnumerable<Model.Question> QuestionList
        {
            get { return question; }
            set { question = value; }
        }

        public void SetData(IEnumerable<MyLib.IO.TSVLine> dataLine, IEnumerable<Model.Question> question)
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
                        if (a.AnswerType2 == Model.AnswerType2.連続値)
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

        public void Run()
        {
            if (wardMethod.IsBusy == false)
            {
                wardMethod.Run();
            }
        }

        public IEnumerable<MyLib.Analyze.WardMethod.Cluster> GetCluster(int num)
        {
            return wardMethod.GetCluster(num);
        }

        public IEnumerable<ClusterViewData> GetClusterView(int num, IEnumerable<Model.Question> question)
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

    [DataContract]
    public class ClusteringData : MyLib.Interface.ITsv
    {
        [DataMember]
        public List<ClusterViewData> ClusterViewDataList { get; set; }
        [DataMember]
        public int ClusterNum { get; set; }
        [DataMember]
        public List<string> UsedQuestion { get; set; }
        [DataMember]
        public int TryCount { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
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


    [DataContract]
    public class ClusterViewData
    {
        [DataMember]
        public IEnumerable<PropertyData> Properties { get; set; }

        public IEnumerable<MySilverlightLibrary.DataVisualization.IData> IDataProperties
        {
            get { return Properties.OfType<MySilverlightLibrary.DataVisualization.IData>().ToList(); }
        }

        Dictionary<Model.Question, List<double>> dic = new Dictionary<Model.Question, List<double>>();
        [DataMember]
        public Dictionary<Model.Question, List<double>> Dic
        {
            get { return dic; }
            set { dic = value; }
        }

        public void AddData(Model.Question key, double value)
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
        [DataMember]
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
        [DataMember]
        public int Order { get; set; }

        public IEnumerable<PropertyData> GetPropertyData(IEnumerable<Model.Question> question)
        {
            return PropertyData.CreatePropertyData(question, dataLines);
        }
    }
    [DataContract]
    public class PropertyData : MySilverlightLibrary.DataVisualization.IData
    {


        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Average { get; set; }
        [DataMember]
        public double Std { get; set; }
        Dictionary<Model.AnswerGroup, KeyCount> keyCountDic = new Dictionary<Model.AnswerGroup, KeyCount>();
        private Model.Question question;
        public Model.Question Question { get { return question; } set { SetQuestion(value); } }
        [DataMember]
        public double? MaxValue2 { get; set; }


        [DataMember]
        public string ImageUrl { get; set; }
        public IEnumerable<ImageData> ImageDataList
        {
            get
            {
                if(Question.Children !=null && Question.Children.Any())
                {
                    var qlist = Question.Children.Select(n => Question.QuestionManage.GetQuestion(n));
                    SimpleSummary ss = new SimpleSummary();
                    ss.CreatePropertyData(qlist);
                    return ss.DataList.Select(n => new ImageData() { Source = n.ImageUrl, Value = n.Value*100, ValueVisibilty = Visibility.Visible }).OrderByDescending(n=>n.Value).Take((int)State.MaxImageViewNum.Value);
                }
                else
                {
                    return new List<ImageData>(){ new ImageData(){ Source = Question.ImageUrl, ValueVisibilty = Visibility.Collapsed}};
                }
            }

        }
        public Visibility ImageVisibility
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl) && (Question.Children == null || Question.Children.Any() == false))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        [DataMember]
        public Dictionary<Model.AnswerGroup, KeyCount> KeyCountDic
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

        public void SetQuestion(Model.Question q)
        {
            this.question = q;
            keyCountDic = new Dictionary<Model.AnswerGroup, KeyCount>();
            foreach (var item in q.AnswerGroupIsActive.Where(n => n.Answeres.Count > 0))
            {
                keyCountDic.Add(item, new KeyCount() { Key = item.TextValue, Value = item.Value, Count = 0, MaxValue = q.MaxValue });
            }
            this.ImageUrl = q.ImageUrl;

        }

        public void Add(Model.AnswerGroup aq)
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

        public static IEnumerable<PropertyData> CreatePropertyData(IEnumerable<Model.Question> question, IEnumerable<MyLib.IO.TSVLine> lines)
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
    [DataContract]
    public class KeyCount : MySilverlightLibrary.DataVisualization.IData
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public double Rate { get; set; }
        //[DataMember]
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
        public Visibility ValueVisibilty { get; set; }
        public double Value { get; set; }
    }
}
