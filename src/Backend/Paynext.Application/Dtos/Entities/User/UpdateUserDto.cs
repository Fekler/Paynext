using Paynext.Domain.Entities._bases;
using Paynext.Domain.Errors;
using System.ComponentModel.DataAnnotations;
using static Paynext.Domain.Entities._bases.Enums;

namespace Paynext.Application.Dtos.Entities.User
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "O ID do Usuário é obrigatório.")]
        public Guid UUID { get; set; }

        [
            Required,
            MinLength(length: 3, ErrorMessage = Error.INVALID_NAME),
            MaxLength(length: Const.NAME_MAX_LENGTH, ErrorMessage = Error.INVALID_NAME)
        ]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [MaxLength(Const.PHONE_MAX_LENGTH, ErrorMessage = "O Telefone não pode exceder 20 caracteres.")]
        public string? Phone { get; set; }

        //[Required(ErrorMessage = "A Senha é obrigatória.")]
        //public string Password { get; set; }

        public string Document { get; set; }

        public string? ClientId { get; set; }

        [Required(ErrorMessage = "O Tipo de Usuário é obrigatório.")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "O Tipo de Usuário não é válido.")]
        public UserRole UserRole { get; set; }
    }
}
