using Microsoft.AspNetCore.Http;
using Vanfist.Entities;

namespace Vanfist.Services;

public interface IAttachmentService
{
    Task<Attachment> UploadAsync(IFormFile file, int modelId);
    Task<IEnumerable<Attachment>> ListByModel(int modelId);
    Task DeleteAsync(int id);
}
