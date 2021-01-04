using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SafeKeys.AttatchedProperties
{
    /// <summary>
    /// base attached property to replace vanilla attached property
    /// </summary>
    /// <typeparam name="T">The parent class to be the attached property</typeparam>
    /// <typeparam name="U">The type of the attached property</typeparam>
    public abstract class BaseAttachedProperty<T, U>
        where T : BaseAttachedProperty<T, U>, new()
    {
        #region Public Events

        /// <summary>
        /// Fired when the value changes to new value
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        /// <summary>
        /// Fired when the value changes even when the value is the same
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Singleton instance of parent class
        /// </summary>
        public static T Instance { get; private set; } = new T();

        #endregion Public Properties

        #region Attached Property Definitions

        /// <summary>
        /// The actual attached property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(U), typeof(BaseAttachedProperty<T, U>), new UIPropertyMetadata(default(U), new PropertyChangedCallback(OnValuePropertyChanged), new CoerceValueCallback(OnValuePropertyUpdated)));

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed, even if it is the same value
        /// </summary>
        /// <param name="d">UI element that had property changed</param>
        /// <param name="baseValue">Arguments for the event</param>
        /// <returns></returns>
        private static object OnValuePropertyUpdated(DependencyObject d, object baseValue)
        {
            //Call the parent function
            Instance.OnValueUpdated(d, baseValue);

            //Call event listeners
            Instance.ValueUpdated(d, baseValue);

            //Return the value
            return baseValue;
        }

        /// <summary>
        /// Callback event when value is changed
        /// </summary>
        /// <param name="d">UI element that had property changed</param>
        /// <param name="e">Arguments for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Call the parent function
            Instance.OnValueChanged(d, e);

            //Call event listeners
            Instance.ValueChanged(d, e);
        }

        /// <summary>
        /// Gets the attached property
        /// </summary>
        /// <param name="d">Element to get the property from</param>
        /// <returns></returns>
        public static U GetValue(DependencyObject d)
        {
            return (U)d.GetValue(ValueProperty);
        }

        /// <summary>
        /// Set the attached property
        /// </summary>
        /// <param name="d">Element to get the property from</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetValue(DependencyObject d, U value)
        {
            d.SetValue(ValueProperty, value);
        }

        #endregion Attached Property Definitions

        #region Event Methods

        /// <summary>
        /// The method that is called when any attached property of this type is changed
        /// </summary>
        /// <param name="sender">The UI element that this property is changed for</param>
        /// <param name="e">Argument for this event</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        /// The method that is called when any attached property of this type is changed even if the value is the same
        /// </summary>
        /// <param name="sender">The UI element that this property is changed for</param>
        /// <param name="e">Argument for this event</param>
        public virtual void OnValueUpdated(DependencyObject sender, object value) { }

        #endregion Event Methods
    }
}