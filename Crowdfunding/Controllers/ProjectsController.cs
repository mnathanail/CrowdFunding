using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using Crowdfunding.services.projects.call;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Crowdfunding.Models.ConcreteModels;
using System.Collections.Generic;

namespace Crowdfunding.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly CrowdfundingContext _context;

        private readonly IProjectsCall _projectsCall;
        public ProjectsController(CrowdfundingContext context, IProjectsCall projectsCall)
        {
            _context = context;
            _projectsCall = projectsCall;
        }

        // GET: Projects
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, string categorySelection)
        {
            var projects = await _context
                .Project
                .ToListAsync();
            var benefits = await _context
                .Benefit
                .ToListAsync();


            var model = new ProjectsBenefits();
            model.project = await _context
                .Project
                .FirstOrDefaultAsync();
            model.benefit = await _context.
                Benefit
                .FirstOrDefaultAsync();
            
            return View(await _projectsCall.ProjectsIndexCall(searchString, categorySelection).ToListAsync());
        }

        // GET: Projects/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Benefit)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,ProjectDescription,AskedFund,Days,NumberOfBenefits,MediaPath,VideoUrl,UserId,StartDate,CategoryId")] Project project, List<Benefit> benefit)
        {
            
            if (ModelState.IsValid)
            {
                var ident = User.Identity as ClaimsIdentity;
                var userID = ident.Claims.FirstOrDefault().Value;
                await _projectsCall.ProjectsCreateCall(project, userID);
                foreach(var ben in benefit)
                {
                    ben.ProjectId = project.ProjectId;
                }
                _context.Benefit.AddRange(benefit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", project.CategoryId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.UserId);
            return View(project);
        }

        // GET: Projects/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", project.CategoryId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.UserId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,ProjectDescription,AskedFund,Days,NumberOfBenefits,MediaPath,VideoUrl,UserId,StartDate,CategoryId")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", project.CategoryId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.UserId);
            return View(project);
        }

        // GET: Projects/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
