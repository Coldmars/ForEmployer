using Habr.BusinessLogic.Services.PostServices.Interfaces;
using Habr.Common.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habr.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/posts")]
    public class PostController : UserIdController
    {
        private readonly IPostService _postService;
        private readonly IDraftService _draftService;

        public PostController(IPostService postService,
                              IDraftService draftService)
        {
            _postService = postService;
            _draftService = draftService;
        }

        [HttpPost]
        public async Task<ActionResult> AddPostAsync([FromBody] DraftPostDto postDto,
                                                     CancellationToken cancellationToken = default)
        {
            postDto.UserId = UserID;
            var post = await _postService.PublishPostAsync(postDto, cancellationToken);
            return CreatedAtAction(nameof(GetPostAsync), new { postId = post.Id}, post);
        }

        [HttpPost]
        [Route("drafts")]
        public async Task<ActionResult> AddDraftAsync([FromBody] DraftPostDto postDto,
                                                      CancellationToken cancellationToken = default)
        {
            postDto.UserId = UserID;
            var draft = await _draftService.AddDraftAsync(postDto, cancellationToken);
            return CreatedAtAction(nameof(GetCurrentUserDraftsAsync), draft);
        }

        [HttpPost]
        [Route("{draftId}")]
        public async Task<ActionResult> PublishDraftAsync([FromRoute] int draftId,
                                                          CancellationToken cancellationToken = default)
        {
            var post = await _draftService.PublishDraftAsync(draftId, cancellationToken);
            return CreatedAtAction(nameof(GetPostAsync), new { postId = post.Id }, post);
        }

        [HttpPost]
        [Route("drafts/{postId}")]
        public async Task<ActionResult> MovePostToDraftAsync([FromRoute] int postId,
                                                             CancellationToken cancellationToken)
        {
            var draft = await _draftService.MovePostToDraftAsync(postId, UserID, cancellationToken);
            return CreatedAtAction(nameof(GetCurrentUserDraftsAsync), draft);
        }

        [HttpPut]
        [Route("drafts/{draftId}")]
        public async Task<ActionResult> EditDraftAsync([FromBody] ChangingDraftDto edited,
                                                       [FromRoute] int draftId,
                                                       CancellationToken cancellationToken = default)
        {
            var draft = await _draftService.EditDraftAsync(edited, draftId, UserID, cancellationToken);
            return Ok(draft);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<PublishedPostDto>> GetAllPostsAsync(CancellationToken cancellationToken = default) =>
            await _postService.GetOrderedListOfPostsAsync(cancellationToken);
        

        [AllowAnonymous]
        [HttpGet]
        [Route("{postId}")]
        public async Task<PublishedPostDto> GetPostAsync([FromRoute] int postId) => 
            await _postService.OpenSelectedPostAsync(postId);
        

        [HttpGet]
        [Route("drafts")]
        public async Task<IEnumerable<DraftDto>> GetCurrentUserDraftsAsync(CancellationToken cancellationToken = default) =>
            await _draftService.GetDraftsByUserIdAsync(UserID, cancellationToken);
        

        [HttpDelete]
        [Route("{postId}")]
        public async Task<ActionResult> RemovePostAsync([FromRoute] int postId, 
                                                        CancellationToken cancellationToken = default)
        {
            await _postService.RemovePostAsync(postId, UserID, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        [Route("drafts/{draftId}")]
        public async Task<ActionResult> RemoveDraftAsync([FromRoute] int draftId,
                                                         CancellationToken cancellationToken = default)
        {
            await _draftService.RemoveDraftAsync(draftId, UserID, cancellationToken);
            return NoContent();
        }
    }
}
