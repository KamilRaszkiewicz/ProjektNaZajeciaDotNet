using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt.Dtos.Posts;
using Projekt.Interfaces;
using Projekt.Models.Entities;

namespace Projekt.Pages
{
    public class UserPostingsModel : PageModel
    {
        private readonly IPostsService _postsService;
        private readonly UserManager<User> _userManager;
        private readonly IAdminService _adminService;

        public IEnumerable<PostDto> Posts { get; set; } = Enumerable.Empty<PostDto>();
        public string UserName { get; set; }

        public UserPostingsModel(IPostsService postsService, UserManager<User> userManager, IAdminService adminService)
        {
            _postsService = postsService;
            _userManager = userManager;
            _adminService = adminService;
        }

        public async Task OnGetAsync(string userName, int? pageIndex)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) 
            {
                Response.Redirect("/");
                return;
            }
            UserName = userName;

            int? currentUserId = int.TryParse(_userManager.GetUserId(User), out var temp) ? temp : null;
            var isAdmin = User.IsInRole("admin");

            Posts = _postsService.GetUserPosts(currentUserId, isAdmin, user.Id, pageIndex ?? 1);
        }

        public async Task OnGetDeleteUser(string userName)
        {
            if (!User.IsInRole("admin") || userName == User.Identity.Name) return;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return;

            await _adminService.DeleteUserAsync(user.Id);
        }
    }
}
