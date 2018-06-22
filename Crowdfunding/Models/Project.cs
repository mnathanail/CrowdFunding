﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crowdfunding.Models
{
    public partial class Project
    {
        public Project()
        {
            Benefit = new HashSet<Benefit>();
        }

        public int ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDescription { get; set; }
        public decimal AskedFund { get; set; }
        public int Days { get; set; }
        [Required]
        [Range(1, 8)]
        public byte NumberOfBenefits { get; set; }
        public string MediaPath { get; set; }
        public string VideoUrl { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/2018", "1/1/2050")]
        public DateTime StartDate { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public AspNetUsers User { get; set; }
        public ICollection<Benefit> Benefit { get; set; }
    }
}
