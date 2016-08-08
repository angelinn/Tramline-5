using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.DataAccess;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Repositories;
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
                List<Line> lines = null;
                await Task.Run(() =>
                {
                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        lines = uow.Lines.All().ToList();
                    }
                });
                LineViewModel.Lines = lines;
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

        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Line item = e.AddedItems.First() as Line;

                using (UnitOfWork uow = new UnitOfWork())
                {

                }
            }
        }
    }
}
