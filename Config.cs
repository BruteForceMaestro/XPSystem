using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace XPSystem
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Hint shown to the players if they have DNT enabled.")]
        public string DNTHint { get; set; } = "We can't track your stats while you have DNT enabled in your game options!";

        [Description("(You may add your own entries) Role1: Role2: XP player with Role1 gets for killing a person with Role2 ")]
        public Dictionary<RoleType, Dictionary<RoleType, int>> KillXP { get; set; } = new Dictionary<RoleType, Dictionary<RoleType, int>>()
        {
            [RoleType.ClassD] = new Dictionary<RoleType, int>()
            {
                [RoleType.Scientist] = 200,
                [RoleType.FacilityGuard] = 150,
                [RoleType.NtfPrivate] = 200,
                [RoleType.NtfSergeant] = 250,
                [RoleType.NtfCaptain] = 300,
                [RoleType.Scp049] = 500,
                [RoleType.Scp0492] = 100,
                [RoleType.Scp106] = 500,
                [RoleType.Scp173] = 500,
                [RoleType.Scp096] = 500,
                [RoleType.Scp93953] = 500,
                [RoleType.Scp93989] = 500,
            },
            [RoleType.Scientist] = new Dictionary<RoleType, int>()
            {
                [RoleType.ClassD] = 50,
                [RoleType.ChaosConscript] = 200,
                [RoleType.ChaosRifleman] = 200,
                [RoleType.ChaosRepressor] = 250,
                [RoleType.ChaosMarauder] = 300,
                [RoleType.Scp049] = 500,
                [RoleType.Scp0492] = 100,
                [RoleType.Scp106] = 500,
                [RoleType.Scp173] = 500,
                [RoleType.Scp096] = 500,
                [RoleType.Scp93953] = 500,
                [RoleType.Scp93989] = 500,
            }
        };

        [Description("How many XP should a player get if their team wins.")]
        public int TeamWinXP { get; set; } = 250;

        [Description("How many XP is required to advance a level.")]
        public int XPPerLevel { get; set; } = 1000;

        [Description("Show a mini-hint if a player gets XP")]
        public bool ShowAddedXP { get; set; } = true;

        [Description("Show a hint to the player if he advances a level.")]
        public bool ShowAddedLVL { get; set; } = true;

        [Description("What hint to show if player advances a level. (if ShowAddedLVL = false, this is irrelevant)")]
        public string AddedLVLHint { get; set; } = "NEW LEVEL: <color=red>%level%</color>";

        [Description("(You may add your own entries) How many XP a player gets for escaping")]
        public Dictionary<RoleType, int> EscapeXP { get; set; } = new Dictionary<RoleType, int>()
        {
            [RoleType.ClassD] = 500,
            [RoleType.Scientist] = 300
        };

        [Description("(You may add your own entries) Level threshold and a badge. %color%. if you get a TAG FAIL in your console, either change your color, or remove special characters like brackets.")]
        public Dictionary<int, string> LevelsBadge { get; set; } = new Dictionary<int, string>()
        {
            [0] = "Visitor %cyan%",
            [1] = "Junior %orange%",
            [5] = "Senior %yellow%",
            [10] = "Veteran %red%",
            [50] = "Nerd %purple%"
        };

        [Description("The structure of the badge displayed in-game. Variables: %lvl% - the level. %badge% earned badge in specified in LevelsBadge. %oldbadge% - base-game badge, like ones specified in config-remoteadmin, or a global badge. can be null.")]
        public string BadgeStructure { get; set; } = "(LVL %lvl% | %badge%) %oldbadge%";
    }
}
