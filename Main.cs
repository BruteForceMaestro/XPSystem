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
        public override Version Version { get; } = new Version(1, 0, 6);
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);
        private EventHandlers handlers;
        public static List<PlayerLog> players = new List<PlayerLog>();

        private void GetOrCreateJson()
        {
            if (!File.Exists(Instance.Config.SavePath))
            {
                Binary.WriteToBinaryFile(Instance.Config.SavePath, players);
            };
            players = Binary.ReadFromBinaryFile<List<PlayerLog>>(Instance.Config.SavePath);
            if (players == null) { players = new List<PlayerLog>(); }
        }
        public override void OnEnabled()
        {
            handlers = new EventHandlers();
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
            Player.Verified -= handlers.OnJoined;
            Player.Dying -= handlers.OnKill;
            Server.RoundEnded -= handlers.OnRoundEnd;
            Player.Escaping -= handlers.OnEscape;
            handlers = null;
            base.OnDisabled();
        }
    }
}