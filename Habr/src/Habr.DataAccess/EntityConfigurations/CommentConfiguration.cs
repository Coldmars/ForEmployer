using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Habr.DataAccess.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            ConfigureCommentTable(builder);

            SetRelationCommentTable(builder);
        }

        private void ConfigureCommentTable(EntityTypeBuilder<Comment> builder)
        {
            builder
                .Property(com => com.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(com => com.Text)
                .IsRequired();

            builder
                .Property(com => com.CreateDate)
                .IsRequired();
        }

        private void SetRelationCommentTable(EntityTypeBuilder<Comment> builder) =>
            builder
                .HasMany(com => com.ChildrenComments)
                .WithOne(com => com.ParentComment)
                .HasForeignKey(com => com.ParentCommentId);
    }
}
