namespace Blog.Controllers
{
    using Blog.Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

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

        public ActionResult Index()
        {
            return RedirectToAction("Show");
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

        public ActionResult Show(int page = 1, string user = null)
        {
            using (var database = new BlogDbContext())
            {
                var pageSize = 2;

                var myDrawings = database.Drawings.AsQueryable();

                if (user != null)
                {
                    myDrawings = myDrawings.Where(a => a.Artist.UserName == user);
                }

                var drawing = myDrawings
                    .OrderByDescending(x => x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include(a => a.Artist)
                    .ToList();

                ViewBag.CurrentPage = page;

                return View(drawing);
            }

        }

        public ActionResult Top()
        {
            using (var database = new BlogDbContext())
            {
                var drawings = database.Drawings
                    .OrderByDescending(x=>x.Likes)
                    .Take(3)
                    .Include(a => a.Artist)
                    .ToList();

                if (drawings == null)
                {
                    return HttpNotFound();
                }

                return View(drawings);
            }
        }

        [Authorize]
        public ActionResult Like(int id)
        {
            using (var database = new BlogDbContext())
            {
                var drawing = database.Drawings
                    .Where(a => a.Id == id)
                    .First(); 

                drawing.Likes++;
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
                var drawing = database.Drawings
                    .Where(a => a.Id == id)
                    .Include(a => a.Artist)
                    .FirstOrDefault();

                if (drawing == null)
                {
                    return HttpNotFound();
                }
                database.Drawings.Remove(drawing);
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