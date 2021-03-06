namespace Habr.DataAccess.Entities
{
    public class Draft
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
