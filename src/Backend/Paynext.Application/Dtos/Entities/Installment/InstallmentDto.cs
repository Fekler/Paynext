using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Application.Dtos.Entities.Installment
{
    public class InstallmentDto
    {
        public Guid UUID { get; set; }
        public Guid ContractUuid { get; set; }
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsAntecipated { get; set; } 
        public InstallmentStatus Status { get; set; }
        public AntecipationStatus? AntecipationStatus { get; set; };

    }
}
