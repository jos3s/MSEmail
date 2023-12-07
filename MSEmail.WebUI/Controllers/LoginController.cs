using Microsoft.AspNetCore.Mvc;
using MSEmail.WebUI.Models;
using MSEmail.WebUI.Models.User;
using MSEmail.WebUI.Service;

namespace MSEmail.WebUI.Controllers;
public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("Login");
    }
    
    [HttpPost("/")]
    public IActionResult Login(LoginViewModel login)
    {
        if (!ModelState.IsValid)
            throw new Exception("AA");
        try
        {
            var response = new ApiService().PostAsync<TokenViewModel, LoginViewModel>("/login", login).Result;

            return new JsonResult( new { response.Token });
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }
}
