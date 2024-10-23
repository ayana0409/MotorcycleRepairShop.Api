using Microsoft.AspNetCore.Http;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface ICloudinaryService<T> where T : class
    {
        Task DeletePhotoAsync(string imageUrl);
        Task DeleteVideoAsync(string videoUrl);
        string UploadPhoto(IFormFile file);
        string UploadVideo(IFormFile file);
    }
}