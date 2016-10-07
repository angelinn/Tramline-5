using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.ViewModels;
using TramlineFive.ViewModels.Wrappers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Dialogs
{
    public sealed partial class StopChooserDialog : ContentDialog
    {
        public StopChooserViewModel StopChooserViewModel { get; private set; }

        public StopChooserDialog()
        {
            this.InitializeComponent();

            this.StopChooserViewModel = new StopChooserViewModel();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await StopChooserViewModel.FavouritesViewModel.LoadFavouritesAsync();
            this.DataContext = StopChooserViewModel;
        }

        private async void OnFavouritesItemClick(object sender, ItemClickEventArgs e)
        {
            StopChooserViewModel.SelectedFavourite = e.ClickedItem as FavouriteViewModel;
            await StopChooserViewModel.LoadAvailableLinesAsync();
        }
    }
}
