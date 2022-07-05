using Habr.BusinessLogic.Services.CommentServices;
using Habr.BusinessLogic.Services.CommentServices.Interfaces;
using Habr.BusinessLogic.Services.PostServices;
using Habr.BusinessLogic.Services.PostServices.Interfaces;
using Habr.BusinessLogic.Services.TokenServices;
using Habr.BusinessLogic.Services.UserServices;
using Habr.BusinessLogic.Services.UserServices.Interfaces;
using Habr.DataAccess.Data;
using Habr.Security.Helpers;
using Habr.Security.Helpers.Interfaces;
using Habr.WebApp.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Habr.WebApp.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IDraftService, DraftService>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IExceptionFilter, ExceptionFilter>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenBuilder, TokenBuilder>();

            return services;
        }
    }
}
