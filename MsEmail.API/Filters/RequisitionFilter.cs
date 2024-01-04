using Microsoft.AspNetCore.Mvc.Filters;
using MsEmail.Domain.Entities.Common;
using MsEmail.Infra.Context;
using MSEmail.Common.Utils;

namespace MsEmail.API.Filters;

public class RequisitionFilter : ActionFilterAttribute
{
    private AppDbContext _context { get; set; }
    private ActionExecutingContext _actionExecutingContext { get; set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        _actionExecutingContext = filterContext;
        _context = filterContext.HttpContext.RequestServices.GetService<AppDbContext>();
        CreateLog("OnActionExecuting", filterContext.RouteData);
    }


    private void CreateLog(string methodName, RouteData routeData)
    {
        SystemLog log = new()
        {
            ControllerName = routeData.Values["controller"].ToString(),
            ActionName = routeData.Values["action"].ToString(),
            ServiceType = MSEmail.Domain.Enums.ServiceType.API,
        };
        log.CreationDate = log.UpdateDate = DateTime.Now;
        log.CreationUserId = log.UpdateUserId = _actionExecutingContext.HttpContext?.User?.GetUserID() ?? ConfigHelper.DefaultUserId;
        _context.SystemLogs.Add(log);
        _context.SaveChanges();
    }
}