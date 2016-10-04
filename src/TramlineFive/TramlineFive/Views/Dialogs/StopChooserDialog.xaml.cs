using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.ViewModels;
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
        public FavouritesViewModel FavouritesViewModel { get; private set; }
        public string Code { get; set; }

        public StopChooserDialog()
        {
            this.InitializeComponent();

            this.FavouritesViewModel = new FavouritesViewModel();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await FavouritesViewModel.LoadFavouritesAsync();
            this.DataContext = FavouritesViewModel;
        }
    }
}
