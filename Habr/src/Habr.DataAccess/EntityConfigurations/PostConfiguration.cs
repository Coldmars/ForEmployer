using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Habr.DataAccess.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {

            ConfigurePostTable(builder);

            SetRelationPostAndComment(builder);
        }

        private void ConfigurePostTable(EntityTypeBuilder<Post> builder)
        {
            builder
                .Property(post => post.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(post => post.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(post => post.Text)
                .IsRequired()
                .HasMaxLength(2000);
        }

        private void SetRelationPostAndComment(EntityTypeBuilder<Post> builder) =>
            builder
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId);
    }
}