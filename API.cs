using Exiled.API.Features;

namespace XPSystem
{
    static public class API
    {
        static public void AddXP(Player player, int xp)
        {
            if (player.DoNotTrack || xp <= 0)
            {
                return;
            }
            PlayerLog log = Main.players.Find(x => x.UserId == player.UserId);
            log.XP += xp;
            int lvlsGained = log.XP / Main.Instance.Config.XPPerLevel;
            if (lvlsGained > 0)
            {
                log.LVL += lvlsGained;
                log.XP -= lvlsGained * Main.Instance.Config.XPPerLevel;
                if (Main.Instance.Config.ShowAddedLVL)
                {
                    player.ShowHint(Regexes.level.Replace(Main.Instance.Config.AddedLVLHint, log.LVL.ToString()));
                }
            }
            else if (Main.Instance.Config.ShowAddedXP)
            {
                player.ShowHint($"+ <color=green>{xp}</color> XP");
            }
            EvaluateRank(player);
            Binary.WriteToBinaryFile(Main.Instance.Config.SavePath, Main.players);
        }
        static public void EvaluateRank(Player player)
        {
            string badgeText = player.Group == null ? string.Empty : player.Group.BadgeText;
            PlayerLog log = Main.players.Find(x => x.UserId == player.UserId);
            if (log == null)
            {
                log = new PlayerLog(player.UserId, 0, 0, badgeText);
                Main.players.Add(log);
            }
            log.OldBadge = badgeText;
            string badge = GetLVLBadge(log);
            player.RankName = Regexes.FindBadge(badge, log);
            player.RankColor = Regexes.colorFind.Match(badge).Value;
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
