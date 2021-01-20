using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SynapseBot.Commands;
using SynapseBot.Configs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SynapseBot
{
    public class Bot : IHostedService
    {
        public Config Config { get; set; }
        private DiscordClient _client;
        private CommandsNextExtension _commands;
        private IServiceCollection _serviceCollection;
        private IServiceProvider _services;
        private ILogger<Bot> _logger;

        public Bot(ILogger<Bot> logger, IServiceCollection serviceColl)
        {
            _logger = logger;
            _services = serviceColl.BuildServiceProvider();
            _serviceCollection = serviceColl;
        }
        private async Task LoadBot()
        {
            Console.Title = Config.ConsoleTitle;
            try
            {
                _client = new DiscordClient(new DiscordConfiguration()
                {
                    AutoReconnect = true,
                    Token = Config.Token,
                    TokenType = TokenType.Bot
                });

                await _client.ConnectAsync(Config.DiscordActivity, UserStatus.Online);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
        private void LoadServices()
        {
            _serviceCollection.AddSingleton(_client);
            _serviceCollection.AddSingleton(this);
            _services = _serviceCollection.BuildServiceProvider();
        }
        private void LoadCommands()
        {
            try
            {
                var cmdCfg = new CommandsNextConfiguration
                {
                    CaseSensitive = false,
                    EnableDefaultHelp = true,
                    StringPrefixes = new[] { Config.Prefix },
                    IgnoreExtraArguments = true,
                    EnableMentionPrefix = false,
                    EnableDms = true,
                    Services = _services
                };

                _commands = _client.UseCommandsNext(cmdCfg);

                _commands.RegisterCommands<TextCommands>();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
        public void LoadConfigs()
        {
            Config cfg = Config.LoadConfigs();
            Config = cfg ?? throw new Exception("Config.json is empty or invalid");
        }
        private async Task SetupBot()
        {
            try
            {
                LoadConfigs();
                await LoadBot();
                LoadServices();
                LoadCommands();
                _logger.LogInformation("Bot loaded successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"Loading Bot failed\n{e}");
                Environment.Exit(0);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
            => await SetupBot();
        public async Task StopAsync(CancellationToken cancellationToken)
            => await _client.DisconnectAsync();
    }
}
