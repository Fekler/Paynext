using Paynext.Domain.Entities._bases;
using Paynext.Domain.Errors;
using Paynext.Domain.Validations;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Domain.Entities
{
    public class User : EntityBase
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Document { get; set; }
        public UserRole UserRole { get; set; }

        public bool IsActive { get; set; } = true;

        public string ClientId { get; set; }
        public string ForeignKey { get; set; }
        public ICollection<Contract> Contracts { get; set; } = [];

        public ICollection<Installment> ActionedInstallments { get; set; } = [];

        public override void Validate()
        {

            base.Validate();

            RuleValidator.Build()
                .When(string.IsNullOrWhiteSpace(FullName) || FullName?.Length > Const.NAME_MAX_LENGTH, Error.INVALID_NAME)
                .When(string.IsNullOrWhiteSpace(Email) || !string.IsNullOrWhiteSpace(Email) && !Validations.Rules.IsValidEmail(Email), Error.INVALID_EMAIL)
                .When(string.IsNullOrWhiteSpace(Phone) || !string.IsNullOrWhiteSpace(Phone) && !Validations.Rules.IsValidPhone(Phone), Error.INVALID_PHONE)
                //.When(string.IsNullOrWhiteSpace(Password) || !Validations.Rules.IsValidPassword(Password), Error.INVALID_PASSWORD)
                .When(string.IsNullOrWhiteSpace(Document), Error.INVALID_DOCUMENT)
                .ThrowExceptionIfExists();
        }
    }
}
