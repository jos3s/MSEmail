using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEmail.API.Filters;
using MsEmail.API.Models;
using MsEmail.API.Models.Token;
using MsEmail.API.Models.UserModels;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Common;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
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

        [HttpPost("create")]
        [RequisitionFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewUserModel))]
        public IActionResult CreateLogin([FromBody] CreateUserModel createUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                User existentUser = _users.GetByLogin(createUserModel.Email);
                if (createUserModel.Email.ToLower().Equals(existentUser?.Email.ToLower()))
                    return BadRequest(new APIResult { Message = APIMsg.ERR0006});

                User user = createUserModel;

                _users.Insert(user).Save();

                ViewUserModel viewUserModel = user;

                return Ok(viewUserModel);
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(CreateLogin), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
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
