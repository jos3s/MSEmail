using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Domain.Enums;
using MsEmail.API.Filters;
using MsEmail.API.Models;
using MsEmail.API.Models.EmailModels;
using MsEmail.API.Service;
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Email>))]
        public IActionResult GetAll(bool withDeletionDate)
        {
            try
            {
                var emails = _emails.GetAll();

                if (!withDeletionDate.IsNull() && (bool)withDeletionDate)
                {
                    return Ok(new
                    {
                        Count = emails.Count(),
                        emails
                    });
                }
                var email = _emails.Find(x => x.DeletionDate == null);

                return Ok(new { Count = emails.Count(), emails });
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(GetAll), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }

        [HttpGet("my")]
        [RequisitionFilter]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Email>))]
        public IActionResult GetAllByUser()
        {
            try
            {
                var userId = this.User.GetUserID();
                var emails = _emails.GetEmailsByCreationUserId((long)userId);
                return Ok(new { Count = emails.Count(), emails });
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(GetAllByUser), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }

        [HttpGet("{id}")]
        [RequisitionFilter]
        [Authorize(Roles = "admin,user")]
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
        public IActionResult Post([FromBody] CreateEmailModel emailDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                Email email = new()
                {
                    EmailFrom = emailDTO.EmailFrom,
                    EmailTo = emailDTO.EmailTo,
                    Subject = emailDTO.Subject,
                    Body = emailDTO.Body,
                    Status = EmailStatus.Created,
                    SendDate = (DateTime)emailDTO.SendDate,
                };
                email.CreationUserId = email.UpdateUserId = (long)this.User.GetUserID();

                _emails.Insert(email).Save();

                new EmailService().SendEmail(email);
                _emails.Update(email).Save();

                return CreatedAtAction(nameof(GetById), new { email.Id }, email);
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

                return Ok(email);
            }
            catch (Exception ex)
            {
                _commonLog.SaveExceptionLog(ex, nameof(Patch), this.GetType().Name, ServiceType.API);
                return Problem(APIMsg.ERR0004);
            }
        }



        [HttpDelete]
        [RequisitionFilter]
        [Authorize]
        public IActionResult Delete(long id)
        {
            try
            {
                Email email = _emails.GetById(id);
                if (email == null) return NotFound();
                
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
