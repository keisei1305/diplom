using ForumService.Application.DTO;
using ForumService.Core.Entities;
using ForumService.Core.Interfaces;
using ForumService.Application.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumService.Application.Services
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _repo;
        private readonly IForumCommentRepository _commentRepo;
        private readonly IUserInfoClient _userInfoClient;
        
        public ForumService(IForumRepository repo, IForumCommentRepository commentRepo, IUserInfoClient userInfoClient)
        {
            _repo = repo;
            _commentRepo = commentRepo;
            _userInfoClient = userInfoClient;
        }

        public async Task<IEnumerable<ForumDto>> GetAllAsync()
        {
            var forums = await _repo.GetAllAsync();
            return forums.Select(f => new ForumDto
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                GameId = f.GameId,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt
            });
        }

        public async Task<IEnumerable<ForumWithAuthorDto>> GetAllWithAuthorsAsync()
        {
            var forums = await _repo.GetAllAsync();
            var result = new List<ForumWithAuthorDto>();
            
            foreach (var f in forums)
            {
                var author = !string.IsNullOrEmpty(f.AuthorId) ? await _userInfoClient.GetUserShortAsync(f.AuthorId) : null;
                
                var comments = await _commentRepo.GetByForumIdAsync(f.Id);
                var commentsList = comments.ToList();
                
                ForumCommentWithAuthorDto? lastComment = null;
                if (commentsList.Any())
                {
                    var lastCommentEntity = commentsList.OrderByDescending(c => c.CreatedAt).First();
                    var lastCommentAuthor = !string.IsNullOrEmpty(lastCommentEntity.AuthorId) ? await _userInfoClient.GetUserShortAsync(lastCommentEntity.AuthorId) : null;
                    
                    lastComment = new ForumCommentWithAuthorDto
                    {
                        Id = lastCommentEntity.Id,
                        ForumId = lastCommentEntity.ForumId,
                        Author = lastCommentAuthor ?? new UserShortDto { Id = 0, Nickname = "Неизвестно" },
                        Content = lastCommentEntity.Content,
                        CreatedAt = lastCommentEntity.CreatedAt
                    };
                }
                
                result.Add(new ForumWithAuthorDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    GameId = f.GameId,
                    Author = author ?? new UserShortDto { Id = 0, Nickname = "Неизвестно" },
                    LastComment = lastComment,
                    CommentsCount = commentsList.Count,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                });
            }
            
            return result;
        }

        public async Task<ForumDto?> GetByIdAsync(int id)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null) return null;
            return new ForumDto
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                GameId = f.GameId,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt
            };
        }

        public async Task<ForumWithAuthorDto?> GetByIdWithAuthorAsync(int id)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null) return null;
            
            var author = !string.IsNullOrEmpty(f.AuthorId) ? await _userInfoClient.GetUserShortAsync(f.AuthorId) : null;
            
            var comments = await _commentRepo.GetByForumIdAsync(f.Id);
            var commentsList = comments.ToList();
            
            ForumCommentWithAuthorDto? lastComment = null;
            if (commentsList.Any())
            {
                var lastCommentEntity = commentsList.OrderByDescending(c => c.CreatedAt).First();
                var lastCommentAuthor = !string.IsNullOrEmpty(lastCommentEntity.AuthorId) ? await _userInfoClient.GetUserShortAsync(lastCommentEntity.AuthorId) : null;
                
                lastComment = new ForumCommentWithAuthorDto
                {
                    Id = lastCommentEntity.Id,
                    ForumId = lastCommentEntity.ForumId,
                    Author = lastCommentAuthor ?? new UserShortDto { Id = 0, Nickname = "Неизвестно" },
                    Content = lastCommentEntity.Content,
                    CreatedAt = lastCommentEntity.CreatedAt
                };
            }
            
            return new ForumWithAuthorDto
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                GameId = f.GameId,
                Author = author ?? new UserShortDto { Id = 0, Nickname = "Неизвестно" },
                LastComment = lastComment,
                CommentsCount = commentsList.Count,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt
            };
        }

        public async Task<ForumDto> CreateAsync(CreateForumRequest request)
        {
            var forum = new Forum
            {
                Title = request.Title,
                Description = request.Description,
                GameId = request.GameId,
                AuthorId = request.AuthorId,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };
            await _repo.AddAsync(forum);
            return new ForumDto
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                GameId = forum.GameId,
                CreatedAt = forum.CreatedAt,
                UpdatedAt = forum.UpdatedAt
            };
        }

        public async Task<bool> UpdateAsync(UpdateForumRequest request)
        {
            var forum = await _repo.GetByIdAsync(request.Id);
            if (forum == null) return false;
            forum.Title = request.Title;
            forum.Description = request.Description;
            forum.GameId = request.GameId;
            forum.UpdatedAt = System.DateTime.UtcNow;
            await _repo.UpdateAsync(forum);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var forum = await _repo.GetByIdAsync(id);
            if (forum == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
} 