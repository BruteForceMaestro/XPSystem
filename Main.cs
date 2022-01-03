using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace XPSystem
{
    public class Main : Plugin<Config>
    {
        public static Main Instance { get; set; }
        private EventHandlers handlers = new();
        public static readonly string path = Environment.ExpandEnvironmentVariables(@"%AppData%\EXILED\Players.json");
        public static List<PlayerLog> players = new List<PlayerLog>();

        private void GetOrCreateJson()
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, JsonSerializer.Serialize(players));
                return;
            };
            players = JsonSerializer.Deserialize<List<PlayerLog>>(File.ReadAllText(path));
            if (players == null) { players = new List<PlayerLog>(); }
        }
        public override void OnEnabled()
        {
            handlers = new();
            Player.Verified += handlers.OnJoined;
            Player.Dying += handlers.OnKill;
            Server.EndingRound += handlers.OnRoundEnd;
            Player.Escaping += handlers.OnEscape;
            Instance = this;
            GetOrCreateJson();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            handlers = null;
            Player.Verified -= handlers.OnJoined;
            Player.Dying -= handlers.OnKill;
            Server.EndingRound -= handlers.OnRoundEnd;
            Player.Escaping -= handlers.OnEscape;
            base.OnDisabled();
        }
    }
}