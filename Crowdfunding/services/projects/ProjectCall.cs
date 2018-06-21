using Crowdfunding.Models;
using Crowdfunding.services.projects.call;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Crowdfunding.services.projects
{
    public class ProjectCall : IProjectsCall
    {
        private readonly CrowdfundingContext _context;
        public ProjectCall(CrowdfundingContext context)
        {
            _context = context;
        }
        public IIncludableQueryable<Project, AspNetUsers> ProjectsIndexCall()
        {
            var crowdfundingContext = _context.Project.Include(p => p.Category).Include(p => p.User);
            return crowdfundingContext;
        }
    }
}
