using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SafeKeys.AttatchedProperties
{
    //Used to flag any control as busy
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(!(sender is Control control))
            {
                return;
            }

            control.Loaded += (s, se) => control.Focus();
        }
    }
}