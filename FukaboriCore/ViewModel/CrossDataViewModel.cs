using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using FukaboriCore.Model;
using GalaSoft.MvvmLight.Command;

namespace FukaboriCore.ViewModel
{
    public class CrossDataViewModel:GalaSoft.MvvmLight.ViewModelBase
    {
        public List<CrossData> DataList { get { return _DataList; } set { Set(ref _DataList, value); } }
        private List<CrossData> _DataList = default(List<CrossData>);

        public void Save(System.IO.Stream stream)
        {
            using (var writer = new System.IO.StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(this));
            }
        }
        public static CrossDataViewModel Load(System.IO.Stream stream)
        {
            using (var reader = new System.IO.StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<CrossDataViewModel>(reader.ReadToEnd());
            }
        }

        public Question SelectedQuestion_1 { get { return _SelectedQuestion_1; } set { Set(ref _SelectedQuestion_1, value); } }
        private Question _SelectedQuestion_1 = null;


        public QuestionList SelectedQuestions_2 { get { return _SelectedQuestions_2; } set { Set(ref _SelectedQuestions_2, value); } }
        private QuestionList _SelectedQuestions_2 = new QuestionList();

        public bool TokkakeisuFlag { get { return _TokkakeisuFlag; } set { Set(ref _TokkakeisuFlag, value); } }
        private bool _TokkakeisuFlag = false;

        public bool IgnoreEmptyFlag { get { return _IgnoreEmptyFlag; } set { Set(ref _IgnoreEmptyFlag, value); } }
        private bool _IgnoreEmptyFlag = false;

        public bool ShowTotalRate { get { return _ShowTotalRate; } set { Set(ref _ShowTotalRate, value); } }
        private bool _ShowTotalRate = false;

        public bool ShowRowRate { get { return _ShowRowRate; } set { Set(ref _ShowRowRate, value); } }
        private bool _ShowRowRate = true;

        public bool ShowColumnRate { get { return _ShowColumnRate; } set { Set(ref _ShowColumnRate, value); } }
        private bool _ShowColumnRate = false;

        public void Submit(Question question,IEnumerable<Question> questions)
        {
            List<CrossData> list = new List<CrossData>();
            if (question == null) return;
            foreach (var item_1 in questions)
            {
                CrossData crossData = new CrossData();
                crossData.Create(item_1, question, Enqueite.Current.AnswerLines,IgnoreEmptyFlag);
                if(TokkakeisuFlag)
                {
                    CrossData allCrossData = new CrossData();
                    allCrossData.Create(item_1, question, Enqueite.Current.AllAnswerLine);
                    crossData.Compare(allCrossData);
                }

                list.Add(crossData);
            }
            DataList = list;
        }

        private void カイ二乗Sort()
        {
            if (DataList != null)
            {
                DataList = DataList.OrderByDescending(n => n.カイ２乗値).ToList();
            }
        }
        #region カイ二乗Sort Command
        /// <summary>
        /// Gets the カイ二乗Sort.
        /// </summary>
        public RelayCommand カイ二乗SortCommand
        {
            get { return _カイ二乗SortCommand ?? (_カイ二乗SortCommand = new RelayCommand(() => { カイ二乗Sort(); })); }
        }
        private RelayCommand _カイ二乗SortCommand;
        #endregion


    }

}
