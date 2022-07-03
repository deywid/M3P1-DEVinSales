using System.ComponentModel.DataAnnotations;
using DevInSales.Core.Entities;

namespace DevInSales.EFCoreApi.Api.DTOs.Request
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public UserResponse(string id, string email, string name, DateTime birthDate)
        {
            Id = id;
            Email = email;
            Name = name;
            BirthDate = birthDate;
        }

        public static UserResponse ConverterParaEntidade(User user)
        {
            return new UserResponse(user.Id, user.Email, user.Nome, user.DataNascimento);
        }
    }
}