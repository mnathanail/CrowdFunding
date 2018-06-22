using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfunding.Models.ConcreteModels
{
    public class ProjectsBenefits
    {
        public Project project { get;  set; }
        public Benefit benefit { get; set; }
    }
}
