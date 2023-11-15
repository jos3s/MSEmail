using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web.Http.Controllers;
using System.Web;
using System.Diagnostics;
using MsEmail.API.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using MsEmail.API.Entities.Common;

namespace MsEmail.API.Filters
{
    public class RequisitionFilter : ActionFilterAttribute
    {
        private AppDbContext _context { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _context = filterContext.HttpContext.RequestServices.GetService<AppDbContext>();
            CreateLog("OnActionExecuting", filterContext.RouteData);
        }


        private void CreateLog(string methodName, RouteData routeData)
        {
            SystemLog log = new()
            {
                ControllerName = routeData.Values["controller"].ToString(),
                ActionName = routeData.Values["action"].ToString(),
            };
            log.CreationDate = log.UpdateDate = DateTime.Now;

            _context.SystemLogs.Add(log);
            _context.SaveChanges();
        }

    }
}
