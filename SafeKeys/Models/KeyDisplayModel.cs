using System;
using System.Collections.Generic;
using System.Text;

namespace SafeKeys.Models
{
    public class KeyDisplayModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public int CategoryId { get; set; }
        public DateTime DatePasswordChanged { get; set; }
        public DateTime DateAccessed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool PasswordGood => (DateTime.Now - DatePasswordChanged).Days < 365;
        public string Password { get; set; }
    }
}