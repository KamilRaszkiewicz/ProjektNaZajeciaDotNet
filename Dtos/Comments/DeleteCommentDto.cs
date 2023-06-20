using Microsoft.Build.Framework;

namespace Projekt.Dtos.Comments
{
    public class DeleteCommentDto
    {
        [Required]     
        
        public int CommentId { get; set; }
    }
}
