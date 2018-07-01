using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crowdfunding.Models
{
    public partial class Benefit
    {
        public Benefit()
        {
            UsersBenefits = new HashSet<UsersBenefits>();
        }

        public int BenefitId { get; set; }
        [Required]
        [Display(Name = "Benefit Name")]
        public string BenefitName { get; set; }
        [Required]
        [Display(Name = "Benefit Description")]
        public string BenefitDesciption { get; set; }
        public int ProjectId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public decimal BenefitPrice { get; set; }

        public Project Project { get; set; }
        public virtual ICollection<UsersBenefits> UsersBenefits { get; set; }


    }
}
