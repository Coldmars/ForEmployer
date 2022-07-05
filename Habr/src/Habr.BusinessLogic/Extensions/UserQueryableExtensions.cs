using Habr.Common.DTOs.Posts;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habr.BusinessLogic.Extensions
{
    public static class UserQueryableExtensions
    {
        public static IQueryable<User> GetUserByEmail(this IQueryable<User> users, 
                                                      string email)
        {
            return users
                .Where(u => u.Email == email)
                .AsNoTracking();
        }

        public static IQueryable<string> GetUserNameById(this IQueryable<User> users,
                                                         int userId)
        {
            return users
                .Where(user => user.Id == userId)
                .Select(n => n.Name)
                .AsNoTracking();
        }
    }
}
