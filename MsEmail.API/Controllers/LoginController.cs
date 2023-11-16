using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsEmail.API.DTO;
using MsEmail.API.Service;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Infra.Repository;

namespace MsEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _users;
        private readonly IOptions<TokenConfiguration> _token;

        public LoginController(AppDbContext context, IOptions<TokenConfiguration> token)
        {
            _users = new UserRepository(context);
            _token = token;
        }

        [HttpPost("create")]
        public IActionResult CreateLogin(UserDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                User user = new User
                {
                    Email = userDTO.Email,
                    Password = userDTO.Password,
                };

                _users.Insert(user).Save();

                user.Password = "";
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        
        [HttpPost]
        public IActionResult Authenticate(UserDTO login)
        {
            try
            {
                User user = _users.GetByLogin(login.Email, login.Password);

                if (user == null)
                    return BadRequest(new { message = "Usuario ou senha invalidos" });

                var token = new TokenServices(_token).GenerateToken(user);

                return Ok(token);
            } 
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
