using System;
using System.Collections.Generic;
namespace Blog.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Photo
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