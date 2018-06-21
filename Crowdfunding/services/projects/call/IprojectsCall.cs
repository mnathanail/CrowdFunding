using Crowdfunding.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfunding.services.projects.call
{
    public interface IProjectsCall
    {
        IIncludableQueryable<Project, AspNetUsers> ProjectsIndexCall();
    }
}
