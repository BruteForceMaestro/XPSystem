namespace XPSystem
{
    public class PlayerLog
    {
        public PlayerLog(string USERID, int lvl, int xp, string OLDBADGE)
        {
            UserId = USERID;
            LVL = lvl;
            XP = xp;
            OldBadge = OLDBADGE;
        }
        public string UserId { get; set; }
        public int LVL { get; set; }
        public int XP { get; set; }
        public string OldBadge { get; set; }
    }
}
