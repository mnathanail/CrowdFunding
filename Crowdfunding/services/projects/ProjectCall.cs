using Crowdfunding.Models;
using Crowdfunding.services.projects.call;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Crowdfunding.services.projects
{
    public class ProjectCall : IProjectsCall
    {
        private readonly CrowdfundingContext _context;
        public ProjectCall(CrowdfundingContext context)
        {
            _context = context;
        }

        public IQueryable<Project> ProjectsIndexCall(string searchString, string categorySelection)
        {
            var crowdfundingContext = _context.Project.Include(p => p.Category).Include(p => p.User);
            if (!String.IsNullOrEmpty(searchString))
            {
                return crowdfundingContext.Where(s => s.ProjectName.ToUpper().Contains(searchString.ToUpper()));
            }
            else if (!String.IsNullOrEmpty(categorySelection))
            {
                bool isCategoryNameInt = int.TryParse(categorySelection, out int categorySelectionInt);
                if (isCategoryNameInt && categorySelectionInt!=9)
                {
                    return crowdfundingContext.Where(s => s.CategoryId == categorySelectionInt);
                }
                else
                {
                    return crowdfundingContext;
                }
            }
            return crowdfundingContext;
        }

        public async Task ProjectsCreateCall(Project project, string userId)
        {
            project.UserId = userId;
            project.StartDate = DateTime.Today.Date;
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

    }
}
