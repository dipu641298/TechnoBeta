using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnoBeta.Models.Data;
using TechnoBeta.Models.ViewModels.Pages;

namespace TechnoBeta.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // Declare PageVM
            PageVM model;
            using (Db db = new Db())
            {
                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // Codfirm page exists
                if(dto == null)
                {
                    return Content("The page does not exist");
                
}

                // Init PageVM
                model = new PageVM(dto);
            }

            // Return view with model
            return View(model);
        }

        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            using ( Db db = new Db())
            {
                int id = model.Id;
                string slug = "home";
                PageDTO dto = db.Pages.Find(id);

                dto.Title = model.Title;
                if( model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                if( db.Pages.Where( x => x.Id != id).Any( x => x.Title != model.Title) || db.Pages.Where(x => x.Id != id).Any(x => x.Slug != slug)){
                    ModelState.AddModelError("", "This title or slug exists");
                }


                dto.Slug = slug;
                dto.Title = model.Title;
                dto.HasSidebar = model.HasSidebar;
                dto.Body = model.Body;
                
                db.SaveChanges();
            }

            TempData["SM"] = "You have edited the page";

            return RedirectToAction("EditPage");
            

        }

        public ActionResult PageDetails(int id)
        {
            PageVM model;
            using(Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                if(dto == null)
                {
                    return Content("The page doesn.t exist");
                }
                model = new PageVM(dto);
            }
            return View(model);
        }

        public ActionResult DeletePage(int id)
        {
            using(Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                db.Pages.Remove(dto);
                db.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using(Db db = new Db())
            {
                int count = 1;
                PageDTO dto;
                foreach(var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++; 
                }

                
            }
            
        }

        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarVM model;
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                model = new SidebarVM(dto);

            }
                return View(model);
        }

        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                dto.Body = model.Body;

                db.SaveChanges();
            }

            TempData["SM"] = "Your Sidebar has been edited !";

            return RedirectToAction("EditSidebar");
        }
    }
}