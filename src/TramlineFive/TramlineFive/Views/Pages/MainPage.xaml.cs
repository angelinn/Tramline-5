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
using Windows.Storage;
using TramlineFive.DataAccess;
using Windows.UI.Xaml.Media;
using TramlineFive.Common.Models;
using TramlineFive.Views.Dialogs;
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
        public ArrivalViewModel ArrivalViewModel { get; set; }
        public FavouritesViewModel FavouritesViewModel { get; set; }
        public VersionViewModel VersionViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            ArrivalViewModel = new ArrivalViewModel();
            FavouritesViewModel = new FavouritesViewModel();
            VersionViewModel = new VersionViewModel();

            DataContext = this;
            NavigationCacheMode = NavigationCacheMode.Enabled;

            Loaded += MainPage_Loaded;
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
            if (firstLoad)
            {
                await CopyDatabaseFileIfNeeded();
                await SetStatusBar();
                await FavouritesViewModel.LoadFavourites();

                firstLoad = false;
            }


            prFavourites.IsActive = false;
            prFavourites.Visibility = Visibility.Collapsed;
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (!prVirtualTables.IsActive)
            {
                prVirtualTables.IsActive = true;
                prVirtualTables.Visibility = Visibility.Visible;

                try
                {
                    if (!await ArrivalViewModel.GetByStopCode(txtStopID.Text))
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

        private async void btnSumc_Click(object sender, RoutedEventArgs e)
        {
            await new QuestionDialog(Strings.SumcRedirect, async () =>
            {
                Uri sumc = new Uri(Urls.Sumc);
                await Launcher.LaunchUriAsync(sumc);
            }).ShowAsync();
        }

        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase attached = FlyoutBase.GetAttachedFlyout(senderElement);
            attached.ShowAt(senderElement);
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            FavouritesViewModel.Favourites.Clear();
            pvMain.SelectedIndex = 1;

            prFavourites.IsActive = true;
            prFavourites.Visibility = Visibility.Visible;

            await FavouritesViewModel.Add(txtStopID.Text);
            await FavouritesViewModel.LoadFavourites();

            prFavourites.IsActive = false;
            prFavourites.Visibility = Visibility.Collapsed;
        }

        private void pvMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pvMain.SelectedIndex == 0)
            {
                pvMain.Focus(FocusState.Pointer);
                if (reloadVirtualTable && !String.IsNullOrEmpty(txtStopID.Text))
                {
                    btnStop_Click(this, new RoutedEventArgs());
                    reloadVirtualTable = false;
                }
            }
        }

        private void lvFavourites_ItemClick(object sender, ItemClickEventArgs e)
        {
            txtStopID.Text = String.Format("{0:D4}", Int32.Parse((e.ClickedItem as FavouriteDO).Code));

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
            await new QuestionDialog(String.Format(Formats.ConfirmDeleteFavourite, item.Name), async () =>
            {
                await FavouritesViewModel.Remove(item);
                lvFavourites.Items.Remove(item);
            }).ShowAsync();
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

        private bool reloadVirtualTable;
        private bool firstLoad = true;
    }
}
