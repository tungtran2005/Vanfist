using Vanfist.Entities;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface IPasswordService : IService
{
    string Encode(string password);
    bool Verify(string hashedPassword, string password);
}