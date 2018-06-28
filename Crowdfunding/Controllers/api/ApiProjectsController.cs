using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;

namespace Crowdfunding.Controllers.api
{
    [Produces("application/json")]
    public class ApiProjectsController : Controller
    {
        private readonly CrowdfundingContext _context;

        public ApiProjectsController(CrowdfundingContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var crowdfundingContext = _context.Project;
            var allProjects = await crowdfundingContext.ToListAsync();
            
            return Json(new
            {
                response = allProjects

            });
            //return View(await crowdfundingContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return Json(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
