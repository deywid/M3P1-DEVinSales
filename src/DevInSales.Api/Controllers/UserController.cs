using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.EFCoreApi.Api.DTOs.Request;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevInSales.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public UserController(UserManager<User> userManager , IUserService userService)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> CadastrarUsuario(UserRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new User(model.Nome, model.Email, model.Senha, model.DataNascimento);
                                
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUsuario(UserLoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValidUser = await _userService.ValidateUser(model);
            if (!isValidUser)
                return Unauthorized();

            var token = new { Token = await _userService.CreateToken() };
            return Accepted(token);
        }

        [HttpGet]
        public async Task<IActionResult> ObterUsers(string? nome, string? DataMin, string? DataMax)
        {

            var users = await _userService.ObterUsers(nome, DataMin, DataMax);
            if (users == null || users.Count == 0)
                return NoContent();

            var ListaDto = users.Select(user => UserResponse.ConverterParaEntidade(user)).ToList();

            return Ok(ListaDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUserPorId(string id)
        {
            var user = await _userService.ObterPorId(id);
            if (user == null)
                return NotFound();

            var UserDto = UserResponse.ConverterParaEntidade(user);

            return Ok(UserDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUser(string id)
        {
            try
            {
                await _userService.RemoverUser(id);
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