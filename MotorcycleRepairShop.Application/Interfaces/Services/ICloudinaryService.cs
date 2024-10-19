using Microsoft.AspNetCore.Http;

namespace MotorcycleRepairShop.Application.Interfaces.Services
{
    public interface ICloudinaryService<T> where T : class
    {
        Task DeletePhotoAsync(string imageUrl);
        string UploadPhoto(IFormFile file);
    }
}