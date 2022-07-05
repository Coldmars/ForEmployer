using System.Text.Json.Serialization;

namespace Habr.Common.DTOs.Posts
{
    public class DraftPostDto
    {
        public string Title { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
