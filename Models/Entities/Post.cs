using System.ComponentModel.DataAnnotations;

namespace Projekt.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }

        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual IList<Image> Images { get; set; }
        public virtual IList<Tag> Tags { get; set; }
    }
}
