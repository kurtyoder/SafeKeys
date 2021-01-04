using System;
using System.Collections.Generic;
using System.Text;

namespace SafeKeys.Library.Models
{
    public class KeyModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public int CategoryId { get; set; }
        public string Password { get; set; }
        public string IV { get; set; }
        public DateTime DatePasswordChanged { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateAccessed { get; set; }
        public DateTime DateCreated { get; set; }
    }
}