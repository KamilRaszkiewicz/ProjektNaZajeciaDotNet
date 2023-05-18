namespace Projekt.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string IP { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }
    }
}
