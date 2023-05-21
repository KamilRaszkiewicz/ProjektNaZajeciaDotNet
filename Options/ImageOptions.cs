namespace Projekt.Options
{
    public class ImageOptions
    {
        public IReadOnlyDictionary<string, string> AllowedContentTypesWithExtensions { get; set; } = new Dictionary<string, string>()
        {
            {"image/jpeg", "jpg" },
            {"image/png", "png" }
        };

        public string ImagesDirectory { get; set; } = "images";
        public int MaxBytesSize { get; set; } = 8 * 1024 * 1024;
    }
}
