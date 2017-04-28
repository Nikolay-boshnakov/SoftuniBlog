namespace Blog.Controllers
{
    using Blog.Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class PhotoController : Controller
    {
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {

                var photo = database.Photos
                    .Where(a => a.Id == id)
                    .Include(a => a.Artist)
                    .FirstOrDefault();

                if (!EditAuthorized(photo))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (photo == null)
                {
                    return HttpNotFound();
                }

                return View(photo);
            }
        }
       

        public ActionResult Index()
        {
            return RedirectToAction("Show");
        }

        public ActionResult Show()
        {
            using (var database = new BlogDbContext())
            {
                var photo = database.Photos
                    .Include(a => a.Artist)
                    .ToList();


                return View(photo);
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
                var photo = database.Photos
                    .Where(a => a.Id == id)
                    .Include(a => a.Artist)
                    .First();

                if (photo == null)
                {
                    return HttpNotFound();
                }

                return View(photo);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upload(Photo photo)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var artistId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    photo.ArtistId = artistId;

                    database.Photos.Add(photo);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(photo);
        }

        public ActionResult Top()
        {
            using (var database = new BlogDbContext())
            {
                var photos = database.Photos
                    .OrderByDescending(x => x.Likes)
                    .Take(3)
                    .ToList();

                if (photos == null)
                {
                    return HttpNotFound();
                }

                return View(photos);
            }
        }

        [Authorize]
        public ActionResult Like(int? id)
        {
            using (var database = new BlogDbContext())
            {
                var photo = database.Photos
                    .Where(a => a.Id == id)
                    .First();

                photo.Likes++;
                database.SaveChanges();

                return RedirectToAction("Show");
            }
        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {
            using (var database = new BlogDbContext())
            {
                var photo = database.Photos
                    .Where(a => a.Id == id)
                    .Include(a => a.Artist)
                    .FirstOrDefault();

                if (photo == null)
                {
                    return HttpNotFound();
                }
                database.Photos.Remove(photo);
                database.SaveChanges();

                return RedirectToAction("Show");
            }
        }

        private bool EditAuthorized(Photo photo)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = photo.IsArtist(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}