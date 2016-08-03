using EasyBus.Common;
using EasyBus.Common.Exceptions;
using EasyBus.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EasyBus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ArrivalViewModel> Arrivals { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            Arrivals = new ObservableCollection<ArrivalViewModel>();
            DataContext = this;

            Loaded += MainPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        private async void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            e.Handled = true;

            if (!rootFrame.CanGoBack)
            {
                MessageDialog exitPrompt = new MessageDialog(Strings.PromptExit);
                exitPrompt.Commands.Add(new UICommand(Strings.Yes));
                exitPrompt.Commands.Add(new UICommand(Strings.No));

                IUICommand result = await exitPrompt.ShowAsync();
                if (result?.Label == Strings.Yes)
                    rootFrame.GoBack();
            }
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await SetStatusBar();
            await SumcManager.Load();
        }

        private async Task SetStatusBar()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                StatusBar statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Color.FromArgb(0, 51, 153, 255);
                    statusBar.ForegroundColor = Colors.White;

                    StatusBarProgressIndicator indicator = statusBar.ProgressIndicator;
                    await indicator.ShowAsync();
                    indicator.ProgressValue = 0;

                    indicator.Text = Strings.StatusBarText;
                }
            }
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (loading)
                return;

            loading = true;

            prVirtualTables.IsActive = true;
            prVirtualTables.Visibility = Visibility.Visible;

            try
            {
                Arrivals.Clear();
                IEnumerable<ArrivalViewModel> arrivals = await SumcManager.GetByStopAsync(txtStopID.Text);

                if (arrivals == null)
                {
                    await new MessageDialog(Strings.InvalidRequest).ShowAsync();
                    return;
                }

                if (arrivals?.Count() == 0)
                {
                    await new MessageDialog(Strings.NoResults).ShowAsync();
                    return;
                }


                foreach (ArrivalViewModel arrival in arrivals)
                    Arrivals.Add(arrival);

            }
            catch (Exception ex)
            {
                Arrivals.Add(new ArrivalViewModel { Direction = ex.Message });
            }
            finally
            {
                prVirtualTables.IsActive = false;
                prVirtualTables.Visibility = Visibility.Collapsed;
                loading = false;
            }
        }

        private void txtStopID_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                btnStop_Click(this, new RoutedEventArgs());
                InputPane.GetForCurrentView().TryHide();

                e.Handled = true;
            }
        }

        private void OnHamburgerClick(object sender, RoutedEventArgs e)
        {
            svMain.IsPaneOpen = !svMain.IsPaneOpen;
        }

        private async void spSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await new MessageDialog("Настройки!").ShowAsync();
        }

        private async void spFeedback_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog feedbackDialog = new MessageDialog(Strings.RequestOutlookRedirect);
            feedbackDialog.Commands.Add(new UICommand(Strings.Yes));
            feedbackDialog.Commands.Add(new UICommand(Strings.No));

            IUICommand result = await feedbackDialog.ShowAsync();
            if (result != null && result.Label == Strings.Yes)
            {
                Uri feedbackEmail = new Uri(Urls.DeveloperEmail);
                await Launcher.LaunchUriAsync(feedbackEmail);
            }
        }

        private void spInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private async void spSchedules_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await new MessageDialog("Разписания!").ShowAsync();
        }

        private async void btnSumc_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog sumcDialog = new MessageDialog(Strings.SumcRedirect);
            sumcDialog.Commands.Add(new UICommand(Strings.Yes));
            sumcDialog.Commands.Add(new UICommand(Strings.No));

            IUICommand result = await sumcDialog.ShowAsync();
            if (result != null && result.Label == Strings.Yes)
            {
                Uri sumc = new Uri(Urls.Sumc);
                await Launcher.LaunchUriAsync(sumc);
            }
        }

        private bool loading;
    }
}
