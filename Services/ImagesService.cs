using Microsoft.Extensions.Options;
using Projekt.Interfaces;
using Projekt.Models.Entities;
using Projekt.Options;
using System.Collections.Generic;

namespace Projekt.Services
{
    public class ImagesService : IImagesService
    {
        private readonly ImageOptions _options;
        private readonly string _wwwRootPath;
        public ImagesService(IWebHostEnvironment env,IOptions<ImageOptions> options)
        {
            _options = options.Value;
            _wwwRootPath = env.WebRootPath;
        }

        public void RemoveFileFromDisk(string filePath)
        {
             File.Delete(_wwwRootPath + "/" + filePath);
        }

        public IEnumerable<Image> SaveImages(IFormFile[] files)
        {
            foreach(var file in files)
            {
                var name = GetRandomName();

                var extension = _options.AllowedContentTypesWithExtensions[file.ContentType];
                var wwwRootName = _options.ImagesDirectory + "/" + name + "." + extension;
                var fullName = _wwwRootPath + "/" + wwwRootName;

                using (var fs = new FileStream(fullName, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

                yield return new Image()
                {
                    ImagePath = wwwRootName
                };
            }
        }

        private string GetRandomName()
        {
            byte[] buffer = new byte[32];

            Random.Shared.NextBytes(buffer);

            return Convert.ToBase64String(buffer).Replace("+", "_").Replace("/", "-").Replace("=", "");
        }
    }
}
