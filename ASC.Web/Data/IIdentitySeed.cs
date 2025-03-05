using ASC.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
namespace ASC.WEB.Data
{
    public interface IIdentitySeed
    {
        Task Seed(UserManager<IdentityUser> userManager,
                  RoleManager<IdentityRole> roleManager,
                  IOptions<ApplicationSettings> options);
    }
}
