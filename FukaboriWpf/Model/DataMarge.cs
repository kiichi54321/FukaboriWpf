using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RawlerLib.MyExtend;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections;
using System.Runtime.Serialization;
using MyWpfLib.Extend;

namespace CrossTableSilverlight.Model
{
    public class DataMarge:GalaSoft.MvvmLight.ViewModelBase
    {
        public ObservableCollection<string> Header { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> KeyHeader { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Question> QuestionList { get; set; } = new ObservableCollection<Model.Question>();

        public string SelectedKey { get; set; }
        public IList SelectedHeader { get; set; }
        public Question SelectedKeyQuestion { get; set; }
        public NotifyProperty<string> InputText { get; set; }
//        public NotifyProperty<string> SelectedFileName { get; set; } = new NotifyProperty<string>();

        public void Init()
        {
            Header = new ObservableCollection<string>();
            KeyHeader = new ObservableCollection<string>();
            QuestionList = new ObservableCollection<Model.Question>();
            QuestionList.SetList(Enqueite.QuestionList);
            InputText = new NotifyProperty<string>(string.Empty);
            SelectedHeader = new ObservableCollection<string>();
        }
  
        public Enqueite Enqueite { get; set; }




        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                InputText.Value = openFileDialog.File.OpenText().ReadToEnd();
            }
        }

        

        public void FileSelect()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog()== true)
            {
                 using (var f = openFileDialog.File.OpenText())
                {
                    var h = f.ReadLines().First().Split('\t').Where(n => n.Length > 0).ToArray();
                    Header.SetList(h);
                    KeyHeader.SetList(h);
                }
            }
        }
        
        
        RelayCommand fileSelectCommand;
        public RelayCommand FileSelectCommand
        {
            get { if(fileSelectCommand == null)
                {
                    fileSelectCommand = new RelayCommand(() => { FileSelect(); });
                }
                return fileSelectCommand;
            }
        }
        RelayCommand margeCommand;
        public RelayCommand MargeCommand
        {
            get
            {
                if(margeCommand == null)
                {
                    margeCommand = new RelayCommand(() => Marge());
                }
                return margeCommand;
            }
        }




        public RelayCommand FileOpenCommand
        {
            get
            {
                if (_FileOpenCommand == null)
                {
                    _FileOpenCommand = new RelayCommand(() => ExecFileOpenCommand());
                }
                return _FileOpenCommand;
            }
        }
        private RelayCommand _FileOpenCommand;

        /// <summary>
        /// Executes the FileOpenCommand command 
        /// </summary>
        private void ExecFileOpenCommand()  
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                InputText.Value = openFileDialog.File.OpenText().ReadToEnd();
            }
        }


        public RelayCommand CreateHeaderCommand
        {
            get
            {
                if (_CreateHeaderCommand == null)
                {
                    _CreateHeaderCommand = new RelayCommand(() => ExecCreateHeaderCommand());
                }
                return _CreateHeaderCommand;
            }
        }
        private RelayCommand _CreateHeaderCommand;

        /// <summary>
        /// Executes the CreateHeaderCommand command 
        /// </summary>
        private void ExecCreateHeaderCommand()
        {
            var l = InputText.Value.ReadLines().First().Split('\t').ToArray();

            Header.SetList(l);
            KeyHeader.SetList(l);
        }



        private void ReadFirstLine()
        {
            if (InputText.Value.Length > 0)
            {
                var l = InputText.Value.ReadLines().First().Split('\t').ToArray();

                Header.SetList(l);
                KeyHeader.SetList(l);
            }
        }
        #region ReadFirstLine Command
        public RelayCommand ReadFirstLineCommand
        {
            get
            {
                if (_ReadFirstLineCommand == null)
                {
                    _ReadFirstLineCommand = new RelayCommand(() => ReadFirstLine());
                }
                return _ReadFirstLineCommand;
            }
        }
        private RelayCommand _ReadFirstLineCommand;
        #endregion



        public void Marge()
        {

            MyLib.IO.TSVText tsv = new MyLib.IO.TSVText(InputText.Value);

            Dictionary<string, List<string>> dataDic = new Dictionary<string, List<string>>();
            Dictionary<string, Dictionary<string, string>> keyDataDic = new Dictionary<string, Dictionary<string, string>>();
            var sHeader = SelectedHeader.OfType<string>();
            foreach (var item in tsv.Lines)
            {
                var key = item.GetValue(SelectedKey, string.Empty);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var h in sHeader)
                {
                    dataDic.AddList(h, item.GetValue(h, null));
                    dic.Add(h, item.GetValue(h, null));
//                    keyDataDic.GetValueOrAdd(key, item.GetValue(h, string.Empty));
                }
                keyDataDic.GetValueOrAdd(key, dic);
            }

            foreach (var item in dataDic)
            {
                var q = Question.Create("Add_" + item.Key, item.Value);
            
                foreach (var line in Enqueite.AllAnswerLine)
                {
                    var key = SelectedKeyQuestion.GetValue(line).TextValue;
                    if( keyDataDic.ContainsKey(key))
                    {
                        line.AddExtendColumn(q.Key, keyDataDic[key][item.Key]);
                    }
                }
                if (q.AnswerType != AnswerType.数値)
                {
                    q.Answers = item.Value.Distinct().ToList();
                }
                Enqueite.QuestionManage.AddExtendQuestion(q);
                q.CreateQuestionAnswer(Enqueite.AllAnswerLine);
            }
            MessageBox.Show("完了");
            

            
        }
        
    }
}
