using System.ComponentModel.DataAnnotations;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Application.Dtos.Entities.Installment
{
    public class UpdateInstallmentDto
    {
        public Guid UUID { get; set; }
        [Required]
        public Guid ContractUuid { get; set; }
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsAntecipated { get; set; } = false;
        public Guid? ActionedByUser { get; set; } = null;
        public InstallmentStatus Status { get; set; } = InstallmentStatus.Open;
        public AntecipationStatus? AntecipationStatus { get; set; } = null;
    }
}

