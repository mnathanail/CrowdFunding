﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using System.Security.Claims;
using MimeKit;

namespace Crowdfunding.Controllers
{
    public class UsersBenefitsController : Controller
    {
        private readonly CrowdfundingContext _context;

        public UsersBenefitsController(CrowdfundingContext context)
        {
            _context = context;
        }

        // GET: UsersBenefits
        public async Task<IActionResult> Index()
        {
            var crowdfundingContext = _context.UsersBenefits
                .Include(u => u.Benefit)
                .Include(u => u.User)
                .Include(u => u.Project);
            return View(await crowdfundingContext.ToListAsync());
        }

        // GET: UsersBenefits/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersBenefits = await _context.UsersBenefits
                .Include(u => u.Benefit)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (usersBenefits == null)
            {
                return NotFound();
            }

            return View(usersBenefits);
        }

        // GET: UsersBenefits/Create
        public IActionResult Create()
        {
            ViewData["BenefitId"] = new SelectList(_context.Benefit, "BenefitId", "BenefitDesciption");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: UsersBenefits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BenefitId,UserId,ProjectId")] UsersBenefits usersBenefits)
        {
            var userId = _GetPersonId();
            if (ModelState.IsValid)
            {
                var hasBenefit = await _context.UsersBenefits.AnyAsync(u => u.UserId == _GetPersonId() && u.BenefitId == usersBenefits.BenefitId);
                if (hasBenefit)
                {
                    return Json(new
                    {
                        status = hasBenefit,
                        message = "You have already bought this package, please select another one!"
                    });
                }
                else
                {
                    usersBenefits.UserId = _GetPersonId();
                    _context.Add(usersBenefits);
                    await _context.SaveChangesAsync();
                    await _SendMailAsync();
                    return Json(new
                    {
                        status = hasBenefit,
                        message = "Congratulations! Smart choice to fund this project!"
                    });
                }
            }
            ViewData["BenefitId"] = new SelectList(_context.Benefit, "BenefitId", "BenefitDesciption", usersBenefits.BenefitId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", usersBenefits.UserId);
            return View(usersBenefits);
        }

        // GET: UsersBenefits/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersBenefits = await _context.UsersBenefits.FindAsync(id);
            if (usersBenefits == null)
            {
                return NotFound();
            }
            ViewData["BenefitId"] = new SelectList(_context.Benefit, "BenefitId", "BenefitDesciption", usersBenefits.BenefitId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", usersBenefits.UserId);
            return View(usersBenefits);
        }

        // POST: UsersBenefits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BenefitId,UserId")] UsersBenefits usersBenefits)
        {
            if (id != usersBenefits.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersBenefits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersBenefitsExists(usersBenefits.UserId))
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
            ViewData["BenefitId"] = new SelectList(_context.Benefit, "BenefitId", "BenefitDesciption", usersBenefits.BenefitId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", usersBenefits.UserId);
            return View(usersBenefits);
        }

        // GET: UsersBenefits/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersBenefits = await _context.UsersBenefits
                .Include(u => u.Benefit)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (usersBenefits == null)
            {
                return NotFound();
            }

            return View(usersBenefits);
        }

        // POST: UsersBenefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usersBenefits = await _context.UsersBenefits.FindAsync(id);
            _context.UsersBenefits.Remove(usersBenefits);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersBenefitsExists(string id)
        {
            return _context.UsersBenefits.Any(e => e.UserId == id);
        }

        private string _GetPersonId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task _SendMailAsync()
        {
            var usercontext = await _context.Project
                                .Join(_context.UsersBenefits,
                                p => p.ProjectId,
                                ub => ub.ProjectId,
                                (p, ub) => new
                                {
                                    pname = p.ProjectName,
                                    asked = p.AskedFund,
                                    email = p.User.Email,
                                    pid = p.ProjectId,
                                    ubid = ub.ProjectId
                                })
                                .Where(p => p.pid == p.ubid)
                                .ToListAsync();
            decimal sum = 0;
            var askedFund = 0M;
            var pname = "";
            var email = "";
            foreach (var item in usercontext)
            {
                sum = await _context.UsersBenefits
               .Where(p => p.ProjectId == item.pid)
               .Select(i => i.Benefit.BenefitPrice).SumAsync();
                askedFund = item.asked;
                email = item.email;
                pname = item.pname;
            }
            if (sum > askedFund)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(pname, "ccrowdfunding@gmail.com"));
                message.To.Add(new MailboxAddress("Hello!", email));

                message.Subject = "Information mail";
                message.Body = new TextPart("plain")
                {
                    Text = $"Your project '{pname}' has been funded"
                };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("ccrowdfunders.info@gmail.com", "Crowdfunders.");
                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
            }
        }
    }
}
