using Paynext.Application.Dtos.Entities.Installment;

namespace Paynext.Application.Dtos.Entities
{
    public class ContractInformationDto
    {
        public Guid ContractId { get; set; }
        public string ContractNumber { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public List<InstallmentDto> Installments { get; set; } = [];
    }
}
