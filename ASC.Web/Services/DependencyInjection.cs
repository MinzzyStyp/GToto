using ASC.Business.Interfaces;
using ASC.Business;
using ASC.DataAccess;
using ASC.DataAccess.Interface;
using ASC.Solution.Services;
using ASC.Web.Configuration;
using ASC.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASC.Web.Services
{
    public static class DependencyInjection
    {
        //Config services
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            // Add AddDbContext with connectionString to mirage database
            var connectionString = config.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            //Add Options and get data from appsettings.json with "AppSettings"
            services.AddOptions(); // Option
            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = config.GetSection("Authentication:Google");
                    options.ClientId = config["Google:Identity:ClientId"];
                    options.ClientSecret = config["Google:Identity:ClientSecret"];
                });

            return services;
        }

        //Add service
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            //Add ApplicationDbContext
            services.AddScoped<DbContext, ApplicationDbContext>();

            //Add IdentityUser IdentityUser
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //Add services
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IIdentitySeed, IdentitySeed>();
            services.AddScoped<IunitOfWork, UnitOfWork>();

            // Add Cache, Session
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDistributedMemoryCache(); // Add distributed cache
            services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();

            //....

            //Add RazorPages, MVC
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();

            //Add MasterDataOperations
            services.AddScoped<IMasterDataOperations, MasterDataOperations>();
            services.AddAutoMapper(typeof(ApplicationDbContext));
            //
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            return services;
        }
    }
}
