using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace XPSystem
{
    internal class YamlPly
    {
        public static void Save()
        {
            using (StreamWriter writer = new StreamWriter(Main.Instance.Config.SavePath))
            {
                var serializer = new SerializerBuilder().Build();
                writer.Write(serializer.Serialize(Main.players));
            }
        }
        public static void Read()
        {
            string text = File.ReadAllText(Main.Instance.Config.SavePath);
            var deserializer = new DeserializerBuilder().Build();
            Main.players = deserializer.Deserialize<Dictionary<string, PlayerLog>>(text);
        }
    }
}
