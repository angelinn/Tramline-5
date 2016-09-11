using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace TramlineFive.Common
{
    public static class BackgroundTaskManager
    {
        public static async Task<bool> RegisterBackgroundTaskAsync()
        {
            if (!await UnregisterBackgroundTaskAsync())
                return false;

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.Name = TASK_NAME;
            builder.TaskEntryPoint = TASK_ENTRY_POINT;
            builder.SetTrigger(new TimeTrigger(REFRESH_TIME_MINUTES, false));

            BackgroundTaskRegistration registration = builder.Register();
            return true;
        }

        public static async Task<bool> UnregisterBackgroundTaskAsync()
        {
            BackgroundAccessStatus accessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (accessStatus != BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity && accessStatus != BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
                return false;

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == TASK_NAME)
                {
                    task.Value.Unregister(true);
                }
            }

            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            return true;
        }

        private const string TASK_NAME = "FavouriteStopBackgroundTask";
        private const string TASK_ENTRY_POINT = "BackgroundTasks.FavouriteStopBackgroundTask";
        private const int REFRESH_TIME_MINUTES = 60;
    }
}
