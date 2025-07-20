using Paynext.Domain.Entities._bases;

namespace Paynext.Domain.Entities
{
    public class Contract : EntityBase
    {
        public string ContractNumber { get; set; }
        public Guid UserUuid { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal RemainingValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFinished { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public User User { get; set; }

        public ICollection<Installment> Installments { get; set; } = [];
    }
}
