using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace TramlineFive.Common.Models
{
    public interface ICaptchaDialog
    {
        IAsyncOperation<ContentDialogResult> ShowAsync();
        void SetUrl(string captchaUrl);

        string CaptchaString { get; }
    }
}
