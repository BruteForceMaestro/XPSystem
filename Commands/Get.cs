using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace XPSystem
{
    internal class Get : ICommand
    {
        public string Command => "get";

        public static Get Instance { get; } = new Get();    

        public string[] Aliases => new string[] { };

        public string Description => "Gets the player's XP and LVL values by userid or in-game id";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("xps.get"))
            {
                response = "You don't have permission (xps.get) to use this command.";
                return false;
            }
            if (arguments.Count == 0)
            {
                response = "Usage : XPSystem get (userid)";
                return false;
            }
            var byId = Player.Get(arguments.At(0));
            if (byId == null)
            {
                response = "Invalid UserId or the player hasn't joined the server yet.";
                return false;
            }
            PlayerLog player = Main.players[byId.UserId];
            response = $"UserId: {byId.UserId} | LVL: {player.LVL} | XP: {player.XP}";
            return true;
        }
    }
}
