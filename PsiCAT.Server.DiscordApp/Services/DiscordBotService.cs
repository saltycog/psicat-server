using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PsiCAT.Server.DiscordApp.Interactions;
using PsiCAT.Server.DiscordApp.Models;

namespace PsiCAT.Server.DiscordApp.Services;

public class DiscordBotService : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactions;
    private readonly CommandHandler _commandHandler;
    private readonly IOptions<DiscordOptions> _options;
    private readonly ILogger<DiscordBotService> _logger;

    public DiscordBotService(
        DiscordSocketClient client,
        InteractionService interactions,
        CommandHandler commandHandler,
        IOptions<DiscordOptions> options,
        ILogger<DiscordBotService> logger)
    {
        _client = client;
        _interactions = interactions;
        _commandHandler = commandHandler;
        _options = options;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
        _client.InteractionCreated += _commandHandler.HandleInteractionAsync;

        _interactions.Log += LogAsync;

        try
        {
            await _client.LoginAsync(TokenType.Bot, _options.Value.BotToken);
            await _client.StartAsync();
            _logger.LogInformation("Discord bot started");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start Discord bot");
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
            _logger.LogInformation("Discord bot stopped");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping Discord bot");
        }
    }

    private async Task ReadyAsync()
    {
        _logger.LogInformation("Discord bot ready");

        await _commandHandler.InitializeAsync();

        if (_options.Value.EnableCommandSync)
        {
            foreach (ulong guildId in _options.Value.GuildIds)
            {
                try
                {
                    await _interactions.RegisterCommandsToGuildAsync(guildId);
                    _logger.LogInformation("Commands registered to guild {GuildId}", guildId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to register commands to guild {GuildId}", guildId);
                }
            }

            if (_options.Value.GlobalCommandSync)
            {
                try
                {
                    await _interactions.RegisterCommandsGloballyAsync();
                    _logger.LogInformation("Commands registered globally");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to register commands globally");
                }
            }
        }
    }

    private static Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;
    }
}
