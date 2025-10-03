using Microsoft.AspNetCore.Http;
using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Services.Base;
using X.PagedList;

namespace Vanfist.Services;

public interface IModelService : IService
{
    Task<IEnumerable<ModelResponse>> FindAllModel();

    Task<IPagedList<ModelResponse>> FilterModel(FilterModelRequest request);


    Task<ModelResponse?> FindByIdModel(int id);

    Task<ModelResponse> AddModel(AddModelRequest request);

    Task<ModelResponse> UpdateModel(UpdateModelRequest request);

    Task<UpdateModelRequest?> GetUpdateModelRequest(int id);

    Task DeleteModel(DeleteModelRequest request);


}
