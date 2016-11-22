namespace AntiBot.Modules {

    using Discord.Commands;
    using System.Threading.Tasks;

    [Group("mock")]
    public class MockGroupModule : ModuleBase {      
        [Group("mocksub")]
        public class MockSubGroup : ModuleBase {       
            [Command("mocksubgroupcommand")]
            public async Task MockSubGroupCommandAsync() {
                await ReplyAsync("MockSubGroupCommandAsync");
            } 
        }                                     

        [Command("1mockcommand")]
        public async Task MockCommandAsync() {
            await ReplyAsync("1MockCommandAsync");
        }    
    }

    public class MockModule : ModuleBase {        

        public class MockSubModule : ModuleBase {       

            [Command("mocksubcommand")]
            public async Task MockSubCommandAsync() {
                await ReplyAsync("MockSubCommandAsync");
            }  
        }                                

        [Command("2mockcommand")]
        public async Task MockCommandAsync() {
            await ReplyAsync("2MockCommandAsync");
        }   
    }
}