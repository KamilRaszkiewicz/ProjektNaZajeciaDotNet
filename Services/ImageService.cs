using Microsoft.Extensions.Options;
using Projekt.Interfaces;
using Projekt.Models.Entities;
using Projekt.Options;
using System.Collections.Generic;

namespace Projekt.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageOptions _options;
        private readonly string _wwwRootPath;
        public ImageService(IWebHostEnvironment env,IOptions<ImageOptions> options)
        {
            _options = options.Value;
            _wwwRootPath = env.WebRootPath;
        }
        public Image SaveImage(IFormFile file)
        {
            var name = GetRandomName();

            var extension = _options.AllowedContentTypesWithExtensions[file.ContentType];
            var wwwRootName = _options.ImagesDirectory + "/" + name + "." + extension;
            var fullName = _wwwRootPath + "/" + wwwRootName;


            using(var fs = new FileStream(fullName, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return new Image()
            {
                ImagePath = wwwRootName
            };
        }

        private string GetRandomName()
        {
            byte[] buffer = new byte[32];

            Random.Shared.NextBytes(buffer);

            return Convert.ToBase64String(buffer).Replace("+", "_").Replace("/", "-").Replace("=", "");
        }
    }
}
