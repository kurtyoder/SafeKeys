using SafeKeys.Animation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SafeKeys.Views
{
    public class BaseView
    {
        public void AnimateIn(UserControl uc)
        {
            var sb = new Storyboard();

            sb.AddFadeIn(1f);

            sb.Begin(uc);         
        }

        public async Task AnimateOut(UserControl uc)
        {
            var sb = new Storyboard();

            sb.AddFadeOut(.3f);

            sb.Begin(uc);

            await Task.Delay(TimeSpan.FromSeconds(.3f));
        }

      
    }
}
