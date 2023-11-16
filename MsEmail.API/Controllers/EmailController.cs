using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsEmail.API.Context;
using MsEmail.API.DTO;
using MsEmail.API.Entities;
using MsEmail.API.Entities.Common;
using MsEmail.API.Entities.Enums;
using MsEmail.API.Filters;
using MsEmail.API.Repository;
using MsEmail.API.Service;
using MSEmail.Common;
using System.Security.Claims;

namespace MsEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RequisitionFilter]
    public class EmailController : ControllerBase
    {
        private readonly EmailRepository _emails;
        private readonly ExceptionLogRepository _exceptions;
        private readonly IOptions<SmtpConfiguration> _smtp;

        public EmailController(AppDbContext context, IOptions<SmtpConfiguration> smtp)
        {
            _emails = new EmailRepository(context);
            _exceptions = new ExceptionLogRepository(context);
            _smtp = smtp;
        }

        [HttpGet]
        [RequisitionFilter]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Email>))]
        public IActionResult GetAll(bool withDeletionDate)
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

        [HttpGet("my")]
        [RequisitionFilter]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Email>))]
        public IActionResult GetAllByUser()
        {
            var userId = this.User.GetUserID();
            var emails = _emails.GetEmailsByCreationUserId(userId);
            return Ok(new { Count = emails.Count(), emails });
        }

        [HttpGet("{id}")]
        [RequisitionFilter]
        [Authorize(Roles = "admin,user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Email))]
        public IActionResult GetById(long id)
        {

            var email = _emails.GetById(id);
            if (email == null) return NotFound();
            return Ok(email);
        }

        [HttpPost("send")]
        [RequisitionFilter]
        [Authorize]
        public IActionResult Post(EmailDTO emailDTO)
        {
            try
            {
                Email email = new()
                {
                    EmailFrom = emailDTO.EmailFrom,
                    EmailTo = emailDTO.EmailTo,
                    Subject = emailDTO.Subject,
                    Body = emailDTO.Body,
                    Status = EmailStatus.Created,
                };
                email.CreationUserId = email.UpdateUserId = this.User.GetUserID();

                _emails.Insert(email).Save();

                new EmailService(_smtp).SendEmail(email);
                _emails.Update(email).Save();

                return CreatedAtAction(nameof(GetById), new { email.Id }, email);
            }
            catch (Exception ex)
            {
                _exceptions.Insert(new ExceptionLog
                {
                    Source = ex.Source,
                    StackTrace = ex.StackTrace,
                    Message = ex.Message.ToString()
                }).Save();
                return Problem(APIMsg.ERR0001);
            }
        }

        [HttpDelete]
        [RequisitionFilter]
        [Authorize]
        public IActionResult Delete(long id)
        {
            Email email = _emails.GetById(id);
            if (email == null) return NotFound();

            email.DeletionDate = DateTime.Now;
            _emails.Update(email).Save();
            return NoContent();
        }
    }
}
