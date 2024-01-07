using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEmail.API.Filters;
using MSEmail.API.Messages;
using MsEmail.API.Models;
using MsEmail.API.Models.Token;
using MsEmail.API.Models.UserModels;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MSEmail.Infra.Repository;
using MSEmail.Infra.Services;

namespace MsEmail.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(ApiResult<ViewTokenModel>))]
    public IActionResult Authenticate(LoginUserModel login)
    {
        try
        {
            User user = _users.GetByLogin(login.Email);

            if (user == null)
                return BadRequest(new ApiResult<string> (APIMsg.ERR0002));

            if (!user.Password.Equals(login.Password.Hashing()))
                return BadRequest(new ApiResult<string>(APIMsg.ERR0003));

            var token = TokenServices.GenerateToken(user);

            return Ok(new ApiResult<ViewTokenModel>(new ViewTokenModel { Email = user.Email, Token = token }));
        } 
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(Authenticate), this.GetType().Name, ServiceType.API);
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0004));
        }
    }
}
