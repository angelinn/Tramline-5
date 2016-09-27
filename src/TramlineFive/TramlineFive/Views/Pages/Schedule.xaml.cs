using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    public sealed partial class Schedule : Page
    {
        public ScheduleChooserViewModel ScheduleChooserViewModel { get; private set; }
        public IList<StopViewModel> Stops { get; set; }

        public Schedule()
        {
            this.InitializeComponent();
            this.Stops = new ObservableCollection<StopViewModel>();

            this.DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ScheduleChooserViewModel = e.Parameter as ScheduleChooserViewModel;

            txtTitle.Text = $"{ScheduleChooserViewModel.SelectedLine.FriendlyName} - {ScheduleChooserViewModel.SelectedDirection.Name}";

            await ScheduleChooserViewModel.SelectedDay.LoadStops();
            foreach (StopViewModel stop in ScheduleChooserViewModel.SelectedDay.Stops)
                Stops.Add(stop);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StopViewModel selected = e.AddedItems.First() as StopViewModel;
            txtTimings.Text = String.Join(", ", selected.Timings);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }
    }
}
