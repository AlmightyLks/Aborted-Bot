using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseBot.Models
{
    public sealed class DiscordActivityConfig
    {
        public string Name { get; set; }
        public ActivityType Activity { get; set; }
    }
}
