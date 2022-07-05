using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Habr.DataAccess.EntityConfigurations
{
    public class DraftConfiguration : IEntityTypeConfiguration<Draft>
    {
        public void Configure(EntityTypeBuilder<Draft> builder)
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
    }
}
