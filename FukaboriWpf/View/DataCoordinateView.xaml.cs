using FukaboriCore.Model;
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

namespace FukaboriWpf.View
{
    /// <summary>
    /// DataCoordinateView.xaml の相互作用ロジック
    /// </summary>
    public partial class DataCoordinateView : UserControl
    {
        public DataCoordinateView()
        {
            InitializeComponent();
        }

        Enqueite enqueite => (Enqueite)this.DataContext;

        private void listBox5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var question = listBox5.SelectedItem as Question;
            if (question != null)
            {
                var list = question.GetValue();
                questionNameTextBox.DataContext = question;
                AnswerGroupControl.ItemsSource = list;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //foreach (Model.QuestionAnswer item in listBox6.SelectedItems)
            //{
            //    enqueite.Add絞込(item);
            //}
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            enqueite.条件Clear();
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            selectedAnswer = border.Tag as QuestionAnswer;

        }

        QuestionAnswer selectedAnswer = null;
        private void AnswerGroupBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectedAnswer != null)
            {
                var border = sender as Border;
                border.Background = new SolidColorBrush(Colors.Purple) { Opacity = 0.4 };
            }
        }

        private void AnswerGroupBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Colors.White);
        }

        private void AnswerGroupBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedAnswer != null)
            {
                var border = sender as Border;
                var g = border.Tag as AnswerGroup;
                g.Move(selectedAnswer);
                selectedAnswer = null;
            }

        }

        #region データ調整関連
        private void AddOutAnswerBuntton_Click(object sender, RoutedEventArgs e)
        {
            var border = sender as Button;
            var g = border.Tag as AnswerGroup;
            enqueite.Add削除(g);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var border = sender as Button;
            var g = border.Tag as AnswerGroup;
            enqueite.Add絞込(g);

        }

        private void removeOutDataButton_Click(object sender, RoutedEventArgs e)
        {
            var answer = ((sender as Button).Tag) as AnswerGroup;
            enqueite.Remove削除(answer);
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            var answer = ((sender as Button).Tag) as AnswerGroup;
            enqueite.Remove絞込(answer);
        }
        #endregion

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            enqueite.DrawInData.Clear();
        }
    }
}
