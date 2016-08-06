using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.DataAccess;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Repositories;
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
            LineViewModel = new LineViewModel();
            DataContext = LineViewModel;
        }

        private void btnFetchSchedules_Click(object sender, RoutedEventArgs e)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                LineViewModel.Lines = uow.Lines.All().ToList();
            }
        }
    }
}
