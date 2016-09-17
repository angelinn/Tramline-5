using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.Common.Managers;
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

        public ScheduleViewModel ScheduleViewModel { get; private set; }

        public DirectionDialog(ScheduleViewModel scheduleViewModel)
        {
            this.InitializeComponent();

            ScheduleViewModel = scheduleViewModel;
            
            DataContext = this;
            Loaded += DirectionDialog_Loaded;
        }

        private async void DirectionDialog_Loaded(object sender, RoutedEventArgs e)
        {
            IsPrimaryButtonEnabled = false;

            await ScheduleViewModel.LoadChoosableData();

            lvDirections.SelectedIndex = 0;
            lvDays.SelectedIndex = 0;

            UIManager.DisableControl(prDirections);

            IsPrimaryButtonEnabled = true;

            UIManager.ShowControl(lvDirections);
            UIManager.ShowControl(lvDays);
        }

        private void cbDirections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScheduleViewModel.SelectedDirection = e.AddedItems.First() as DirectionDO;
        }

        private void cbDays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScheduleViewModel.SelectedDay = e.AddedItems.First() as DayDO;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!ScheduleViewModel.IsValid())
                args.Cancel = true;
        }
    }
}
