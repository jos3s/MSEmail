using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsEmail.API.Context;
using MsEmail.API.DTO;
using MsEmail.API.Entities;
using MsEmail.API.Entities.Enums;
using MsEmail.API.Service;

namespace MsEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailContext _context;
        private readonly IOptions<SmtpConfiguration> _smtp;

        public EmailController(EmailContext context, IOptions<SmtpConfiguration> smtp)
        {
            _context = context;
            _smtp = smtp;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Emails.Where(x => x.DeletionDate == null));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var email = _context.Emails.FirstOrDefault(x => x.Id == id);
            if (email == null) return NotFound();
            return Ok(email);
        }

        [HttpPost("send")]
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

                _context.Emails.Add(email);
                _context.SaveChanges();

                new EmailService(_context, _smtp).SendEmail(email);
                return CreatedAtAction(nameof(GetById), new { email.Id }, email);
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpDelete]
        public IActionResult Delete(long id)
        {
            Email email = _context.Emails.FirstOrDefault(x => x.Id == id);
            if (email == null) return NotFound();

            email.DeletionDate = DateTime.Now;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
