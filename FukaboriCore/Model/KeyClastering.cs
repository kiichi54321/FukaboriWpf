using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RawlerLib.MyExtend;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections;
using System.Runtime.Serialization;

namespace FukaboriCore.Model
{
    
    public class KeyClustering : GalaSoft.MvvmLight.ObservableObject
    {
        public void Init(Enqueite e)
        {
            this.Enqueite = e;
            foreach (var item in Result)
            {
                item.Parent = this;
                item.Init();
            }
            this.Enqueite.QuestionListChanged += Enqueite_QuestionListChanged;
            QuestionList = this.Enqueite.QuestionList;
            SelectedQuestion = new ObservableCollection<Question>();
            ConnectNum = 5;
        }

        private void Enqueite_QuestionListChanged(object sender, EventArgs e)
        {
            QuestionList = this.Enqueite.QuestionList;
        }

        int idCount = 0;
        
        public int IdCount
        {
            get { return idCount; }set { idCount = value; }
        }
        
        public int ClusterNum
        {
            get
            {
                return clusterNum;
            }

            set
            {
                clusterNum = value;
                RaisePropertyChanged(nameof(ClusterNum));
            }
        }

        int clusterNum = 4;

        [Newtonsoft.Json.JsonIgnore]
        public IList SelectedQuestion { get; set; } = new ObservableCollection<Question>();
        [Newtonsoft.Json.JsonIgnore]
        public Enqueite Enqueite { get; set; }
 
        public int TryCount { get; set; } = 20;
        [Newtonsoft.Json.JsonIgnore]
        public List<Question> QuestionList { get { return _QuestionList; } set { Set(ref _QuestionList, value); } }
        private List<Question> _QuestionList = default(List<Question>);

        int connectNum = 5;

        RelayCommand searchQuestionCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand SearchQuestionCommand
        {
            get { if (searchQuestionCommand == null) {
                    searchQuestionCommand = new RelayCommand(() => QuestionList = Enqueite.QuestionManage.SearchQuestion(SearchText).ToList());
                }
                return searchQuestionCommand;
            }
        }

        public string SearchText { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand RunCommand
        {
            get
            {
                if(runCommand == null)
                {
                    runCommand = new RelayCommand(async () => await Analyze());
                }
                return runCommand;
            }

            set
            {
                runCommand = value;
            }
        }

        RelayCommand runCommand;
    
        public ObservableCollection<QuestionClusterManage> Result { get; set; } = new ObservableCollection<QuestionClusterManage>();
        QuestionClusterManage currentClusterResult = new QuestionClusterManage();
        public QuestionClusterManage CurrentClusterResult
        {
            get
            {
                return currentClusterResult;
            }
            set
            {
                currentClusterResult = value;
                RaisePropertyChanged(nameof(CurrentClusterResult));
            }
        }

        public int ConnectNum
        {
            get
            {
                return connectNum;
            }

            set
            {
                connectNum = value;
                RaisePropertyChanged(nameof(ConnectNum));
            }
        }

        private bool isBusy = false;
        public async Task Analyze()
        {
            MyLib.Analyze.K_MeansForTask kMeans = new MyLib.Analyze.K_MeansForTask(SelectedQuestion.Count);
            QuestionClusterManage questionClusterManage = new QuestionClusterManage() { Parent = this, Title = "無題クラスタ" + idCount };
            questionClusterManage.CreateData(Enqueite, SelectedQuestion.OfType<Question>());
            idCount++;
            foreach (var item in questionClusterManage.DataList)
            {
                kMeans.AddData(item.Soukan.Select(n => n.Value).ToArray(), Enqueite.QuestionManage.GetQuestion(item.QuestionKey));
            }
            isBusy = true;
            var cluster_list = await Task.Run<List<MyLib.Analyze.K_MeansForTask.Cluster>>(() => kMeans.Run(ClusterNum, TryCount));

            int i = 0;
            foreach (var item in cluster_list)
            {
                QuestionCluster qc = new QuestionCluster() { Name = "c_" + i, Manage = questionClusterManage };
                qc.Init();
                foreach (var item1 in item.DataList)
                {
                    Question q = item1.Tag as Question;
                    qc.Add(QuestionClusterItem.Create(q, item1.Data));
                }
                questionClusterManage.Clusters.Add(qc);
                i++;
            }
            questionClusterManage.CreateClusterItemDic();
            CurrentClusterResult = questionClusterManage;
            Result.Add(currentClusterResult);
            await currentClusterResult.CountUser();
            isBusy = false;

        }

        internal void DeleteResult(QuestionClusterManage questionClusterManage)
        {
            Result.Remove(questionClusterManage);
        }

        internal void UpdateResult(QuestionClusterManage questionClusterManage)
        {
            CurrentClusterResult = questionClusterManage;
        }

    }
    
