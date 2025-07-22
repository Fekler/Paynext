using Paynext.Domain.Entities._bases;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Domain.Entities
{
    public class Installment : EntityBase
    {
        public string InstallmentId { get; set; }
        public Guid ContractUuid { get; set; }
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public InstallmentStatus Status { get; set; } = InstallmentStatus.Open;
        public Contract Contract { get; set; }
        public bool IsAntecipated { get; set; } = false;

        public Guid? ActionedByUserUuiD { get; set; }
        public User? ActionedByUser { get; set; }
    }
}
