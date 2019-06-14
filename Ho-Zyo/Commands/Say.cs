using System.Threading.Tasks;
using Discord.Commands;

namespace Ho_Zyo.Commands
{
    // Create a module with no prefix
    public class Command : ModuleBase
    {
        // ~say hello -> hello
        [Command("say"), Summary("Echos a message.")]
        public async Task SendSay([Remainder, Summary("Need echo text")] string echo)
        {
            // ReplyAsync is a method on ModuleBase
            await ReplyAsync(echo);
        }
    }
}