using System.Text.Json.Serialization;

namespace PsiCAT.Server.DiscordApp.Models;

public class Quote
{
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
