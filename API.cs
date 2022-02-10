using Exiled.API.Features;

namespace XPSystem
{
    static public class API
    {
        static public void AddXP(Player player, int xp)
        {
            if (player.DoNotTrack)
            {
                return;
            }
            PlayerLog log = Main.players.Find(x => x.UserId == player.UserId);
            log.XP += xp;
            if (log.XP >= Main.Instance.Config.XPPerLevel)
            {
                log.LVL += log.XP / Main.Instance.Config.XPPerLevel;
                log.XP -= log.XP % Main.Instance.Config.XPPerLevel;
                if (Main.Instance.Config.ShowAddedLVL)
                {
                    player.ShowHint(Regexes.level.Replace(Main.Instance.Config.AddedLVLHint, log.LVL.ToString()));
                }
            }
            else if (Main.Instance.Config.ShowAddedXP && xp > 0)
            {
                player.ShowHint($"+ <color=green>{xp}</color> XP");
            }

            EvaluateRank(player);
            Binary.WriteToBinaryFile(Main.Instance.Config.SavePath, Main.players);
        }
        static public void EvaluateRank(Player player)
        {
            string badgeText = player.Group == null ? string.Empty : player.Group.BadgeText;
            if (!Main.players.Exists(x => x.UserId == player.UserId))
            {
                Main.players.Add(new PlayerLog(player.UserId, 0, 0, badgeText));
            }
            PlayerLog log = Main.players.Find(x => x.UserId == player.UserId);
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
                if (player.LVL <= pair.Key)
                {
                    break;
                }
                biggestLvl = pair.Value;
            }
            return biggestLvl;
        }
    }
}
