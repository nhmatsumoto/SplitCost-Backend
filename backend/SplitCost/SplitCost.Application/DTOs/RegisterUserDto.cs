using System.ComponentModel.DataAnnotations;

namespace SplitCost.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [MinLength(4, ErrorMessage = "O nome de usuário deve ter pelo menos 4 caracteres.")]
        [MaxLength(20, ErrorMessage = "O nome de usuário deve ter no máximo 20 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O primeiro nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "O primeiro nome deve ter no máximo 50 caracteres.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        [StringLength(50, ErrorMessage = "O sobrenome deve ter no máximo 50 caracteres.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare("Password", ErrorMessage = "A confirmação de senha não corresponde à senha.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
