namespace Habr.DataAccess.Entities
{
    public class User
    {
        public User()
        {
            Posts = new List<Post>();
            Comments = new List<Comment>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
