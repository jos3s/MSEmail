using System.Text.Json.Serialization;
using MSEmail.Domain.Enums;

namespace MSEmail.WebUI.Models;

public class EmailModel
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("emailFrom")]
    public string EmailFrom { get; set; }

    [JsonPropertyName("emailTo")]

    public string EmailTo { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonPropertyName("status")]
    public EmailStatus Status { get; set; }

    [JsonPropertyName("sendDate")]
    public DateTime? SendDate { get; set; }

    [JsonPropertyName("creationDate")]
    public DateTime CreationDate { get; set; }

    [JsonPropertyName("updateDate")]
    public DateTime UpdateDate { get; set; }

    [JsonPropertyName("deletionDate")]
    public DateTime? DeletionDate { get; set; }
}
