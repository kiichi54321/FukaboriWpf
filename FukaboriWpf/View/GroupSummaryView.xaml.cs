using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FukaboriWpf.View
{
    public partial class GroupSummaryView : UserControl
    {
        public GroupSummaryView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void GroupSumSortButton_Click(object sender, RoutedEventArgs e)
        {
            ((FukaboriCore.ViewModel.GroupQuestionSumViewModel)this.DataContext).Sort(((Button)sender).Tag);

        }
    }
}
