using Microsoft.AspNetCore.Http;
using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface IModelService : IService
{
    Task<IEnumerable<ModelResponse>> FindAllModel();
    Task<ModelResponse?> FindByIdModel(int id);
    Task<ModelResponse> AddModel(AddModelRequest request, IFormFile? imageFile);
    Task<ModelResponse> UpdateModel(UpdateModelRequest request, IFormFile? imageFile);
    Task DeleteModel(DeleteModelRequest request);
}
