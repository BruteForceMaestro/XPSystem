using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
            bool levelUp = false;
            log.XP += xp;
            while (log.XP >= Main.Instance.Config.XPPerLevel)
            {
                levelUp = true;
                log.LVL += 1;
                log.XP -= Main.Instance.Config.XPPerLevel;
                Timing.RunCoroutine(EvaluateRank(player));
                if (Main.Instance.Config.ShowAddedLVL)
                {
                    string hint = Regex.Replace(Main.Instance.Config.AddedLVLHint, @"%level%", log.LVL.ToString());
                    player.ShowHint(hint);
                }
            }
            if (!levelUp && Main.Instance.Config.ShowAddedXP && xp > 0)
            {
                player.ShowHint($"+ <color=green>{xp}</color> XP");
            }
            Binary.WriteToBinaryFile(Main.path, Main.players);
        }
        static public IEnumerator<float> EvaluateRank(Player player)
        {
            yield return Timing.WaitForOneFrame;

            if (!Main.players.Exists(x => x.UserId == player.UserId))
            {
                if (player.Group == null)
                {
                    Main.players.Add(new PlayerLog(player.UserId, 0, 0, string.Empty));
                }
                else
                {
                    Main.players.Add(new PlayerLog(player.UserId, 0, 0, player.Group.BadgeText));
                }
            }
            Binary.WriteToBinaryFile(Main.path, Main.players);
            PlayerLog log = Main.players.Find(x => x.UserId == player.UserId);
            string badge = GetLVLBadge(player);
            string color = Regex.Match(badge, @"(?<=%).*(?=%)").Value;
            badge = Regex.Replace(badge, @"%.*%", string.Empty);
            string replaced = Regex.Replace(Main.Instance.Config.BadgeStructure, "%lvl%", log.LVL.ToString());
            replaced = Regex.Replace(replaced, "%badge%", badge.Trim());
            replaced = Regex.Replace(replaced, "%oldbadge%", log.OldBadge.Trim());
            player.RankName = replaced;
            player.RankColor = color;
        }
        static private string GetLVLBadge(Player player)
        {
            PlayerLog log = Main.players.Find(x => x.UserId == player.UserId);
            string biggestLvl = string.Empty;
            foreach (KeyValuePair<int, string> lvl in Main.Instance.Config.LevelsBadge)
            {
                if (log.LVL >= lvl.Key)
                {
                    biggestLvl = lvl.Value;
                }
                else
                {

                    break;
                }
            }
            return biggestLvl;
        }
    }
}
