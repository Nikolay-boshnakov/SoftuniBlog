using Blog.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class DrawingViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [Url]
        [ImageUrl]
        [Display(Name ="Image URL")]
        public string ImageUrl { get; set; }

        public string ArtistId { get; set; }

        public int Likes { get; set; }
    }
}