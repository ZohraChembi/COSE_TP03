using System.ComponentModel.DataAnnotations;

namespace CostumerService.Api.Dtos
{
    public class UserPatchRequest
    {
        public string? UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
