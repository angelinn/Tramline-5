using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace TramlineFive.Common.Managers
{
    public static class AnimationManager
    {
        public static TransitionCollection GeneratePageTransitions()
        {
            if (transitions == null)
            {
                TransitionCollection collection = new TransitionCollection();
                NavigationThemeTransition theme = new NavigationThemeTransition();

                ContinuumNavigationTransitionInfo info = new ContinuumNavigationTransitionInfo();

                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);

                transitions = collection;
            }

            return transitions;
        }

        private static TransitionCollection transitions;
    }
}
