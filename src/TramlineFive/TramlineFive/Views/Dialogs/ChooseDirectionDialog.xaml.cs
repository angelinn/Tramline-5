using System;
using System.Collections.Generic;
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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Dialogs
{
    public sealed partial class ChooseDirectionDialog : ContentDialog
    {
        public ScheduleChooserViewModel ScheduleViewModel { get; private set; }

        public ChooseDirectionDialog(LineViewModel line)
        {
            this.InitializeComponent();
            this.ScheduleViewModel = new ScheduleChooserViewModel()
            {
                SelectedLine = line
            };

            this.DataContext = ScheduleViewModel;

            this.Transitions = AnimationManager.GeneratePageTransitions();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ScheduleViewModel.LoadChoosableData();
        }

        private void OnSecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!ScheduleViewModel.IsValid())
                args.Cancel = true;
        }
    }
}
