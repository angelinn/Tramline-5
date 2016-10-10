using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.Common.Managers;
using TramlineFive.Common.Models;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class FavouriteStopBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            if (DateTime.Now.Hour > MORNING_QUIET_HOUR)
            {
                await TileManager.QueryStopAndUpdate();
            }

            deferral.Complete();
        }

        private const int MORNING_QUIET_HOUR = 5;
    }
}
