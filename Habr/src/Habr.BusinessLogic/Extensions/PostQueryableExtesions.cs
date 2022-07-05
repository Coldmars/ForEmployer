using Habr.Common.DTOs.Posts;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habr.BusinessLogic.Extensions
{
    public static class PostQueryableExtesions
    {
        public static IQueryable<PublishedPostDto> GetAllOrderedPosts(this IQueryable<Post> posts)
        {
            return posts
                .Include(p => p.User)
                .Select(p => new PublishedPostDto
                {
                    Title = p.Title,
                    AuthorEmail = p.User.Email,
                    Created = p.Created
                })
               .OrderByDescending(p => p.Created)
               .AsNoTracking();
        }

        public static IQueryable<DraftDto> GetAllOrderedDraftsByUserId(this IQueryable<Draft> drafts, 
                                                                       int userId)
        {
            return drafts
                .Where(d => d.UserId == userId)
                .Select(d => new DraftDto
                {
                    Title = d.Title,
                    Created = d.Created,
                    LastUpdated = d.LastUpdated
                })
               .OrderByDescending(d => d.LastUpdated)
               .AsNoTracking();
        }

        public static IQueryable<PublishedPostDto> GetSelectedPost(this IQueryable<Post> posts, 
                                                                   int postId)
        {
            return posts
                .Where(p => p.Id == postId)
                .Select(p => new PublishedPostDto
                {
                    Title = p.Title,
                    Text = p.Text,
                    AuthorEmail = p.User.Email,
                    Created = p.Created,
                    Comments = p.Comments
                })
                .AsNoTracking();
        }

        public static IQueryable<Post> GetPostWithComments(this IQueryable<Post> posts, 
                                                           int postId)
        {
            return posts
                .Where(p => p.Id == postId)
                .Include(p => p.Comments);
        }
    }
}
