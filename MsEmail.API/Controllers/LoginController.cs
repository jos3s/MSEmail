using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEmail.API.Filters;
using MsEmail.API.Models;
using MsEmail.API.Models.Token;
using MsEmail.API.Models.UserModels;
using MSEmail.Common;
using MsEmail.Domain.Entities;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MsEmail.Infra.Context;
using MSEmail.Infra.Repository;
using MSEmail.Infra.Services;

namespace MsEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _users;
        private readonly CommonLog _commonLog;

        public LoginController(AppDbContext context)
        {
            _users = new UserRepository(context);
            _commonLog = new CommonLog(context);
        }
        
        [HttpPost]
        [RequisitionFilter]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(ViewTokenModel))]
        public IActionResult Authenticate(LoginUserModel login)
        {
            try
            {
                User user = _users.GetByLogin(login.Email);

                if (user == null)
                    return BadRequest(new APIResult{ Message = APIMsg.ERR0002 });

                if (!user.Password.Equals(login.Password.Hashing()))
                    return BadRequest(new APIResult{ Message = APIMsg.ERR0003 });

                var token = TokenServices.GenerateToken(user);

                return Ok(new ViewTokenModel { Email = user.Email, Token = token});
            } 
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(Authenticate), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }
    }
}
