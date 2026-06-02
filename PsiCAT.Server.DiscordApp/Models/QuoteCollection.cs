using System.Text.Json.Serialization;

namespace PsiCAT.Server.DiscordApp.Models;

public class QuoteCollection
{
    [JsonPropertyName("quotes")]
    public List<Quote> Quotes { get; set; } = new();
}
