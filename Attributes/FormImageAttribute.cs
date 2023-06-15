using Microsoft.Extensions.Options;
using Projekt.Options;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Attributes
{
    /// <summary>
    /// Attribute used to validate form images binded to IFormFile properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FormImageAttribute : ValidationAttribute
    {
        public FormImageAttribute(){}

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var formImages = value as IFormFile[];

            if (formImages == null)
            {
                throw new InvalidOperationException("FormImageAttribute can be used only on properties of type IFormFile");
            }

            var options = validationContext.GetRequiredService<IOptions<ImageOptions>>().Value;

            var validContentTypes = options.AllowedContentTypesWithExtensions.Keys;
            var maxBytesSize = options.MaxBytesSize;

            foreach(var img in formImages)
            {
                if (!validContentTypes.Contains(img.ContentType)) return new ValidationResult("Invalid file format");

                if (img.Length > maxBytesSize) return new ValidationResult("File too big");
            }

            return ValidationResult.Success;
        }
    }
}
