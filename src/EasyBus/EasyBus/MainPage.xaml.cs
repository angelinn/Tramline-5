﻿using EasyBus.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }

        private async void btnStop_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<ArrivalViewModel> arrivals = await SumcManager.GetByStopAsync(txtStopID.Text);
            foreach (ArrivalViewModel arrival in arrivals)
                Arrivals.Add(arrival);
        }
    }
}
