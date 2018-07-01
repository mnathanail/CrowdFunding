using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using Crowdfunding.services.projects.call;

namespace Crowdfunding.Controllers.api
{
    [Produces("application/json")]
    public class ApiProjectsController : Controller
    {
        private readonly CrowdfundingContext _context;
        private readonly IProjectsCall _projectsCall;
        public ApiProjectsController(CrowdfundingContext context, IProjectsCall projectsCall)
        {
            _context = context;
            _projectsCall = projectsCall;
        }

        // GET: Projects
        public IActionResult Index(int? page)
        {
            //var allProjects = await _projectsCall.ProjectsIndexCallJson(page);
            //return Json(new
            //{
            //    allProjects
            //});
            return View();
        }

        public async Task<IActionResult> GetAllProjects(int? page)
        {
            var allProjects = await _projectsCall.ProjectsIndexCallJson(page);
            return Json(new
            {
                getAllProjects = allProjects
            });
        }

        // GET: Projects/Details/5
        public IActionResult Details(int? id)
        {
            return View();
        }

        public async Task<IActionResult> GetProjectDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return Json(new
            {
                getProjectsDetails = project
            });
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
