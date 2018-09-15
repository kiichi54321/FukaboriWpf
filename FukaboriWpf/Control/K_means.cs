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
using FukaboriCore.ViewModel;
using FukaboriCore.Model;

namespace FukaboriWpf.Model
{
    public class K_means
    {
        MyLib.Analyze.K_MeansForTask kMeans = new MyLib.Analyze.K_MeansForTask();
        IEnumerable<Question> question;
        bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
        }
        bool haveResult = false;

        public bool HaveResult
        {
            get { return haveResult; }
            set { haveResult = value; }
        }


        public event EventHandler<MyLib.Event.Args<int>> ReportProgress;


        public void Run(IEnumerable<MyLib.IO.TSVLine> dataLine, IEnumerable<Question> question, int clusterCount, int tryCount, Action<List<MyLib.Analyze.K_MeansForTask.Cluster>> Completed)
        {
            isBusy = true;
            haveResult = false;
            this.question = question;
            kMeans = new MyLib.Analyze.K_MeansForTask(question.Count());
            kMeans.ReportProgress += new EventHandler<MyLib.Event.Args<int>>(kMeans_ReportProgress);
            foreach (var item in dataLine)
            {
                List<double> data = new List<double>();
                foreach (var item2 in question)
                {
                    var a = item2.GetValue(item);
                    if(a !=null)
                    {
                        data.Add(a.Value);
                    }
                }
                kMeans.AddData(data.ToArray(), item);
            }
            clusteringData = new ClusteringData()
            {
                ClusterNum = clusterCount,
                TryCount = tryCount, UsedQuestion = new List<string>(question.Select(n=>n.ViewText))
            };
            
            
            kMeans.Run(clusterCount, tryCount, (n) =>
            {
                isBusy = false;
                haveResult = true;
                clusteringData.ClusterViewDataList =  GetClusterView(question).ToList();
                Completed(n);
            });

        }

        void kMeans_ReportProgress(object sender, MyLib.Event.Args<int> e)
        {
            if (ReportProgress != null)
            {
                ReportProgress(sender, e);
            }
        }

        public IEnumerable<ClusterViewData> GetClusterView(IEnumerable<Question> question)
        {
            List<ClusterViewData> list = new List<ClusterViewData>();
            int order = 1;
            foreach (var item in this.kMeans.ClusterDataList.OrderBy(n => MyLib.MathLib.GetDistance(n.clusterData)))
            {
                ClusterViewData cvd = new ClusterViewData();
                cvd.Order = order;
                list.Add(cvd);
                foreach (var line in item.DataList.Select(n => n.Tag).OfType<MyLib.IO.TSVLine>())
                {
                    foreach (var q in question)
                    {
                        var l = q.GetValue(line);
                        if (l != null)
                        {
                            cvd.AddData(q, l.Value);
                        }
                    }
                    cvd.Add(line);
                }
                cvd.CreateData();
                order++;
            }
            return list;
        }

        ClusteringData clusteringData = new ClusteringData();

        public ClusteringData ClusteringData
        {
            get { return clusteringData; }
            set { clusteringData = value; }
        }

   

        private Question newQuestion;

        public Question NewQuestion
        {
            get
            {
                return newQuestion;
            }
            set { newQuestion = value; }
        }

        public void CreateNewQuestion(string name)
        {           
            newQuestion = Question.Create(this.kMeans.ClusterDataList.Count,"C_");
            newQuestion.Key = name;
            clusteringData.Key = newQuestion.Key;
            int i = 0;
            foreach (var item in kMeans.ClusterDataList.OrderBy(n => MyLib.MathLib.GetDistance(n.clusterData)))
            {
                item.Tag = newQuestion.QuestionAnsweres.ElementAt(i);
                i++;
            }
        }

        public void AddClusterData()
        {
            foreach (var item in kMeans.ClusterDataList.OrderBy(n => MyLib.MathLib.GetDistance(n.clusterData)))
            {
                var q = item.Tag as QuestionAnswer;
                foreach (var item2 in item.DataList)
                {
                    var line = item2.Tag as MyLib.IO.TSVLine;
                    line.AddExtendColumn(q.Question.Key, q.Value.ToString());
                }
            }
        }

    }
}
