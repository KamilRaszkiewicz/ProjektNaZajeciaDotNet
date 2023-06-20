using Projekt.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Dtos.Posts
{
    public class CreatePostDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
        public string TagsString { get; set; }

        [FormImages]
        public IFormFile[]? FormFiles { get; set; } 
    }
}
