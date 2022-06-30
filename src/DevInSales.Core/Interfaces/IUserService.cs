using DevInSales.Core.Entities;

namespace DevInSales.EFCoreApi.Core.Interfaces
{
    public interface IUserService
    {
        public List<User> ObterUsers(string? name, string? DateMin, string? DateMax);

        public User? ObterPorId(string id);

        public string CriarUser(User user);

        public void RemoverUser(string id);
    }
}