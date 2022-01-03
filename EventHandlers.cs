using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

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
            if (ev.IsAllowed && ev.Killer != null && ev.Target != null)
            {
                Dictionary<RoleType, int> role;
                if (Main.Instance.Config.KillXP.TryGetValue(ev.Killer.Role, out role))
                {
                    int xp;
                    if (role.TryGetValue(ev.Target.Role, out xp))
                    {
                        API.AddXP(ev.Killer, xp);
                    }
                }
            }
        }

        public void OnEscape(EscapingEventArgs ev)
        {
            if (ev.IsAllowed)
            {
                API.AddXP(ev.Player, Main.Instance.Config.EscapeXP[ev.Player.Role]);
            }
        }



        public void OnRoundEnd(EndingRoundEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (player.LeadingTeam == ev.LeadingTeam && ev.IsAllowed && !Round.IsLocked)
                {
                    API.AddXP(player, Main.Instance.Config.TeamWinXP);
                }
            }
        }
    }
}
