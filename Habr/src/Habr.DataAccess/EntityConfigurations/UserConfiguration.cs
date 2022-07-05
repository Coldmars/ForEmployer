using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Habr.DataAccess.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ConfigureUserTable(builder);

            SetRelationUserAndPost(builder);
            SetRelationUserAndComment(builder);
        }

        private void ConfigureUserTable(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(user => user.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasIndex(user => user.Email)
                .IsUnique();

            builder
                .Property(user => user.PasswordHash)
                .IsRequired();
        }

        private void SetRelationUserAndPost(EntityTypeBuilder<User> builder) =>
            builder
                .HasMany(user => user.Posts)
                .WithOne(post => post.User)
                .HasForeignKey(post => post.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        
        private void SetRelationUserAndComment(EntityTypeBuilder<User> builder) =>
            builder
                .HasMany(user => user.Comments)
                .WithOne(comment => comment.User)
                .HasForeignKey(comment => comment.UserId)
                .OnDelete(DeleteBehavior.SetNull);
    }
}
