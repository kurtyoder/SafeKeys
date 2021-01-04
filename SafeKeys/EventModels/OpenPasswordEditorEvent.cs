using SafeKeys.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SafeKeys.EventModels
{
    /// <summary>
    /// Used to tell event aggregator to open the password editor
    /// </summary>
    public class OpenPasswordEditorEvent
    {
        public KeyDisplayModel key;

        public OpenPasswordEditorEvent(KeyDisplayModel key)
        {
            this.key = key;
        }
    }
}