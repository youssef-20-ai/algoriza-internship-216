
using AlgorizaProject.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace APIDemo.BLL.Interface
{
    public interface ITokenService
    {
        public Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager);
    }
}
