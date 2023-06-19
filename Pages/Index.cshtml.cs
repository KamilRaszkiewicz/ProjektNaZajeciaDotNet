using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt.Attributes;
using Projekt.Dtos.Posts;
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

        private readonly UserManager<User> _userManager;

        [BindProperty]
        public CreatePostDto CreatePostForm { get; set; }

        public IEnumerable<PostDto> Posts { get; set; } = Enumerable.Empty<PostDto>();


        public IndexModel(
            ILogger<IndexModel> logger,
            IImagesService imageService,
            IPostsService postsService,
            UserManager<User> userManager)
        {
            _logger = logger;
            _imageService = imageService;
            _postsService = postsService;
            _userManager = userManager;
        }

        public void OnGet(int? pageIndex)
        {
            int? currentUserId = int.TryParse(_userManager.GetUserId(User), out var temp) ? temp : null;
            var isAdmin = User.IsInRole("admin");

            Posts = _postsService.GetPosts(currentUserId, isAdmin, pageIndex ?? 1);
        }
        public IActionResult OnPost(int? pageIndex)
        {
            int? currentUserId = int.TryParse(_userManager.GetUserId(User), out var temp) ? temp : null;
            var isAdmin = User.IsInRole("admin");

            if (ModelState.IsValid && currentUserId != null)
            {
                _postsService.CreatePost(CreatePostForm, currentUserId.Value);
            }

            Posts = _postsService.GetPosts(currentUserId, isAdmin, pageIndex ?? 1);


            return Page();
        }

        public void OnGetDeletePost(int postId)
        {
            int? currentUserId = int.TryParse(_userManager.GetUserId(User), out var temp) ? temp : null;
            var isAdmin = User.IsInRole("admin");

            _postsService.DeletePost(currentUserId, isAdmin, postId);

            Response.Redirect(Request.Headers["Referer"].ToString());
        }

    }
}