using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace XPSystem
{

    internal class Set : ICommand
    {
        public string Command => "set";

        public string[] Aliases => new string[] { };

        public static Set Instance { get; } = new Set();

        private readonly string usage = "Usage : XPSystem set (UserId | in-game id) (int amount)";

        public string Description => $"set a certain value in player's lvl variable. {usage}";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("xps.set"))
            {
                response = "You don't have permission (xps.set) to use this command.";
                return false;
            }
            if (arguments.Count != 2)
            {
                response = usage;
                return false;
            }
            PlayerLog player = Main.players.Find(x => x.UserId == arguments.At(0));
            if (player == default(PlayerLog))
            {
                var byId = Player.Get(arguments.At(0));
                if (byId == null)
                {
                    response = "Invalid UserId or the player hasn't joined the server yet.";
                    return false;
                }
                player = Main.players.Find(x => x.UserId == byId.UserId);
            }
            if (int.TryParse(arguments.At(1), out int lvl) && lvl >= 0)
            {
                player.LVL = lvl;
                response = $"{player.UserId}'s LVL is now {player.LVL}";
                API.EvaluateRank(Player.Get(player.UserId));
                Binary.WriteToBinaryFile(Main.Instance.Config.SavePath, Main.players);
                return true;
            }
            else
            {
                response = $"Invalid amount of LVLs : {lvl}";
                return false;
            }
        }
    }
}
