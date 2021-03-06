﻿using System;
using System.Collections.Generic;

namespace Crowdfunding.Models
{
    public partial class UsersBenefits
    {
        public int BenefitId { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }

        public Benefit Benefit { get; set; }
        public AspNetUsers User { get; set; }
        public Project Project { get; set; }
    }
}
