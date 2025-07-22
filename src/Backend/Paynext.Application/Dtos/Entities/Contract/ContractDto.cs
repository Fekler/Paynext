using Paynext.Application.Dtos.Entities.Installment;
using Paynext.Application.Dtos.Entities.User;


namespace Paynext.Application.Dtos.Entities.Contract
{
    public class ContractDto
    {
        public Guid Uuid { get; set; }
        public string ContractNumber { get; set; }
        public Guid UserUuid { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal RemainingValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFinished { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public UserDto User { get; set; }

        public ICollection<InstallmentDto> Installments { get; set; } = [];
    }
}
