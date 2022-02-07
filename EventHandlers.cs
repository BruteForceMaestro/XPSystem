using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Linq;

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
            Timing.RunCoroutine(API.EvaluateRank(ev.Player));
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
            if (ev.Handler.Type == Exiled.API.Enums.DamageType.PocketDimension && Main.Instance.Config.KillXP.TryGetValue(RoleType.Scp106, out var xp106) && xp106.TryGetValue(ev.Target.Role, out var xp1))
            {
                foreach (Player scp106 in Player.Get(RoleType.Scp106))
                {
                    API.AddXP(scp106, xp1);
                }
            }
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            API.AddXP(ev.Player, Main.Instance.Config.EscapeXP[ev.Player.Role]);
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (Player player in Player.List.Where(x => x.LeadingTeam == ev.LeadingTeam))
            {
                API.AddXP(player, Main.Instance.Config.TeamWinXP);
            }
        }
    }
}
