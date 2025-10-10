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

        return new ModelResponse
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,

            // Bổ sung đầy đủ thông số kỹ thuật
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
            CategoryName = model.Category?.Name ?? "",
            Attachments = model.Attachments?.Select(a => new AttachmentItem
            {
                Id = a.Id,
                FileName = a.FileName,
                Url = $"/uploads/{model.Id}/{a.FileName}",
                ContentType = a.Type
            }).ToList() ?? new List<AttachmentItem>()
        };
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
        await _modelRepository.SaveChanges(); // cần Id để tạo thư mục uploads/{modelId}

        // Upload nhiều ảnh nếu có
        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            foreach (var file in request.Attachments.Where(f => f != null && f.Length > 0))
            {
                var attachment = await SaveAttachment(file, model.Id);
                await _attachmentRepository.Save(attachment);
            }

            await _attachmentRepository.SaveChanges();
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
        if (model == null)
        {
            return null;
        }

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

        // Xóa nhiều ảnh nếu có chỉ định
        if (request.DeletedAttachmentIds != null && request.DeletedAttachmentIds.Count > 0)
        {
            foreach (var attId in request.DeletedAttachmentIds.Distinct())
            {
                var att = await _attachmentRepository.FindById(attId);
                if (att != null && att.ModelId == model.Id)
                {
                    DeleteAttachmentFile(model.Id, att.FileName);
                    await _attachmentRepository.Delete(att);
                }
            }

            await _attachmentRepository.SaveChanges();
        }

        // Upload thêm ảnh mới (nếu có)
        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            foreach (var file in request.Attachments.Where(f => f != null && f.Length > 0))
            {
                var attachment = await SaveAttachment(file, model.Id);
                await _attachmentRepository.Save(attachment);
            }

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

        // Xóa file vật lý của tất cả attachments trước (nếu repo không cascade)
        if (model.Attachments != null && model.Attachments.Count > 0)
        {
            foreach (var a in model.Attachments)
            {
                DeleteAttachmentFile(model.Id, a.FileName);
            }
        }

        await _modelRepository.Delete(model);
        await _modelRepository.SaveChanges();

        // Tùy cấu hình cascade, nếu cần có thể gọi _attachmentRepository.SaveChanges() ở đây
    }

    private static string GetModelUploadFolder(int modelId)
        => Path.Combine("wwwroot", "uploads", modelId.ToString());

    private static void DeleteAttachmentFile(int modelId, string fileName)
    {
        var path = Path.Combine(GetModelUploadFolder(modelId), fileName);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }

    /// <summary>
    /// Lưu file ảnh vào wwwroot/uploads/{modelId}/ và trả về entity Attachment
    /// </summary>
    private async Task<Attachment> SaveAttachment(IFormFile file, int modelId)
    {
        var folder = GetModelUploadFolder(modelId);
        Directory.CreateDirectory(folder);

        var ext = Path.GetExtension(file.FileName);
        var safeExt = string.IsNullOrWhiteSpace(ext) ? "" : ext;
        var fileName = $"{Guid.NewGuid()}{safeExt}";
        var savePath = Path.Combine(folder, fileName);

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