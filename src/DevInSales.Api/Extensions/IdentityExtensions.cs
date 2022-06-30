using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DevInSales.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(q => q.User.RequireUniqueEmail = true);

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
        }
    }
}
