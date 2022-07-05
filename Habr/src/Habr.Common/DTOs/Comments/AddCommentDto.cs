using System.Text.Json.Serialization;

namespace Habr.Common.DTOs.Comments
{
    public class AddCommentDto
    {
        public string Text { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}
