using FukaboriCore.Model;
using GalaSoft.MvvmLight.Command;
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
        private int _ClusterNum = default(int);

        public int TryCount { get { return _TryCount; } set { Set(ref _TryCount, value); } }
        private int _TryCount = default(int);

        public QuestionList SelectedQuestions { get { return _SelectedQuestions; } set { Set(ref _SelectedQuestions, value); } }
        private QuestionList _SelectedQuestions = new QuestionList();

        MyLib.Analyze.K_MeansForTask K_MeansForTask;


        public MyLib.Analyze.K_MeansForTask.Cluster[] ClusterList { get { return _ClusterList; } set { Set(ref _ClusterList, value); } }
        private MyLib.Analyze.K_MeansForTask.Cluster[] _ClusterList = default(MyLib.Analyze.K_MeansForTask.Cluster[]);   



        private async Task Run()
        {
            K_MeansForTask = new MyLib.Analyze.K_MeansForTask(SelectedQuestions.Count);

            foreach (var item in Enqueite.Current.AnswerLines)
            {
                List<double> data = new List<double>();
                bool flag = true;
                foreach (var item2 in SelectedQuestions)
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
                    K_MeansForTask.AddData(data.ToArray(), item);
                }
            }

            ClusterList = (await K_MeansForTask.Run(ClusterNum, TryCount)).ToArray();

            

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


    }

    public class ClusterViewModel
    {

    }
}
