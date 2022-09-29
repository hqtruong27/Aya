using Aya.Bussiness;
using Aya.Bussiness.Interface;
using Aya.Common;
using Aya.Infrastructure;
using Aya.Infrastructure.Models;
using Aya.Infrastructure.UOW;
using Aya.Services;
using Aya.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aya.Core.IoC
{
    public static class InversionOfControl
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            //Signleton
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<AyaDbContext>()
                    .AddDefaultTokenProviders();

            services.AddDbContext(configuration);

            //Scoped
            services.TryAddScoped<IUserValidator<User>, UserValidator<User>>();
            services.TryAddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
            services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<User>, RoleValidator<User>>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
            services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<User>>();

            services.TryAddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, Role>>();
            services.TryAddScoped<UserManager<User>>();
            services.TryAddScoped<RoleManager<Role>>();
            services.TryAddScoped<SignInManager<User>>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<ICategoryService, CategoryService>();

            //Transient

            return services;
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<AyaDbContext>(
                options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString(Database.ConnectionStringName)),
                ServiceLifetime.Singleton
            );
        }
    }
}