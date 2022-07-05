using AutoMapper;
using Habr.BusinessLogic.Services.PostServices;
using Habr.BusinessLogicTests.Helpers;
using Habr.Common.DTOs.Posts;
using Habr.Common.Exceptions;
using Habr.Common.Helpers.AutoMapper;
using Habr.Common.Resourses;
using Habr.DataAccess.Data;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Habr.BusinessLogicTests.Services
{
    public class DraftServiceTests
    {
        private readonly DbContextOptions<DataContext> _dbContextOptions;
        private readonly ILogger<DraftService> _logger;
        private readonly IMapper _mapper;
        private readonly TestEntitiesCreator _entitiesCreator;
        private const int TitleMaxLength = 200;
        private const int TextMaxLength = 2000;

        public DraftServiceTests()
        {
            _logger = new Mock<ILogger<DraftService>>().Object;
            _mapper = GetPostMapper();
            _dbContextOptions = ConfigureInMemoryDatabase();
            _entitiesCreator = new TestEntitiesCreator();
        }

        private CancellationToken CancellationToken = default;

        [Fact]
        public async void PublishPost_ValidDraft_AddToPost()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draft = await AddTestDraftToContext(context, user.Id);
            var draftService = new DraftService(context, _logger, _mapper);

            // Act
            var expectedPostDto = await draftService.PublishDraftAsync(draft.Id, CancellationToken);
            var expectedFalseResult = await context.Drafts.AnyAsync(d => d.Id == draft.Id);
            var actualPostsCollection = _mapper.Map<IEnumerable<PostDto>>(context.Posts);

            // Assert
            Assert.Contains(expectedPostDto, actualPostsCollection);
            Assert.False(expectedFalseResult);
        }

        [Fact]
        public async Task PublishPost_DraftTitleIsNull_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftPostDto = _entitiesCreator.GetDraftPostWithoutTitle(user.Id);
            var invalidDraft = AddInvalidDraftToContext(context, draftPostDto);
            var draftService = new DraftService(context, _logger, _mapper);

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await draftService.PublishDraftAsync(invalidDraft.Id, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidTitleLength, nameof(Post), TitleMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async Task PublishPost_DraftTitleIsOverMax_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftPostDto = _entitiesCreator.GetDraftPostWhereTitleIsOverMax(user.Id, TitleMaxLength);
            var invalidDraft = await AddInvalidDraftToContext(context, draftPostDto);
            var draftService = new DraftService(context, _logger, _mapper);

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await draftService.PublishDraftAsync(invalidDraft.Id, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidTitleLength, nameof(Post), TitleMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async Task PublishPost_DraftTextIsNull_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftPostDto = _entitiesCreator.GetDraftPostWithoutText(user.Id);
            var invalidDraft = AddInvalidDraftToContext(context, draftPostDto);
            var draftService = new DraftService(context, _logger, _mapper);

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await draftService.PublishDraftAsync(invalidDraft.Id, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidFieldLength, nameof(Post), TextMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async Task PublishPost_DraftTextIsOverMax_ThrowValidationException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draftPostDto = _entitiesCreator.GetDraftPostWhereTextIsOverMax(user.Id, TextMaxLength);
            var invalidDraft = await AddInvalidDraftToContext(context, draftPostDto);
            var draftService = new DraftService(context, _logger, _mapper);

            // Act
            var ex = await Assert.ThrowsAsync<ValidationException>(
                                    async () =>
                                        await draftService.PublishDraftAsync(invalidDraft.Id, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidFieldLength, nameof(Post), TextMaxLength);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async Task PublishPost_NotExistDraft_ThrowNotFoundException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var draft = await AddTestDraftToContext(context, user.Id);
            var draftService = new DraftService(context, _logger, _mapper);
            var invalidDraftId = _entitiesCreator.GetInvalidDraftId(context);

            // Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(
                                    async () =>
                                        await draftService.PublishDraftAsync(invalidDraftId, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.NotFound, nameof(Draft));
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async void MovePostToDraft_ValidPost_AddToDraft()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var post = await AddTestPostToContext(context, user.Id);
            var draftService = new DraftService(context, _logger, _mapper);

            // Act
            var expectedDraftDto = await draftService.MovePostToDraftAsync(post.Id, user.Id, CancellationToken);
            var actualDraftsCollection = _mapper.Map<IEnumerable<DraftDto>>(context.Drafts);
            var expectedFalseResult = await context.Posts.AnyAsync(p => p.Id == post.Id);

            // Assert
            Assert.Contains(expectedDraftDto, actualDraftsCollection);
            Assert.False(expectedFalseResult);
        }

        [Fact]
        public async void MovePostToDraft_NotExistPost_ThrowNotFoundException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var post = await AddTestPostToContext(context, user.Id);
            var draftService = new DraftService(context, _logger, _mapper);
            var invalidPostId = _entitiesCreator.GetInvalidPostId(context);

            // Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(
                                    async () =>
                                        await draftService.MovePostToDraftAsync(invalidPostId, user.Id, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.NotFound, nameof(Post));
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async void MovePostToDraft_InvalidAuthor_ThrowForbiddenException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var post = await AddTestPostToContext(context, user.Id);
            var draftService = new DraftService(context, _logger, _mapper);
            var invalidUserId = post.UserId + 1;

            // Act
            var ex = await Assert.ThrowsAsync<ForbiddenException>(
                                    async () =>
                                        await draftService.MovePostToDraftAsync(post.Id, (int)invalidUserId, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.InvalidDraftsUser);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        [Fact]
        public async void MovePostToDraft_PostWithComments_ThrowBusinessLogicException()
        {
            // Arrange
            var context = new DataContext(_dbContextOptions);
            var user = await AddTestUserToContextAsync(context);
            var post = await AddTestPostToContext(context, user.Id);
            var draftService = new DraftService(context, _logger, _mapper);

            var comment = _entitiesCreator.GetTestComment(user.Id, post.Id);
            context.Comments.Add(comment);
            context.SaveChanges();

            // Act 
            var ex = await Assert.ThrowsAsync<BusinessLogicException>(
                                    async () =>
                                        await draftService.MovePostToDraftAsync(post.Id, user.Id, CancellationToken));
            var expectedExMessage = String.Format(ExceptionMessagesResourse.PostHasComments);
            await context.DisposeAsync();

            // Assert
            Assert.Contains(expectedExMessage, ex.Message);
        }

        private DbContextOptions<DataContext> ConfigureInMemoryDatabase()
        {
            var dbName = $"db_{System.DateTime.Now}";

            return new DbContextOptionsBuilder<DataContext>()
                            .UseInMemoryDatabase<DataContext>(dbName)
                            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                            .Options;
        }

        private IMapper GetPostMapper()
        {
            if (_mapper is not null)
                return _mapper;

            var mappingConfig = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new PostProfile());
                });

            return mappingConfig.CreateMapper();
        }

        private async Task<User> AddTestUserToContextAsync(DataContext context)
        {
            var user = _entitiesCreator.GetTestUser();

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        private async Task<Post> AddTestPostToContext(DataContext context, int authorId)
        {
            var draftpostDto = _entitiesCreator.GetDraftPostDto(authorId);
            var post = _mapper.Map<Post>(draftpostDto);

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            return post;
        }

        private async Task<Draft> AddTestDraftToContext(DataContext context, int authorId)
        {
            var draftPostDto = _entitiesCreator.GetDraftPostDto(authorId);
            var draft = _mapper.Map<Draft>(draftPostDto);

            await context.Drafts.AddAsync(draft);
            await context.SaveChangesAsync();

            return draft;
        }

        private async Task<Draft> AddInvalidDraftToContext(DataContext context,
                                                           DraftPostDto invalidDraftPostDto)
        {
            var draft = _mapper.Map<Draft>(invalidDraftPostDto);

            await context.Drafts.AddAsync(draft);
            await context.SaveChangesAsync();

            return draft;
        }
    }
}
