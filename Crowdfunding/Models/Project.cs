using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Crowdfunding.Models
{
    public partial class Project
    {
        public Project()
        {
            Benefit = new List<Benefit>();
        }

        public int ProjectId { get; set; }
        [Required]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        [Required]
        [DisplayName("Description")]
        public string ProjectDescription { get; set; }
        [DisplayName("Goal")]
        [Range (1, 500000)]
        public decimal AskedFund { get; set; }
        [Range(7, 60, ErrorMessage = "The funding period must be between 7 to 60 days.")]
        public int Days { get; set; }
        [Required]
        [Range(1, 8)]
        [DisplayName("Number of Packages")]
        public byte NumberOfBenefits { get; set; }
        public string MediaPath { get; set; }
        public string VideoUrl { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public AspNetUsers User { get; set; }
        public ICollection<Benefit> Benefit { get; set; }
        public ICollection<UsersBenefits> UsersBenefits { get; set; }
    }
}
