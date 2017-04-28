namespace Blog.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public ApplicationUser()
        {
            this.userArticle = new HashSet<Article>();
            this.userDrawing = new HashSet<Drawing>();
            this.userPhoto = new HashSet<Photo>();
        }
        public virtual ICollection<Drawing> userDrawing { get; set; }

        public virtual ICollection<Article> userArticle { get; set; }

        public virtual ICollection<Photo> userPhoto { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}