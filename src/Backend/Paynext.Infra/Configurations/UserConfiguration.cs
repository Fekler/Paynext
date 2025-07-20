using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paynext.Domain.Entities;
using Paynext.Domain.Entities._bases;
using Paynext.Infra.Configurations._bases;

namespace Paynext.Infra.Configurations
{
    public class UserConfiguration : EntityBaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            TableName = "Users";

            base.Configure(builder);


            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(Const.NAME_MAX_LENGTH);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(Const.EMAIL_MAX_LENGTH);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Phone)
                .HasMaxLength(Const.PHONE_MAX_LENGTH);

            builder.HasIndex(u => u.Phone)
                .IsUnique();

            builder.Property(u => u.Password)
                .IsRequired();

            builder.Property(u => u.Document)
                .HasMaxLength(Const.DOCUMENT_MAX_LENGTH);

            builder.HasIndex(u => u.Document)
                .IsUnique();

            builder.Property(u => u.UserRole)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);


        }
    }
}