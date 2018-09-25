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


        public void Submit(Question question,IEnumerable<Question> questions)
        {
            List<CrossData> list = new List<CrossData>();
            if (question == null) return;
            foreach (var item_1 in questions)
            {
                CrossData crossData = new CrossData();
                crossData.Create(item_1, question, Enqueite.Current.AnswerLines);
                list.Add(crossData);
            }
            DataList = list;
        }
        //#region Submit Command
        ///// <summary>
        ///// Gets the Submit.
        ///// </summary>
        //public RelayCommand<IEnumerable<Question>> SubmitCommand
        //{
        //    get { return _SubmitCommand ?? (_SubmitCommand = new RelayCommand<IEnumerable<Question>>((n) => {// Submit(n); })); }
        //}
        //private RelayCommand<IEnumerable<Question>> _SubmitCommand;
        //#endregion

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
