using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class Timings : Page
    {
        public TimingsViewModel TimingsViewModel { get; set; }

        public Timings()
        {
            this.InitializeComponent();
            this.TimingsViewModel = new TimingsViewModel();

            this.Transitions = AnimationManager.GeneratePageTransitions();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StopViewModel svm = e.Parameter as StopViewModel;
            TimingsViewModel.Update(svm.Name, svm.Timings);
            DataContext = TimingsViewModel;
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }
    }
}
