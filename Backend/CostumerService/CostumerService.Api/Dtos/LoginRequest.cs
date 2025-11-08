using System.ComponentModel.DataAnnotations;

namespace CostumerService.Api.Dtos
{
    public class LoginRequest
    {

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
