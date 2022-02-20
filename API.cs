using Exiled.API.Features;
using System.Text.RegularExpressions;

namespace XPSystem
{
    static public class API
    {
        public static Regex ColorFind { get; } = new Regex(@"(?<=%).*(?=%)", RegexOptions.Compiled);
        static public void AddXP(Player player, int xp)
        {
            if (player.DoNotTrack || xp <= 0)
            {
                return;
            }
            PlayerLog log = Main.players[player.UserId];
            log.XP += xp;
            int lvlsGained = log.XP / Main.Instance.Config.XPPerLevel;
            if (lvlsGained > 0)
            {
                log.LVL += lvlsGained;
                log.XP -= lvlsGained * Main.Instance.Config.XPPerLevel;
                if (Main.Instance.Config.ShowAddedLVL)
                {
                    player.ShowHint(Main.Instance.Config.AddedLVLHint.Replace("%level%", log.LVL.ToString()));
                }
                EvaluateRank(player);
            }
            else if (Main.Instance.Config.ShowAddedXP)
            {
                player.ShowHint($"+ <color=green>{xp}</color> XP");
            }
        }
        static public void EvaluateRank(Player player)
        {
            string badgeText = player.Group == null ? string.Empty : player.Group.BadgeText;
            if (!Main.players.TryGetValue(player.UserId, out PlayerLog log))
            {
                log = new PlayerLog()
                {
                    LVL = 0,
                    XP = 0
                };
                Main.players[player.UserId] = log;
            }
            string badge = GetLVLBadge(log);
            player.RankName = Main.Instance.Config.BadgeStructure.Replace("%lvl%", log.LVL.ToString()).Replace("%badge%", ColorFind.Replace(badge, string.Empty)).Replace("%oldbadge%", badgeText);
            player.RankColor = ColorFind.Match(badge).Value;
        }
        static private string GetLVLBadge(PlayerLog player)
        {
            string biggestLvl = string.Empty;
            foreach (var pair in Main.Instance.Config.LevelsBadge) // might seem ugly but this is actually O(n)
            {
                if (player.LVL < pair.Key)
                {
                    break;
                }
                biggestLvl = pair.Value;
            }
            return biggestLvl;
        }
    }
}
