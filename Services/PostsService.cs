using Projekt.Dtos;
using Projekt.Dtos.Comments;
using Projekt.Dtos.Posts;
using Projekt.Interfaces;
using Projekt.Models.Entities;

namespace Projekt.Services
{
    public class PostsService : IPostsService
    {
        const int POSTS_PER_PAGE = 10;

        private readonly IImagesService _imagesService;
        private readonly IRepository<Post> _postsRepository;
        private readonly IRepository<Tag> _tagsRepository;
        private readonly IRepository<Comment> _commentsRepository;

        public PostsService(
            IImagesService imagesService,
            IRepository<Post> postsRepository, 
            IRepository<Tag> tagsRepository,
            IRepository<Comment> commentsRepository)
        {
            _imagesService = imagesService;
            _postsRepository = postsRepository;
            _tagsRepository = tagsRepository;
            _commentsRepository = commentsRepository;
        }

        public int CreateComment(CreateCommentDto dto, int userId, string IP)
        {
            var entity = new Comment
            {
                AuthorId = userId,
                Content = dto.Content,
                IP = IP,
                CreatedAt = DateTime.Now,
            };

            _commentsRepository.Add(entity);
            _commentsRepository.SaveChanges();

            return entity.Id;
        }

        public int CreatePost(CreatePostDto dto, int userId)
        {
            var normalizedTagNames = dto.TagsString.Trim().Split(" ").Select(s => s.ToLower());

            var relatedTags = _tagsRepository.Get(t => normalizedTagNames.Contains(t.Name)).ToList();
            var nonExistingTagNames = normalizedTagNames.Except(relatedTags.Select(x => x.Name)).ToList();

            relatedTags.AddRange(nonExistingTagNames.Select(x => new Tag { Name = x }));

            var entity = new Post
            {
                AuthorId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Tags = relatedTags,
                CreatedAt = DateTime.Now,
                Images = dto.FormFiles != null ? _imagesService.SaveImages(dto.FormFiles).ToList() : new List<Image>()  
            };

            _postsRepository.Add(entity);
            _postsRepository.SaveChanges();

            return entity.Id;
        }

        public bool DeleteComment(int? currentUserId, bool isAdmin, int commentId)
        {
            var entity = _commentsRepository.Get(x => x.Id == commentId).FirstOrDefault();
            
            if(entity == null) return false;

            if (entity.AuthorId != currentUserId && !isAdmin) return false;

            _commentsRepository.Remove(entity);

            return _commentsRepository.SaveChanges() > 0;
        }

        public bool DeletePost(int? currentUserId, bool isAdmin, int postId)
        {
            var postAndTagsToBeDeleted = _postsRepository
                .Get(x => x.Id == postId, include => include.Tags)
                .Select(x => new
                {
                    Post = x,
                    TagsToBeDeleted = x.Tags.Where(t => t.Posts.Count == 1).ToList()
                }).FirstOrDefault();

            if (postAndTagsToBeDeleted == null) return false;
            if (postAndTagsToBeDeleted.Post.AuthorId != currentUserId && !isAdmin) return false;

            _postsRepository.Remove(postAndTagsToBeDeleted.Post);
            foreach(var tag in postAndTagsToBeDeleted.TagsToBeDeleted)
            {
                _tagsRepository.Remove(tag);
            }

            _tagsRepository.SaveChanges();
            return _postsRepository.SaveChanges() > 0;
        }

        public IEnumerable<PostDto> GetPosts(int? currentUserId, bool IsAdmin, int pageNr = 1)
        {
            if(pageNr < 1) return Enumerable.Empty<PostDto>();

            var skipCount = (pageNr - 1) * POSTS_PER_PAGE;

            return _postsRepository.GetAll(x => x.Author, x => x.Comments, x => x.Tags, x => x.Images)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skipCount)
                .Take(POSTS_PER_PAGE)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Author = x.Author.UserName,
                    Title = x.Title,
                    Description = x.Description,

                    Tags = x.Tags.Select(y => y.Name),

                    Images = x.Images.Select(y => new ImageDto 
                    {
                        Id = y.Id,
                        ImagePath = y.ImagePath,
                        CanDelete = (x.AuthorId == currentUserId)
                    }),

                    Comments = x.Comments.Select(y => new CommentDto
                    {
                        Id = y.Id,
                        Author = y.Author.UserName,
                        Content = y.Content,
                        CreatedAt = y.CreatedAt,
                        IP = y.IP,
                    }),

                    CanDelete = IsAdmin || (x.AuthorId == currentUserId)
                });
        }

        public IEnumerable<PostDto> GetUserPosts(int? currentUserId, bool IsAdmin, int userId, int pageNr = 1)
        {
            if (pageNr < 1) return Enumerable.Empty<PostDto>();

            var skipCount = (pageNr - 1) * POSTS_PER_PAGE;

            return _postsRepository.Get(post => post.AuthorId == userId ,x => x.Author, x => x.Comments, x => x.Tags, x => x.Images)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skipCount)
                .Take(POSTS_PER_PAGE)
                .Select(x => new PostDto
                {
                    Id = x.Id,
                    Author = x.Author.UserName,

                    Title = x.Title,
                    Description = x.Description,

                    Tags = x.Tags.Select(y => y.Name),

                    Images = x.Images.Select(y => new ImageDto
                    {
                        Id = y.Id,
                        ImagePath = y.ImagePath,
                        CanDelete = (x.AuthorId == currentUserId)
                    }),

                    Comments = x.Comments.Select(y => new CommentDto
                    {
                        Id = y.Id,
                        Author = y.Author.UserName,
                        Content = y.Content,
                        CreatedAt = y.CreatedAt,
                        IP = y.IP,
                    }),

                    CanDelete = IsAdmin || (x.AuthorId == currentUserId)
                });
        }

        public bool UpdatePost(int postId, CreatePostDto dto)
        {
            var post = _postsRepository.Get(x => x.Id == postId, include => include.Tags)
                .FirstOrDefault();

            if (post == null) return false;

            var normalizedTagNames = dto.TagsString.Trim().Split(" ").Select(s => s.ToLower());

            var relatedTags = _tagsRepository.Get(t => normalizedTagNames.Contains(t.Name)).ToList();
            var nonExistingTagNames = normalizedTagNames.Except(relatedTags.Select(x => x.Name)).ToList();

            relatedTags.AddRange(nonExistingTagNames.Select(x => new Tag { Name = x }));

            post.Title = dto.Title;
            post.Description = dto.Description;
            post.Tags = relatedTags;

            //TOOD: somehow update images

            _postsRepository.Update(post);

            return _postsRepository.SaveChanges() > 1;
        }
    }
}
