using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; private set; }
        public DateTime BirthDate { get; private set; }

        public User(string email, string name, DateTime birthDate)
        {
            Email = email;
            Name = name;
            BirthDate = birthDate;
        }
        public User(string id, string email, string name, DateTime birthDate)
        {
            Id = id;
            Email = email;
            Name = name;
            BirthDate = birthDate;
        }   
    }
}