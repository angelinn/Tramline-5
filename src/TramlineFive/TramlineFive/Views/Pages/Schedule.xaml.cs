using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.DataAccess.DomainLogic;
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
        public ObservableCollection<StopDO> Stops { get; set; }

        public LineDO Line { get; set; }
        public DirectionDO Direction { get; set; }
        public DayDO Day { get; set; }

        public Schedule()
        {
            this.InitializeComponent();
            Stops = new ObservableCollection<StopDO>();

            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Dictionary<string, object> incoming = e.Parameter as Dictionary<string, object>;
            Direction = incoming["Direction"] as DirectionDO;
            Day = incoming["Day"] as DayDO;
            Line = incoming["Line"] as LineDO;

            txtTitle.Text = $"{Line} - {Direction.Name}";

            await Day.LoadStops();
            foreach (StopDO stop in Day.Stops)
                Stops.Add(stop);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StopDO selected = e.AddedItems.First() as StopDO;
            txtTimings.Text = String.Join(", ", selected.Timings);
        }
    }
}
