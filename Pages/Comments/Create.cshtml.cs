using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt.Dtos.Comments;
using Projekt.Interfaces;
using Projekt.Models.Entities;

namespace Projekt.Pages
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IPostsService _postsService;

        [BindProperty]
        public CreateCommentDto Form { get; set; }

        public CreateModel(UserManager<User> userManager, IPostsService postsService)
        {
            _userManager = userManager;
            _postsService = postsService;
        }
        public void OnGet()
        {
            Response.Redirect("/");
        }
    
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return BadRequest();

            int? currentUserId = int.TryParse(_userManager.GetUserId(User), out var temp) ? temp : null;

            if (currentUserId == null) return Unauthorized();

            string ip;

            var ipObject = HttpContext.Connection.RemoteIpAddress;

            if (ipObject == null)
            {
                ip = Request.Headers["X-Forwarded-For"].First()!.ToString();
            }
            else
            {
                ip = ipObject.ToString();
            }

            _postsService.CreateComment(Form, currentUserId.Value, ip);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
