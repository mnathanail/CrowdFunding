using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Crowdfunding.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly CrowdfundingContext _context;

        public UserController(CrowdfundingContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var userId = _GetPersonId();
            if (userId == null)
            {
                return NotFound();
            }

            //var usercontext = await _context.Project
            //    .Include(u => u.User)
            //    .Where(p => p.UserId == userId).ToListAsync();

            //var usercontext = await _context.Benefit
            //   .Include(p => p.Project)
            //   .Where(a => a.ProjectId == a.Project.ProjectId)
            //   .GroupBy(n => n.ProjectId)
            //   .Select(p => new GiveItATry
            //   {
            //       //AskedFund = p.ToDictionary(a => a, a => Project.Askedfund.Value),
            //       ProjectId = p.Key,
            //       Benefit = p.ToList(),
            //       Backer = p.Count(),
            //       Sum = p.Sum(oi => oi.BenefitPrice),
            //   }).ToListAsync();

            var usercontext = await _context.Benefit
               .Include(p => p.Project)
               .Where(a => a.ProjectId == a.Project.ProjectId)
               .GroupBy(n => n.ProjectId)
               .Select(p => new GiveItATry
               {
                   ProjectId = p.Key,
                   Benefit = p.ToList(),
                   Backer = p.Count(),
                   Sum = p.Sum(oi => oi.BenefitPrice),
               }).ToListAsync();

            //var usercontext = await _context.UsersBenefits
            //    .Include(b => b.Benefit)
            //    .Include(p => p.User)
            //    .Where(p => p.UserId == userId)
            //    .Include(p => p.Project)
            //    .Where(a => a.ProjectId == a.Project.ProjectId)
            //    .GroupBy(n => n.ProjectId)
            //    .Select(p => new GiveItATry
            //    {

            //        ProjectId = p.Key,
            //        Backer = p.Count(),
            //        Sum = p.Sum(oi => oi.Benefit.BenefitPrice),
            //        AskedFund = p.ToDictionary(a => a, a => Project.Askedfund.Value)
            //    })
            //    .ToListAsync();

            return View(usercontext);
        }
        
        private string _GetPersonId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
