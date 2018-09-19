using FukaboriCore.Model;
using FukaboriCore.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FukaboriWpf.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IEnqueite
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            Enqueite = Enqueite.Current;
            ChangeEnqueite?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ChangeEnqueite;

        private async Task Save()
        {
            await SimpleIoc.Default.GetInstance<IFileService>().Save("", ".fukabori", 
                n => {
                    Enqueite.Save(n);
                });            
        }
        #region Save Command
        /// <summary>
        /// Gets the Save.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(async () => {await Save(); })); }
        }
        private RelayCommand _SaveCommand;
        #endregion


        private async Task Load()
        {
            this.Enqueite = await SimpleIoc.Default.GetInstance<IFileService>().Load("", ".fukabori",
                n => Task.Run<Enqueite>(() => Enqueite.Load(n)));
            ChangeEnqueite?.Invoke(this, EventArgs.Empty);
        }
        #region Load Command
        /// <summary>
        /// Gets the Load.
        /// </summary>
        public RelayCommand LoadCommand
        {
            get { return _LoadCommand ?? (_LoadCommand = new RelayCommand(async () => { await Load(); })); }
        }
        private RelayCommand _LoadCommand;
        #endregion


        private async Task LoadQuestions()
        {
            await SimpleIoc.Default.GetInstance<IFileService>().Load("", ".tsv",
                n => Task.Run(() => Enqueite.QuestionLoad(new System.IO.StreamReader(n) )));
            ChangeEnqueite?.Invoke(this, EventArgs.Empty);
        }
        #region LoadQuestions Command
        /// <summary>
        /// Gets the LoadQuestions.
        /// </summary>
        public RelayCommand LoadQuestionsCommand
        {
            get { return _LoadQuestionsCommand ?? (_LoadQuestionsCommand = new RelayCommand(async () => { await LoadQuestions(); })); }
        }
        private RelayCommand _LoadQuestionsCommand;
        #endregion


        private async Task DataLoad()
        {
            await SimpleIoc.Default.GetInstance<IFileService>().Load("", ".tsv",
              n => Task.Run(() => Enqueite.DataLoad(new System.IO.StreamReader(n))));
            ChangeEnqueite?.Invoke(this, EventArgs.Empty);
        }
        #region DataLoad Command
        /// <summary>
        /// Gets the DataLoad.
        /// </summary>
        public RelayCommand DataLoadCommand
        {
            get { return _DataLoadCommand ?? (_DataLoadCommand = new RelayCommand(async () => { await DataLoad(); })); }
        }
        private RelayCommand _DataLoadCommand;
        #endregion

        public Version Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public Enqueite Enqueite { get { return _Enqueite; } set { Set(ref _Enqueite, value); } }
        private Enqueite _Enqueite = default(Enqueite);

        public readonly static Brush DefaultBrush = new SolidColorBrush(Colors.White);
        public Brush CurrentBrush{ get; set; } = new SolidColorBrush( Colors.White);
        private void SetColor(Brush color)
        {
            CurrentBrush = color;
        }
        #region SetColor Command
        /// <summary>
        /// Gets the SetColor.
        /// </summary>
        public RelayCommand<Brush> SetColorCommand
        {
            get { return _SetColorCommand ?? (_SetColorCommand = new RelayCommand<Brush>((n) => { SetColor(n); })); }
        }
        private RelayCommand<Brush> _SetColorCommand;
        #endregion


    }
}