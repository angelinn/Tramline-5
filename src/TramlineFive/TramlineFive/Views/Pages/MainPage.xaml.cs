using TramlineFive.Common;
using TramlineFive.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using TramlineFive.DataAccess;
using Windows.UI.Xaml.Media;
using TramlineFive.Common.Models;
using TramlineFive.Views.Dialogs;
using TramlineFive.DataAccess.Entities;

using Newtonsoft.Json;
using TramlineFive.DataAccess.Repositories;
using TramlineFive.DataAccess.DomainLogic;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TramlineFive.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ArrivalViewModel> Arrivals { get; set; }
        public ObservableCollection<FavouriteDO> Favourites { get; set; }
        public VersionViewModel Version { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            Arrivals = new ObservableCollection<ArrivalViewModel>();
            Favourites = new ObservableCollection<FavouriteDO>();
            Version = new VersionViewModel();

            DataContext = this;
            NavigationCacheMode = NavigationCacheMode.Enabled;

            Loaded += MainPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            svMain.IsPaneOpen = false;
        }

        private async void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            e.Handled = true;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
            else
            {
                MessageDialog exitPrompt = new MessageDialog(Strings.PromptExit);
                exitPrompt.Commands.Add(new UICommand(Strings.Yes));
                exitPrompt.Commands.Add(new UICommand(Strings.No));

                IUICommand result = await exitPrompt.ShowAsync();
                if (result?.Label == Strings.Yes)
                    CoreApplication.Exit();
            }
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                await CopyDatabaseFileIfNeeded();
                await SetStatusBar();

                SumcManager.Load();
                foreach (FavouriteDO favourite in await FavouriteDO.AllAsync())
                    Favourites.Add(favourite);

                prFavourites.IsActive = false;
                prFavourites.Visibility = Visibility.Collapsed;
                loaded = true;
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
                IEnumerable<Arrival> arrivals = await SumcManager.GetByStopAsync(txtStopID.Text, new CaptchaDialog());

                if (arrivals?.Count() == 0)
                {
                    await new MessageDialog(Strings.NoResults).ShowAsync();
                    return;
                }

                foreach (Arrival arrival in arrivals ?? Enumerable.Empty<Arrival>())
                    Arrivals.Add(ArrivalViewModel.FromArrival(arrival));
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

        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase attached = FlyoutBase.GetAttachedFlyout(senderElement);
            attached.ShowAt(senderElement);
        }

        private async void btnFeedback_Click(object sender, RoutedEventArgs e)
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

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void btnSchedules_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Schedules));
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

        private async Task CopyDatabaseFileIfNeeded()
        {
            try
            {
                StorageFile dbFile =
                    await ApplicationData.Current.LocalFolder.TryGetItemAsync(TramlineFiveContext.DatabaseName) as StorageFile;

                if (dbFile == null)
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    Uri originalDbFileUri = new Uri($"ms-appx:///Assets/App_Data/{TramlineFiveContext.DatabaseName}");
                    StorageFile originalDbFile = await StorageFile.GetFileFromApplicationUriAsync(originalDbFileUri);

                    if (originalDbFile != null)
                    {
                        dbFile = await originalDbFile.CopyAsync(localFolder, TramlineFiveContext.DatabaseName, NameCollisionOption.ReplaceExisting);
                    }
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                await new MessageDialog(Strings.DatabaseNotFound).ShowAsync();
                throw ex;
            }
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            await FavouriteDO.Add(txtStopID.Text);
        }

        private async void spFavourite_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtStopID.Text = String.Format("{0:D4}", Int32.Parse((lvFavourites.SelectedItem as FavouriteDO).Code));
            btnStop_Click(this, new RoutedEventArgs());
            await Task.Delay(500);

            pvMain.SelectedIndex = 0;
        }

        private bool loading;
        private bool loaded;
    }
}
