using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.Common.Models;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.Views.Dialogs;
using TramlineFive.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TramlineFive.Common.Managers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Schedules : Page
    {
        public LineViewModel LineViewModel { get; set; }
        public Schedules()
        {
            this.InitializeComponent();
            this.Transitions = AnimationManager.GeneratePageTransitions();

            this.LineViewModel = new LineViewModel();
            this.DataContext = LineViewModel;

            this.Loaded += Schedules_Loaded;
        }

        private async void Schedules_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LineViewModel.Lines = (await LineDO.AllAsync()).Where(l => l.Type != VehicleType.None)
                                                               .OrderBy(l => l.SortID)
                                                               .ThenBy(l => l.Number);

                LineViewModel.Grouped = LineViewModel.Lines.GroupBy(l => VehicleTypeManager.Stringify(l.Type, true));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prLines.IsEnabled = false;
                prLines.Visibility = Visibility.Collapsed;
            }
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            await PromptForDirection(e.ClickedItem as LineDO);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = LineViewModel.Lines.Where(l => l.NumberString.Contains(sender.Text));
            }
        }

        private async void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            await PromptForDirection(args.SelectedItem as LineDO);
        }

        private async Task PromptForDirection(LineDO line)
        {
            DirectionDialog dialog = new DirectionDialog(line);
            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Frame.Navigate(typeof(Schedule), new Dictionary<string, object>
                {
                    { "Direction", dialog.SelectedDirection },
                    { "Day", dialog.SelectedDay },
                    { "Line", line }
                });
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }
    }
}
