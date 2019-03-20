using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnoBeta.Models.Data;
using TechnoBeta.Models.ViewModels.Pages;

namespace TechnoBeta.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // declare list of PageVM
            List<PageVM> pagesList;


            // initialize the list
            using (Db db = new Db())
            {
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            // return view with the list
            return View(pagesList);
        }

        [HttpGet]
        // GET: Admin/Pages/AddPage
        public ActionResult AddPage()
        {

            return View();
        }

        [HttpPost]
        // POST: Admin/Pages/AddPage
        public ActionResult AddPage(PageVM model)
        {
            // Check model state
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {

                // Declare slug
                string slug;

                // Init PageDTO
                PageDTO dto = new PageDTO();

                // DTO title
                dto.Title = model.Title;

                // Check for and set slug if needed
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // Make sure title and slug are unique
                if(db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "Title or Slug already exists");
                    return View(model);
                }

                // DTO the reset
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                // Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            // Set the TempData message
            TempData["SM"] = "You have added a new page.";

            // Redirect
            return RedirectToAction("AddPage");

        }
    }
}