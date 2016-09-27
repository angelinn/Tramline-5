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
using TramlineFive.DataAccess.DomainLogic;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Background;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TramlineFive.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public VirtualTableViewModel ArrivalViewModel { get; set; }
        public FavouritesViewModel FavouritesViewModel { get; set; }
        public HistoryViewModel HistoryViewModel { get; set; }
        public VersionViewModel VersionViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            this.ArrivalViewModel = new VirtualTableViewModel();
            this.FavouritesViewModel = new FavouritesViewModel();
            this.HistoryViewModel = new HistoryViewModel();
            this.VersionViewModel = new VersionViewModel();

            this.DataContext = this;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.Loaded += MainPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        // Prevents panel being invisibly open on other pages, causing double back click needed to go back
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
                await new QuestionDialog(Strings.PromptExit, () => CoreApplication.Exit()).ShowAsync();
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await FavouritesViewModel.LoadFavouritesAsync();

            prFavourites.IsActive = false;
            prFavourites.Visibility = Visibility.Collapsed;

            if (FavouritesViewModel.Favourites.Count == 0)
                txtNoFavourites.Visibility = Visibility.Visible;

            await HistoryViewModel.LoadHistoryAsync();

            prHistory.IsActive = false;
            prHistory.Visibility = Visibility.Collapsed;

            if (HistoryViewModel.History.Count == 0)
                txtNoHistory.Visibility = Visibility.Visible;
        }

        private async void txtStopCode_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                InputPane.GetForCurrentView().TryHide();
                e.Handled = true;

                await QueryVirtualTableAsync();
            }
        }

        private async void btnSumc_Click(object sender, RoutedEventArgs e)
        {
            await new QuestionDialog(Strings.SumcRedirect, async () => await Launcher.LaunchUriAsync(new Uri(Urls.Sumc))).ShowAsync();
        }

        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase attached = FlyoutBase.GetAttachedFlyout(senderElement);
            attached.ShowAt(senderElement);
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            await AddFavouriteAsync();
        }

        private async void pvMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void lvFavourites_ItemClick(object sender, ItemClickEventArgs e)
        {
            txtStopCode.Text = String.Format("{0:D4}", Int32.Parse((e.ClickedItem as FavouriteDO).Code));

            reloadVirtualTable = true;
            pvMain.SelectedIndex = 0;
        }

        private void OnHamburgerClick(object sender, RoutedEventArgs e)
        {
            svMain.IsPaneOpen = !svMain.IsPaneOpen;
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

        private async void btnRemoveFavourite_Click(object sender, RoutedEventArgs e)
        {
            FavouriteDO item = (sender as Button).DataContext as FavouriteDO;
            await new QuestionDialog(String.Format(Formats.ConfirmDeleteFavourite, item.Name), async () => await FavouritesViewModel.Remove(item)).ShowAsync();

            if (FavouritesViewModel.Favourites.Count == 0)
                txtNoFavourites.Visibility = Visibility.Visible;
        }

        private async void btnStopCode_Click(object sender, RoutedEventArgs e)
        {
            await QueryVirtualTableAsync();
        }

        private async void btnFavourite_Click(object sender, RoutedEventArgs e)
        {
            await AddFavouriteAsync();
        }

        private async Task AddFavouriteAsync()
        {
            pbFavourites.Visibility = Visibility.Visible;
            pvMain.SelectedIndex = 1;

            await FavouritesViewModel.AddAsync(txtStopCode.Text);

            if (FavouritesViewModel.Favourites.Count > 0)
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
                    if (!await ArrivalViewModel.GetByStopCode(txtStopCode.Text))
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
                await HistoryViewModel.AddHistoryAsync(txtStopCode.Text);
                txtNoHistory.Visibility = HistoryViewModel.History.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                pbHistory.Visibility = Visibility.Collapsed;
            }
        }

        private void lvHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            txtStopCode.Text = String.Format("{0:D4}", Int32.Parse((e.ClickedItem as HistoryDO).Code));

            reloadVirtualTable = true;
            pvMain.SelectedIndex = 0;
        }

        private bool reloadVirtualTable;
    }
}
