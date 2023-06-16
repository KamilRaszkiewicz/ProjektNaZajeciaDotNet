﻿using Projekt.Dtos;
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
                Images = _imagesService.SaveImages(dto.FormFiles).ToList(),
            };

            _postsRepository.Add(entity);
            _postsRepository.SaveChanges();

            return entity.Id;
        }

        public bool DeleteComment(int commentId)
        {
            var entity = _commentsRepository.Get(x => x.Id == commentId).FirstOrDefault();
            
            if(entity == null) return false;

            _commentsRepository.Remove(entity);

            return _commentsRepository.SaveChanges() > 0;
        }

        public bool DeletePost(int postId)
        {
            var postAndTagsToBeDeleted = _postsRepository
                .Get(x => x.Id == postId, include => include.Tags)
                .Select(x => new
                {
                    Post = x,
                    TagsToBeDeleted = x.Tags.Where(t => t.Posts.Count == 1).ToList()
                }).FirstOrDefault();

            if (postAndTagsToBeDeleted == null) return false;

            _postsRepository.Remove(postAndTagsToBeDeleted.Post);
            foreach(var tag in postAndTagsToBeDeleted.TagsToBeDeleted)
            {
                _tagsRepository.Remove(tag);
            }

            _tagsRepository.SaveChanges();
            return _postsRepository.SaveChanges() > 0;
        }

        public IEnumerable<PostDto> GetPosts(int pageNr = 1)
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
                    
                    Title = x.Title,
                    Description = x.Description,

                    Tags = x.Tags.Select(y => y.Name),

                    ImagePaths = x.Images.Select(y => new ImageDto 
                    {
                        Id = y.Id,
                        ImagePath = y.ImagePath,
                    }),

                    Comments = x.Comments.Select(y => new CommentDto
                    {
                        Id = y.Id,
                        Author = y.Author.Name,
                        Content = y.Content,
                        CreatedAt = y.CreatedAt,
                        IP = y.IP,
                    })
                });
        }

        public IEnumerable<PostDto> GetUserPosts(int userId, int pageNr = 1)
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

                    Title = x.Title,
                    Description = x.Description,

                    Tags = x.Tags.Select(y => y.Name),

                    ImagePaths = x.Images.Select(y => new ImageDto
                    {
                        Id = y.Id,
                        ImagePath = y.ImagePath,
                    }),

                    Comments = x.Comments.Select(y => new CommentDto
                    {
                        Id = y.Id,
                        Author = y.Author.Name,
                        Content = y.Content,
                        CreatedAt = y.CreatedAt,
                        IP = y.IP,
                    })
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
