using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt.Attributes;
using Projekt.Interfaces;
using Projekt.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IImagesService _imageService;
        private readonly IPostsService _postsService;

        [BindProperty]
        public IFormFile[] FormImages  { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IImagesService imageService, IPostsService postsService)
        {
            _logger = logger;
            _imageService = imageService;
            _postsService = postsService;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            Images = _imageService.SaveImages(FormImages);

            return Page();
        }
    }
}