using FukaboriWpf.ViewModel;
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
    /// ClusteringViewModel.xaml の相互作用ロジック
    /// </summary>
    public partial class ClusteringView : UserControl
    {
        public ClusteringView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = (Border)sender;
            var brush = GalaSoft.MvvmLight.Ioc.SimpleIoc.Default.GetInstance<MainViewModel>().CurrentBrush;
            if (brush != border.Background)
            {
                border.Background = brush;
            }
            else
            {
                border.Background = MainViewModel.DefaultBrush;
            }
        }
    }
}
