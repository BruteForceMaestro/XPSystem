using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace XPSystem
{
    public class Main : Plugin<Config>
    {
        public static Main Instance { get; set; }
        private EventHandlers handlers = new();
        public static readonly string path = Environment.ExpandEnvironmentVariables(@"%AppData%\EXILED\Players.bin");
        public static List<PlayerLog> players = new List<PlayerLog>();

        private void GetOrCreateJson()
        {
            if (!File.Exists(path))
            {
                Binary.WriteToBinaryFile(path, players);
            };
            players = Binary.ReadFromBinaryFile<List<PlayerLog>>(path);
            if (players == null) { players = new List<PlayerLog>(); }
        }
        public override void OnEnabled()
        {
            handlers = new();
            Player.Verified += handlers.OnJoined;
            Player.Dying += handlers.OnKill;
            Server.RoundEnded += handlers.OnRoundEnd;
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
            Server.RoundEnded -= handlers.OnRoundEnd;
            Player.Escaping -= handlers.OnEscape;
            base.OnDisabled();
        }
    }
}