using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Drawing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(222)]
        public string Title { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public string ArtistId { get; set; }

        public int Likes { get; set; }       

        public virtual ApplicationUser Artist { get; set; }

        public bool IsArtist(string name)
        {
            return this.Artist.UserName.Equals(name);
        }
    }
}