using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{

    public static long? GetUserID(this ClaimsPrincipal User)
    {
        try
        {
            return long.Parse(User.Claims.First(i => i.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string GetRole(this ClaimsPrincipal User)
    {
        return User.Claims.First(i => i.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
    }
}
