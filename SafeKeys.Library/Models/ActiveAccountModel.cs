using System;
using System.Collections.Generic;
using System.Text;

namespace SafeKeys.Library.Models
{
    public class ActiveAccountModel
    {
        public byte[] Key { get; set; }
        public string DisplayTitle { get; set; }
        public DateTime Timestamp { get; set; }
        public string FilePath { get; set; }

        public void Logout()
        {
            Key = new byte[32];
            DisplayTitle = null;
            FilePath = null;
            Timestamp = DateTime.MinValue;
        }
    }
}
