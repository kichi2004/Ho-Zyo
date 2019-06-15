using Newtonsoft.Json;

namespace Ho_Zyo
{
    public class Settings
    {
        public int SlotCallCount { get; set; }
        public int SlotAchievementCount { get; set; }

        public static Settings FromJson(string str) => JsonConvert.DeserializeObject<Settings>(str);
    }

    public static class Serialize
    {
        public static string ToJson(this Settings settings) => JsonConvert.SerializeObject(settings);
    }
}