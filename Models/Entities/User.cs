using Microsoft.AspNetCore.Identity;

namespace Projekt.Models.Entities
{
    public class User : IdentityUser<int>
    {
        public string? Name { get; set; }
        public virtual IList<Post> Posts { get; set; }
        public virtual IList<Comment> Comments { get; set; }
    }
}
