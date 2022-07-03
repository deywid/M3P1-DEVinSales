using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DevInSales.EFCoreApi.Core.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> ObterUsers(string? name, string? DataMin, string? DataMax);
        public Task<User?> ObterPorId(string id);
        public Task<IdentityResult> RemoverUser(string id);
        public Task<string> CreateToken();
        public Task<bool> ValidateUser(UserLoginRequest model);
    }
}