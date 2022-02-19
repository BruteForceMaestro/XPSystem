using System;

namespace XPSystem
{
    [Serializable]
    public class PlayerLog
    {
        public PlayerLog() { } // for yamldotnet
        
        public PlayerLog(int lvl, int xp, string OLDBADGE)
        {
            LVL = lvl;
            XP = xp;
            OldBadge = OLDBADGE;
        }
        public int LVL { get; set; }
        public int XP { get; set; }
        public string OldBadge { get; set; }
    }
}
