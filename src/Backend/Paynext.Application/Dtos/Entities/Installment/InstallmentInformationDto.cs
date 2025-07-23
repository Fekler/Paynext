namespace Paynext.Application.Dtos.Entities.Installment
{
    public class InstallmentInformationDto
    {
        public Guid InstallmentId { get; set; }
        public DateTime DuaDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public bool Antecipated { get; set; }
    }
}
