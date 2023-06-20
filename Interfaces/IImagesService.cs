using Projekt.Models.Entities;
using System.Collections.Generic;

namespace Projekt.Interfaces
{
    public interface IImagesService
    {
        IEnumerable<Image> SaveImages(IFormFile[] files);
        void RemoveFileFromDisk(string filePath);
    }
}
