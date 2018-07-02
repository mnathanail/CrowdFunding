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
using MimeKit;

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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TestProject", "testproject@gmail.com"));
            message.To.Add(new MailboxAddress("Hello", "ccrowdfunders.info@gmail.com"));
            message.Subject = "test mail";
            message.Body = new TextPart("plain")
            {
                Text = "Hi! hello world"
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("ccrowdfunders.info@gmail.com", "Crowdfunders.");
                client.Disconnect(true);
            }

            var userId = _GetPersonId();

            var usercontext = await _context.Project
                .Include(u => u.User)
                .Select(p => new Dashboard
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Amount = p.AskedFund
                })
                .ToListAsync();

            foreach (var item in usercontext)
            {
                //item.Backers = await _context.UsersBenefits
                //.GroupBy(x => x.UserId).Select(x => x.First())
                //.Where(p => p.ProjectId == item.ProjectId)
                //.Select(i => i.Benefit)
                //.CountAsync();

                item.Sum = await _context.UsersBenefits
                .Where(p => p.ProjectId == item.ProjectId)
                .Select(i => i.Benefit.BenefitPrice).SumAsync();
            }
            //var sortedList = usercontext.OrderByDescending(l => l.Backers).Take(10).ToList();
            var sortedListFunds = usercontext.OrderByDescending(f => f.Sum).Take(10).ToList();

            return View(sortedListFunds);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Our application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Our contact page.";

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
