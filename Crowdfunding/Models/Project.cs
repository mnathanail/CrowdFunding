using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Project
    {
        public Project()
        {
            Benefit = new HashSet<Benefit>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public decimal AskedFund { get; set; }
        public int Days { get; set; }
        public byte NumberOfBenefits { get; set; }
        public string MediaPath { get; set; }
        public string VideoUrl { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public AspNetUsers User { get; set; }
        public ICollection<Benefit> Benefit { get; set; }
    }
}
