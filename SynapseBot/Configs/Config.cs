using DSharpPlus.Entities;
using Newtonsoft.Json;
using SynapseBot.Models;
using System.Collections.Generic;
using System.IO;

namespace SynapseBot.Configs
{
    public sealed class Config
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public Dictionary<string, ulong> CommandsWhitelist { get; set; }
        public List<PluginChannel> PluginChannels { get; set; }
        public DiscordActivityConfig DiscordActivityCfg { get; set; }
        public string ConsoleTitle { get; set; }

        [JsonIgnore]
        public DiscordActivity DiscordActivity { get; set; }

        public Config()
        {
            Token = "Not today.";
            Prefix = "!";
            ConsoleTitle = "";
            DiscordActivityCfg = new DiscordActivityConfig()
            {
                Name = "Synapse",
                Activity = ActivityType.Watching
            };
            CommandsWhitelist = new Dictionary<string, ulong>();
            PluginChannels = new List<PluginChannel>();
        }
        public Config(Config cfg)
        {
            Token = cfg.Token;
            Prefix = cfg.Prefix;
            ConsoleTitle = cfg.ConsoleTitle;
            DiscordActivityCfg = cfg.DiscordActivityCfg;
            CommandsWhitelist = cfg.CommandsWhitelist;
            PluginChannels = cfg.PluginChannels;
        }
        public static Config LoadConfigs()
        {
            Config cfg = new Config();

            if (!File.Exists("Config.json"))
                File.WriteAllText("Config.json", JsonConvert.SerializeObject(cfg));
            else
                cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));

            if (cfg != null)
                cfg.DiscordActivity = new DiscordActivity(cfg.DiscordActivityCfg.Name, cfg.DiscordActivityCfg.Activity);
            else
                cfg.DiscordActivity = new DiscordActivity("Synapse", ActivityType.Watching);

            return cfg;
        }
    }
}
