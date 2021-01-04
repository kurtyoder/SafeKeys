using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace SafeKeys.Animation
{
    public static class StoryboardHelpers
    {
        public static void AddFadeIn(this Storyboard storyboard, float seconds)
        {
            //Shift page using margin
            var slideAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1
            };

            //Set the target property name
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Opacity"));

            //Add this to the storyboard
            storyboard.Children.Add(slideAnimation);
        }

        public static void AddFadeOut(this Storyboard storyboard, float seconds)
        {
            //Shift page using margin
            var slideAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0
            };

            //Set the target property name
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Opacity"));

            //Add this to the storyboard
            storyboard.Children.Add(slideAnimation);
        }

    }
}
