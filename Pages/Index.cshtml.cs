using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [Required]
        [FormImage]
        [BindProperty]
        public IFormFile Image  { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {

            return Page();
        }
    }
}