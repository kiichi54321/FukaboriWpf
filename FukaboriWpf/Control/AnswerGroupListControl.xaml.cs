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
    /// AnswerGroupListControl.xaml の相互作用ロジック
    /// </summary>
    public partial class AnswerGroupListControl : UserControl
    {
        public AnswerGroupListControl()
        {
            InitializeComponent();
            ListBox.SelectionChanged += ListBox_SelectionChanged;
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            this.Loaded += AnswerGroupListControl_Loaded;
        }

        private void AnswerGroupListControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SimpleIoc.Default.IsRegistered<MainViewModel>())
            {
                SimpleIoc.Default.GetInstance<MainViewModel>().ChangeEnqueite += AnswerGroupListControl_ChangeEnqueite;
            }
        }

        private void AnswerGroupListControl_ChangeEnqueite(object sender, EventArgs e)
        {
            Enqueite.Current.PropertyChanged += AnswerGroupListControl_PropertyChanged;
            Enqueite.Current.QuestionListChanged += AnswerGroupListControl_CollectionChanged;
            this.ListBox.ItemsSource = FilterAnswerGroup(this.SearchTextBox.Text);
        }

        private void AnswerGroupListControl_CollectionChanged(object sender, EventArgs e)
        {
            this.ListBox.ItemsSource = FilterAnswerGroup(this.SearchTextBox.Text);
        }

        private void AnswerGroupListControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "" || e.PropertyName == "QuestionList")
            {
                this.ListBox.ItemsSource = FilterAnswerGroup(this.SearchTextBox.Text);
            }
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ListBox.ItemsSource = FilterAnswerGroup(SearchTextBox.Text);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedAnswerGroup = (AnswerGroup)ListBox.SelectedItem;
            this.SelectedAnswerGroupList = ListBox.SelectedItems.Cast<AnswerGroup>();
        }

        public SelectionMode SelectionMode
        {
            get { return ListBox.SelectionMode; }
            set { ListBox.SelectionMode = value; }
        }

        public AnswerGroup SelectedAnswerGroup
        {
            get { return (AnswerGroup)GetValue(SelectedAnswerGroupProperty); }
            set { SetValue(SelectedAnswerGroupProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedQuestion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedAnswerGroupProperty =
            DependencyProperty.Register("SelectedAnswerGroup", typeof(AnswerGroup), typeof(AnswerGroupListControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SelectedQuestionChangedCallback)));

        public static AnswerGroup GetSelectedAnswerGroup(DependencyObject dependencyObject)
        {
            return (AnswerGroup)dependencyObject.GetValue(SelectedAnswerGroupProperty);
        }

        public static void SetSelectedAnswerGroup(DependencyObject dependencyObject, object obj)
        {
            dependencyObject.SetValue(SelectedAnswerGroupProperty, obj);
        }

        private static void SelectedQuestionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public IEnumerable<AnswerGroup> SelectedAnswerGroupList
        {
            get { return (IEnumerable<AnswerGroup>)GetValue(SelectedAnswerGroupListProperty); }
            set { SetValue(SelectedAnswerGroupListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedQuestions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedAnswerGroupListProperty =
            DependencyProperty.Register("SelectedQuestionList", typeof(IEnumerable<AnswerGroup>), typeof(AnswerGroupListControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(SelectedQuestionsChangedCallback)));

        public static IEnumerable<AnswerGroup> GetSelectedAnswerGroupList(DependencyObject dependencyObject)
        {
            return (IEnumerable<AnswerGroup>)dependencyObject.GetValue(SelectedAnswerGroupListProperty);
        }

        public static void SetSelectedAnswerGroupList(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(SelectedAnswerGroupListProperty, value);
        }


        private static void SelectedQuestionsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        IEnumerable<AnswerGroup> AnswerGroupList
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>()?.Enqueite.AllAnswerGroupList; }
        }

        IEnumerable<AnswerGroup> FilterAnswerGroup(string searchText)
        {
            var words = searchText.Split(' ');
            if (words.Any())
            {
                return AnswerGroupList.Where(n => words.All(m => n.ViewText.Contains(m)));
            }
            return AnswerGroupList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            ListBox.UnselectAll();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ListBox.SelectAll();
        }

    
}
}
