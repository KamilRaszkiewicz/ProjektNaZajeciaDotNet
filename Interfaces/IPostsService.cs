using Projekt.Dtos.Comments;
using Projekt.Dtos.Posts;
using Projekt.Models.Entities;

namespace Projekt.Interfaces
{
    public interface IPostsService
    {
        int CreatePost(CreatePostDto dto, int userId);

        IEnumerable<PostDto> GetPosts(int pageNr = 1);

        IEnumerable<PostDto> GetUserPosts(int userId, int pageNr = 1);

        int CreateComment(CreateCommentDto dto, int userId, string IP);

        bool UpdatePost(int postId, CreatePostDto dto);

        bool DeletePost(int postId);

        bool DeleteComment(int commentId);
    }
}
