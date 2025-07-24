using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paynext.Domain.Entities;
using Paynext.Domain.Entities._bases;
using Paynext.Infra.Configurations._bases;

namespace Paynext.Infra.Configurations
{
    public class InstallmentConfiguration : EntityBaseConfiguration<Installment>
    {
        public override void Configure(EntityTypeBuilder<Installment> builder)
        {
            TableName = "Installments";

            base.Configure(builder);

            builder.Property(i => i.InstallmentId)
                .IsRequired()
                .HasMaxLength(Const.STRING_MAX_LENGTH);

            builder.Property(i => i.Value)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(i => i.DueDate)
                .IsRequired();

            builder.Property(i => i.PaymentDate)
                .IsRequired(false);

            builder.Property(i => i.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(i => i.AntecipationStatus)
                .IsRequired(false)
                .HasConversion<string>();

            builder.Property(i => i.IsAntecipated)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(i => i.Contract)
                .WithMany(c => c.Installments)
                .HasForeignKey(i => i.ContractUuid)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(i=> i.ActionedByUserUuiD)
                .IsRequired(false);

            builder.HasOne(i => i.ActionedByUser)
                .WithMany(u => u.ActionedInstallments)
                .HasForeignKey(i => i.ActionedByUserUuiD)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
