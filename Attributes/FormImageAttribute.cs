using System.ComponentModel.DataAnnotations;

namespace Projekt.Attributes
{
    /// <summary>
    /// Attribute used to validate form images binded to IFormFile properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FormImageAttribute : ValidationAttribute
    {
        private readonly static string[] _allowedContentTypes =
        {
            "image/jpeg",
            "image/png"
        };

        private readonly static int _maxBytesLength = 8 * 1024 * 1024;

        public override bool IsValid(object? value)
        {
            if(value == null) return true;

            var formImage = value as IFormFile;

            if (formImage == null)
            {
                throw new InvalidOperationException("FormImageValidationAttribute can be used only on properties of type IFormFile");
            }

            if (!_allowedContentTypes.Contains(formImage.ContentType)) return false;

            if (formImage.Length > _maxBytesLength) return false;

            return true;
        }
    }
}
