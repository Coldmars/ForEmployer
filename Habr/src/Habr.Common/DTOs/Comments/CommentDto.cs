namespace Habr.Common.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }
    }
}
