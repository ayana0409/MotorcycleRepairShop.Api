using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using MotorcycleRepairShop.Application.Interfaces.Services;
using System.Text.RegularExpressions;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class CloudinaryService<T> : ICloudinaryService<T> where T : class
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _folder;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
            _folder = $"MotorcycleRepairShop/{typeof(T).Name}";
        }

        public string UploadPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Crop("fill").Gravity("face").Width(500).Height(500),
                        Folder = _folder
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return uploadResult.Url.ToString();
        }

        public async Task DeletePhotoAsync(string imageUrl)
        {
            string pattern = _folder + @"/([^/]+)\.png";
            Match match = Regex.Match(imageUrl, pattern);

            if (match.Success)
            {
                string publicId = _folder + "/" + match.Groups[1].Value;
                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                if (result.Result != "ok")
                {
                    var errorMessage = result.Error?.Message ?? "Unknown error occurred.";
                    throw new Exception($"Failed to delete image: {errorMessage}");
                }
            }
            else
            {
                throw new Exception("No match found.");
            }
        }
    }
}
