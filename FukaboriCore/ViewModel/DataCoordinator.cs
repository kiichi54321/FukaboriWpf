using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Collections.Generic;
using FukaboriCore.ViewModel;
using FukaboriCore.Model;

namespace FukaboriCore.ViewModel
{
    /// <summary>
    /// データ調整のViewModel
    /// </summary>
    public class DataCoordinator:GalaSoft.MvvmLight.ViewModelBase
    {
        public Enqueite Enqueite { get; set; }
        public string ExtendAttributeName { get; set; } 
        public string ExtendValueName { get; set; }

        public void AddExtendValue()
        {
            var q = Enqueite.QuestionManage.GetQuestion("Ex_"+ ExtendAttributeName);
            foreach (var item in Enqueite.AnswerLines)
            {
                item.AddExtendColumn("Ex_" + ExtendAttributeName, ExtendValueName);
            }
            if (q == null)
            {
                Enqueite.QuestionManage.AddExtendQuestion( new Question() { AnswerType = AnswerType.ラベル, AnswerType2 = AnswerType2.離散, Key ="Ex_"+ ExtendAttributeName, Text = ExtendAttributeName });
                q = Enqueite.QuestionManage.GetQuestion("Ex_"+ ExtendAttributeName);
            }
            q.Answers = q.Answers.Union(new string[] { ExtendValueName }).ToList();
            q.Init();
        }
        RelayCommand addExtendValueCommand;
        public RelayCommand AddExtendValueCommand
        {
            get
            {
                if(addExtendValueCommand ==null)
                {
                    addExtendValueCommand = new RelayCommand(() => AddExtendValue());
                }
                return addExtendValueCommand;
            }
        }

    }
}
