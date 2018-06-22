﻿using Crowdfunding.Models;
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
        IQueryable<Project> ProjectsIndexCall(string searchString, string categorySelection);
        Task ProjectsCreateCall(Project project, string userId);
    }
}
