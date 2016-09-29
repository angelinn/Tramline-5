using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.Common.Managers;
using TramlineFive.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChooseSchedule : Page
    {
        public ScheduleChooserViewModel ScheduleViewModel { get; private set; }

        public ChooseSchedule()
        {
            this.InitializeComponent();
            this.ScheduleViewModel = new ScheduleChooserViewModel();

            this.DataContext = ScheduleViewModel;
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ScheduleViewModel.LoadChoosableData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ScheduleViewModel.UpdateFrom(e.Parameter as ScheduleChooserViewModel);
        }

        private void OnOpenScheduleClicked(object sender, RoutedEventArgs e)
        {
            if (ScheduleViewModel.IsValid())
                Frame.Navigate(typeof(Schedule), ScheduleViewModel);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }
    }
}
