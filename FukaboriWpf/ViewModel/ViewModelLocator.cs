/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:FukaboriWpf"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using FukaboriCore.Model;
using FukaboriCore.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace FukaboriWpf.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SimpleSummaryViewModel>();
            SimpleIoc.Default.Register<GroupQuestionSumViewModel>();
            SimpleIoc.Default.Register<CrossDataViewModel>();

        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public Enqueite Enqueite => ServiceLocator.Current.GetInstance<MainViewModel>().Enqueite;

        public SimpleSummaryViewModel SimpleSummary => ServiceLocator.Current.GetInstance<SimpleSummaryViewModel>();

        public GroupQuestionSumViewModel GroupQuestionSum => ServiceLocator.Current.GetInstance<GroupQuestionSumViewModel>();

        public KeyWordSummary KeyWordSummary => ServiceLocator.Current.GetInstance<KeyWordSummary>();

        public CrossDataViewModel CrossData => ServiceLocator.Current.GetInstance<CrossDataViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}