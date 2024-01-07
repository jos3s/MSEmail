using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEmail.API.Filters;
using MSEmail.API.Messages;
using MsEmail.API.Models;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.API.Models.User;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MSEmail.Infra.Repository;

namespace MSEmail.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly UserRepository _users;
    private readonly CommonLog _commonLog;

    public UserController(AppDbContext context)
    {
        _users = new UserRepository(context);
        _commonLog = new CommonLog(context);
    }

    [HttpPost]
    [RequisitionFilter]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<ViewUserModel>))]
    public IActionResult Create([FromBody] CreateUserModel createUserModel)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User existentUser = _users.GetByLogin(createUserModel.Email);
            if (createUserModel.Email.Equals(existentUser?.Email))
                return BadRequest(new ApiResult<string>(APIMsg.ERR0006));

            User user = createUserModel;
            _users.Insert(user).Save();
            ViewUserModel viewUserModel = user;

            return Ok(new ApiResult<ViewUserModel>(viewUserModel));
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(Create), this.GetType().Name, ServiceType.API);
            return Problem(APIMsg.ERR0004);
        }
    }

    [HttpGet("all")]
    [RequisitionFilter]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<List<ViewUserModel>>))]
    public IActionResult GetAllUsers()
    {
        try
        {
            List<User> user = _users.GetAll();
            List<ViewUserModel> viewUsers = user.Select(user => (ViewUserModel)user).ToList();
                
            return Ok(new ApiResult<ListUserModel>(new ListUserModel { Count = viewUsers.Count, Users = viewUsers }));
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(GetAllUsers), this.GetType().Name, ServiceType.API);
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0004));
        }
    }
}