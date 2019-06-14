using System.Threading.Tasks;
using Discord.Commands;

namespace Ho_Zyo.Commands
{
    public class Help : ModuleBase
    {
        [Command("help")]
        public async Task SendHelp()
        {
            var descriptions = new[]
            {
                "```",
                "slot [num] -- 絵文字でスロットを回します。[num]回回します。未指定の場合は1。上限回数は10回です。",
                "establish -- これまで成功した確率を出します。slotが成功した時に自動で実行されます。",
                "help -- コマンドリストと説明を表示します。",
                "寿司 -- ( '-' 🍣)ｽｼﾊﾟﾝﾁします。",
                "```"
            };

            await ReplyAsync(string.Join("\n", descriptions));
        }
    }
}