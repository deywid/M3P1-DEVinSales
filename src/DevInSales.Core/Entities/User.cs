using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Entities
{
    public class User : IdentityUser
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }

        public User(string nome, string email, string senha, DateTime dataNascimento): base()
        {
            Nome = nome;
            Email = email;
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(this, senha);
            DataNascimento = dataNascimento;
            UserName = email;
            NormalizedEmail = email.ToUpper();
        }

        public User() { }
    }
}