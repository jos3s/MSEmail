using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsEmail.API.Context;
using MsEmail.API.DTO;
using MsEmail.API.Entities;
using MsEmail.API.Entities.Common;
using MsEmail.API.Entities.Enums;
using MsEmail.API.Filters;
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
        private readonly AppDbContext _context;
        private readonly IOptions<SmtpConfiguration> _smtp;

        public EmailController(AppDbContext context, IOptions<SmtpConfiguration> smtp)
        {
            _context = context;
            _smtp = smtp;
        }

        [HttpGet]
        [RequisitionFilter]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Email>))]
        public IActionResult GetAll(bool withDeletionDate)
        {
            var emails = _context.Emails.ToList();

            if (withDeletionDate) 
                return Ok(new {NumberOfEmails = emails.Count(), 
                    Emails = emails});

            emails = emails.Where(x => x.DeletionDate == null).ToList();

            return Ok(new { NumberOfEmails = emails.Count(), Emails =  emails});
        }

        [HttpGet("my")]
        [RequisitionFilter]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Email>))]
        public IActionResult GetAllByUser()
        {
            var UserId = this.User.GetUserID();
            var emails = _context.Emails.Where(x => x.CreationUserId == UserId);
            return Ok(emails);
        }

        [HttpGet("{id}")]
        [RequisitionFilter]
        [Authorize(Roles = "admin,user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Email))]
        public IActionResult GetById(long id)
        {

            var email = _context.Emails.FirstOrDefault(x => x.Id == id);
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
                email.CreationDate = email.UpdateDate = DateTime.Now;
                email.CreationUserId = email.UpdateUserId = this.User.GetUserID();

                _context.Emails.Add(email);
                _context.SaveChanges();

                new EmailService(_context, _smtp).SendEmail(email);
                return CreatedAtAction(nameof(GetById), new { email.Id }, email);
            }
            catch (Exception ex)
            {
                var date = DateTime.Now;
                _context.ExceptionLogs.Add(new ExceptionLog { Source = ex.Source, StackTrace = ex.StackTrace, Message = ex.Message.ToString(), CreationDate = date, UpdateDate = date });
                _context.SaveChanges();
                return Problem(APIMsg.ERR0001);
            }
        }

        [HttpDelete]
        [RequisitionFilter]
        [Authorize]
        public IActionResult Delete(long id)
        {
            Email email = _context.Emails.FirstOrDefault(x => x.Id == id);
            if (email == null) return NotFound();

            email.DeletionDate = DateTime.Now;
            _context.Emails.Update(email);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
