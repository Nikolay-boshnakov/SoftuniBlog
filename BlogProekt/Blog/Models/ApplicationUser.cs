using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public ApplicationUser()
        {
            this.userArticle = new HashSet<Article>();
            this.userDrawing = new HashSet<Drawing>();
        }
        public virtual ICollection<Drawing> userDrawing { get; set; }

        public virtual ICollection<Article> userArticle { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            //this.FullName = 
            return userIdentity;
        }
    }
}