namespace Blog.Models
{
    using Blog.Validations;
    using System.ComponentModel.DataAnnotations;

    public class PhotoViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [Url]
        [ImageUrl]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        public string ArtistId { get; set; }

        public int Likes { get; set; }
    }
}