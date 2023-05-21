using Projekt.Models.Entities;

namespace Projekt.Interfaces
{
    public interface IImageService
    {
        Image SaveImage(IFormFile file);
    }
}
