using Microsoft.AspNetCore.Identity;
using MyWebApi.Domain.Models;

namespace MyWebApi.Domain.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    Task<UserDetailsModel> GetUserByIdAsync(string userId);
    Task<IdentityResult> UpdateUserAsync(string userId, UpdateModel model);
    Task<IdentityResult> DeleteUserAsync(string userId);
}
