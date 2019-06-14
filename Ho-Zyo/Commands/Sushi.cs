using System.Threading.Tasks;
using Discord.Commands;

namespace Ho_Zyo.Commands
{
    public class Sushi : ModuleBase
    {
        [Command("寿司")]
        public async Task SendSushi()
        {
            await ReplyAsync($"🍣=͟͟͞ =🍣=͟͟͞=( '-' 🍣 )ｽｼﾊﾟﾝﾁ");
        }
    }
}