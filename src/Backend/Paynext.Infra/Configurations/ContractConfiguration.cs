using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paynext.Domain.Entities;
using Paynext.Domain.Entities._bases;
using Paynext.Infra.Configurations._bases;

namespace Paynext.Infra.Configurations
{
    public class ContractConfiguration : EntityBaseConfiguration<Contract>
    {
        public override void Configure(EntityTypeBuilder<Contract> builder)
        {
            TableName = "Contracts";

            base.Configure(builder);

            builder.Property(c => c.ContractNumber)
                .IsRequired()
                .HasMaxLength(Const.STRING_MAX_LENGTH);

            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasMaxLength(Const.STRING_MAX_LENGTH);

            builder.Property(c => c.InitialAmount)
                .IsRequired()
                .HasPrecision(18, 2);
            
            builder.Property(c => c.RemainingValue)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(c => c.UserUuid)
                .IsRequired();

            builder.Property(c => c.StartDate)
                .IsRequired();

            builder.Property(c => c.EndDate)
                .IsRequired();

            builder.Property(c => c.IsFinished)
                .IsRequired(false)

                .HasDefaultValue(false);
            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(c => c.Installments)
                .WithOne(i => i.Contract)
                .HasForeignKey(i => i.ContractUuid)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(builder => builder.User)
                .WithMany(user => user.Contracts)
                .HasForeignKey(c => c.UserUuid)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
