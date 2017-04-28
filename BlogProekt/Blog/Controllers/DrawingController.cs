using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class DrawingController : Controller
    {

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {

                var drawing = database.Drawings
                    .Where(a => a.Id == id)
                    .Include(a => a.Artist)
                    .FirstOrDefault();

                if (!EditAuthorized(drawing))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (drawing == null)
                {
                    return HttpNotFound();
                }

                return View(drawing);
            }
        }

        //
        // GET: Article/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {

                var drawing = database.Drawings
                    .Where(a => a.Id == id)
                    .First();

                if (!EditAuthorized(drawing))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (drawing == null)
                {
                    return HttpNotFound();
                }
                var model = new DrawingViewModel();
                model.Id = drawing.Id;
                model.Title = drawing.Title;
                model.ImageUrl = drawing.ImageUrl;             
                return View(model);
            }
        }
        //
        // Post: Article/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(DrawingViewModel model)
        {
            if (ModelState.IsValid)
            {

                using (var database = new BlogDbContext())
                {

                    var drawing = database.Drawings
                        .FirstOrDefault(a => a.Id == model.Id);


                    if (drawing == null)
                    {
                        return HttpNotFound();
                    }

                    drawing.Title = model.Title;
                    drawing.ImageUrl = model.ImageUrl;

                    database.Entry(drawing).State = EntityState.Modified;
                    database.SaveChanges();

                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return RedirectToAction("Show");
        }

        public ActionResult Show()
        {
            using (var database = new BlogDbContext())
            {
                var drawing = database.Drawings
                    .Include(a => a.Artist)
                    .ToList();


                return View(drawing);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var drawing = database.Drawings
                    .Where(a => a.Id == id)
                    .Include(a => a.Artist)
                    .First();

                if (drawing == null)
                {
                    return HttpNotFound();
                }

                return View(drawing);
            }
        }

        // GET: Drawing
        [Authorize]
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upload(Drawing drawing)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var artistId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    drawing.ArtistId = artistId;

                    database.Drawings.Add(drawing);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(drawing);
        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {
            using (var database = new BlogDbContext())
            {
                var drawing = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .FirstOrDefault();

                if (drawing == null)
                {
                    return HttpNotFound();
                }
                database.Articles.Remove(drawing);
                database.SaveChanges();

                return RedirectToAction("Show");
            }
        }

        private bool EditAuthorized(Drawing drawing)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = drawing.IsArtist(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}