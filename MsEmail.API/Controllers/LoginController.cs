using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsEmail.API.Context;
using MsEmail.API.DTO;
using MsEmail.API.Entities;
using MsEmail.API.Entities.Enums;
using MsEmail.API.Service;

namespace MsEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IOptions<TokenConfiguration> _token;

        public LoginController(AppDbContext context, IOptions<TokenConfiguration> token)
        {
            _context = context;
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
                user.Role = "user";
                user.CreationDate = user.UpdateDate = DateTime.Now;

                _context.Users.Add(user);
                _context.SaveChanges();

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
            User  user = _context.Users.FirstOrDefault(x => x.Email == login.Email && login.Password == x.Password);

            if(user == null)
                return BadRequest(new {message = "Usuario ou senha invalidos"});


            var token = new TokenServices(_token).GenerateToken(user);

            return Ok(token);
        }
    }
}
