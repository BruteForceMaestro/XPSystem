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
        public override Version Version { get; } = new Version(1, 0, 7);
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);
        private EventHandlers handlers;
        public static Dictionary<string, PlayerLog> players = new Dictionary<string, PlayerLog>();

        private void Deserialize()
        {
            if (!File.Exists(Instance.Config.SavePath))
            {
                YamlPly.Save();
                return;
            }
            YamlPly.Read();
        }
        public override void OnEnabled()
        {
            handlers = new EventHandlers();
            Player.Verified += handlers.OnJoined;
            Player.Dying += handlers.OnKill;
            Server.RoundEnded += handlers.OnRoundEnd;
            Player.Escaping += handlers.OnEscape;
            Instance = this;
            Deserialize();
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