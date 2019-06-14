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
        private int _callCount = 0;
        private int _achievementCount = 0;

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

            for (int j = 0; j < num; j++)
            {
                var chosen = new List<string>();

                for (int i = 0; i < 3; i++)
                {
                    var emoji = _emojis[random.Next(_emojis.Count - 1)];
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

            send.Add($"{GetEstablishment():F5} ({_achievementCount}/{_callCount})");
            await ReplyAsync(string.Join("\n", send));
        }

        private double GetEstablishment()
        {
            return _achievementCount == 0 ? 0 : (double) _achievementCount / _callCount;
        }

        [Command("establish")]
        public async Task Establishment()
        {
            await ReplyAsync($"{GetEstablishment():F5} ({_achievementCount}/{_callCount})");
        }
    }
}