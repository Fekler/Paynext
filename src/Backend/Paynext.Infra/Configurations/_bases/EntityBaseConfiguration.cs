using Paynext.Domain.Entities._bases;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Paynext.Infra.Configurations._bases
{
    public abstract class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {
        protected string TableName { get; set; } = string.Empty;
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(e => e.UUID);

            builder.Property(e => e.UUID)
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(e => e.Id)
                 .ValueGeneratedOnAdd();
            builder.HasIndex(e => e.Id)
                 .IsUnique();
            builder.Property(e => e.CreateAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(e => e.UpdateAt)
                .IsRequired(false);

        }

    }
}
