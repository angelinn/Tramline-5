using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.DataAccess.DomainLogic;
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

namespace TramlineFive.Dialogs
{
    public sealed partial class DayDialog : ContentDialog
    {
        public IEnumerable<DayDO> Days { get; set; }
        public IEnumerable<StopDO> Stops { get; set; }

        public DayDialog(IEnumerable<DayDO> days)
        {
            this.InitializeComponent();
            Days = days;

            DataContext = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string selected = (sender as Button).Content as string;
            DayDO choice = Days.Where(d => d.Type == selected).First();
            await choice.LoadStops();

            Stops = choice.Stops;
        }
    }
}
