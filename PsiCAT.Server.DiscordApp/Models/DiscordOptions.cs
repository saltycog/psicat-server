namespace PsiCAT.Server.DiscordApp.Models;

public class DiscordOptions
{
    public string BotToken { get; set; } = string.Empty;
    public ulong[] GuildIds { get; set; } = [];
    public bool EnableCommandSync { get; set; } = true;
    public bool GlobalCommandSync { get; set; } = false;

    // Migrate old single-guild config format: "GuildId": 123 → "GuildIds": [123]
    public ulong GuildId { set { if (GuildIds.Length == 0 && value != 0) GuildIds = [value]; } }
}
