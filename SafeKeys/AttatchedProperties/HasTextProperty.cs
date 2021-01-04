using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SafeKeys.AttatchedProperties
{
    /// <summary>
    /// The HasText attached property for a <see cref="PasswordBox"/>
    /// </summary>
    public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
    {
        /// <summary>
        /// Sets has text password based on whether caller has any text
        /// </summary>
        /// <param name="sender"></param>
        public static void SetValue(DependencyObject sender)
        {
            HasTextProperty.SetValue((PasswordBox)sender, ((PasswordBox)sender).SecurePassword.Length > 0);
        }
    }
}