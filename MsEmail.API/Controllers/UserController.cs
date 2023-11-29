using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsEmail.API.Filters;
using MsEmail.API.Models.UserModels;
using MsEmail.API.Models;
using MSEmail.Common;
using MsEmail.Domain.Entities;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MsEmail.Infra.Context;
using MSEmail.Infra.Repository;

namespace MSEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewUserModel))]
        public IActionResult Create([FromBody] CreateUserModel createUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                User existentUser = _users.GetByLogin(createUserModel.Email);
                if (createUserModel.Email.ToLower().Equals(existentUser?.Email.ToLower()))
                    return BadRequest(new APIResult { Message = APIMsg.ERR0006 });

                User user = createUserModel;
                _users.Insert(user).Save();
                ViewUserModel viewUserModel = user;

                return Ok(viewUserModel);
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(Create), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }
    }
}
