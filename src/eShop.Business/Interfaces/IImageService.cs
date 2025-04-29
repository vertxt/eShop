using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace eShop.Business.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadResult> AddImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}