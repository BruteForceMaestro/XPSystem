using System.Text.RegularExpressions;

namespace XPSystem
{
    public static class Regexes
    {
        public static Regex level = new Regex("%level%", RegexOptions.Compiled);
        public static Regex colorFind = new Regex(@"(?<=%).*(?=%)", RegexOptions.Compiled);
        public static Regex badgeFind = new Regex(@"%.*%", RegexOptions.Compiled);
        static Regex lvlFind = new Regex("%lvl%", RegexOptions.Compiled);
        static Regex bFind = new Regex("%badge%", RegexOptions.Compiled);
        static Regex oldBadge = new Regex("%oldbadge%", RegexOptions.Compiled);

        public static string FindBadge(string badge, PlayerLog log)
        {
            badge = badgeFind.Replace(badge, string.Empty);
            string replaced = lvlFind.Replace(Main.Instance.Config.BadgeStructure, log.LVL.ToString());
            replaced = bFind.Replace(replaced, badge.Trim());
            replaced = oldBadge.Replace(replaced, log.OldBadge.Trim());
            return replaced;
        }
    }
}
