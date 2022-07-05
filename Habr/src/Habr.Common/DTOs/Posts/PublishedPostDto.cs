using Habr.DataAccess.Entities;

namespace Habr.Common.DTOs.Posts
{
    public class PublishedPostDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string AuthorEmail { get; set; }

        public DateTime Created { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
