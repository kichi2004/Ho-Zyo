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
        private readonly List<Emoji> _emojis = Emoji.GetEmojis().Where(x => x.Animated == false).ToList();
        private readonly Random _random = new Random();


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
            #if DEBUG
              , "[Debug Message]"
            #endif
            };

            for (int j = 0; j < num; j++)
            {
                var chosen = new List<string>();

                for (int i = 0; i < 3; i++)
                {
                    var emoji = _emojis[_random.Next(_emojis.Count - 1)];
                    chosen.Add(emoji.ToString());
                }

                send.Add(string.Join(' ', chosen));
                DiscordBot.CallCount++;

                if (chosen.Any(x => x != chosen.First())) continue;
                send.Add($"おめでとうございます! {chosen.First()}");
                DiscordBot.AchievementCount++;
            }

            send.Add(GetEstablishment());
            await ReplyAsync(string.Join("\n", send));
        }

        private string GetEstablishment()
        {
            int achievementCount = DiscordBot.AchievementCount,
                callCount = DiscordBot.CallCount;
            var establish = achievementCount == 0 ? 0 : 100.0 * achievementCount / callCount;
            return $"{establish:F2} ({achievementCount}/{callCount})";
        }

        [Command("establish")]
        public async Task SendEstablishment()
        {
            await ReplyAsync(GetEstablishment());
        }
    }
}