using System;
using System.Net;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MySilverlightLibrary.Neural;
using System.Threading.Tasks;
using MyWpfLib.Extend;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using RawlerLib.MyExtend;
using System.Collections;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace FukaboriWpf.Model
{

    public class SimpleNeural : GalaSoft.MvvmLight.ViewModelBase2,IDisposableContainer
    {
        public override void Cleanup()
        {
            base.Cleanup();
            this.DisposableListClear();
        }

        public Enqueite Enqueite { get;set;}

        public ObservableCollection<Question> InputQuestion { get; set; } = new ObservableCollection<Question>();
        public ObservableCollection<Question> OutputQuestion { get; set; } = new ObservableCollection<Question>();

        public IList SelectedInputQuestion { get; set; } = new ObservableCollection<Question>();
        public Question SelectedOutputQuesttion { get; set; }

  //      public NotifyProperty<string> Result { get; set; }
        public NotifyProperty<int> HiddenNeuronCount { get; set; }
        public NotifyProperty<int> LoopCount { get; set; }
        public NotifyProperty<int> TabPage { get; set; }

        public ObservableCollection<SimpleNeuralResult> ResultList { get; set; }

        SimpleNeuralResult currentResult;
        public SimpleNeuralResult CurrentResult
        {
            get
            {
                return currentResult;
            }
            set
            {
                currentResult = value;
                RaisePropertyChanged(nameof(CurrentResult));
            }
        }


        RelayCommand searchQuestionCommand;
        public RelayCommand SearchQuestionCommand
        {
            get
            {
                if (searchQuestionCommand == null)
                {
                    searchQuestionCommand = new RelayCommand(() => InputQuestion.Update(Enqueite.QuestionManage.SearchQuestion(SearchText)));
                }
                return searchQuestionCommand;
            }
        }

        public string SearchText { get; set; }

        public SimpleNeural()
        {
            HiddenNeuronCount = new NotifyProperty<int>(5);
            LoopCount = new NotifyProperty<int>(5000);
            ResultList = new ObservableCollection<SimpleNeuralResult>();
            TabPage = new NotifyProperty<int>(0);

            EnqueiteData.Current.Subcribe((n) => {
                Enqueite = n;
                Init();
            }).RegistDisposableList(this);
            Messenger.Default.Register<QuestionChangeMessage>(this, (n) => { Init(); });
        }

        public void Init()
        {
            if (Enqueite != null)
            {
                InputQuestion.Update(Enqueite.QuestionList);
                OutputQuestion.Update(Enqueite.QuestionList);
            }
        }

        public void SelectResult(SimpleNeuralResult r)
        {
            CurrentResult = r;
            TabPage.Value = 1;
        }

        int modeCount = 1;
        private void Run()
        {
            SimpleNeuralResult r = new SimpleNeuralResult()
            {
                InputQuestion = SelectedInputQuestion.OfType<Question>().ToList(),
                OutputQuesetion = SelectedOutputQuesttion,
                HiddenNeuronCount = HiddenNeuronCount.Value,
                LoopCount = LoopCount.Value,
                Parent = this
            };
            r.ResultName.Value = "Model" + modeCount;
            CurrentResult = r;
            r.Run();
            ResultList.Add(r);
        }
        #region Run Command
        public RelayCommand RunCommand
        {
            get
            {
                if (_RunCommand == null)
                {
                    _RunCommand = new RelayCommand(() => Run());
                }
                return _RunCommand;
            }
        }

        public List<IDisposable> DisposableList { get; set; } = new List<IDisposable>();

        private RelayCommand _RunCommand;
        #endregion

        public void PageToModel()
        {
            TabPage.Value = 0;
        }

        public void PageToView()
        {
            TabPage.Value = 1;
        }



        private void Load()
        {
            var r = SimpleNeuralResult.Load();
            if (r != null)
            {
                r.Parent = this;
                CurrentResult = r;
                PageToView();
            }
        }
        #region Load Command
        public RelayCommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                {
                    _LoadCommand = new RelayCommand(() => Load());
                }
                return _LoadCommand;
            }
        }
        private RelayCommand _LoadCommand;
        #endregion




        private void CreateData()
        {
            SimpleNeuralResult r = new SimpleNeuralResult()
            {
                InputQuestion = SelectedInputQuestion.OfType<Question>().ToList(),
                OutputQuesetion = SelectedOutputQuesttion,
                HiddenNeuronCount = HiddenNeuronCount.Value,
                LoopCount = LoopCount.Value,
                Parent = this
            };

                var d = r.CreateDataSet();

            Clipboard.SetText(JObject.FromObject(d).ToString(Newtonsoft.Json.Formatting.Indented));


        }
        #region CreateData Command
        public RelayCommand CreateDataCommand
        {
            get
            {
                if (_CreateDataCommand == null)
                {
                    _CreateDataCommand = new RelayCommand(() => CreateData());
                }
                return _CreateDataCommand;
            }
        }
        private RelayCommand _CreateDataCommand;
        #endregion






    }

    [DataContract]
    public class SimpleNeuralResult:GalaSoft.MvvmLight.ViewModelBase2
    {
        [DataMember]
        public NotifyProperty<string> ResultName { get; set; }
        [DataMember]
        public NeuralNetworkCore NeuralCore { get; set; }
        [DataMember]
        public int HiddenNeuronCount { get; set; }
        public List<Question> InputQuestion { get; set; } = new List<Question>();
        public Question OutputQuesetion { get; set; }
        public List<InputData> InputData { get; set; }
        public SimpleNeural Parent { get; set; }

        public NotifyProperty<int> Progress { get; set; } = new NotifyProperty<int>(0);
        [DataMember]
        public ObservableCollection<SimpleInputNeural> InputView { get; set; } = new ObservableCollection<SimpleInputNeural>();
        [DataMember]
        public ObservableCollection<SimpleOutputNeural> OutputView { get; set; } = new ObservableCollection<SimpleOutputNeural>();
        [DataMember]
        public ObservableCollection<SimpleMiddelNeural> MiddelView { get; set; } = new ObservableCollection<SimpleMiddelNeural>();
        [DataMember]
        public int LoopCount { get; set; }

        public SimpleNeuralResult()
        {
            ResultName = new NotifyProperty<string>();
            Progress = new NotifyProperty<int>(0);
        }

        public void CreateViewData()
        {
            InputView.Clear();
            OutputView.Clear();
            MiddelView.Clear();

            foreach (var item in InputQuestion)
            {
                InputView.Add(new SimpleInputNeural() { Parent = this, Text = item.Text,ImageUrl= item.ImageUrl }.Init());
            }
            for (int i = 0; i < NeuralCore.count_unit_hidden; i++)
            {
                var m = new SimpleMiddelNeural() { }.Init();
                for (int l = 0;  l < NeuralCore.count_unit_in;  l++)
                {
                    m.inputWeightView.Add(new NeuralWeightDataView() { Text =InputQuestion[l].ViewText, Value = NeuralCore.weight_in_to_hidden[i, l].ToString("F5") });
                }
                for (int l = 0; l < NeuralCore.count_unit_out; l++)
                {
                    m.outputWeightView.Add(new NeuralWeightDataView() { Text = NeuralCore.OutputLabels[l], Value = NeuralCore.weight_hidden_to_out[l, i].ToString("F5") });
                }
                MiddelView.Add(m);
            }
            for (int i = 0; i < NeuralCore.count_unit_out; i++)
            {
                OutputView.Add(new SimpleOutputNeural() {  Text = NeuralCore.OutputLabels[i], }.Init());
            }      


        }

        public DataSet CreateDataSet()
        {
            DataSet dataset = new DataSet();
            var inputLabel = InputQuestion.Select(n => n.ViewText).ToArray();
            Func<double, double> f = (n) => { if (double.IsNaN(n)) return 0; else return n; };

            foreach (var item in Parent.Enqueite.AnswerLines)
            {
                InputData data = new InputData();

                List<double> d = new List<double>();
                foreach (var q in InputQuestion.OrderBy(n=>n.ViewText))
                {
                    d.Add(f( q.GetValue(item).Value));
                }
                data.Input = d.ToArray();
                data.TeacherLabel = new string[] { OutputQuesetion.GetValue(item).TextValue };
                dataset.List.Add(data);
            }
            dataset.InputLabels = inputLabel;
            dataset.CreateTeacherData();

            return dataset;
        }

        public void Run()
        {
            List<InputData> inputData = new List<InputData>();
            var inputLabel = InputQuestion.Select(n => n.ViewText).ToArray();
            foreach (var item in Parent.Enqueite.AnswerLines)
            {
                InputData data = new InputData();

                List<double> d = new List<double>();
                foreach (var q in InputQuestion)
                {
                    d.Add(q.GetValue(item).Value);
                }
                data.Input = d.ToArray();
                data.TeacherLabel = new string[] { OutputQuesetion.GetValue(item).TextValue };
            //    data.InputLabels = inputLabel;
                inputData.Add(data);
            }


            NeuralCore = NeuralNetworkCore.Create(inputData, HiddenNeuronCount);
            NeuralCore.InputLabels = InputQuestion.Select(n => n.ViewText).ToArray();

            Task.Factory.StartNew(() => {
                NeuralCore.Learn(LoopCount, (n) => Progress.Value = n);
                MyLib.Task.Utility.UITask(() => { CreateViewData(); });
            });
        }

        public void ForwardPropagation()
        {
            var d = InputView.Select(n => n.Checked.Value.ToDouble()).ToArray();
            var r = NeuralCore.GetForwardPropagation2(d);
            var h_max = r.Item1.Max();
            var h_min = r.Item1.Min();
            for (int i = 0; i < r.Item1.Length; i++)
            {
                MiddelView[i].OnColor(); 
                MiddelView[i].Value.Value = r.Item1[i];
                MiddelView[i].Opacity.Value = (r.Item1[i]-h_min)  / (h_max -h_min);
            }
            var o_max = r.Item2.Max();
            var o_min = r.Item2.Min();
            for (int i = 0; i < r.Item2.Length; i++)
            {
                OutputView[i].Value.Value = r.Item2[i];
                OutputView[i].Opacity.Value = (r.Item2[i]- o_min)  /( o_max-o_min);
                OutputView[i].OnColor();
            }
        }

        public static SimpleNeuralResult Load()
        {
            OpenFileDialog d = new OpenFileDialog();
            SimpleNeuralResult ct =null;
            if (d.ShowDialog()== true)
            {
                var stream = d.File.OpenRead();
                DataContractSerializer serializer = new DataContractSerializer(typeof(SimpleNeuralResult));

                ct = (SimpleNeuralResult)serializer.ReadObject(stream);
                foreach (var item in ct.InputView)
                {
                    item.Init();
                }
                foreach (var item in ct.MiddelView)
                {
                    item.Init();
                }
                foreach (var item in ct.OutputView)
                {
                    item.Init();
                }
                stream.Close();
            }
            return ct;
        }

        private void Save()
        {
            SaveFileDialog d = new SaveFileDialog();
            if(d.ShowDialog()== true)
            {
                var stream = d.OpenFile();
                DataContractSerializer serializer = new DataContractSerializer(typeof(SimpleNeuralResult));
                serializer.WriteObject(stream, this);
                stream.Close();
            }
        }
        #region Save Command
        public RelayCommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new RelayCommand(() => Save());
                }
                return _SaveCommand;
            }
        }
        private RelayCommand _SaveCommand;
        #endregion





        private void SelectResult()
        {
            this.Parent.SelectResult(this);
        }
        #region SelectResult Command
        public RelayCommand SelectResultCommand
        {
            get
            {
                if (_SelectResultCommand == null)
                {
                    _SelectResultCommand = new RelayCommand(() => SelectResult());
                }
                return _SelectResultCommand;
            }
        }
        private RelayCommand _SelectResultCommand;
        #endregion



        private void SaveJson()
        {
            SaveFileDialog d = new SaveFileDialog();
            if(d.ShowDialog()==true)
            {
                using (var f =  d.OpenFile())
                {
                    f.Write(System.Text.Encoding.UTF8.GetBytes(NeuralCore.ToJson()), 1024, 0);                   
                }
            }
        }
        #region SaveJson Command
        public RelayCommand SaveJsonCommand
        {
            get
            {
                if (_SaveJsonCommand == null)
                {
                    _SaveJsonCommand = new RelayCommand(() => SaveJson());
                }
                return _SaveJsonCommand;
            }
        }
        private RelayCommand _SaveJsonCommand;
        #endregion



    }

    public static class NeuralExted
    {
        public static double ToDouble(this bool flag)
        {
            if (flag) return 1;
            return 0;
        }
    }


    [DataContract]
    public class SimpleInputNeural : GalaSoft.MvvmLight.ViewModelBase2
    {
        static Brush OnBrush = new SolidColorBrush(Colors.Orange);
        static Brush OffBrush = new SolidColorBrush(Colors.White);

        [DataMember]
        public string Text{get;set;}
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public NotifyProperty<bool> Checked { get; set; }
       
        public NotifyProperty<Brush> BackBrush { get; set; }
        public SimpleNeuralResult Parent { get; set; }



        public SimpleInputNeural Init()
        {
            Checked = new NotifyProperty<bool>(false);
            Checked.Subcribe((n) => ChangeBGColor());
            BackBrush = new NotifyProperty<Brush>(OffBrush);
            return this;
        }

        void ChangeBGColor()
        {
            if(Checked.Value)
            {
                BackBrush.Value = OnBrush;
            }
            else
            {
                BackBrush.Value = OffBrush;
            }
        }

        private void Click()
        {
            Checked.Value = !Checked.Value;
            Parent.ForwardPropagation();
        }
        #region Click Command
        public RelayCommand ClickCommand
        {
            get
            {
                if (_ClickCommand == null)
                {
                    _ClickCommand = new RelayCommand(() => Click());
                }
                return _ClickCommand;
            }
        }
        private RelayCommand _ClickCommand;
        #endregion


    }

    [DataContract]
    public class SimpleMiddelNeural:SimpleOutputNeural
    {

        [DataMember]
        public ObservableCollection<NeuralWeightDataView> inputWeightView { get; set; }
        [DataMember]
        public ObservableCollection<NeuralWeightDataView> outputWeightView { get; set; }

        public SimpleMiddelNeural()
           : base()
        {
            inputWeightView = new ObservableCollection<NeuralWeightDataView>();
            outputWeightView = new ObservableCollection<NeuralWeightDataView>();
        }

        public new SimpleMiddelNeural Init()
        {
            base.Init();
            return this;
        }

        

    }
    [DataContract]
    public class NeuralWeightDataView : GalaSoft.MvvmLight.ViewModelBase2
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string Value { get; set; }
        public NotifyProperty<Brush> BackBrush { get; set; }
       
    }

    [DataContract]
    public class SimpleOutputNeural:GalaSoft.MvvmLight.ViewModelBase2
    {
//        static Color OnColor = Colors.Orange;
//        static Color OffColor = Colors.White;
       static Brush OnBrush = new SolidColorBrush(Colors.Orange);
        static Brush OffBrush = new SolidColorBrush(Colors.White);

   //     public Question Quesetion { get; set; }
        string text;
        [DataMember]
        public string Text
        {
            get { return text; }
            set { text = value;RaisePropertyChanged(nameof(Text)); }
        }
        public NotifyProperty<Brush> BackBrush { get; set; }
        [DataMember]
        public NotifyProperty<double> Opacity { get; set; }
        [DataMember]
        public NotifyProperty<double> Value { get; set; }

       

        public virtual SimpleOutputNeural Init()
        {
            BackBrush = new NotifyProperty<Brush>(OffBrush);
            Opacity = new NotifyProperty<double>(0);
        //    Opacity.Subcribe((n) => { var b = new SolidColorBrush(OnColor); b.Opacity = n; BackBrush.Value = b; });
            Value = new NotifyProperty<double>(0);
            return this;
        }

        public void OnColor()
        {
            BackBrush.Value = OnBrush;
        }
        public void OffColor()
        {
            BackBrush.Value = OffBrush;
        }
    }
}
