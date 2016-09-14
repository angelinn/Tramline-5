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
using TramlineFive.Common.Extensions;
using TramlineFive.Common.Managers;

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

            this.SettingsViewModel = new SettingsViewModel();
            this.DataContext = SettingsViewModel;
            this.Transitions = AnimationManager.GeneratePageTransitions();

            this.Loaded += Settings_Loaded;
        }

        private async void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            FillTypesComboBox();
            await UpdateFavouriteStopFromSettingsAsync();
        }

        private async Task UpdateFavouriteStopFromSettingsAsync()
        {
            string type = SettingsManager.ReadValue(SettingsKeys.FavouriteType);

            cbTypes.SelectedIndex = (type == null) ? 0 : (int)VehicleTypeManager.Destringify(type);

            string line = SettingsManager.ReadValue(SettingsKeys.FavouriteLine);
            txtLine.Text = line ?? String.Empty;

            string index = SettingsManager.ReadValue(SettingsKeys.FavouriteIndex);
            if (index != null)
            {
                await FetchStopsAsync();
                int idx = Int32.Parse(index);

                if (cbStops.Items.Count >= idx)
                    cbStops.SelectedIndex = Int32.Parse(index);
            }
        }

        private void FillTypesComboBox()
        {
            cbTypes.ItemsSource = VehicleTypeManager.GetNameValuePair();
            cbTypes.DisplayMemberPath = "Name";
            cbTypes.SelectedValuePath = "Value";
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
            bool allFieldsFilled = !String.IsNullOrEmpty(txtLine.Text) && cbStops.SelectedItem != null;

            if (SettingsViewModel.LiveTile != tsLiveTile.IsOn)
            {
                if (!allFieldsFilled && !tsLiveTile.IsOn)
                {
                    tsLiveTile.IsOn = !tsLiveTile.IsOn;
                    return;
                }

                tsLiveTile.IsEnabled = false;
                string converted = ParseManager.ToStopCode((cbStops.SelectedItem as StopDO).Code);

                if (tsLiveTile.IsOn && converted != SettingsManager.ReadValue(SettingsKeys.FavouriteStopCode))
                {
                    SettingsManager.UpdateValue(SettingsKeys.FavouriteStopCode, converted);
                    SettingsManager.UpdateValue(SettingsKeys.FavouriteIndex, cbStops.SelectedIndex);
                    SettingsManager.UpdateValue(SettingsKeys.FavouriteType, ((NameValueObject)(cbTypes.SelectedItem)).Name);
                    SettingsManager.UpdateValue(SettingsKeys.FavouriteLine, txtLine.Text);

                    if (!await BackgroundTaskManager.RegisterBackgroundTaskAsync())
                        tsLiveTile.IsOn = !tsLiveTile.IsOn;
                }
                else if (await BackgroundTaskManager.UnregisterBackgroundTaskAsync())
                {
                    SettingsManager.ClearValue(SettingsKeys.FavouriteStopCode);
                    SettingsManager.ClearValue(SettingsKeys.FavouriteLine);
                    SettingsManager.ClearValue(SettingsKeys.FavouriteType);
                    SettingsManager.ClearValue(SettingsKeys.FavouriteIndex);

                    await UpdateFavouriteStopFromSettingsAsync();
                    cbStops.IsEnabled = false;
                    cbStops.SelectedIndex = -1;
                }
                else
                    tsLiveTile.IsOn = !tsLiveTile.IsOn;

                tsLiveTile.IsEnabled = true;

                SettingsViewModel.LiveTile = tsLiveTile.IsOn;
            }
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

        private async void btnSearchStops_Click(object sender, RoutedEventArgs e)
        {
            prSearchStops.IsActive = true;
            prSearchStops.Visibility = Visibility.Visible;
            btnSearchStops.IsEnabled = false;

            await FetchStopsAsync();

            prSearchStops.IsActive = false;
            prSearchStops.Visibility = Visibility.Collapsed;
            btnSearchStops.IsEnabled = true;
        }
    }
}
