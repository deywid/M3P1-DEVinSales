using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.EFCoreApi.Api.DTOs.Request;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegexExamples;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
       // private readonly SignInManager<User> _signInManager;

        private readonly IUserService _userService;

        public UserController(UserManager<User> userManager , IUserService userService)
        {
            _userService = userService;
            _userManager = userManager;
           // _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> CadastrarUsuario(UserRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new User(model.Nome, model.Email, model.Senha, model.DataNascimento);
            usuario.UserName = model.Email;
            var retorno = await _userManager.CreateAsync(usuario, model.Senha);

            if (!retorno.Succeeded)
            {
                foreach (var erro in retorno.Errors)
                {
                    ModelState.AddModelError(erro.Code, erro.Description);
                }
                return BadRequest(ModelState);
            }

            return Accepted();
        }

        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> LoginUsuario(UserLoginRequest model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var retorno = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, false);

        //    if (!retorno.Succeeded)
        //        return Unauthorized("Usuário ou senha não conferem");

        //    return Accepted();
        //}

        [HttpGet]
        public ActionResult<List<User>> ObterUsers(string? nome, string? DataMin, string? DataMax)
        {

            var users = _userService.ObterUsers(nome, DataMin, DataMax);
            if (users == null || users.Count == 0)
                return NoContent();

            var ListaDto = users.Select(user => UserResponse.ConverterParaEntidade(user)).ToList();

            return Ok(ListaDto);
        }

        [HttpGet("{id}")]
        public ActionResult<User> ObterUserPorId(string id)
        {
            var user = _userService.ObterPorId(id);
            if (user == null)
                return NotFound();

            var UserDto = UserResponse.ConverterParaEntidade(user);

            return Ok(UserDto);
        }

        //[HttpPost]
        //public ActionResult CriarUser(AddUser model)
        //{
        //    var user = new User(model.Email, model.Password, model.Name, model.BirthDate);

        //    var verifyEmail = new EmailValidate();

        //    if (!verifyEmail.IsValidEmail(user.Email))
        //        return BadRequest("Email inválido");

        //    if (user.BirthDate.AddYears(18) > DateTime.Now)
        //        return BadRequest("Usuário não tem idade suficiente");

        //    //if (user.Password.Length < 4 || user.Password.Length == 0 || user.Password.All(ch => ch == user.Password[0]))
        //    //    return BadRequest("Senha inválida, deve conter pelo menos 4 caracteres e deve conter ao menos um caracter diferente");


        //    var id = _userService.CriarUser(user);

        //    return CreatedAtAction(nameof(ObterUserPorId), new { id = id }, id);
        //}

        [HttpDelete("{id}")]
        public ActionResult ExcluirUser(string id)
        {
            try
            {
                _userService.RemoverUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("usuario não existe"))
                    return NotFound();

                return StatusCode(StatusCodes.Status500InternalServerError, new { Mensagem = ex.Message });
            }
        }
    }
}