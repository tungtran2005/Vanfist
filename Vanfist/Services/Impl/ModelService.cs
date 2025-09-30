using Microsoft.AspNetCore.Http;
using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public class ModelService : Service, IModelService
{
    private readonly IModelRepository _modelRepository;
    private readonly IAttachmentRepository _attachmentRepository;

    public ModelService(IModelRepository modelRepository, IAttachmentRepository attachmentRepository)
    {
        _modelRepository = modelRepository;
        _attachmentRepository = attachmentRepository;
    }

    public async Task<IEnumerable<ModelResponse>> FindAllModel()
    {
        var models = await _modelRepository.FindAll();
        return models.Select(ModelResponse.FromEntity);
    }

    public async Task<ModelResponse?> FindByIdModel(int id)
    {
        var model = await _modelRepository.FindById(id);
        return model == null ? null : ModelResponse.FromEntity(model);
    }

    public async Task<ModelResponse> AddModel(AddModelRequest request, IFormFile? imageFile)
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
        await _modelRepository.SaveChanges();

        // Upload ảnh nếu có
        if (imageFile != null && imageFile.Length > 0)
        {
            var attachment = await SaveAttachment(imageFile, model.Id);
            await _attachmentRepository.Save(attachment);
            await _attachmentRepository.SaveChanges();
        }

        return ModelResponse.FromEntity(model);
    }

    public async Task<ModelResponse> UpdateModel(UpdateModelRequest request, IFormFile? imageFile)
    {
        var model = await _modelRepository.FindById(request.Id);
        if (model == null)
        {
            throw new KeyNotFoundException("Model not found");
        }

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

        // Upload ảnh mới nếu có
        if (imageFile != null && imageFile.Length > 0)
        {
            var attachment = await SaveAttachment(imageFile, model.Id);
            await _attachmentRepository.Save(attachment);
            await _attachmentRepository.SaveChanges();
        }

        return ModelResponse.FromEntity(model);
    }

    public async Task DeleteModel(DeleteModelRequest request)
    {
        var model = await _modelRepository.FindById(request.Id);
        if (model == null)
        {
            throw new KeyNotFoundException("Model not found");
        }

        await _modelRepository.Delete(model);
        await _modelRepository.SaveChanges();
    }

    /// <summary>
    /// Lưu file ảnh vào wwwroot/uploads và tạo đối tượng Attachment
    /// </summary>
    private async Task<Attachment> SaveAttachment(IFormFile file, int modelId)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var savePath = Path.Combine("wwwroot/uploads", fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
        using (var stream = new FileStream(savePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return new Attachment
        {
            FileName = fileName,
            Type = file.ContentType,
            ModelId = modelId
        };
    }
}
