using TramlineFive.Common;
using TramlineFive.ViewModels;
using System;
using System.Collections.Generic;
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
using TramlineFive.Views.Dialogs;
using Windows.ApplicationModel.Core;
using TramlineFive.ViewModels.Wrappers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TramlineFive.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public HomeViewModel HomeViewModel { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();

            this.HomeViewModel = new HomeViewModel();

            this.DataContext = HomeViewModel;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.Loaded += OnLoaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        // Prevents panel being invisibly open on other pages, causing double back click needed to go back
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            svMain.IsPaneOpen = false;
        }

        private async void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            e.Handled = true;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
            else
                await new QuestionDialog(Strings.PromptExit, () => CoreApplication.Exit()).ShowAsync();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await HomeViewModel.FavouritesViewModel.LoadFavouritesAsync();

            HomeViewModel.IsLoadingFavourites = false;

            if (HomeViewModel.FavouritesViewModel.Favourites.Count == 0)
                txtNoFavourites.Visibility = Visibility.Visible;

            await HomeViewModel.HistoryViewModel.LoadHistoryAsync();

            HomeViewModel.IsLoadingHistory = false;

            if (HomeViewModel.HistoryViewModel.History.Count == 0)
                txtNoHistory.Visibility = Visibility.Visible;
        }

        private async void OnStopCodeKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                InputPane.GetForCurrentView().TryHide();
                e.Handled = true;

                await QueryVirtualTableAsync();
            }
        }

        private async void OnSumcClick(object sender, RoutedEventArgs e)
        {
            await new QuestionDialog(Strings.SumcRedirect, async () => await Launcher.LaunchUriAsync(new Uri(Urls.Sumc))).ShowAsync();
        }

        private void OnArrivalHolding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase attached = FlyoutBase.GetAttachedFlyout(senderElement);
            attached.ShowAt(senderElement);
        }

        private async void OnFlyoutItemClick(object sender, RoutedEventArgs e)
        {
            await AddFavouriteAsync();
        }

        private async void OnPivotSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pvMain.SelectedIndex == 0)
            {
                Focus(FocusState.Programmatic);
                if (reloadVirtualTable && !String.IsNullOrEmpty(txtStopCode.Text))
                {
                    await QueryVirtualTableAsync();
                    reloadVirtualTable = false;
                }
            }
        }

        private void OnFavouritesItemClick(object sender, ItemClickEventArgs e)
        {
            txtStopCode.Text = String.Format("{0:D4}", Int32.Parse((e.ClickedItem as FavouriteViewModel).Code));

            reloadVirtualTable = true;
            pvMain.SelectedIndex = 0;
        }

        private void OnHamburgerClick(object sender, RoutedEventArgs e)
        {
            svMain.IsPaneOpen = !svMain.IsPaneOpen;
        }

        private void OnAboutClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void OnSchedulesClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Schedules));
        }

        private async void OnRemoveFavouriteClick(object sender, RoutedEventArgs e)
        {
            FavouriteViewModel item = (sender as Button).DataContext as FavouriteViewModel;
            await new QuestionDialog(String.Format(Formats.ConfirmDeleteFavourite, item.Name),
                async () => await HomeViewModel.FavouritesViewModel.Remove(item)).ShowAsync();

            if (HomeViewModel.FavouritesViewModel.Favourites.Count == 0)
                txtNoFavourites.Visibility = Visibility.Visible;
        }

        private async void OnStopCodeClick(object sender, RoutedEventArgs e)
        {
            await QueryVirtualTableAsync();
        }

        private async void OnAddFavouriteClick(object sender, RoutedEventArgs e)
        {
            await AddFavouriteAsync();
        }

        private async Task AddFavouriteAsync()
        {
            pbFavourites.Visibility = Visibility.Visible;
            pvMain.SelectedIndex = 1;

            await HomeViewModel.FavouritesViewModel.AddAsync(txtStopCode.Text);

            if (HomeViewModel.FavouritesViewModel.Favourites.Count > 0)
                txtNoFavourites.Visibility = Visibility.Collapsed;

            pbFavourites.Visibility = Visibility.Collapsed;
        }

        private async Task QueryVirtualTableAsync()
        {
            if (!prVirtualTables.IsActive)
            {
                prVirtualTables.IsActive = true;
                prVirtualTables.Visibility = Visibility.Visible;

                try
                {
                    if (!await HomeViewModel.VirtualTableViewModel.GetByStopCode(txtStopCode.Text))
                        await new MessageDialog(Strings.NoResults).ShowAsync();
                }
                catch (Exception ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }
                finally
                {
                    prVirtualTables.IsActive = false;
                    prVirtualTables.Visibility = Visibility.Collapsed;
                }

                pbHistory.Visibility = Visibility.Visible;

                await HomeViewModel.HistoryViewModel.AddHistoryAsync(txtStopCode.Text);

                txtNoHistory.Visibility = HomeViewModel.HistoryViewModel.History.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                pbHistory.Visibility = Visibility.Collapsed;
            }
        }

        private void OnHistoryItemClick(object sender, ItemClickEventArgs e)
        {
            txtStopCode.Text = String.Format("{0:D4}", Int32.Parse((e.ClickedItem as HistoryEntryViewModel).Code));

            reloadVirtualTable = true;
            pvMain.SelectedIndex = 0;
        }

        private bool reloadVirtualTable;
    }
}
