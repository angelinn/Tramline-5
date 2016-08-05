using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace TramlineFive.Common
{
    public static class AnimationManager
    {
        public static TransitionCollection SetUpPageAnimation()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            ContinuumNavigationTransitionInfo info = new ContinuumNavigationTransitionInfo();
            
            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            return collection;
        }
    }
}
