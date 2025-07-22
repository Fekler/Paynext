using System.ComponentModel.DataAnnotations;

namespace Paynext.Application.Dtos.Entities.Contract
{
    public class CreateContractDto
    {
        [Required]
        public string ContractNumber { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int InstallmentCount { get; set; }
        public Guid UserUuid { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ClientId { get; set; }
    }
}
