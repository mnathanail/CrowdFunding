using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Category
    {
        public Category()
        {
            Project = new HashSet<Project>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Project> Project { get; set; }
    }
}
