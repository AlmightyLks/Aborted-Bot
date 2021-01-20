using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SynapseBot.Configs;
using SynapseBot.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseBot.Commands
{
    public class TextCommands : BaseCommandModule
    {
        public ILogger<Bot> Logger { private get; set; }
        public IServiceCollection ServiceCollection { private get; set; }
        public Bot Bot { private get; set; }

        [Command("reload")]
        [Cooldown(1, 2.5, CooldownBucketType.User)]
        [RequireGuild]
        public async Task Reload(CommandContext ctx)
        {
            //If command has whitelisted role ids & none of the member's roles are whitelisted
            if (Bot.Config.CommandsWhitelist.ContainsKey(ctx.Command.Name) &&
                !ctx.Member.Roles.Any(_ => Bot.Config.CommandsWhitelist[ctx.Command.Name] == _.Id))
                return; 

            try
            {
                Bot.LoadConfigs();
                await ctx.RespondAsync("Reload successful");
            }
            catch (Exception e)
            {
                await ctx.RespondAsync("Reloading configs failed.");
                Logger.LogError($"Exception thrown when executing \"{Bot.Config.Prefix}{ctx.Command.Name}\":\n{e}");
            }
        }

        [Command("ping")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        [RequireGuild]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task Ping(CommandContext ctx)
        {
            //If Author & Channel match
            PluginChannel pluginChannel = Bot.Config.PluginChannels.Find(_ => _.CreatorID == ctx.Member.Id && _.ChannelID == ctx.Channel.Id);

            if (pluginChannel == null)
                return;

            DiscordRole role = ctx.Guild.GetRole(pluginChannel.RoleID);
            await ctx.RespondAsync(role.Mention);
            await ctx.Message.DeleteAsync();
        }
    }
}
