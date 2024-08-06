using Microsoft.AspNetCore.Identity;
using MyWebApi.Models;


public class UserRepository(UserManager<IdentityUser> _userManager) : IUserRepository
{
   

    public async Task<UserDetailsModel?> GetUserByIdAsync(string userId)
{
    var user = await _userManager.FindByIdAsync(userId);
    return MapIdentityUserToUserDetailsModel(user);
}

public async Task<UserDetailsModel?> GetUserByEmailAsync(string email)
{
    var user = await _userManager.FindByEmailAsync(email);
    return MapIdentityUserToUserDetailsModel(user);
}

    public async Task<UserDetailsModel?> AddUserAsync(UserDetailsModel user)
    {
        var identityUser = new IdentityUser { UserName = user.ClientId, Email = user.ClientId };
        var result = await _userManager.CreateAsync(identityUser);

        if (result.Succeeded)
        {
            return MapIdentityUserToUserDetailsModel(identityUser);
        }
        else
        {
            return null;
        }
    }

    public async Task<UserDetailsModel?> UpdateUserAsync(UserDetailsModel user)
    {
        var identityUser = await _userManager.FindByIdAsync(user.UserId);
        if (identityUser == null)
        {
            return null;
        }

        identityUser.Email = user.ClientId;

        var result = await _userManager.UpdateAsync(identityUser);

        if (result.Succeeded)
        {
            return MapIdentityUserToUserDetailsModel(identityUser);
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var identityUser = await _userManager.FindByIdAsync(userId);
        if (identityUser == null)
        {
            return false; 
        }

        var result = await _userManager.DeleteAsync(identityUser);
        return result.Succeeded;
    }

private UserDetailsModel MapIdentityUserToUserDetailsModel(IdentityUser? user)
{
    if (user == null)
    {
        throw new ArgumentNullException(nameof(user));
    }

    return new UserDetailsModel
    {
        UserId = user.Id,
        ClientId = user.Email ?? throw new ArgumentNullException(nameof(user.Email)),
    };
}


}
