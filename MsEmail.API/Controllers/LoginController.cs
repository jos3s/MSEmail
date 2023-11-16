using Microsoft.AspNetCore.Mvc;
using MsEmail.API.DTO;
using MsEmail.API.Filters;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Common;
using MSEmail.Infra.Repository;
using MSEmail.Infra.Services;

namespace MsEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _users;

        public LoginController(AppDbContext context)
        {
            _users = new UserRepository(context);
        }

        [HttpPost("create")]
        [RequisitionFilter]
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
        [RequisitionFilter]
        public IActionResult Authenticate(UserDTO login)
        {
            try
            {
                User user = _users.GetByLogin(login.Email);

                if (user == null)
                    return BadRequest(new { Message = APIMsg.ERR0002 });

                if (user.Password != login.Password)
                    return BadRequest(new { Message = APIMsg.ERR0003 });

                var token = TokenServices.GenerateToken(user);

                return Ok(token);
            } 
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
