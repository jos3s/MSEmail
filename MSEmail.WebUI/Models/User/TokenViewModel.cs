using System.Text.Json.Serialization;

namespace MSEmail.WebUI.Models.User;

public class TokenViewModel
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

}
