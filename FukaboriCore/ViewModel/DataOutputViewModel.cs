using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using FukaboriCore.Service;
using FukaboriCore.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FukaboriCore.ViewModel
{
    public class DataOutputViewModel:GalaSoft.MvvmLight.ViewModelBase
    {
        public string Text { get { return _Text; } set { Set(ref _Text, value); } }
        private string _Text = default(string);

        public MappingDataType TextType { get { return _TextType; } set { Set(ref _TextType, value); } }
        private MappingDataType _TextType =  MappingDataType.TSV;

        public QuestionList SelectedQuestionList { get { return _SelectedQuestions; } set { Set(ref _SelectedQuestions, value); } }
        private QuestionList _SelectedQuestions = new QuestionList();

        public string GenereteText()
        {
            if(TextType == MappingDataType.Json)
            {
                return GenereteJson();
            }
            if(TextType == MappingDataType.TSV)
            {
                return GenereteTsv();
            }
            return string.Empty;
        }

        public string GenereteJson()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (var line in Enqueite.Current.AnswerLines)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                foreach (var question in SelectedQuestionList)
                {
                    var answer = question.GetValue(line);
                    if (answer.AnswerType == AnswerType.数値)
                    {
                        dic.Add(answer.QuestionText, answer.Value);
                    }
                    else if (question.AnswerType == AnswerType.タグ)
                    {
                        dic.Add(answer.QuestionText, question.GetValueList(line).Select(n => n.TextValue));
                    }
                    else
                    {
                        dic.Add(answer.QuestionText, answer.TextValue);
                    }
                }
                list.Add(dic);
            }
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        public string GenereteTsv()
        {
            MyLib.IO.TsvBuilder tsvBuilder = new MyLib.IO.TsvBuilder();
            foreach (var line in Enqueite.Current.AnswerLines)
            {
                foreach (var question in SelectedQuestionList)
                {
                    var answer = question.GetValue(line);
                    if (answer.AnswerType == AnswerType.数値)
                    {
                        tsvBuilder.Add(answer.QuestionText, answer.Value);
                    }
                    else if (question.AnswerType == AnswerType.タグ)
                    {
                        tsvBuilder.Add(answer.QuestionText, string.Join(",", question.GetValueList(line).Select(n => n.TextValue)));
                    }
                    else
                    {
                        tsvBuilder.Add(answer.QuestionText, answer.TextValue);
                    }
                }
                tsvBuilder.NextLine();
            }

            return tsvBuilder.ToString();
        }


        private void SubmitJson()
        {
            TextType = MappingDataType.Json;
            Text = GenereteText();
        }
        #region SubmitJson Command
        /// <summary>
        /// Gets the SubmitJson.
        /// </summary>
        public RelayCommand SubmitJsonCommand
        {
            get { return _SubmitJsonCommand ?? (_SubmitJsonCommand = new RelayCommand(() => { SubmitJson(); })); }
        }
        private RelayCommand _SubmitJsonCommand;
        #endregion


        private void Submit()
        {
            TextType = MappingDataType.TSV;
            Text = GenereteText();
        }
        #region Submit Command
        /// <summary>
        /// Gets the Submit.
        /// </summary>
        public RelayCommand SubmitCommand
        {
            get { return _SubmitCommand ?? (_SubmitCommand = new RelayCommand(() => { Submit(); })); }
        }
        private RelayCommand _SubmitCommand;
        #endregion

        private async Task FileSave()
        {
            if (TextType == MappingDataType.Json)
            {
                await SimpleIoc.Default.GetInstance<IFileService>().Save("", ".json", Text);
            }
            if (TextType == MappingDataType.TSV)
            {
               await SimpleIoc.Default.GetInstance<IFileService>().Save("", ".tsv", Text);
            }
        }
        #region FileSave Command
        /// <summary>
        /// Gets the FileSave.
        /// </summary>
        public RelayCommand FileSaveCommand
        {
            get { return _FileSaveCommand ?? (_FileSaveCommand = new RelayCommand(async () => { await FileSave(); })); }
        }
        private RelayCommand _FileSaveCommand;
        #endregion


        private void Clip()
        {
            SimpleIoc.Default.GetInstance<ISetClipBoardService>().SetTextWithMessage(Text);
        }
        #region Clip Command
        /// <summary>
        /// Gets the Clip.
        /// </summary>
        public RelayCommand ClipCommand
        {
            get { return _ClipCommand ?? (_ClipCommand = new RelayCommand(() => { Clip(); })); }
        }
        private RelayCommand _ClipCommand;
        #endregion


        public enum MappingDataType
        {
            Json,TSV
        }
    }
}