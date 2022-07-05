using Habr.Common.DTOs.Posts;

namespace Habr.BusinessLogic.Services.PostServices.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> PublishPostAsync(DraftPostDto postDto,
                                       CancellationToken cancellationToken);

        Task<IEnumerable<PublishedPostDto>> GetOrderedListOfPostsAsync(CancellationToken cancellationToken);

        Task RemovePostAsync(int postId,
                             int userId,
                             CancellationToken cancellationToken);

        Task<PublishedPostDto> OpenSelectedPostAsync(int postId);
    }
}
