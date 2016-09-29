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

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateFavouriteStopFromSettingsAsync();
            SettingsViewModel.IsLiveTileEnabled = SettingsManager.ReadValue(SettingsKeys.LiveTile) == null ? default(bool) : Boolean.Parse(SettingsManager.ReadValue(SettingsKeys.LiveTile));
        }

        private void UpdateFavouriteStopFromSettingsAsync()
        {
            string type = SettingsManager.ReadValue(SettingsKeys.FavouriteType);
            SettingsViewModel.SelectedType = (type == null) ? SettingsViewModel.VehicleTypes.First() : SettingsViewModel.VehicleTypes.Where(t => t.Name == type).First();

            string line = SettingsManager.ReadValue(SettingsKeys.FavouriteLine);
            SettingsViewModel.LineNumber = line ?? String.Empty;

            string code = SettingsManager.ReadValue(SettingsKeys.FavouriteStopCode);
            SettingsViewModel.StopCode = code ?? String.Empty;
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

        private async void OnLiveTileToggled(object sender, RoutedEventArgs e)
        {
            if (SettingsViewModel.IsLiveTileEnabled != Boolean.Parse(SettingsManager.ReadValue(SettingsKeys.LiveTile)))
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
                    // Give the UI time to refresh
                    await Task.Delay(50);

                    if (SettingsViewModel.IsLiveTileEnabled)
                    {
                        if (!await SettingsViewModel.DoesStopExist())
                        {
                            await new MessageDialog($"{SettingsViewModel.SelectedType.Name} №{SettingsViewModel.LineNumber} не спира на спирка с код {SettingsViewModel.StopCode}").ShowAsync();
                            return;
                        }

                        SettingsManager.UpdateValue(SettingsKeys.FavouriteStopCode, SettingsViewModel.StopCode);
                        SettingsManager.UpdateValue(SettingsKeys.FavouriteType, SettingsViewModel.SelectedType.Name);
                        SettingsManager.UpdateValue(SettingsKeys.FavouriteLine, SettingsViewModel.LineNumber);

                        if (!await BackgroundTaskManager.RegisterBackgroundTaskAsync())
                            undo = true;
                    }
                    else if (await BackgroundTaskManager.UnregisterBackgroundTaskAsync())
                    {
                        SettingsManager.ClearValue(SettingsKeys.FavouriteStopCode);
                        SettingsManager.ClearValue(SettingsKeys.FavouriteLine);
                        SettingsManager.ClearValue(SettingsKeys.FavouriteType);
                    }
                    else
                        undo = true;
                }

                if (undo)
                    SettingsViewModel.IsLiveTileEnabled = !SettingsViewModel.IsLiveTileEnabled;

                SettingsViewModel.IsSwitchable = true;
                SettingsManager.UpdateValue(SettingsKeys.LiveTile, SettingsViewModel.IsLiveTileEnabled);
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
    }
}
