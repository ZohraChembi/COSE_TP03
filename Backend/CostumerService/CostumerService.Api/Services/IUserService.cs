using CostumerService.Api.Dtos;
using Microsoft.AspNetCore.Identity;

namespace CostumerService.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(string id);
        Task<IdentityResult> CreateAsync(UserRequest request);
        Task<IdentityResult> PatchAsync(string id, UserPatchRequest request);

        Task<IdentityResult> DeleteAsync(string id);
    }
}
