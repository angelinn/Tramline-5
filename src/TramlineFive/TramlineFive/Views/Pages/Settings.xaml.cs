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

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            FillTypesComboBox();
            UpdateFavouriteStopFromSettingsAsync();
        }

        private void UpdateFavouriteStopFromSettingsAsync()
        {
            string type = SettingsManager.ReadValue(SettingsKeys.FavouriteType);
            cbTypes.SelectedIndex = (type == null) ? 0 : (int)VehicleTypeManager.Destringify(type);

            string line = SettingsManager.ReadValue(SettingsKeys.FavouriteLine);
            txtLine.Text = line ?? String.Empty;

            string code = SettingsManager.ReadValue(SettingsKeys.FavouriteStopCode);
            txtCode.Text = code ?? String.Empty;
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

            if (SettingsViewModel.LiveTile != tsLiveTile.IsOn)
            {
                if (String.IsNullOrEmpty(txtLine.Text) && tsLiveTile.IsOn)
                {
                    // Simulate attempt to turn on
                    await Task.Delay(200);
                    tsLiveTile.IsOn = !tsLiveTile.IsOn;
                    return;
                }

                tsLiveTile.IsEnabled = false;

                // Give the UI time to refresh
                await Task.Delay(50);

                if (tsLiveTile.IsOn && txtCode.Text != SettingsManager.ReadValue(SettingsKeys.FavouriteStopCode))
                {
                    prCheckingStop.IsActive = true;
                    prCheckingStop.Visibility = Visibility.Visible;

                    NameValueObject type = (NameValueObject)(cbTypes.SelectedItem);
                    bool stopExists = await LineDO.DoesStopAt((VehicleType)type.Value, txtLine.Text, txtCode.Text);

                    prCheckingStop.IsActive = false;
                    prCheckingStop.Visibility = Visibility.Collapsed;

                    if (!stopExists)
                    {
                        await new MessageDialog($"{type.Name} №{txtLine.Text} не спира на спирка с код {txtCode.Text}").ShowAsync();
                        tsLiveTile.IsOn = !tsLiveTile.IsOn;
                        tsLiveTile.IsEnabled = true;

                        return;
                    }

                    SettingsManager.UpdateValue(SettingsKeys.FavouriteStopCode, txtCode.Text);
                    SettingsManager.UpdateValue(SettingsKeys.FavouriteType, type.Name);
                    SettingsManager.UpdateValue(SettingsKeys.FavouriteLine, txtLine.Text);

                    if (!await BackgroundTaskManager.RegisterBackgroundTaskAsync())
                        tsLiveTile.IsOn = !tsLiveTile.IsOn;
                }
                else if (await BackgroundTaskManager.UnregisterBackgroundTaskAsync())
                {
                    SettingsManager.ClearValue(SettingsKeys.FavouriteStopCode);
                    SettingsManager.ClearValue(SettingsKeys.FavouriteLine);
                    SettingsManager.ClearValue(SettingsKeys.FavouriteType);

                    UpdateFavouriteStopFromSettingsAsync();
                }
                else
                    tsLiveTile.IsOn = !tsLiveTile.IsOn;

                tsLiveTile.IsEnabled = true;

                SettingsViewModel.LiveTile = tsLiveTile.IsOn;
            }
        }
    }
}
