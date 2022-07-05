using Habr.Common.DTOs.Posts;
using Habr.DataAccess.Data;
using Habr.DataAccess.Entities;
using System.Linq;

namespace Habr.BusinessLogicTests.Helpers
{
    internal class TestEntitiesCreator
    {
        internal User GetTestUser() =>
            new User
            {
                Name = "Test",
                PasswordHash = "Test",
                Email = "test@gmail.com"
            };

        internal DraftPostDto GetDraftPostDto(int userId) =>
           new DraftPostDto
           {
               Title = "title",
               Text = "text",
               UserId = userId
           };

        internal DraftPostDto GetDraftPostWithoutTitle(int userId) =>
            new DraftPostDto
            {
                Text = "text",
                UserId = userId
            };

        internal DraftPostDto GetDraftPostWhereTitleIsOverMax(int userId, int titleMaxLength)
        {
            var overMaxTitle = new string('r', titleMaxLength + 1);

            return new DraftPostDto
            {
                Title = overMaxTitle,
                Text = "text",
                UserId = userId
            };
        }

        internal DraftPostDto GetDraftPostWithoutText(int userId) =>
            new DraftPostDto
            {
                Title = "title",
                UserId = userId
            };

        internal DraftPostDto GetDraftPostWhereTextIsOverMax(int userId, int textMaxLength)
        {
            var overMaxText = new string('r', textMaxLength + 1);

            return new DraftPostDto
            {
                Title = "title",
                Text = overMaxText,
                UserId = userId
            };
        }

        internal Comment GetTestComment(int userId, int postId) =>
            new Comment
            {
                Text = "Test",
                UserId = userId,
                PostId = postId,
                CreateDate = System.DateTime.Now
            };

        internal int GetInvalidPostId(DataContext context)
        {
            if (context.Posts.Count() == 0)
                return 1;

            return context.Posts
                            .OrderBy(p => p.Id)
                            .Last().Id + 1;
        }

        internal int GetInvalidDraftId(DataContext context)
        {
            if (context.Drafts.Count() == 0)
                return 1;

            return context.Drafts
                            .OrderBy(p => p.Id)
                            .Last().Id + 1;
        }
    }
}
