using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfunding.Models
{
    public class GiveItATry
    {
        public int ProjectId { get; set; }
        public List<Benefit> Benefit = new List<Benefit>();
        [DisplayName("Total Backers")]
        public int Backer { get; set; }
        public decimal Sum { get; set; }
        [DisplayName("Goal")]
        public decimal AskedFund { get; set; }
        public string ProjectName { get; set; }
    }
}