    public class KeyClusteringData
    {
        public double[] Orignal { get; set; }
        public double[] rinsetu { get; set; }
        public string QuestionKey { get; set; }
        public List<KeyValuePair<string, double>> Soukan { get; set; } = new List<KeyValuePair<string, double>>();
    }

    
    public class QuestionClusterManage : GalaSoft.MvvmLight.ObservableObject
    {
        public QuestionClusterManage()
        {
            selected質問追加の手法 = QuestionAddMethod.First();
        }
        
        public void Init()
        {
            foreach (var item in Clusters)
            {
                item.Manage = this;
                item.Init();
            }
            ClusterItemDic = Clusters.SelectMany(n => n.Items).ToDictionary(n => n.Question.Key, n => n);
            selected質問追加の手法 = QuestionAddMethod.First();

        }


        public List<KeyClusteringData> DataList { get; set; } = new List<KeyClusteringData>();
        [Newtonsoft.Json.JsonIgnore]
        public KeyClustering Parent { get; set; }
        Dictionary<string, QuestionClusterItem> ClusterItemDic = new Dictionary<string, QuestionClusterItem>();
        
        public ObservableCollection<QuestionCluster> Clusters { get; set; } = new ObservableCollection<QuestionCluster>();
        string title = "無題クラスタ";
        public int ClusterNum { get { return Clusters.Count; } }
        
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        bool canMove = false;
        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<QuestionClusterItem> AllItems
        {
            get { return ClusterItemDic.Values; }
        }

        public void CreateClusterItemDic()
        {
            ClusterItemDic = Clusters.SelectMany(n => n.Items).ToDictionary(n => n.Question.Key, n => n);
            var dic = DataList.ToDictionary(n =>  n.QuestionKey, n => n);
            foreach (var item in ClusterItemDic)
            {
                item.Value.Manage = this;
                item.Value.Data = dic[item.Key];
            }
            RaisePropertyChanged(nameof(AllItems));
        }

        public void CreateData(Enqueite enqueite, IEnumerable<Question> qList)
        {
            foreach (var item in qList)
            {
                //連続値のみ
      //          if (item.AnswerType2 == AnswerType2.連続値)
                {
                    List<double> list = new List<double>();
                    foreach (var line in enqueite.AnswerLines)
                    {
                        var q = item.GetValue(line);
                        if (q == null)
                        {
                            list.Add(double.NaN);
                        }
                        else
                        {
                            list.Add(item.GetValue(line).Value);
                        }
                    }
                    DataList.Add(new KeyClusteringData() { Orignal = list.ToArray(), QuestionKey = item.Key });
                }
            }
            foreach (var item in DataList)
            {
                foreach (var item1 in DataList)
                {
                    item.Soukan.Add(new KeyValuePair<string, double>(item1.QuestionKey, soukan(item.Orignal, item1.Orignal)));
                }
            }
        }



        double soukan(double[] d1, double[] d2)
        {
            double s = 0;
            double d1_s = 0;
            double d2_s = 0;
            for (int i = 0; i < Math.Min(d1.Length, d2.Length); i++)
            {
                if (double.IsNaN(d1[i]) || double.IsNaN(d2[i]))
                {

                }
                else
                {
                    s = s + d1[i] * d2[i];
                    d1_s = d1_s + Math.Pow(d1[i], 2);
                    d2_s = d2_s + Math.Pow(d2[i], 2);
                }
            }
            return s / (Math.Sqrt(d1_s) * Math.Sqrt(d2_s));
        }



