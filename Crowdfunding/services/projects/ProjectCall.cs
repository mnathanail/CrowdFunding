using Crowdfunding.Models;
using Crowdfunding.services.projects.call;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Crowdfunding.Utilities;
using Microsoft.AspNetCore.Http;
using Crowdfunding.Utils;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting.Server;
using System.Web;

namespace Crowdfunding.services.projects
{
    public class ProjectCall : IProjectsCall
    {
        private readonly CrowdfundingContext _context;
        private readonly IHostingEnvironment _environment;
        public ProjectCall(CrowdfundingContext context, IHostingEnvironment IHostingEnvironment)
        {
            _context = context;
            _environment = IHostingEnvironment;
        }

        public async Task<Pagination<Project>> ProjectsIndexCall(string searchString, string categorySelection, int? page)
        {
            var crowdfundingContext = _context.Project.Include(p => p.Category).Include(p => p.User);
            var pageSize = 4;
            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
                return await Pagination<Project>.CreateAsync(crowdfundingContext.Where(s => s.ProjectName.ToUpper().Contains(searchString.ToUpper())).AsNoTracking(), page??1 , pageSize);
            }
            else if (!String.IsNullOrEmpty(categorySelection))
            {
                bool isCategoryNameInt = int.TryParse(categorySelection, out int categorySelectionInt);
                if (isCategoryNameInt && categorySelectionInt!= 0)
                {
                    return await Pagination<Project>.CreateAsync(crowdfundingContext.Where(s => s.CategoryId == categorySelectionInt).AsNoTracking(), page ?? 1, pageSize);
                }
                else
                {
                    return await Pagination<Project>.CreateAsync(crowdfundingContext.AsNoTracking(), page ?? 1, pageSize);
                }
            }
            return await Pagination<Project>.CreateAsync(crowdfundingContext.AsNoTracking(), page ?? 1, pageSize);
        }

        public async Task<Pagination<Project>> ProjectsIndexCallJson(int? page)
        {
            var crowdfundingContext = _context.Project;
            var pageSize = 2;
            return await Pagination<Project>.CreateAsync(crowdfundingContext.AsNoTracking(), page ?? 1, pageSize);
        }

        public async Task ProjectsCreateCall(Project project, string userId, IFormFileCollection httpFiles)
        {
            AddMediaFiles(project, userId, httpFiles);
            project.UserId = userId;
            project.StartDate = DateTime.Today.Date;
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        private void AddMediaFiles(Project project, string userId, IFormFileCollection httpFiles)
        {
            if (httpFiles.Count()>0)
            {
                string pathToDir = string.Empty;
                var photoName = "";
                var mediaFile = httpFiles;
                var isValidPhotos = (MediaChecking.checkPhotosContenType(mediaFile) &&
                                    MediaChecking.checkPhotosExtension(mediaFile)) ||
                                    (MediaChecking.checkVideoContenType(mediaFile) &&
                                    MediaChecking.checkVideoExtension(mediaFile));
                var folder = Path.Combine(_environment.WebRootPath, "media");
                var createdDirectory = Directory.CreateDirectory(folder + "\\"+ userId+"\\" + $"{project.ProjectName.ToLower()}");
                project.MediaPath = createdDirectory.ToString();
                foreach (var photo in mediaFile)
                {
                    if (photo.Length > 0)
                    {
                        photoName = ContentDispositionHeaderValue.Parse(photo.ContentDisposition).FileName.Trim('"');
                        pathToDir = createdDirectory + "\\" + photoName;
                    }
                    using (FileStream fs = new FileStream(pathToDir, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        photo.CopyTo(fs);
                        fs.Flush();
                    }
                }

            }
        }
    }
}
