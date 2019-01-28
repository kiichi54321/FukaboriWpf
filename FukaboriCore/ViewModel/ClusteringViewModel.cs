using FukaboriCore.Model;
using FukaboriCore.Service;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MyLib.Analyze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukaboriCore.ViewModel
{
    public class ClusteringViewModel:GalaSoft.MvvmLight.ViewModelBase
    {
        public int ClusterNum { get { return _ClusterNum; } set { Set(ref _ClusterNum, value); } }
        private int _ClusterNum = 6;

        public int TryCount { get { return _TryCount; } set { Set(ref _TryCount, value); } }
        private int _TryCount = 10;

        public QuestionList SelectedQuestions { get { return _SelectedQuestions; } set { Set(ref _SelectedQuestions, value); } }
        private QuestionList _SelectedQuestions = new QuestionList();

        K_MeansForTask K_MeansForTask;

        public ClusterViewModel[] ClusterList { get { return _ClusterList; } set { Set(ref _ClusterList, value); } }
        private ClusterViewModel[] _ClusterList = default(ClusterViewModel[]);

        public string ClusteringTitle { get { return _Title; } set { Set(ref _Title, value); } }
        private string _Title = "クラスタ_1";

        public int DefaultValue { get { return _DefaultValue; } set { Set(ref _DefaultValue, value); } }
        private int _DefaultValue = default(int);

        double ConvertValue(double value)
        {
            double v = DefaultValue;            
            if (!double.IsNaN(value))
            {
                v = value;
            }
            return v;
        }


        private async Task Run()
        {
            K_MeansForTask = new MyLib.Analyze.K_MeansForTask(SelectedQuestions.Count);
            List<(double[] data, MyLib.IO.TSVLine line)> lineList = new List<(double[], MyLib.IO.TSVLine)>();

            foreach (var item in Enqueite.Current.AnswerLines)
            {
                List<double> data = new List<double>();
                bool flag = true;
                foreach (var item2 in SelectedQuestions)
                {
                    var a = item2.GetValue(item);
                    if (a != null)
                    {
                        if (a.IsTextValue() == false)
                        {
                            data.Add(ConvertValue(a.Value));
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    K_MeansForTask.AddData(data.ToArray(), item);
                    lineList.Add((data.ToArray(), item));
                }
            }

            ClusterList = (await K_MeansForTask.Run(ClusterNum, TryCount))
                            .OrderBy(n => n.Data.Sum())
                            .Select((n, i) => new ClusterViewModel() { Cluster = n, Id = i, Name = $"cluster_{i}" })
                            .ToArray();

            var lineDic = lineList.AsParallel()
                .Select(n => new { n.line, cluster = K_MeansForTask.SearchCluster(n.data) })
                .ToLookup(n => n.cluster,n=>n.line);

            foreach (var item in ClusterList)
            {
                if (lineDic[item.Cluster] != null)
                {
                    item.Lines = lineDic[item.Cluster].ToList();
                    item.GenerateProperties(SelectedQuestions);
                }
            }


        }
        #region Run Command
        /// <summary>
        /// Gets the Run.
        /// </summary>
        public RelayCommand RunCommand
        {
            get { return _RunCommand ?? (_RunCommand = new RelayCommand(async () => {await Run(); })); }
        }
        private RelayCommand _RunCommand;
        #endregion


        private async Task AddQuestion()
        {
            var q = Enqueite.Current.QuestionManage.GetQuestion(this.ClusteringTitle);
            if(q is null)
            {
                q = new Question() { AnswerType = AnswerType.離散, AnswerType2 = AnswerType2.離散, Key = ClusteringTitle, Text = ClusteringTitle };
                Enqueite.Current.QuestionManage.AddExtendQuestion(q);
            }
            else
            {
                if( q.IsExtendQuestion ==false)
                {
                    SimpleIoc.Default.GetInstance<IShowMessageService>().Show("クラスタの名前が既存の列と同じです。", "名前変更してください");
                    return;
                }
                else
                {
                    var result = await SimpleIoc.Default.GetInstance<IShowMessageService>().ShowAsync("すでにある名前です。上書きしますか", "上書きしますか？", MessageBoxType.YesNo);
                    if(result == false)
                    {
                        return;
                    }
                }
            }

            q.Answers = this.ClusterList.Select(n => $"{n.Id}:{n.Name}").ToList();

            q.Init();

            foreach (var cluster in ClusterList)
            {
                foreach (var line in cluster.Lines)
                {
                    line.AddExtendColumn(q.Key, cluster.Id.ToString());
                }
            }
            SimpleIoc.Default.GetInstance<IShowMessageService>().Show($"「{ClusteringTitle}」を追加しました", "完了");
        }
        #region AddQuestion Command
        /// <summary>
        /// Gets the AddQuestion.
        /// </summary>
        public RelayCommand AddQuestionCommand
        {
            get { return _AddQuestionCommand ?? (_AddQuestionCommand = new RelayCommand(async () => {await AddQuestion(); })); }
        }
        private RelayCommand _AddQuestionCommand;
        #endregion



    }

    public class ClusterViewModel:GalaSoft.MvvmLight.ObservableObject
    {
        public string Name { get { return _Name; } set { Set(ref _Name, value); } }
        private string _Name = default(string);
        public int Id { get; set; }

        public K_MeansForTask.Cluster Cluster { get; set; }

        public List<MyLib.IO.TSVLine> Lines { get; set; }
        public int Count => Lines.Count;

        public PropertyData[] Properties { get; set; }

        public void GenerateProperties(IEnumerable<Question> questions)
        {
            Properties = PropertyData.CreatePropertyData(questions, this.Lines).ToArray();
        }
    }
}
