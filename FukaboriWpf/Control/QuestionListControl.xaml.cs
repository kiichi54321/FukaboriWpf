﻿using FukaboriCore.Model;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FukaboriWpf.Control
{
    /// <summary>
    /// QuestionListControl.xaml の相互作用ロジック
    /// </summary>
    public partial class QuestionListControl : UserControl
    {
        public QuestionListControl()
        {
            InitializeComponent();
            ListBox.SelectionChanged += ListBox_SelectionChanged;
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            this.DataContextChanged += QuestionListControl_DataContextChanged;
            this.Loaded += QuestionListControl_Loaded;
        }

        private void QuestionListControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SimpleIoc.Default.IsRegistered<IEnqueite>())
            {
                SimpleIoc.Default.GetInstance<IEnqueite>().ChangeEnqueite += QuestionListControl_ChangeEnqueite;
            }
        }

        private void QuestionListControl_ChangeEnqueite(object sender, EventArgs e)
        {
            this.ListBox.ItemsSource = FilterQuestions(this.SearchTextBox.Text);
        }

        private void QuestionListControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ListBox.ItemsSource = FilterQuestions(this.SearchTextBox.Text);
            ((Enqueite)DataContext).PropertyChanged += QuestionListControl_PropertyChanged;
            ((Enqueite)DataContext).QuestionList.CollectionChanged += QuestionList_CollectionChanged;
        }

        private void QuestionList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.ListBox.ItemsSource = FilterQuestions(this.SearchTextBox.Text);
        }

        private void QuestionListControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == "QuestionList")
            {
                this.ListBox.ItemsSource = FilterQuestions(this.SearchTextBox.Text);
            }
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ListBox.ItemsSource = FilterQuestions(SearchTextBox.Text);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedQuestion = (Question)ListBox.SelectedItem;
            this.SelectedQuestions = ListBox.SelectedItems.Cast<Question>();
        }

        public SelectionMode SelectionMode
        {
            get { return ListBox.SelectionMode; }
            set { ListBox.SelectionMode = value; }
        }

        public Question SelectedQuestion
        {
            get { return (Question)GetValue(SelectedQuestionProperty); }
            set { SetValue(SelectedQuestionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedQuestion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedQuestionProperty =
            DependencyProperty.Register("SelectedQuestion", typeof(Question), typeof(QuestionListControl), new PropertyMetadata(null));



        public IEnumerable<Question> SelectedQuestions
        {
            get { return (IEnumerable<Question>)GetValue(SelectedQuestionsProperty); }
            set { SetValue(SelectedQuestionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedQuestions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedQuestionsProperty =
            DependencyProperty.Register("SelectedQuestions", typeof(IEnumerable<Question>), typeof(QuestionListControl), new PropertyMetadata(Enumerable.Empty<Question>()));



        IEnumerable<Question> QuestionList
        {
            get { return ((Enqueite)DataContext).QuestionList; }
        }

        IEnumerable<Question> FilterQuestions(string searchText)
        {
            var words = searchText.Split(' ');
            if (words.Any())
            {
                return QuestionList.Where(n => words.All(m => n.ViewText.Contains(m)));
            }
            return QuestionList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
        }
    }
}