using Paynext.Domain.Entities._bases;
using System.ComponentModel.DataAnnotations;

namespace Paynext.Application.Dtos.Entities.User
{
    public class ChangePasswordDto
    {
        [Required]
        [MinLength(Const.PASSWORD_MIN_LENGTH, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        [MaxLength(Const.PASSWORD_MAX_LENGTH, ErrorMessage = "A senha não pode exceder 20 caracteres.")]
        public string Password { get; set; }

        [Required]
        [MinLength(Const.PASSWORD_MIN_LENGTH, ErrorMessage = "A confirmação de senha deve ter pelo menos 6 caracteres.")]
        [MaxLength(Const.PASSWORD_MAX_LENGTH, ErrorMessage = "A confirmação de senha não pode exceder 20 caracteres.")]
        [Compare("Password", ErrorMessage = "A confirmação de senha não confere com a senha.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        [Length(36,36)]
        public Guid UserUuid { get; set; }

        [Required]
        [MinLength(Const.PASSWORD_MIN_LENGTH, ErrorMessage = "A senha antiga deve ter pelo menos 6 caracteres.")]
        [MaxLength(Const.PASSWORD_MAX_LENGTH, ErrorMessage = "A senha antiga não pode exceder 20 caracteres.")]
        public string OldPassword { get; set; }


    }
}