        public void Click(QuestionClusterItem questionClusterItem)
        {
            foreach (var item in Clusters.SelectMany(n => n.Items))
            {
                item.BackColor = ColorType.BackColor;
            }
            if (ClusterItemDic.ContainsKey(questionClusterItem.Question.Key))
            {
                var list = ClusterItemDic[questionClusterItem.Question.Key].Data.Soukan.OrderByDescending(n => n.Value).Skip(1).Take(Parent.ConnectNum).Select(n => n.Key).ToArray();
                int i = 0;
                foreach (var item in list)
                {
                    if (ClusterItemDic.ContainsKey(item))
                    {
                        if (i < Parent.ConnectNum / 2)
                        {
                            ClusterItemDic[item].BackColor = ColorType.RelationColor;
                        }
                        else
                        {
                            ClusterItemDic[item].BackColor = ColorType.RelationColor2;                        
                        }
                        i++;
                    }
                }
                ClusterItemDic[questionClusterItem.Question.Key].BackColor = ColorType.SeletedColor;
            }

        }

        RelayCommand deleteCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(() =>
                    {
                        Parent.DeleteResult(this);
                    });
                }
                return deleteCommand;
            }

            set
            {
                deleteCommand = value;
            }
        }

        RelayCommand selectCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand SelectCommand
        {
            get
            {
                if (selectCommand == null)
                {
                    selectCommand = new RelayCommand(() => { Parent.UpdateResult(this); });
                }
                return selectCommand;
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(this.Title);
            sb.AppendLine();
            foreach (var item in Clusters)
            {
                sb.AppendLine(item.Name);
                foreach (var item2 in item.Items)
                {
                    sb.AppendLine("\t" + item2.Question.ViewText);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public bool CanMove
        {
            get
            {
                return canMove;
            }

            set
            {
                canMove = value;
                RaisePropertyChanged(nameof(CanMove));
            }
        }

        internal QuestionClusterItem selectedClusterItem;
        internal void SetCurrntItem(QuestionClusterItem questionClusterItem)
        {
            selectedClusterItem = questionClusterItem;
        }

        public void MoveItem(QuestionCluster questionCluster)
        {
            if (selectedClusterItem != null)
            {
                if (CanMove)
                {
                    if (selectedClusterItem.Cluster != questionCluster)
                    {
                        selectedClusterItem.Cluster.Remove(selectedClusterItem);
                        questionCluster.Add(selectedClusterItem);
                        Task.Factory.StartNew(() => CountUser());
                    }
                }
            }
            selectedClusterItem = null;
        }
        MyWpf.ComboBoxEnumItem<質問追加の手法>[] questionAddMethod = MyWpf.EnumLib.MakeComboBoxEnum<質問追加の手法>().ToArray();
        [Newtonsoft.Json.JsonIgnore]
        public MyWpf.ComboBoxEnumItem<質問追加の手法>[] QuestionAddMethod { get {
                if(questionAddMethod == null)questionAddMethod = MyWpf.EnumLib.MakeComboBoxEnum<質問追加の手法>().ToArray();
                return questionAddMethod; } }
        MyWpf.ComboBoxEnumItem<質問追加の手法> selected質問追加の手法;
        public MyWpf.ComboBoxEnumItem<質問追加の手法> Selected質問追加の手法
        {
            get { return selected質問追加の手法; }
            set { selected質問追加の手法 = value; }
        }
       
        TaskScheduler ui
        {
            get {
                return MyLib.Task.Utility.UISyncContext;
                    }
        }

        RelayCommand addQuestionCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand AddQuestionCommand
        {
            get
            {
                if (addQuestionCommand == null)
                {
                    addQuestionCommand = new RelayCommand(async () =>
                    {
                        Func<Task> action = null;
                        switch (Selected質問追加の手法.Code)
                        {
                            case (質問追加の手法.単純加算):
                                action = () => AddQuestion単純加算();
                                break;
                            case (質問追加の手法.標準化加算):
                                action = () => AddQuestion標準化加算();
                                break;
                            case (質問追加の手法.標準化平均):
                                action = () => AddQuestion標準化平均();
                                break;
                            case (質問追加の手法.単純加算判別):
                                action = () => AddQuestion単純加算勝者();
                                break;
                            case (質問追加の手法.標準化加算判別):
                                action = () => AddQuestion標準化加算判別();
                                break;
                            case (質問追加の手法.標準化平均判別):
                                action = () => AddQuestion標準化平均判別();
                                break;                            

                            default:
                                break;
                        }
                       await action();
                    });
                }
                return addQuestionCommand;
            }
            set
            {
                addQuestionCommand = value;
            }
        }

        public async Task AddQuestion単純加算()
        {
            foreach (var item in Clusters)
            {
                Question q = new Question() { AnswerType = AnswerType.数値, AnswerType2 = AnswerType2.連続値, Key = "CQ_" + Title + "_" + item.Name, Text = "CQ単純加算" + Title + "_" + item.Name };
                var data_list = await Task<List<double>>.Run(() =>
                {
                    List<double> list = new List<double>();
                    foreach (var line in this.Parent.Enqueite.AnswerLines)
                    {
                        var v = item.Items.Select(n => n.Question.GetValue(line).GetValue()).Where(n => double.IsNaN(n) == false);
                        if (v.Any())
                        {
                            var v2 = v.Sum();
                            line.AddExtendColumn(q.Key, v2.ToString());
                            list.Add(v2);
                        }
                    }
                    return list;
                });

                this.Parent.Enqueite.QuestionManage.AddExtendQuestion(q);
                q.CreateQuestionAnswer(data_list);
            }
        }

        public async Task AddQuestion単純加算勝者()
        {
            Question q = new Question() { AnswerType = AnswerType.ラベル, AnswerType2 = AnswerType2.離散, Key = "CQ_" + Title, Text = "CQ単純加算判別_" + Title };

            await Task.Run(() =>
            {
                foreach (var line in this.Parent.Enqueite.AnswerLines)
                {
                    Dictionary<QuestionCluster, double> d = new Dictionary<QuestionCluster, double>();
                    foreach (var item in Clusters)
                    {
                        var v = item.Items.Select(n => n.Question.GetValue(line).GetValue()).Where(n => double.IsNaN(n) == false);
                        if (v.Any())
                        {
                            d.Add(item, v.Sum());
                        }
                    }
                    if (d.Any()) line.AddExtendColumn(q.Key, d.OrderByDescending(n => n.Value).First().Key.Name);
                }
            });
            this.Parent.Enqueite.QuestionManage.AddExtendQuestion(q);
            q.Answers = Clusters.Select(n => n.Name).ToList();
            q.Init();
        }


        public async Task CountUser()
        {

            var count_dic = await Task<Dictionary<QuestionCluster, int>>.Run(() =>
           {
               var dic = CreateQuestionStatisticsDic();
               Dictionary<QuestionCluster, int> countDic = new Dictionary<QuestionCluster, int>();
               foreach (var line in this.Parent.Enqueite.AnswerLines)
               {
                   Dictionary<QuestionCluster, double> d = new Dictionary<QuestionCluster, double>();
                   foreach (var item in Clusters)
                   {
                       if (item.Items.Count > 0)
                       {
                           var v = item.Items.Select(n => dic[n.Question].標準化(n.Question.GetValue(line).GetValue())).Where(n => double.IsNaN(n) == false);
                           if (v.Any())
                           {
                               d.Add(item, v.Average());
                           }
                       }
                   }
                   if (d.Any()) countDic.AddCount(d.OrderByDescending(n => n.Value).First().Key);
               }
               return countDic;
           });

            foreach (var item in Clusters)
            {
                item.CountStr = string.Empty;
            }
            foreach (var item in count_dic)
            {
                item.Key.CountStr = item.Value.ToString();
            }
        }

        public async Task AddQuestion標準化加算判別()
        {
            var question = await Task<Question>.Run(() =>
            {
                Question q = new Question() { AnswerType = AnswerType.ラベル, AnswerType2 = AnswerType2.離散, Key = "CQ_" + Title, Text = "CQ標準化加算判別_" + Title };
                Dictionary<Question, MyLib.Statistics.AvgStd> dic = CreateQuestionStatisticsDic();

                foreach (var line in this.Parent.Enqueite.AnswerLines)
                {
                    Dictionary<QuestionCluster, double> d = new Dictionary<QuestionCluster, double>();
                    foreach (var item in Clusters)
                    {
                        var v = item.Items.Select(n => dic[n.Question].標準化(n.Question.GetValue(line).GetValue())).Where(n => double.IsNaN(n) == false);
                        if (v.Any())
                        {
                            d.Add(item, v.Sum());
                        }
                    }
                    if (d.Any())
                    {
                        line.AddExtendColumn(q.Key, d.OrderByDescending(n => n.Value).First().Key.Name);
                    }
                }
                return q;
            });
            this.Parent.Enqueite.QuestionManage.AddExtendQuestion(question);
            question.Answers = Clusters.Select(n => n.Name).ToList();
            question.Init();
        }

        public async Task AddQuestion標準化平均判別()
        {
            var question = await Task<Question>.Run(() =>
            {
                Question q = new Question() { AnswerType = AnswerType.ラベル, AnswerType2 = AnswerType2.離散, Key = "CQ_" + Title, Text = "CQ標準化平均判別_" + Title };

                Dictionary<Question, MyLib.Statistics.AvgStd> dic = CreateQuestionStatisticsDic();
                foreach (var line in this.Parent.Enqueite.AnswerLines)
                {
                    Dictionary<QuestionCluster, double> d = new Dictionary<QuestionCluster, double>();
                    foreach (var item in Clusters)
                    {
                        var v = item.Items.Select(n => dic[n.Question].標準化(n.Question.GetValue(line).GetValue())).Where(n => double.IsNaN(n) == false);
                        if (v.Any())
                        {
                            d.Add(item, v.Average());
                        }
                    }
                    if (d.Any())
                    {
                        line.AddExtendColumn(q.Key, d.OrderByDescending(n => n.Value).First().Key.Name);
                    }
                }
                return q;
            });

            this.Parent.Enqueite.QuestionManage.AddExtendQuestion(question);
            question.Answers = Clusters.Select(n => n.Name).ToList();
            question.Init();
        }

        Dictionary<Question, MyLib.Statistics.AvgStd> questionStatisticsDic = null;

        /// <summary>
        /// 質問ごとに統計情報を作成する。
        /// </summary>
        /// <returns></returns>
        Dictionary<Question, MyLib.Statistics.AvgStd> CreateQuestionStatisticsDic()
        {
            if(questionStatisticsDic!=null )
            {
                return questionStatisticsDic;
            }
            Dictionary<Question, MyLib.Statistics.AvgStd> dic = new Dictionary<Question, MyLib.Statistics.AvgStd>();
            foreach (var line in this.Parent.Enqueite.AnswerLines)
            {
                foreach (var q in Clusters.SelectMany(n => n.Items).Select(n => n.Question))
                {
                    dic.GetValueOrAdd(q, new MyLib.Statistics.AvgStd()).Add(q.GetValue(line).GetValue());
                }
            }
            return dic;
        }

        public async Task AddQuestion標準化加算()
        {
            Dictionary<Question, MyLib.Statistics.AvgStd> dic = CreateQuestionStatisticsDic();

            foreach (var item in Clusters)
            {
                var (question,data_list) = await Task<(Question, List<double> )>.Run(() =>
                {
                    Question q = new Question() { AnswerType = AnswerType.数値, AnswerType2 = AnswerType2.連続値, Key = "CQ_" + Title + "_" + item.Name, Text = "CQ標準化加算_" + Title + "_" + item.Name };
                    List<double> list = new List<double>();
                    foreach (var line in this.Parent.Enqueite.AnswerLines)
                    {
                        var v = item.Items.Select(n => dic[n.Question].標準化(n.Question.GetValue(line).GetValue())).Where(n => double.IsNaN(n) == false);
                        if (v.Any())
                        {
                            line.AddExtendColumn(q.Key, v.Sum().ToString());
                            list.Add(v.Sum());
                        }
                    }
                    return (q,list);
                });

                this.Parent.Enqueite.QuestionManage.AddExtendQuestion(question);
                question.CreateQuestionAnswer(data_list);
            }
        }

        public async Task AddQuestion標準化平均()
        {
            Dictionary<Question, MyLib.Statistics.AvgStd> dic = CreateQuestionStatisticsDic();

            foreach (var item in Clusters)
            {
                var (question, data_list) = await Task<(Question, List<double>)>.Run(() =>
                {
                    List<double> list = new List<double>();
                    Question q = new Question() { AnswerType = AnswerType.数値, AnswerType2 = AnswerType2.連続値, Key = "CQ_" + Title + "_" + item.Name, Text = "CQ標準化平均_" + Title + "_" + item.Name };
                    foreach (var line in this.Parent.Enqueite.AnswerLines)
                    {
                        var v = item.Items.Select(n => dic[n.Question].標準化(n.Question.GetValue(line).GetValue())).Where(n => double.IsNaN(n) == false);
                        if (v.Any())
                        {
                            var v1 = v.Average();
                            line.AddExtendColumn(q.Key, v1.ToString());
                            list.Add(v1);
                        }
                    }
                    return (q, list);
                });
                this.Parent.Enqueite.QuestionManage.AddExtendQuestion(question);
                question.CreateQuestionAnswer(data_list);

            }
        }

        public bool IsDrag { get; set; } = false;

        RelayCommand upMouseCommand;
        [Newtonsoft.Json.JsonIgnore]
        RelayCommand UpMouseCommand
        {
            get { if(upMouseCommand == null)
                {
                    upMouseCommand = new RelayCommand(() => { IsDrag = false; SetCurrntItem(null); });
                }
                return upMouseCommand;
            }
        }

        internal void RightMove(QuestionCluster questionCluster)
        {
            Clusters.MoveBack(questionCluster);
        }

        internal void LeftMove(QuestionCluster questionCluster)
        {
            Clusters.MoveForward(questionCluster);
        }

        RelayCommand updateImageCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand UpdateImageCommand
        {
            get
            {
                if(updateImageCommand == null)
                {
                    updateImageCommand = new RelayCommand(() =>
                    {
                        foreach (var item in ClusterItemDic)
                        {
                            item.Value.UpdateImage();
                        }
                    });
                }
                return updateImageCommand;
            }
        }

        public List<ClusterLink> GetClusterRelation()
        {
            List<ClusterVec> list = new List<ClusterVec>();
            foreach (var item in this.Clusters)
            {
                list.Add(new ClusterVec() { Cluster = item, Vec = item.UserVec.ToArray() });
            }
            var list2 = list.ToArray();

            List<ClusterLink> list3 = new List<ClusterLink>();
            for (int i = 0; i < list2.Length-1; i++)
            {
                for (int l = 1; l < list2.Length; l++)
                {
                    if (i != l)
                    {
                        list3.Add(new ClusterLink() { Cluster1 = list2[i].Cluster, Cluster2 = list2[l].Cluster, Value = MyLib.Statistics.Correlation.Get(list2[i].Vec, list2[l].Vec) });
                    }
                }
            }
            return list3;
        }


        public struct ClusterVec
        {
            public QuestionCluster Cluster { get;set;}
            public double[] Vec
            {
                get; set;
            }
        }

        public struct ClusterLink
        {
            public QuestionCluster Cluster1 { get;set;}
            public QuestionCluster Cluster2 { get; set; }
            public double Value { get; set; }
        }
    }

    public enum 質問追加の手法
    {
        単純加算, 標準化加算, 標準化平均, 単純加算判別, 標準化加算判別, 標準化平均判別,
    }

    /// <summary>
    /// クラスターの縦の列
    /// </summary>
    
    public class QuestionCluster:GalaSoft.MvvmLight.ObservableObject
    {
        public void Init()
        {
            foreach (var item in Items)
            {
                item.Cluster = this;
                item.Manage = this.Manage;
                item.Init();
            }
        }
        
        public ObservableCollection<QuestionClusterItem> Items { get; set; } = new ObservableCollection<QuestionClusterItem>();
        [Newtonsoft.Json.JsonIgnore]
        public QuestionClusterManage Manage { get; set; }
        ColorType lineColor = ColorType.PrimaryColor;

        [Newtonsoft.Json.JsonIgnore]
        public List<double> UserVec
        {
            get
            {
                List<double> vec = new List<double>();
                foreach (var line in Manage.Parent.Enqueite.AnswerLines)
                {
                    double c = 0;
                    foreach (var item in Items.Select(n=>n.Question))
                    {
                        var q = item.GetValue(line);
                        if (q != null)
                        {
                            c += item.GetValue(line).Value;
                        }
                    }
                    vec.Add(c/Items.Count);
                }
                return vec;
            }
        } 

        RelayCommand mouseEnterCommand;
        RelayCommand mouseLeaveCommand;
        RelayCommand mouseUpCommand;
        string name = string.Empty;
        
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        string countStr = string.Empty;
        
        public string CountStr
        {
            get { return countStr; }
            set { countStr = value; RaisePropertyChanged(nameof(CountStr)); }
        }

        RelayCommand rightMoveCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand RightMoveCommand
        {
            get
            {
                if(rightMoveCommand == null)
                {
                    rightMoveCommand = new RelayCommand(() => { Manage.RightMove(this); });
                }
                return rightMoveCommand;
            }
        }

        RelayCommand leftMoveCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand LeftMoveCommand
        {
            get
            {
                if(leftMoveCommand == null)
                {
                    leftMoveCommand = new RelayCommand(() => { Manage.LeftMove(this); });
                }
                return leftMoveCommand;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand MouseEnterCommand
        {
            get
            {
                if (mouseEnterCommand == null)
                {
                    mouseEnterCommand = new RelayCommand(() => {
                        if (Manage.selectedClusterItem !=null)
                        {
                            LineColor = ColorType.SeletedColor;
                        }
                    });
                }
                return mouseEnterCommand;
            }

            set
            {
                mouseEnterCommand = value;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand MouseUpCommand
        {
            get
            {
                if(mouseUpCommand == null)
                {
                    mouseUpCommand = new RelayCommand(() => {
                        Manage.MoveItem(this);
                     
                    });
                }
                return mouseUpCommand;
            }

            set
            {
                mouseUpCommand = value;
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand MouseLeaveCommand
        {
            get
            {
                if(mouseLeaveCommand == null)
                {
                    mouseLeaveCommand = new RelayCommand(() => { LineColor = ColorType.PrimaryColor; });
                }
                return mouseLeaveCommand;
            }

            set
            {
                mouseLeaveCommand = value;
            }
        }

        public ColorType LineColor
        {
            get
            {
                return lineColor;
            }

            set
            {
                lineColor = value;
                RaisePropertyChanged(nameof(LineColor));
            }
        }

        public void Add(QuestionClusterItem item)
        {
            item.Cluster = this;
            Items.Add(item);
        }

        public void Remove(QuestionClusterItem item)
        {
            Items.Remove(item);
        }

    }


    /// <summary>
    /// クラスタ自体のクラス
    /// </summary>
    
    public class QuestionClusterItem : GalaSoft.MvvmLight.ObservableObject
    {
        
        public string Name { get; set; }
        
        public string QuestionKey { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Question Question { get; set; }
        
        public double[] Vector { get; set; }
        public QuestionCluster Cluster { get; set; }
        public QuestionClusterManage Manage { get; set; }
        
        public KeyClusteringData Data { get; set; }
        public string ImageUrl
        {
            get { return this.Question.ImageUrl; }
        }
        public void Init()
        {
            Question = Manage.Parent.Enqueite.QuestionManage.GetQuestion(this.QuestionKey);
            //ImageUrl = this.Question.ImageUrl;
        }

        public void UpdateImage()
        {
            RaisePropertyChanged(nameof(ImageUrl));
        }

        public static QuestionClusterItem Create(Question q,double[] vecter)
        {
            return new QuestionClusterItem() { Question = q, QuestionKey = q.Key,Vector = vecter,Name = q.ViewText };
        }

        ColorType backColor = ColorType.BackColor;
        public ColorType BackColor
        {
            get { return backColor; }
            set {  backColor = value; RaisePropertyChanged(nameof(BackColor)); }
        }

        //public void SetBackColor(ColorType c)
        //{
        //    BackColor = c;
        //}

   //     Image Image { get; set; }

        RelayCommand clickCommand;
        [Newtonsoft.Json.JsonIgnore]
        public RelayCommand Click
        {
            get
            {
                if(clickCommand == null)
                {
                    clickCommand = new RelayCommand(() =>
                    {
                        Manage.Click(this);
                        Manage.SetCurrntItem(this);
                    });
                }
                return clickCommand;
            }
        }

        double x = 0;
        double y = 0;
        public double X
        {
            get { return x; }
            set { x = value; RaisePropertyChanged(nameof(X)); }
        }
        public double Y
        {
            get { return y; }
            set { y = value;RaisePropertyChanged(nameof(Y)); }
        }

    }
}
