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

namespace TramlineFive.Views.Pages
{
    public sealed partial class Direction : Page
    {
        public ScheduleChooserViewModel ScheduleChooserViewModel { get; private set; }

        public Direction()
        {
            this.InitializeComponent();
            this.ScheduleChooserViewModel = new ScheduleChooserViewModel();

            this.DataContext = ScheduleChooserViewModel;

            this.Transitions = AnimationManager.GeneratePageTransitions();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.Loaded += OnLoaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!(e.Parameter as LineViewModel).Equals(ScheduleChooserViewModel.SelectedLine))
            {
                ScheduleChooserViewModel.SelectedLine = e.Parameter as LineViewModel;
                shouldReload = true;
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (shouldReload)
            {
                await ScheduleChooserViewModel.LoadChoosableData();
                shouldReload = false;
            }
        }

        private void OnSecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!ScheduleChooserViewModel.IsValid())
                args.Cancel = true;
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Stops), ScheduleChooserViewModel);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private bool shouldReload;
    }
}
