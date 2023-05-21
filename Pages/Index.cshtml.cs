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
        private readonly IImageService _imageService;

        [Required]
        [FormImage]
        [BindProperty]
        public IFormFile FormImage  { get; set; }

        public Image Image { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IImageService imageService)
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

            Image = _imageService.SaveImage(FormImage);

            return Page();
        }
    }
}