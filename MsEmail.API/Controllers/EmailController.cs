using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsEmail.API.Filters;
using MSEmail.API.Messages;
using MsEmail.API.Models;
using MsEmail.API.Models.EmailModel;
using MsEmail.Domain.Entities;
using MsEmail.Infra.Context;
using MSEmail.API.Models.Email;
using MSEmail.Domain.Enums;
using MSEmail.Infra.Business;
using MSEmail.Infra.Repository;
using MSEmail.API.Controllers.Base;

namespace MsEmail.API.Controllers;

[RequisitionFilter]
public class EmailController : BaseController
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<List<Email>>))]
    public async Task<IActionResult> GetAll(bool withDeletionDate)
    {
        try
        {
            List<Email> emails = new();
            emails = this.User.GetRole().Equals("admin") 
                ? _emails.GetAll(withDeletionDate) 
                : _emails.GetEmailsByUserId((long)this.User.GetUserID(), withDeletionDate);

            List<ViewEmailModel> viewEmailModels = emails.Select(e => (ViewEmailModel)e).ToList();

            return Ok(new ApiResult<ListEmailModel>(new ListEmailModel { Count = emails.Count(), Emails = viewEmailModels }));
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(GetAll), this.GetType().Name, ServiceType.API);
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0004));
        }
    }

    [HttpGet("{id}")]
    [RequisitionFilter]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<Email>))]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var email = _emails.GetById(id);
            if (email == null) return NotFound();
            return Ok(new ApiResult<Email>(email));
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(GetById), this.GetType().Name, ServiceType.API);
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0004));
        }
    }

    [Authorize]
    [HttpPost("send")]
    [RequisitionFilter]
    public async Task<IActionResult> Post([FromBody] CreateEmailModel createEmailModel)
    {
        try
        {
            if (!ModelState.IsValid)
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
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0001));
        }
    }

    [Authorize]
    [HttpPatch("{id}")]
    [RequisitionFilter]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<ViewEmailModel>))]
    public async Task<IActionResult> Patch([FromRoute] long id, UpdateEmailModel updateEmail)
    {
        try
        {
            if (updateEmail.IsNull())
                return BadRequest(new ApiResult<string>(APIMsg.REQ0002));

            var email = _emails.GetById(id);

            if (email == null) return NotFound(id);

            if (email.Status.Equals(EmailStatus.Sent))
                return StatusCode(409, new ApiResult<string> (APIMsg.ERR0005));

            if (!updateEmail.SendDate.IsNull())
                email.SendDate = (DateTime)updateEmail.SendDate!;

            if (!updateEmail.Subject.IsNull())
                email.Subject = updateEmail.Subject;

            if (!updateEmail.Body.IsNull())
                email.Body = updateEmail.Body;

            email.UpdateUserId = (long)User.GetUserID();
            _emails.Update(email).Save();

            ViewEmailModel viewEmailModel = email;

            return Ok(new ApiResult<ViewEmailModel>(viewEmailModel));
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(Patch), this.GetType().Name, ServiceType.API);
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0004));
        }
    }

    [HttpDelete("{id}")]
    [RequisitionFilter]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NoContentResult))]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var email = _emails.GetById(id);
            if (email == null) return NotFound();

            if (email.Status.Equals(EmailStatus.Sent))
                return BadRequest(new ApiResult<string> (APIMsg.ERR0007));

            email.DeletionDate = DateTime.Now;
            email.UpdateUserId = (long)this.User.GetUserID();
            _emails.Update(email).Save();
            return NoContent();
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(Delete), this.GetType().Name, ServiceType.API);
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0004));
        }
    }

    [Authorize]
    [HttpGet("drafts")]
    [RequisitionFilter]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<ListEmailModel>))]
    public async Task<IActionResult> GetEmailsInDraft()
    {
        try
        {
            List<Email> emails = _emails.GetEmailsByStatusAndUserId(EmailStatus.Draft, (long)this.User.GetUserID());
            
            if(emails.Count == 0)
                return NoContent();

            List<ViewEmailModel> viewEmailModels = emails.Select(email => (ViewEmailModel)email).ToList();

            return Ok(new ApiResult<ListEmailModel>(new ListEmailModel { Count = viewEmailModels.Count, Emails = viewEmailModels }));
        }
        catch (Exception ex)
        {
            _commonLog.SaveExceptionLog(ex, nameof(GetEmailsInDraft), this.GetType().Name, ServiceType.API);
            return StatusCode(500, new ApiResult<string>(APIMsg.ERR0004));
        }
    }
}