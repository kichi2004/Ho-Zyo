using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Ho_Zyo.Model;

namespace Ho_Zyo.Commands
{
    public class Slot : ModuleBase
    {
        private static int _callCount = 0;
        private static int _achievementCount = 0;

        [Command("slot")]
        public async Task DoSlot(int num = 1)
        {
            const int limit = 10;
            if (num > limit)
            {
                await ReplyAsync($"{Context.User.Mention} {limit}回以下にしてください！");
                return;
            }

            if (num < 1)
            {
                await ReplyAsync($"{Context.User.Mention} 1以上にしてください！");
                return;
            }

            var send = new List<string>
            {
                Context.User.Mention
            };

            var random = new Random();
            var emojis = Emoji.GetEmojis().Where(x => x.Animated == false).ToList();

            for (int j = 0; j < num; j++)
            {
                var chosen = new List<string>();

                for (int i = 0; i < 3; i++)
                {
                    var emoji = emojis[random.Next(emojis.Count - 1)];
                    chosen.Add(emoji.ToString());
                }

                send.Add(string.Join(' ', chosen));
                _callCount++;

                if (chosen.All(x => x == chosen.First()))
                {
                    send.Add($"おめでとうございます! {chosen.First()}");
                    _achievementCount++;
                }
            }

            send.Add(GetEstablishment().ToString());
            await ReplyAsync(string.Join("\n", send));
        }

        private static double GetEstablishment()
        {
            return (double) _achievementCount / _callCount;
        }

        [Command("establish")]
        public async Task Establishment()
        {
            var establishment = (double) _achievementCount / _callCount;
            if (_achievementCount == 0)
            {
                await ReplyAsync($"0% (0 / {_callCount})");
            }
            else
            {
                await ReplyAsync($"{establishment * 100:F3}% ({_achievementCount} / {_callCount})");
            }
        }
    }
}