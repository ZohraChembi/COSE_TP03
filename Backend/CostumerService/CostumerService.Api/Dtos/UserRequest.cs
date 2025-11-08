using System.ComponentModel.DataAnnotations;

namespace CostumerService.Api.Dtos
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MinLength(12)]
        public string Password { get; set; } = string.Empty;
    }
}
