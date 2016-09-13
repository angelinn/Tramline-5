using TramlineFive.Common;
using TramlineFive.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Popups;
using TramlineFive.DataAccess.DomainLogic;
using Newtonsoft.Json;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;
using NotificationsExtensions;
using System.Threading.Tasks;
using TramlineFive.Common.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public SettingsViewModel SettingsViewModel { get; private set; }
        public Settings()
        {
            this.InitializeComponent();

            SettingsViewModel = new SettingsViewModel();
            this.DataContext = SettingsViewModel;
            this.Transitions = AnimationManager.SetUpPageAnimation();

            Loaded += Settings_Loaded;
        }

        private async void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<object>();
            foreach (int r in Enum.GetValues(typeof(VehicleType)))
            {
                if (r >= 0)
                    list.Add(new { Name = VehicleTypeManager.TypeToString((VehicleType)Enum.ToObject(typeof(VehicleType), r)), Value = r });
            }

            cbTypes.ItemsSource = list;
            cbTypes.DisplayMemberPath = "Name";
            cbTypes.SelectedValuePath = "Value";

            string type = SettingsManager.ReadValue("FavouriteType");
            cbTypes.SelectedIndex = (type == null) ? 0 : Int32.Parse(type);

            string line = SettingsManager.ReadValue("FavouriteLine");
            txtLine.Text = line ?? String.Empty;

            string index = SettingsManager.ReadValue("FavouriteIndex");
            if (index != null)
            {
                await FetchStopsAsync();
                cbStops.SelectedIndex = Int32.Parse(index);
            }

        }

        private async void btnExport_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker();
            StorageFolder folder = await picker.PickSingleFolderAsync();

            if (folder != null)
            {
                string serialized = JsonConvert.SerializeObject(await FavouriteDO.AllAsync());
                StorageFile file = await folder.CreateFileAsync($"{Strings.AppName}_{DateTime.Now.ToString(Formats.Timestamp)}.{Strings.BackupExtension}");
                await FileIO.WriteTextAsync(file, serialized);

                await new MessageDialog($"{Formats.ExportSuccess} - {file.Name}").ShowAsync();
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private async void btnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            btnClearHistory.IsEnabled = false;
            prClearHistory.IsActive = true;
            prClearHistory.Visibility = Visibility.Visible;

            await SettingsViewModel.ClearHistoryAsync();

            ToastBindingGeneric text = new ToastBindingGeneric();
            text.Children.Add(new AdaptiveText() { Text = Strings.HistoryCleared });
            ToastContent cont = new ToastContent()
            {
                Scenario = ToastScenario.Default,
                Visual = new ToastVisual()
                {
                    BindingGeneric = text
                },
            };

            ToastNotification notification = new ToastNotification(cont.GetXml());
            notification.ExpirationTime = DateTime.Now.AddSeconds(3);

            ToastNotificationManager.CreateToastNotifier().Show(notification);

            btnClearHistory.IsEnabled = true;
            prClearHistory.IsActive = false;
            prClearHistory.Visibility = Visibility.Collapsed;
        }

        private async void tsLiveTile_Toggled(object sender, RoutedEventArgs e)
        {
            if (SettingsViewModel.LiveTile != tsLiveTile.IsOn)
            {
                tsLiveTile.IsEnabled = false;

                if (tsLiveTile.IsOn)
                {
                    if (!await BackgroundTaskManager.RegisterBackgroundTaskAsync())
                        tsLiveTile.IsOn = !tsLiveTile.IsOn;
                }
                else if (await BackgroundTaskManager.UnregisterBackgroundTaskAsync())
                {
                    SettingsManager.ClearValue("Favourite");
                    SettingsManager.ClearValue("FavouriteLine");
                    SettingsManager.ClearValue("FavouriteType");
                    SettingsManager.ClearValue("FavouriteIndex");
                }
                else
                    tsLiveTile.IsOn = !tsLiveTile.IsOn;

                tsLiveTile.IsEnabled = true;

                SettingsViewModel.LiveTile = tsLiveTile.IsOn;
            }
        }

        private async void txtLine_LostFocus(object sender, RoutedEventArgs e)
        {
            await FetchStopsAsync();
        }

        private async Task FetchStopsAsync()
        {
            int testVariable;

            if (Int32.TryParse(txtLine.Text, out testVariable))
            {
                cbStops.IsEnabled = true;
                IEnumerable<StopDO> stops = await LineDO.FetchByVehicleAsync((VehicleType)cbTypes.SelectedValue, txtLine.Text);

                if (stops != null)
                {
                    cbStops.DisplayMemberPath = "Name";
                    cbStops.ItemsSource = stops;

                    tsLiveTile.IsEnabled = true;
                }
            }
        }

        private void cbStops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string converted = CommonManager.ToStopCode((cbStops.SelectedItem as StopDO).Code);

            if (converted != SettingsManager.ReadValue("Favourite"))
            {
                SettingsManager.UpdateValue("Favourite", converted);
                SettingsManager.UpdateValue("FavouriteIndex", cbStops.SelectedIndex);
                SettingsManager.UpdateValue("FavouriteType", cbTypes.SelectedIndex);
                SettingsManager.UpdateValue("FavouriteLine", txtLine.Text);

                tsLiveTile.IsOn = true;
            }
        }
    }
}
