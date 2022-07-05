using Habr.BusinessLogic.Services.CommentServices.Interfaces;
using Habr.Common.DTOs.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/comments")]
    public class CommentController : UserIdController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService serivce)
        {
            _commentService = serivce;
        }

        [HttpPost]
        public async Task<ActionResult> LeaveCommentAsync([FromBody] AddCommentDto commentDto, 
                                                          CancellationToken cancellationToken = default)
        {
            commentDto.UserId = UserID;

            var comment = await _commentService
                                    .LeaveCommentAsync(commentDto, cancellationToken);

            return CreatedAtAction(nameof(GetCommentAsync), new {commentId = comment.Id}, comment);
        }

        [HttpPost]
        [Route("reply")]
        public async Task<ActionResult> ReplyToCommentAsync([FromBody] ReplyCommentDto commentDto, 
                                                             CancellationToken cancellationToken = default)
        {
            commentDto.UserId = UserID;

            var comment = await _commentService
                                    .ReplyToCommentAsync(commentDto, cancellationToken);

            return CreatedAtAction(nameof(GetCommentAsync), new { commentId = comment.Id }, comment);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{commentId}")]
        public async Task<ActionResult<CommentDto>> GetCommentAsync([FromRoute] int commentId, 
                                                                     CancellationToken cancellationToken = default) =>
            await _commentService.GetCommentAsync(commentId, cancellationToken);
        

        [HttpDelete]
        [Route("{commentId}")]
        public async Task<ActionResult> DeleteCommentAsync([FromRoute] int commentId, 
                                                            CancellationToken cancellationToken = default)
        {
            await _commentService.DeleteCommentAsync(commentId, UserID, cancellationToken);

            return NoContent();
        }
    }
}
