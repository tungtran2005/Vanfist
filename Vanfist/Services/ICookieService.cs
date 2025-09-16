using Vanfist.Entities;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface ICookieService : IService
{
    void CreateLoginCookie(Account account);
}