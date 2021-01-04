using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SafeKeys.AttatchedProperties
{
    public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //get the caller

            //ensure it is passwordbox
            if (!(sender is PasswordBox passwordBox))
            {
                return;
            }

            //Remove previous events
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            //If caller sets Monitor password to true
            if ((bool)e.NewValue == true)
            {
                //Set the default value
                HasTextProperty.SetValue(passwordBox);

                //Listen for password changes in the control
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        /// <summary>
        /// Triggered when password box password changes for boxes that have on monitor password property as true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Set the hastext property for this password box
            HasTextProperty.SetValue((PasswordBox)sender);
        }
    }
}