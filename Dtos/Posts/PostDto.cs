using Projekt.Dtos.Comments;

namespace Projekt.Dtos.Posts
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<ImageDto> ImagePaths { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
