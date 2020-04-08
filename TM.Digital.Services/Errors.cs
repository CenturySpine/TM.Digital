using System;

namespace TM.Digital.Services
{
    internal static class Errors
    {
        internal static InvalidOperationException ErrorGameIdNotFound(Guid selectionGameId)
        {
            return new InvalidOperationException($"Game id {selectionGameId} not found");
        }
    }
}