using System.Text.Json.Serialization;

namespace Habr.Common.DTOs.Comments
{
    public class ReplyCommentDto
    {
        public string Text { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public int PostId { get; set; }

        public int ParentCommentId { get; set; }
    }
}
