﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TramlineFive.Common.Models;
using TramlineFive.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TramlineFive.Common.Managers;
using TramlineFive.ViewModels.Wrappers;
using TramlineFive.Views.Dialogs;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Schedules : Page
    {
        public AllLinesViewModel LineViewModel { get; private set; }

        public Schedules()
        {
            this.InitializeComponent();
            this.Transitions = AnimationManager.GeneratePageTransitions();

            this.LineViewModel = new AllLinesViewModel();
            this.DataContext = LineViewModel;

            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await LineViewModel.LoadAndGroupLinesAsync();
            lvLines.Focus(FocusState.Programmatic);
        }

        private void OnSchedulesItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(Direction), e.ClickedItem);           
        }

        private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Frame.Navigate(typeof(Direction), args.SelectedItem);
        }

        private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = LineViewModel.Lines.Where(l => l.NumberString.Contains(sender.Text));
            }
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
                rootFrame.GoBack();
        }
    }
}
