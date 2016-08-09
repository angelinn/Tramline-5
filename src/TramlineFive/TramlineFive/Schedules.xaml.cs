using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.DataAccess;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.Dialogs;
using TramlineFive.Extensions;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive
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
            this.Transitions = AnimationManager.SetUpPageAnimation();

            LineViewModel = new LineViewModel();
            DataContext = LineViewModel;

            Loaded += Schedules_Loaded;
        }

        private async void Schedules_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LineViewModel.Lines = (await LineDO.AllAsync()).Where(l => l.Type != null)
                                                               .OrderBy(l => l.Type)
                                                               .ThenBy(l => l.Number);

                LineViewModel.Grouped = LineViewModel.Lines.GroupBy(l => l.Type);
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
            LineDO line = e.ClickedItem as LineDO;
            await line.LoadDirections();

            DirectionDialog dialog = new DirectionDialog(line.Directions);
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

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var matching = LineViewModel.Lines.Where(l => l.Name.Contains(sender.Text));
                sender.ItemsSource = matching;
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }
    }
}
