using Microsoft.AspNetCore.Identity;
using MyWebApi.Models;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<UserDetailsModel?> GetUserByIdAsync(string userId);
    Task<IdentityResult> UpdateUserAsync(string userId, UpdateModel model);
    Task<IdentityResult> DeleteUserAsync(string userId);
}
