using Microsoft.AspNetCore.Identity;
using MyWebApi.Domain.Interfaces;
using MyWebApi.Domain.Models;

namespace MyWebApi.Application.Services
{
    public class UserService(UserManager<IdentityUser> _userManager) : IUserService
    {

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            var user = new IdentityUser { UserName = model.ClientId, Email = model.ClientId };
            var result = await _userManager.CreateAsync(user, model.ClientSecret);
            return result;
        }

        public async Task<UserDetailsModel> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserDetailsModel
            {
                UserId = user.Id!,
                ClientId = user.Email!,
            };
        }

        public async Task<IdentityResult> UpdateUserAsync(string userId, UpdateModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            user.UserName = model.ClientId;
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            return result;
        }
        public async Task<IEnumerable<UserDetailsModel>> GetAllUsersAsync()
        {
            var users = await Task.FromResult(_userManager.Users.ToList());
            return users.Select(user => new UserDetailsModel
            {
                UserId = user.Id,
                ClientId = user.UserName
            });
        }

    }
}