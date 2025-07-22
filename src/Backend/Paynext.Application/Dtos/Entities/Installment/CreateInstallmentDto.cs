using System.ComponentModel.DataAnnotations;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Application.Dtos.Entities.Installment
{
    public class CreateInstallmentDto
    {
        [Required]
        public Guid ContractUuid { get; set; }
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsAntecipated { get; set; } = false;
        public InstallmentStatus Status { get; set; } = InstallmentStatus.Open;
    }
}
