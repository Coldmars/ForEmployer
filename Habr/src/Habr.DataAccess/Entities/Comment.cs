namespace Habr.DataAccess.Entities
{
    public class Comment
    {
        public Comment()
        {
            ChildrenComments = new List<Comment>();
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public Post Post { get; set; }

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }

        public Comment ParentComment { get; set; }

        public ICollection<Comment> ChildrenComments { get; set; }
    }
}
