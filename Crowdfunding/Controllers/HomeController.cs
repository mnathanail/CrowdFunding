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
using System.Diagnostics;

namespace Crowdfunding.Controllers
{
    public class HomeController : Controller
    {
        private readonly CrowdfundingContext _context;
        public HomeController(CrowdfundingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _GetPersonId();

            var usercontext = await _context.Project
                .Include(u => u.User)
                //.Include(b => b.UsersBenefits)
                //.Where(p => p.UserId == userId)
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
            }
            var sortedList = usercontext.OrderByDescending(l => l.Backers).Take(10)
                             .ToList();


            return View(sortedList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string _GetPersonId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
