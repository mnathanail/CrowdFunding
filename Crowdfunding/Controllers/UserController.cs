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

            var usercontext = await _context.Project
                .Include(u => u.User)
                //.Include(b => b.UsersBenefits)
                .Where(p => p.UserId == userId)
                .Select(p => new Dashboard
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Amount = p.AskedFund
                })
                .ToListAsync();

            foreach (var item in usercontext)
            {
                item.Backers = await _context.UsersBenefits
                .Where(p => p.ProjectId == item.ProjectId)
                .Select(i => i.Benefit).CountAsync();

                item.Sum = await _context.UsersBenefits
                .Where(p => p.ProjectId == item.ProjectId)
                .Select(i => i.Benefit.BenefitPrice).SumAsync();
            }
            
            return View(usercontext);
        }

        private string _GetPersonId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
