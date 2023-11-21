using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEmail.API.Filters;
using MsEmail.API.Models;
using MsEmail.API.Models.EmailModel;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.Common;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MSEmail.Infra.Repository;

namespace MsEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RequisitionFilter]
    public class EmailController : ControllerBase
    {
        private readonly EmailRepository _emails;
        private readonly CommonLog _commonLog;

        public EmailController(AppDbContext context)
        {
            _emails = new EmailRepository(context);
            _commonLog = new CommonLog(context);
        }

        [HttpGet]
        [RequisitionFilter]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Email>))]
        public IActionResult GetAll(bool withDeletionDate)
        {
            try
            {
                List<Email> emails = new();
                if (this.User.GetRole().Equals("admin"))
                    emails = _emails.GetAll(withDeletionDate);
                else
                    emails = _emails.GetEmailsByUserId((long)this.User.GetUserID(), withDeletionDate);

                return Ok(new { Count = emails.Count(), emails });
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(GetAll), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }

        [HttpGet("{id}")]
        [RequisitionFilter]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Email))]
        public IActionResult GetById(long id)
        {
            try
            {
                var email = _emails.GetById(id);
                if (email == null) return NotFound();
                return Ok(email);
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(GetById), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }

        [Authorize]
        [HttpPost("send")]
        [RequisitionFilter]
        public IActionResult Post([FromBody] CreateEmailModel createEmailModel)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                Email email = createEmailModel;
                email.CreationUserId = email.UpdateUserId = (long)this.User.GetUserID();

                _emails.Insert(email).Save();

                ViewEmailModel viewEmailModel = email;

                return CreatedAtAction(nameof(GetById), new { email.Id }, viewEmailModel);
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(Post), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0001);
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        [RequisitionFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ViewEmailModel))]
        public IActionResult Patch([FromRoute]long id, UpdateEmailModel updateEmail)
        {
            try
            {
                if(updateEmail.IsNull())
                    return BadRequest(new APIResult { Message = APIMsg.REQ0002 });
                
                Email email = _emails.GetById(id);

                if (email == null) return NotFound(id);

                if (email.Status.Equals(EmailStatus.Sent))
                    return StatusCode(409, new APIResult { Message = APIMsg.ERR0005 } );

                if(!updateEmail.SendDate.IsNull())
                    email.SendDate = (DateTime)updateEmail.SendDate;

                if(!updateEmail.Subject.IsNull())
                    email.Subject = updateEmail.Subject;

                if(!updateEmail.Body.IsNull())
                    email.Body = updateEmail.Body;

                email.UpdateUserId = (long)this.User.GetUserID();
                _emails.Update(email).Save();

                ViewEmailModel viewEmailModel = email;

                return Ok(viewEmailModel);
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(Patch), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }

        [HttpDelete("{id}")]
        [RequisitionFilter]
        [Authorize]
        public IActionResult Delete(long id)
        {
            try
            {
                Email email = _emails.GetById(id);
                if (email == null) return NotFound();
                
                if(email.Status.Equals(EmailStatus.Sent))
                    return BadRequest(new APIResult { Message = APIMsg.ERR0007});

                email.DeletionDate = DateTime.Now;
                email.UpdateUserId = (long)this.User.GetUserID();
                _emails.Update(email).Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(Delete), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }
    }
}
