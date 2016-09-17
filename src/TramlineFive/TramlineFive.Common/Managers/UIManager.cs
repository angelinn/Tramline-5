using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TramlineFive.Common.Managers
{
    public static class UIManager
    {
        public static void CollapseControl(UIElement control)
        {
            control.Visibility = Visibility.Collapsed;
        }

        public static void ShowControl(UIElement control)
        {
            control.Visibility = Visibility.Visible;
        }

        public static void DisableControl(Control control)
        {
            control.IsEnabled = false;
            control.Visibility = Visibility.Collapsed;
        }

        public static void EnableControl(Control control)
        {
            control.IsEnabled = true;
            control.Visibility = Visibility.Visible;
        }
    }
}
