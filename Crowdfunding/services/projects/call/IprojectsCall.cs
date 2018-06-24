using Crowdfunding.Models;
using Crowdfunding.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Crowdfunding.services.projects.call
{
    public interface IProjectsCall
    {
        Task<Pagination<Project>> ProjectsIndexCall(string searchString, string categorySelection, int? page);
        Task ProjectsCreateCall(Project project, string userId, IFormFileCollection httpFiles);
    }
}
