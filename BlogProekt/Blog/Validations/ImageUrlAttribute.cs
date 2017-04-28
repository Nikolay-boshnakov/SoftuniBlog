using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Validations
{
    public class ImageUrlAttribute  : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var ImageUrl = value as string;
            if(ImageUrl == null)
            {
                return true;
            }
            return ImageUrl.EndsWith("jpg") ||
                ImageUrl.EndsWith("gif") ||
                ImageUrl.EndsWith("jpeg") ||
                ImageUrl.EndsWith("img");     
        }
    }
}