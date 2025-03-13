using ASC.Model.BaseTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ASC.Web.Configuration;
using ASC.Web.Data;
namespace ASC.Web.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
IOptions<ApplicationSettings> options)
        {
            var roles = options.Value.Roles.Split(new char[] { ',' });
            // Create roles if they don't exist 
            foreach (var role in roles)
            {
                try
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                    {
                        IdentityRole storageRole = new IdentityRole
                        {
                            Name = role
                        };
                        IdentityResult roleResult = await roleManager.CreateAsync(storageRole);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            var admin = await userManager.FindByEmailAsync(options.Value.AdminPassword);
            if (admin == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = options.Value.AdminName,
                    Email = options.Value.AdminEmail,
                    EmailConfirmed = true
                };
                IdentityResult result = await userManager.CreateAsync(user, options.Value.AdminPassword);
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", options.Value.AdminEmail));
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsActive", "True"));
                // Add Admin to Admin roles 
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
            // Create a service engineer if he doesn't exist 
            var engineer = await userManager.FindByEmailAsync(options.Value.EngineerEmail);
            if (engineer == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = options.Value.EngineerName,
                    Email = options.Value.EngineerEmail,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                IdentityResult result = await userManager.CreateAsync(user, options.Value.EngineerPassword);
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", options.Value.EngineerEmail));
                // Add Service Engineer to Engineer role 
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsActive", "True"));
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Engineer.ToString());
                }
            }
        }
    }
}