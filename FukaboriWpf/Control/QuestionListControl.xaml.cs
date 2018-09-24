using FukaboriCore.Model;
using FukaboriWpf.ViewModel;
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
            this.Loaded += QuestionListControl_Loaded;
        }

        private void QuestionListControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SimpleIoc.Default.IsRegistered<MainViewModel>())
            {
                SimpleIoc.Default.GetInstance<MainViewModel>().ChangeEnqueite += QuestionListControl_ChangeEnqueite;
            }
        }

        private void QuestionListControl_ChangeEnqueite(object sender, EventArgs e)
        {
            Enqueite.Current.PropertyChanged += QuestionListControl_PropertyChanged;
            Enqueite.Current.QuestionListChanged += QuestionList_CollectionChanged;
            this.ListBox.ItemsSource = FilterQuestions(this.SearchTextBox.Text);
        }

        private void QuestionList_CollectionChanged(object sender, EventArgs e)
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
            DependencyProperty.Register("SelectedQuestion", typeof(Question), typeof(QuestionListControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SelectedQuestionChangedCallback)));

        public static Question GetSelectedQuestion(DependencyObject dependencyObject)
        {
            return (Question)dependencyObject.GetValue(SelectedQuestionProperty);
        }

        public static void SetSelectedQuestion(DependencyObject dependencyObject,object obj)
        {
            dependencyObject.SetValue(SelectedQuestionProperty, obj);
        }

        private static void SelectedQuestionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public IEnumerable<Question> SelectedQuestions
        {
            get { return (IEnumerable<Question>)GetValue(SelectedQuestionsProperty); }
            set { SetValue(SelectedQuestionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedQuestions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedQuestionsProperty =
            DependencyProperty.Register("SelectedQuestions", typeof(IEnumerable<Question>), typeof(QuestionListControl), new FrameworkPropertyMetadata(Enumerable.Empty<Question>(),new PropertyChangedCallback(SelectedQuestionsChangedCallback)));

        public static IEnumerable<Question> GetSelectedQuestions(DependencyObject dependencyObject)
        {
            return (IEnumerable<Question>)dependencyObject.GetValue(SelectedQuestionsProperty);
        }



        private static void SelectedQuestionsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        IEnumerable<Question> QuestionList
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>()?.Enqueite.QuestionList; }
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
