using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MSEmail.WebUI.Models.User;

public class LoginViewModel
{
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
