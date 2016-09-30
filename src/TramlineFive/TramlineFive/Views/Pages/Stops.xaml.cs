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
        public ScheduleChooserViewModel ScheduleChooserViewModel { get; private set; }
        public IList<StopViewModel> LineStops { get; private set; }

        public Stops()
        {
            this.InitializeComponent();
            this.LineStops = new ObservableCollection<StopViewModel>();

            this.Transitions = AnimationManager.GeneratePageTransitions();
            this.DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ScheduleChooserViewModel = e.Parameter as ScheduleChooserViewModel;

            txtTitle.Text = $"{ScheduleChooserViewModel.SelectedLine.ShortName} - {ScheduleChooserViewModel.SelectedDirection.Name.ToUpper()}";

            ScheduleChooserViewModel.SelectedDay = ScheduleChooserViewModel.SelectedDirection.Days.Where(d => d.Type == ScheduleChooserViewModel.SelectedDay.Type).First();
            await ScheduleChooserViewModel.SelectedDay.LoadStops();
            foreach (StopViewModel stop in ScheduleChooserViewModel.SelectedDay.Stops)
                LineStops.Add(stop);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private void OnListViewItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(Schedule), (e.ClickedItem as StopViewModel).Timings);
        }

        private void OnVirtualTableClick(object sender, RoutedEventArgs e)
        {
            (App.Current as App).AppViewModel.StopCode = String.Format("{0:D4}", Int32.Parse(((sender as Button).DataContext as StopViewModel).Code));
            Frame.Navigate(typeof(MainPage), true);
        }
    }
}
