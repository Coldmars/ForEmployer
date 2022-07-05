namespace Habr.Common.DTOs.Posts
{
    public class PostDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Edited { get; set; }

        public override bool Equals(object obj)
        {
            return this.Id == (obj as PostDto).Id &&
                   this.Title == (obj as PostDto).Title &&
                   this.Text == (obj as PostDto).Text &&
                   this.Created == (obj as PostDto).Created;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Title, this.Text);
        }
    }
}
