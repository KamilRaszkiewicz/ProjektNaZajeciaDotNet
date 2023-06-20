using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt.Dtos.Comments;
using Projekt.Interfaces;
using Projekt.Models.Entities;

namespace Projekt.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IPostsService _postsService;

        [BindProperty]
        public DeleteCommentDto Form { get; set; }

        public DeleteModel(UserManager<User> userManager, IPostsService postsService)
        {
            _userManager = userManager;
            _postsService = postsService;
        }


    }
}
