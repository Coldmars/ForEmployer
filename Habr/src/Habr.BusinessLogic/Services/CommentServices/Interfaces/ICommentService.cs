using Habr.Common.DTOs.Comments;

namespace Habr.BusinessLogic.Services.CommentServices.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> LeaveCommentAsync(AddCommentDto commentDto,
                                           CancellationToken cancellationToken);

        Task<CommentDto> ReplyToCommentAsync(ReplyCommentDto commentDto,
                                             CancellationToken cancellationToken);

        Task<CommentDto> GetCommentAsync(int id,
                                         CancellationToken cancellationToken);

        Task DeleteCommentAsync(int commentId,
                                int userId,
                                CancellationToken cancellationToken);
    }
}
