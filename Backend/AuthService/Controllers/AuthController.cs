using System;
using AuthService.Configurations;
using AuthService.Models;
using registerReq = AuthService.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models.Dtos;
namespace AuthService.Controllers;

public class AuthController: ControllerBase
{
    private readonly UserManager<TenantUser> _userManager;
    private readonly JwtConfiguration _config;

    public AuthController(UserManager<TenantUser> userManager, JwtConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    // Register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] registerReq.RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tenantUser = new TenantUser
        {
            UserName = request.Email,
            Email = request.Email,
            TenantId = request.TenantId,
            Role = request.Role
        };

        var result = await _userManager.CreateAsync(tenantUser, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User registered successfully.");
    }

    //New User by admin
    [HttpPost("addUserToTenant")]
    public async Task<IActionResult> AddUserToTenant([FromBody] RegisterRequest request)
    {
        // Get the current logged-in user
        var currentUser = await _userManager.GetUserAsync(User); // Get the user from the current HTTP request

        if (currentUser == null)
        {
            return Unauthorized("User not found.");
        }

        // Ensure user has the right role (Owner/Admin)
        if (currentUser.Role != "Owner" && currentUser.Role != "Admin")
        {
            return Unauthorized("Only owners and admins can add new users.");
        }

        // Get TenantId from the current user's claims if needed
        var tenantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TenantId");
        if (tenantIdClaim != null)
        {
            var tenantId = Guid.Parse(tenantIdClaim.Value);  // Extract TenantId from claims
            currentUser.TenantId = tenantId; // Ensure TenantId is linked to current user
        }

        // Add new user to tenant
        var tenantUser = new TenantUser
        {
            UserName = request.Email,
            Email = request.Email,
            TenantId = currentUser.TenantId, // Associate this user with the same TenantId
            Role = request.Role
        };

        var result = await _userManager.CreateAsync(tenantUser, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User added to tenant successfully.");
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if(!isPasswordValid)
        {
            return Unauthorized("Invalid email or password");
        }

        var token = _config.GenerateJWTToken(user);
        return Ok(new { Token = token });
    }

}
