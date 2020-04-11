using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Model.Board;
using TM.Digital.Model.Player;
using TM.Digital.Services.Common;
using TM.Digital.Transport.Hubs.Hubs;

namespace TM.Digital.Services
{
    public static class Hubconcentrator
    {
        public static IHubContext<ClientNotificationHub> Hub { get; set; }
    }
    internal class ActionPlanner
    {
        private static readonly Queue<Action<Player, Board>> RemainingActions = new Queue<Action<Player, Board>>();

        public void Plan(Action<Player, Board> action)
        {
            RemainingActions.Enqueue(action);
        }

        public async Task FollowPlan(Player p, Board b)
        {

            if (RemainingActions.Any())
            {
                await Task.Run(() =>
                {
                    var action = RemainingActions.Dequeue();
                    action.Invoke(p, b);
                });
            }
        }

        public async Task Continue(Player player,Board board)
        {
            if (RemainingActions.Any())
            {
                var action = RemainingActions.Dequeue();
                action.Invoke(player, board);
            }
            else
            {
                await Logger.Log(player.Name, $"All actions choices done. Sending game update to all players");

                OnActionFinished(player);
            }

        }

        public event AllActionFinishedEventHandler ActionFinished;


        protected virtual void OnActionFinished(Player player)
        {
            ActionFinished?.Invoke(player);
        }
    }

    public delegate void AllActionFinishedEventHandler(Player player);
}