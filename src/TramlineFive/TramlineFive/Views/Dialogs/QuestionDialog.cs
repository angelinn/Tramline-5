using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using Windows.Foundation;
using Windows.UI.Popups;

namespace TramlineFive.Views.Dialogs
{
    public class QuestionDialog
    {
        public QuestionDialog(string message)
        {
            coreDialog = new MessageDialog(message);
            coreDialog.Commands.Add(new UICommand(Strings.Yes));
            coreDialog.Commands.Add(new UICommand(Strings.No));
        }

        public IAsyncOperation<IUICommand> ShowAsync()
        {
            return coreDialog.ShowAsync();
        }

        private MessageDialog coreDialog;
    }
}
