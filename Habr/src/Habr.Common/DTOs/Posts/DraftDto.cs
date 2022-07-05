namespace Habr.Common.DTOs.Posts
{
    public class DraftDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastUpdated { get; set; }

        public override bool Equals(object obj)
        {
            return this.Id == (obj as DraftDto).Id &&
                   this.Title == (obj as DraftDto).Title &&
                   this.Text == (obj as DraftDto).Text;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Title, this.Text);
        }
    }
}
