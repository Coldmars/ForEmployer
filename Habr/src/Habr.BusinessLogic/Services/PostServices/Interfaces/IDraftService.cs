using Habr.Common.DTOs.Posts;

namespace Habr.BusinessLogic.Services.PostServices.Interfaces
{
    public interface IDraftService
    {
        Task<IEnumerable<DraftDto>> GetDraftsByUserIdAsync(int userId,
                                                           CancellationToken cancellationToken);

        Task<DraftDto> AddDraftAsync(DraftPostDto postDto,
                                     CancellationToken cancellationToken);

        Task<DraftDto> MovePostToDraftAsync(int postId,
                                            int userId,
                                            CancellationToken cancellationToken);

        Task<PostDto> PublishDraftAsync(int draftId,
                                        CancellationToken cancellationToken);

        Task<DraftDto> EditDraftAsync(ChangingDraftDto edited,
                                      int draftId,
                                      int userId,
                                      CancellationToken cancellationToken);

        Task RemoveDraftAsync(int draftId,
                              int userId,
                              CancellationToken cancellationToken);
    }
}
