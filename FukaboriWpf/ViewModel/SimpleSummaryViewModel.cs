using FukaboriCore.Model;
using FukaboriCore.Service;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukaboriWpf.ViewModel
{
    public class SimpleSummaryViewModel:GalaSoft.MvvmLight.ViewModelBase
    {
        public IEnumerable<Question> SelectedQuestions { get { return _SelectedQuestions; } set { Set(ref _SelectedQuestions, value); } }
        private IEnumerable<Question> _SelectedQuestions = default(IEnumerable<Question>);


        public List<PropertyData> DataList { get { return _DataList; } set { Set(ref _DataList, value); } }
        private List<PropertyData> _DataList = default(List<PropertyData>);


        private void Submit(IEnumerable<Question> questions)
        {
            DataList = PropertyData.CreatePropertyData(questions, SimpleIoc.Default.GetInstance<MainViewModel>().Enqueite.AnswerLines).ToList();
        }
        #region Submit Command
        /// <summary>
        /// Gets the Submit.
        /// </summary>
        public RelayCommand<IEnumerable<Question>> SubmitCommand
        {
            get { return _SubmitCommand ?? (_SubmitCommand = new RelayCommand<IEnumerable<Question>>((n) => { Submit(n); })); }
        }
        private RelayCommand<IEnumerable<Question>> _SubmitCommand;
        #endregion

        bool sortFlag = true;

        List<PropertyData> SortDataList<T>(Func<PropertyData,T> func)
        {
            List<PropertyData> list;
            if (sortFlag)
            {
                list = DataList.OrderBy(func).ToList();
            }
            else
            {
                list = DataList.OrderByDescending(func).ToList();
            }
            sortFlag = !sortFlag;
            return list;
        }

        private void NameSort()
        {
            DataList = SortDataList(n => n.Name);
        }
        #region NameSort Command
        /// <summary>
        /// Gets the NameSort.
        /// </summary>
        public RelayCommand NameSortCommand
        {
            get { return _NameSortCommand ?? (_NameSortCommand = new RelayCommand(() => { NameSort(); })); }
        }
        private RelayCommand _NameSortCommand;
        #endregion


        private void AvgSort()
        {
            DataList = SortDataList(n => n.Average);
        }
        #region AvgSort Command
        /// <summary>
        /// Gets the AvgSort.
        /// </summary>
        public RelayCommand AvgSortCommand
        {
            get { return _AvgSortCommand ?? (_AvgSortCommand = new RelayCommand(() => { AvgSort(); })); }
        }
        private RelayCommand _AvgSortCommand;
        #endregion


        private void StdSort()
        {
            DataList = SortDataList(n => n.Std);
        }
        #region StdSort Command
        /// <summary>
        /// Gets the StdSort.
        /// </summary>
        public RelayCommand StdSortCommand
        {
            get { return _StdSortCommand ?? (_StdSortCommand = new RelayCommand(() => { StdSort(); })); }
        }
        private RelayCommand _StdSortCommand;
        #endregion



        private void ClipTsv()
        {
            MyLib.IO.TsvBuilder tsvBuilder = new MyLib.IO.TsvBuilder();
            foreach (var item in DataList)
            {
                item.ToTsv(tsvBuilder);
                tsvBuilder.NextLine();
            }
            SimpleIoc.Default.GetInstance<ISetClipBoardService>().SetTextWithMessage(tsvBuilder.ToString());
        }
        #region ClipTsv Command
        /// <summary>
        /// Gets the ClipTsv.
        /// </summary>
        public RelayCommand ClipTsvCommand
        {
            get { return _ClipTsvCommand ?? (_ClipTsvCommand = new RelayCommand(() => { ClipTsv(); })); }
        }
        private RelayCommand _ClipTsvCommand;
        #endregion

    }
}
