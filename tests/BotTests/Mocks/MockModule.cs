

namespace BotTests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    public class MockModule : ModuleBase
    {
        [Command("mockcommand")]
        public async Task MockCommandAsync() { }

        public class MockSubModule : ModuleBase {
            [Command("mocksubcommand")]
            public async Task MockSubCommandAsync() { }
        }
    }

    [Group("mock")]
    public class MockGroupModule : ModuleBase {
        [Command("mockcommand")]
        public async Task MockCommandAsync() { }

        [Group("mocksub")]
        public class MockSubGroup : ModuleBase {
            [Command("mocksubgroupcommand")]
            public async Task MockSubGroupCommandAsync() { }
        }
    }
}
