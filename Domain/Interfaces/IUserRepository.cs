using MyWebApi.Domain.Models;

namespace MyWebApi.Domain.Interfaces;

public interface IUserRepository
{
    Task<UserDetailsModel> GetUserByIdAsync(string userId);
    Task<UserDetailsModel> GetUserByEmailAsync(string email);
    Task<UserDetailsModel> AddUserAsync(UserDetailsModel user);
    Task<UserDetailsModel> UpdateUserAsync(UserDetailsModel user);
    Task<bool> DeleteUserAsync(string userId);
}
