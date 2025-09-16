using Vanfist.Entities;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface IPasswordService : IService
{
    string Encode(Account account, string password);
    bool Verify(Account account, string hashedPassword, string password);
}