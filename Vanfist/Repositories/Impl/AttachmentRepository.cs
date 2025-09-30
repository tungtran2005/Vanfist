using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories;

public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
