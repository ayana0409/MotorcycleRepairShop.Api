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

        public string UploadVideo(IFormFile file)
        {
            var uploadResult = new VideoUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new VideoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation()
                                            .Width(1280)
                                            .Height(720) 
                                            .Crop("limit")
                                            .BitRate("500k")
                                            .Quality("auto:low"),
                        Format = "mp4",
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

        public async Task DeleteVideoAsync(string videoUrl)
        {
            string publicId = ExtractPublicIdFromVideoUrl(videoUrl);
            var deletionParams = new DeletionParams(publicId);
            deletionParams.ResourceType = ResourceType.Video;
            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (!string.Equals(result.Result, "ok", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Failed to delete video: {result.StatusCode} - {result.Error?.Message}");
            }
        }

        public async Task DeletePhotoAsync(string imageUrl)
        {
            string publicId = ExtractPublicIdFromImageUrl(imageUrl);
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Result != "ok")
            {
                var errorMessage = result.Error?.Message ?? "Unknown error occurred.";
                throw new Exception($"Failed to delete image: {errorMessage}");
            }
        }

        private string ExtractPublicIdFromVideoUrl(string videoUrl)
        {
            string pattern = _folder + @"/([^/]+)\.mp4";
            Match match = Regex.Match(videoUrl, pattern);

            if (match.Success)
            {
                return _folder + "/" + match.Groups[1].Value;
            }
            throw new Exception("Invalid video URL format or no match found.");
        }

        private string ExtractPublicIdFromImageUrl(string imageUrl)
        {
            string pattern = _folder + @"/([^/]+)\.(png|jpg)";
            Match match = Regex.Match(imageUrl, pattern);

            if (match.Success)
            {
                return _folder + "/" + match.Groups[1].Value;
            }
            throw new Exception($"Invalid image URL format or no match found. {imageUrl}");
        }

    }
}
