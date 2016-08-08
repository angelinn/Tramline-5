using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TramlineFive.DataAccess.DomainLogic;
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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Dialogs
{
    public sealed partial class DirectionDialog : ContentDialog
    {
        public IEnumerable<DirectionDO> Directions { get; set; }

        public DirectionDialog(IEnumerable<DirectionDO> directions)
        {
            this.InitializeComponent();
            Directions = directions;
            
            DataContext = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string selected = (sender as Button).Content as string;
            DirectionDO choice = Directions.Where(d => d.Name == selected).First();
            await choice.LoadDays();

            Hide();
            await new DayDialog(choice.Days).ShowAsync();
        }
    }
}
