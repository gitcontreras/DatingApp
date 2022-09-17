using CloudinaryDotNet.Actions;

namespace MyAngularApp.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletionPhotoAsync(string publicId);


    }
}
