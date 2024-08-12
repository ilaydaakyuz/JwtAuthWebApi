using Microsoft.AspNetCore.Mvc;
using MyWebApi.Domain.Interfaces;
using MyWebApi.Domain.Models;


namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService _userService, ILogger<UserController> _logger) : ControllerBase
{


    [HttpPost("Create")]
    public async Task<IActionResult> CreateUser(RegisterModel model)
    {
        try
        {
            var result = await _userService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully: {Email}", model.ClientId);
                return Ok(new { message = "User created successfully" });
            }
            else
            {
                _logger.LogError("User creation failed: {@Errors}", result.Errors);
                return BadRequest(new { errors = result.Errors });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user creation");
            return StatusCode(500, "An error occurred during user creation. Please try again later.");
        }
    }

    [HttpGet("Read")]
    public async Task<IActionResult> GetUser(string userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user != null)
            {
                // Return user information as needed
                return Ok(user);
            }
            else
            {
                return NotFound("User not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching user");
            return StatusCode(500, "An error occurred while fetching user. Please try again later.");
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateUser(string userId,UpdateModel model)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(userId, model);

            if (result.Succeeded)
            {
                _logger.LogInformation("User updated successfully: {UserId}", userId);
                return Ok(new { message = "User updated successfully" });
            }
            else
            {
                _logger.LogError("User update failed: {@Errors}", result.Errors);
                return BadRequest(new { errors = result.Errors });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user update");
            return StatusCode(500, "An error occurred during user update. Please try again later.");
        }
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(userId);

            if (result.Succeeded)
            {
                _logger.LogInformation("User deleted successfully: {UserId}", userId);
                return Ok(new { message = "User deleted successfully" });
            }
            else
            {
                _logger.LogError("User deletion failed: {@Errors}", result.Errors);
                return BadRequest(new { errors = result.Errors });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during user deletion");
            return StatusCode(500, "An error occurred during user deletion. Please try again later.");
        }
    }
}

