using Microsoft.AspNetCore.Http;
using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services.Base;
using X.PagedList;
using X.PagedList.Extensions;

namespace Vanfist.Services;

public class ModelService : Service, IModelService
{
    private readonly IModelRepository _modelRepository;
    private readonly IAttachmentService _attachmentService; 

    public ModelService(IModelRepository modelRepository,
                        IAttachmentService attachmentService) 
    {
        _modelRepository = modelRepository;
        _attachmentService = attachmentService; 
    }

    public async Task<IEnumerable<ModelResponse>> FindAllModel()
    {
        var models = await _modelRepository.FindAll();
        return models.Select(ModelResponse.FromEntity);
    }

    public async Task<IPagedList<ModelResponse>> FilterModel(FilterModelRequest request)
    {
        var models = await _modelRepository.FindByCategoriesId(request.CategoryIds);
        var result = models.Select(ModelResponse.FromEntity);
        return result.ToPagedList(request.Page, request.PageSize);
    }

    public async Task<ModelResponse?> FindByIdModel(int id)
    {
        var model = await _modelRepository.FindById(id);
        if (model == null) return null;
        return ModelResponse.FromEntity(model); // dùng 1 chỗ mapping duy nhất
    }

    public async Task<ModelResponse> AddModel(AddModelRequest request)
    {
        var model = new Model
        {
            Name = request.Name,
            Price = request.Price,
            Length = request.Length,
            Width = request.Width,
            Height = request.Height,
            Wheelbase = request.Wheelbase,
            NEDC = request.NEDC,
            MaximumPower = request.MaximumPower,
            MaximumTorque = request.MaximumTorque,
            RimSize = request.RimSize,
            Color = request.Color,
            CategoryId = request.CategoryId
        };

        await _modelRepository.Save(model);
        await _modelRepository.SaveChanges(); // cần Id

        // Upload nhiều ảnh (ủy quyền cho AttachmentService)
        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            foreach (var file in request.Attachments.Where(f => f != null && f.Length > 0))
            {
                await _attachmentService.UploadAsync(file, model.Id);
            }
        }

        return ModelResponse.FromEntity(model);
    }

    public async Task<UpdateModelRequest?> GetUpdateModelRequest(int id)
    {
        var model = await _modelRepository.FindById(id);
        if (model == null) return null;

        return new UpdateModelRequest
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            Length = model.Length,
            Width = model.Width,
            Height = model.Height,
            Wheelbase = model.Wheelbase,
            NEDC = model.NEDC,
            MaximumPower = model.MaximumPower,
            MaximumTorque = model.MaximumTorque,
            RimSize = model.RimSize,
            Color = model.Color,
            CategoryId = model.CategoryId,
            ExistingAttachmentIds = model.Attachments?.Select(a => a.Id).ToList(),
            ExistingAttachments = model.Attachments?.ToList()
        };
    }

    public async Task<ModelResponse> UpdateModel(UpdateModelRequest request)
    {
        var model = await _modelRepository.FindById(request.Id);
        if (model == null) return null;

        // Cập nhật thông tin cơ bản
        model.Name = request.Name;
        model.Price = request.Price;
        model.Length = request.Length;
        model.Width = request.Width;
        model.Height = request.Height;
        model.Wheelbase = request.Wheelbase;
        model.NEDC = request.NEDC;
        model.MaximumPower = request.MaximumPower;
        model.MaximumTorque = request.MaximumTorque;
        model.RimSize = request.RimSize;
        model.Color = request.Color;
        model.CategoryId = request.CategoryId;

        await _modelRepository.Update(model);
        await _modelRepository.SaveChanges();

        // Xóa ảnh (nếu có)
        if (request.DeletedAttachmentIds != null && request.DeletedAttachmentIds.Count > 0)
        {
            foreach (var attId in request.DeletedAttachmentIds.Distinct())
            {
                await _attachmentService.DeleteAsync(attId);
            }
        }

        // Upload ảnh mới (nếu có)
        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            foreach (var file in request.Attachments.Where(f => f != null && f.Length > 0))
            {
                await _attachmentService.UploadAsync(file, model.Id);
            }
        }

        return ModelResponse.FromEntity(model);
    }

    public async Task DeleteModel(DeleteModelRequest request)
    {
        var model = await _modelRepository.FindById(request.Id);
        if (model == null) throw new KeyNotFoundException("Model not found");

        // Snapshot Id ảnh trước khi xóa để tránh sửa collection khi đang enumerate
        var attIds = (model.Attachments != null && model.Attachments.Count > 0)
            ? model.Attachments.Select(a => a.Id).ToList()
            : new List<int>();

        // Xóa từng ảnh theo danh sách snapshot
        foreach (var id in attIds)
        {
            await _attachmentService.DeleteAsync(id);
        }

        await _modelRepository.Delete(model);
        await _modelRepository.SaveChanges();
    }
}