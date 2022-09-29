using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Aya.Common.Extensions
{
    public class AuthorizeExtensions
    {
        public static IList<AuthorizeAttribute> GetNameAuthorizeAttribute()
        {
            var list = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                .SelectMany(x => x.GetCustomAttributes<AuthorizeAttribute>()))
                .ToList();

            return list;
        }
    }
}