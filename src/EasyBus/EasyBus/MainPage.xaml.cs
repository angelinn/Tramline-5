using EasyBus.Common;
using EasyBus.Common.Exceptions;
using EasyBus.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EasyBus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ArrivalViewModel> Arrivals { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            Arrivals = new ObservableCollection<ArrivalViewModel>();
            DataContext = this;

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await SumcManager.Load();
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            prVirtualTables.IsActive = true;
            prVirtualTables.Visibility = Visibility.Visible;

            try
            {
                Arrivals.Clear();
                IEnumerable<ArrivalViewModel> arrivals = await SumcManager.GetByStopAsync(txtStopID.Text);

                if (arrivals.Count() == 0)
                {
                    Arrivals.Add(new ArrivalViewModel { Timings = "Няма резултати." });
                }


                foreach (ArrivalViewModel arrival in arrivals)
                    Arrivals.Add(arrival);

            }
            catch (Exception ex)
            {
                Arrivals.Add(new ArrivalViewModel { Timings = ex.Message });
            }
            finally
            {
                prVirtualTables.IsActive = false;
                prVirtualTables.Visibility = Visibility.Collapsed;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            SumcManager.ResetCookie();
        }
    }
}
