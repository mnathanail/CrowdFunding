using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class Benefit
    {
        public Benefit()
        {
            UsersBenefits = new HashSet<UsersBenefits>();
        }

        public int BenefitId { get; set; }
        public string BenefitName { get; set; }
        public string BenefitDesciption { get; set; }
        public int ProjectId { get; set; }
        public decimal BenefitPrice { get; set; }

        public Project Project { get; set; }
        public ICollection<UsersBenefits> UsersBenefits { get; set; }
    }
}
