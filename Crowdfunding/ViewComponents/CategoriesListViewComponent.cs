using Crowdfunding.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfunding.ViewComponents 
{
    [ViewComponent(Name = "CategoriesDropdownListComponent")]
    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly CrowdfundingContext _context;

        public CategoriesListViewComponent(CrowdfundingContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetListCategoriesAsync();
            return View(items);
        }

        private Task<List<Category>> GetListCategoriesAsync()
        {
            return _context.Category.ToListAsync();
        }
    }
}

