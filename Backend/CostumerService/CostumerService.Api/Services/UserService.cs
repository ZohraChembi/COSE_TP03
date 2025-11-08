using CostumerService.Api.Dtos;
using CostumerService.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CostumerService.Api.Services
{
    public class UserService : IUserService
    {
 
        
            private readonly UserManager<User> _userManager;

            public UserService(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IEnumerable<UserResponse>> GetAllAsync()
            {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new UserResponse
                {
                    Id = u.Id,
                    UserName = u?.UserName?? string.Empty,
                    Email = u?.Email ?? string.Empty,
                    PhoneNumber = u?.PhoneNumber ?? string.Empty
                });
            }

            public async Task<UserResponse?> GetByIdAsync(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                return user == null ? null : new UserResponse
                {
                    Id = user?.Id ?? string.Empty,
                    UserName = user?.UserName ?? string.Empty,
                    Email = user?.Email ?? string.Empty,
                    PhoneNumber = user?.PhoneNumber ?? string.Empty
                };
            }

            public async Task<IdentityResult> CreateAsync(UserRequest request)
            {
                var user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                };

                return await _userManager.CreateAsync(user, request.Password);
            }

        public async Task<IdentityResult> PatchAsync(string id, UserPatchRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return IdentityResult.Failed();

            if (!string.IsNullOrEmpty(request.UserName))
                user.UserName = request.UserName;

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!string.IsNullOrEmpty(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!passwordResult.Succeeded)
                    return IdentityResult.Failed(passwordResult.Errors.ToArray());
            }

            return result;
        }


        public async Task<IdentityResult> DeleteAsync(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return IdentityResult.Failed();
                return await _userManager.DeleteAsync(user);
            }
        }
    
}
