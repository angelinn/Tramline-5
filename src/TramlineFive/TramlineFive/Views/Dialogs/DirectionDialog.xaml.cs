using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Dialogs
{
    public sealed partial class DirectionDialog : ContentDialog
    {
        public ObservableCollection<DirectionDO> Directions { get; set; }
        public ObservableCollection<DayDO> Days { get; set; }

        public DirectionDO SelectedDirection { get; set; }
        public DayDO SelectedDay { get; set; }

        public DirectionDialog(LineDO lineDO)
        {
            this.InitializeComponent();

            line = lineDO;
            Directions = new ObservableCollection<DirectionDO>();
            Days = new ObservableCollection<DayDO>();
            
            DataContext = this;
            Loaded += DirectionDialog_Loaded;
        }

        private async void DirectionDialog_Loaded(object sender, RoutedEventArgs e)
        {
            IsPrimaryButtonEnabled = false;

            await line.LoadDirections();
            foreach (DirectionDO dir in line.Directions)
                Directions.Add(dir);

            prDirections.IsEnabled = false;
            prDirections.Visibility = Visibility.Collapsed;

            IsPrimaryButtonEnabled = true;

            cbDays.Visibility = Visibility.Visible;
            cbDirections.Visibility = Visibility.Visible;
        }

        private async void cbDirections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbDays.IsEnabled = true;
            SelectedDirection = e.AddedItems.First() as DirectionDO;
            await SelectedDirection.LoadDays();

            foreach (DayDO day in SelectedDirection.Days)
                Days.Add(day);
        }

        private void cbDays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDay = e.AddedItems.First() as DayDO;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (SelectedDirection == null || SelectedDay == null)
                args.Cancel = true;
        }

        private LineDO line;
    }
}
