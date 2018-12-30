using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;
using FukaboriCore.ViewModel;
using GalaSoft.MvvmLight.Command;

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
    

}
