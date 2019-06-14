using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Ho_Zyo.Model;

namespace Ho_Zyo.Commands
{
    public class Yosano : ModuleBase
    {
        [Command("与謝野")]
        public async Task SendYosano([Remainder] int repeat = 1)
        {
            var yosano = Emoji.GetEmojis().Single(x => x.Name == "yosano");
            var sb = new StringBuilder();
            for (int i = 0; i < repeat; i++)
            {
                sb.Append(yosano.ToMsg());
            }

            await ReplyAsync(sb.ToString());
        }
    }
}