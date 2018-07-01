using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfunding.Models
{
    public class Dashboard
    {
        public int ProjectId { get; set; }
        [DisplayName("Project Title")]
        public string ProjectName { get; set; }
        [DisplayName("Goal")]
        public decimal Amount { get; set; }

        public List<Benefit> Benefit = new List<Benefit>();
        [DisplayName("Total Backers")]
        public int Backers { get; set; }
        [DisplayName("Raised so far")]
        public decimal Sum { get; set; }
    }
}