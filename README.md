# PsiCAT Server

A collection of bots and automations for personal use. Only intended for my own personal use, but hey, steal my code!

### Available Components

- **[PsiCAT.Server.DiscordApp](PsiCAT.Server.DiscordApp)** - Discord app/bot for mostly pointless fun n' gags among my friends on a personal Discord server.

---
## Quick Start - Remote Installation

Deploy PsiCAT directly on a Linux server with one command:

```bash
sudo /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/saltycog/psicat-server/main/install.sh)"
```

### Fresh Installation

The installation script will:
1. Download the latest release from GitHub
2. Extract application files
3. Verify/install .NET 8 runtime
4. Create a `psicat` system user
5. Configure the systemd service
6. Prompt for configuration

After installation, configure your bot token and guild ID in `/opt/psicat/discord/appsettings.json`, then start the service with `sudo systemctl start psicat-discord`.

### Updates

Run the same command on an existing installation. The script will:
1. Detect the current version
2. Check for a newer release
3. Create a backup at `/opt/psicat/discord.backup`
4. Stop the service
5. Update binaries while preserving configuration and data files
6. Restart the service

If anything goes wrong, you can rollback:
```bash
sudo systemctl stop psicat-discord
sudo rm -rf /opt/psicat/discord
sudo cp -r /opt/psicat/discord.backup /opt/psicat/discord
sudo systemctl start psicat-discord
```

### Deployment

For detailed deployment and service management information, see the documentation in each component:

- **PsiCAT.Server.DiscordApp**: [PsiCAT.Server.DiscordApp/daemon/README.md](PsiCAT.Server.DiscordApp/daemon/README.md)
- **Bot Development**: [PsiCAT.Server.DiscordApp/README.md](PsiCAT.Server.DiscordApp/README.md)

### Service Management

After deployment, manage the bot service with `systemctl`:

```bash
# Check status
sudo systemctl status psicat-discord

# View live logs
sudo journalctl -u psicat-discord -f

# Restart
sudo systemctl restart psicat-discord

# Stop
sudo systemctl stop psicat-discord
```
