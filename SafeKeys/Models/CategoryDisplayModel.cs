using System;
using System.Collections.Generic;
using System.Text;

namespace SafeKeys.Models
{
    public class CategoryDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string CategoryCount { get; set; } = "1";
    }
}