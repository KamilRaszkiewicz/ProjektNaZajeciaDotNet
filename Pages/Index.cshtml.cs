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

        [Required]
        [FormImage]
        [BindProperty]
        public IFormFile[] FormImages  { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IImagesService imageService)
        {
            _logger = logger;
            _imageService = imageService;
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