using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseBot.Models
{
    public sealed class PluginChannel
    {
        public ulong CreatorID { get; set; }
        public ulong ChannelID { get; set; }
        public ulong RoleID { get; set; }
    }
}
