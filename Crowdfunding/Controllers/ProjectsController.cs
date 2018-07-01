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
using System.ComponentModel.DataAnnotations;
using Crowdfunding.Utilities;
using System.Collections.Generic;
using System.Web;
using System.Security.Policy;
using System.IO;
using RestSharp;

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
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, string categorySelection, int? page)
        {            
            return View(await _projectsCall.ProjectsIndexCall(searchString, categorySelection, page));
        }

        // GET: Projects/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(b => b.Benefit).Where(p=>p.ProjectId==id)
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
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
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,ProjectDescription,AskedFund,Days,NumberOfBenefits,MediaPath,VideoUrl,UserId,StartDate,CategoryId")] Project project, List<Benefit> benefits)
        {
            if (ModelState.IsValid)
            {
                var httpFiles = HttpContext.Request.Form.Files;
                var userId = _GetPersonId();
                await _projectsCall.ProjectsCreateCall(project, userId,httpFiles);
                foreach (var benefit in benefits)
                {
                    benefit.ProjectId = project.ProjectId;
                }
                _context.Benefit.AddRange(benefits);
                if (project.VideoUrl != null)
                {
                    project.VideoUrl = _EmbeddedVideo(project.VideoUrl);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", project.CategoryId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.UserId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = _GetPersonId();
            if (id == null)
            {
                return NotFound();
            }
            if (await _ownThisProjectAsync(id, userid) == false) {
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, Project project)
        {
            var userid = _GetPersonId();
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (await _ownThisProjectAsync(id, userid) == false)
            {
                return RedirectToAction(nameof(Index));
            }
            var getProjectDetails = await _context.Project.FindAsync(id);
            getProjectDetails.VideoUrl = project.VideoUrl;
            getProjectDetails.AskedFund = project.AskedFund;
            getProjectDetails.ProjectDescription = project.ProjectDescription;
            getProjectDetails.ProjectName = project.ProjectName;
            getProjectDetails.CategoryId = project.CategoryId;
            getProjectDetails.UserId = userid;
            var rows = 0;
            try
            {
                _context.Update(getProjectDetails);
                rows = await _context.SaveChangesAsync();
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
            if(rows > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", project.CategoryId);
                ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", project.UserId);
                return View(project);
            }
        }

        // GET: Projects/Delete/5
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

        public async Task<TransactionResult> ChargeAsync(string vivaWalletToken)
        {
            var cl = new RestClient("https://demo.vivapayments.com/api/")
            {
                Authenticator = new HttpBasicAuthenticator("29e14ab7-52b2-4096-96db-09e0730da416", "iLs)h6")
            };
            var request = new RestRequest("transactions", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("PaymentToken", vivaWalletToken);

            var response = await cl.ExecuteTaskAsync<TransactionResult>(request);

            return response.ResponseStatus == ResponseStatus.Completed &&
                response.StatusCode == System.Net.HttpStatusCode.OK ? response.Data : null;
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }

        private string _GetPersonId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private string _EmbeddedVideo(string videoUrl)
        {
            return videoUrl.Replace("https://www.youtube.com/watch?v=", "https://www.youtube.com/embed/");
        }

        private async Task<bool> _ownThisProjectAsync(int? id, string userId)
        {
            return id == null ? false :  await _context.Project.AnyAsync(p => p.UserId == userId && p.ProjectId == id);
        }

    }
}
