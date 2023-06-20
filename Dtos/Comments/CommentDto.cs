namespace Projekt.Dtos.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string? Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string IP { get; set; }

        public bool CanDelete { get; set; }
    }
}
