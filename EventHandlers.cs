using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using System.Linq;
using MEC;

namespace XPSystem
{
    class EventHandlers
    {
        public void OnJoined(VerifiedEventArgs ev)
        {
            if (ev.Player.DoNotTrack)
            {
                ev.Player.OpenReportWindow(Main.Instance.Config.DNTHint);
                return;
            }
            Timing.CallDelayed(0.15f, () => API.EvaluateRank(ev.Player));
        }

        public void OnKill(DyingEventArgs ev)
        {
            if (ev.Target == null)
            {
                return;
            }
            if (ev.Killer != null && Main.Instance.Config.KillXP.TryGetValue(ev.Killer.Role, out var role) && role.TryGetValue(ev.Target.Role, out var xp))
            {
                API.AddXP(ev.Killer, xp);
                return;
            }
            if (ev.Handler.Type == DamageType.PocketDimension && Main.Instance.Config.KillXP.TryGetValue(RoleType.Scp106, out var xp106) && xp106.TryGetValue(ev.Target.Role, out var xp1))
            {
                var scp106s = Player.Get(RoleType.Scp106).FirstOrDefault();
                if (scp106s != null) // reason this is not in the if statement above is because it would be needlessly iterating over the whole player list
                {
                    API.AddXP(scp106s, xp1);
                }
            }
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            API.AddXP(ev.Player, Main.Instance.Config.EscapeXP[ev.Player.Role]);
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.LeadingTeam == ev.LeadingTeam)
                {
                    API.AddXP(player, Main.Instance.Config.TeamWinXP);
                }
            }
            YamlPly.Save();
        }
    }
}
