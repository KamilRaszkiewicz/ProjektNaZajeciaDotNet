using Microsoft.AspNetCore.Identity;
using Projekt.Interfaces;
using Projekt.Models.Entities;

namespace Projekt.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;

        public AdminService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);

            if(roles != null)
            {
                if (roles.Contains("Admin")) return false;
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}
