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

        public QuestionDialog(string message, Action success)
        {
            coreDialog = new MessageDialog(message);
            coreDialog.Commands.Add(new UICommand(Strings.Yes));
            coreDialog.Commands.Add(new UICommand(Strings.No));

            successMethod = success;
        }

        public async Task ShowAsync()
        {
            IUICommand result = await coreDialog.ShowAsync();
            if (result?.Label == Strings.Yes)
                successMethod.Invoke();
        }

        private Action successMethod;
        private MessageDialog coreDialog;
    }
}
