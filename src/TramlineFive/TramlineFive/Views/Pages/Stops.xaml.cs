using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.Common.Managers;
using TramlineFive.ViewModels;
using TramlineFive.ViewModels.Wrappers;
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
    public sealed partial class Stops : Page
    {
        public StopsViewModel StopsViewModel { get; private set; }

        public Stops()
        {
            this.InitializeComponent();

            this.Transitions = AnimationManager.GeneratePageTransitions();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            StopsViewModel = new StopsViewModel(e.Parameter as ScheduleChooserViewModel);
            await StopsViewModel.LoadStops();

            DataContext = StopsViewModel;
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private void OnListViewItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(Timings), e.ClickedItem);
        }

        private void OnVirtualTableClick(object sender, RoutedEventArgs e)
        {
            (App.Current as App).AppViewModel.StopCode = ((sender as Button).DataContext as StopViewModel).Code;
            Frame.Navigate(typeof(MainPage), true);
        }
    }
}
