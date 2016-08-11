using TramlineFive.Common;
using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;
using TramlineFive.Common.Models;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TramlineFive.Views.Dialogs
{
    public sealed partial class CaptchaDialog : ContentDialog, ICaptchaDialog
    {
        public string CaptchaString { get; set; }

        public CaptchaDialog()
        {
            this.InitializeComponent();
        }

        public void SetUrl(string captchaUrl)
        {
            imgCaptcha.Source = new BitmapImage(new Uri(captchaUrl));
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CaptchaString = txtCaptcha.Text;
        }
    }
}
