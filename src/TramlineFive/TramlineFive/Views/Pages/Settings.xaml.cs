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
using Newtonsoft.Json;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;
using NotificationsExtensions;
using System.Threading.Tasks;
using TramlineFive.Common.Managers;
using TramlineFive.Views.Dialogs;
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

            this.SettingsViewModel = new SettingsViewModel();
            this.DataContext = SettingsViewModel;
            this.Transitions = AnimationManager.GeneratePageTransitions();
            
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateFavouriteStopFromSettingsAsync();
            SettingsViewModel.IsLiveTileEnabled = SettingsManager.ReadValue(SettingsKeys.LiveTile) == null ? default(bool) : Boolean.Parse(SettingsManager.ReadValue(SettingsKeys.LiveTile));
            SettingsViewModel.Update();
        }

        private void UpdateFavouriteStopFromSettingsAsync()
        {
            SettingsViewModel.FavouriteNumber = SettingsManager.ReadValue(SettingsKeys.FavouriteLine);
            SettingsViewModel.FavouriteName = SettingsManager.ReadValue(SettingsKeys.FavouriteName);
            SettingsViewModel.StopCode = SettingsManager.ReadValue(SettingsKeys.FavouriteStopCode);

            string type = SettingsManager.ReadValue(SettingsKeys.FavouriteType);
            SettingsViewModel.FavouriteType = type == null ? VehicleType.None : VehicleTypeManager.Destringify(type);
        }

        private async void OnExportClick(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker();
            StorageFolder folder = await picker.PickSingleFolderAsync();

            if (folder != null)
            {
                string serialized = await SettingsViewModel.GetSerializedFavourites();
                StorageFile file = await folder.CreateFileAsync($"{Strings.AppName}_{DateTime.Now.ToString(Formats.Timestamp)}.{Strings.BackupExtension}");
                await FileIO.WriteTextAsync(file, serialized);

                await new MessageDialog($"{Formats.ExportSuccess} - {file.Name}").ShowAsync();
            }
        }

        private void OnImportClick(object sender, RoutedEventArgs e)
        {
            throw new NotSupportedException();
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private async void OnClearHistoryClick(object sender, RoutedEventArgs e)
        {
            await SettingsViewModel.ClearHistoryAsync();
            ShowClearedToastNotification();
        }

        public async void OnChooseFromFavourites(object sender, RoutedEventArgs e)
        {
            StopChooserDialog dialog = new StopChooserDialog();
            if ((await dialog.ShowAsync()) == ContentDialogResult.Secondary && dialog.StopChooserViewModel.SelectedLine != null)
            {
                SettingsViewModel.StopCode = ParseManager.ToStopCode(dialog.StopChooserViewModel.SelectedFavourite.Code);
                SettingsViewModel.FavouriteNumber = dialog.StopChooserViewModel.SelectedLine.Number.ToString();
                SettingsViewModel.FavouriteType = dialog.StopChooserViewModel.SelectedLine.Type;
                SettingsViewModel.FavouriteName = dialog.StopChooserViewModel.SelectedFavourite.Name;

                if (SettingsViewModel.IsLiveTileEnabled)
                    SettingsViewModel.IsLiveTileEnabled = false;

                SettingsViewModel.IsLiveTileEnabled = true;
            }
        }

        private async void OnLiveTileToggled(object sender, RoutedEventArgs e)
        {
            if (SettingsViewModel.IsLiveTileEnabled != Boolean.Parse(SettingsManager.ReadValue(SettingsKeys.LiveTile) ?? "false"))
            {
                bool undo = false;

                if (!SettingsViewModel.IsValid())
                {
                    // Simulate attempt to turn on
                    await Task.Delay(200);
                    undo = true;
                }
                else
                {
                    SettingsViewModel.IsSwitchable = false;

                    // Give the UI time to refresh
                    await Task.Delay(50);

                    if (SettingsViewModel.IsLiveTileEnabled)
                    {
                        SettingsManager.UpdateValue(SettingsKeys.FavouriteStopCode, SettingsViewModel.StopCode);
                        SettingsManager.UpdateValue(SettingsKeys.FavouriteType, VehicleTypeManager.Stringify(SettingsViewModel.FavouriteType));
                        SettingsManager.UpdateValue(SettingsKeys.FavouriteName, SettingsViewModel.FavouriteName);
                        SettingsManager.UpdateValue(SettingsKeys.FavouriteLine, SettingsViewModel.FavouriteNumber);

                        if (!await BackgroundTaskManager.RegisterBackgroundTaskAsync())
                            undo = true;
                    }
                    else if (await BackgroundTaskManager.UnregisterBackgroundTaskAsync())
                    {
                        SettingsManager.ClearValue(SettingsKeys.FavouriteStopCode);
                        SettingsManager.ClearValue(SettingsKeys.FavouriteLine);
                        SettingsManager.ClearValue(SettingsKeys.FavouriteType);
                        SettingsManager.ClearValue(SettingsKeys.FavouriteName);
                    }
                    else
                        undo = true;
                }

                if (undo)
                    SettingsViewModel.IsLiveTileEnabled = !SettingsViewModel.IsLiveTileEnabled;

                SettingsViewModel.IsSwitchable = true;
                SettingsManager.UpdateValue(SettingsKeys.LiveTile, SettingsViewModel.IsLiveTileEnabled);
                SettingsViewModel.Update();
            }
        }

        private void ShowClearedToastNotification()
        {
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
        }

        private void OnRateClick(object sender, RoutedEventArgs e)
        {
            throw new NotSupportedException();
        }
    }
}
