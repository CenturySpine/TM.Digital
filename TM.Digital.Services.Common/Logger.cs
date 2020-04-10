using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TM.Digital.Model;
using TM.Digital.Transport.Hubs.Hubs;

namespace TM.Digital.Services.Common
{
    public static class Logger
    {
        private static string logpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
            "services.log");

        static Logger()
        {
            if (File.Exists(logpath))
            {
                using (var sw = new StreamWriter(logpath, false))
                {
                    sw.WriteLine("Init");
                }
            }
        }

        public static IHubContext<ClientNotificationHub> HubContext { get; set; }

        public static async Task Log(string playerName, string message)
        {

            // create a global mutex
            //using (var mutex = new Mutex(false, "tmDigitalLogFileLock"))
            //{
            //    Console.WriteLine("Waiting for mutex");
            //    var mutexAcquired = false;
            //    try
            //    {
            //        // acquire the mutex (or timeout after 60 seconds)
            //        // will return false if it timed out
            //        mutexAcquired = mutex.WaitOne(60000);
            //    }
            //    catch (AbandonedMutexException)
            //    {
            //        // abandoned mutexes are still acquired, we just need
            //        // to handle the exception and treat it as acquisition
            //        mutexAcquired = true;
            //    }

            //    // if it wasn't acquired, it timed out, so can handle that how ever we want
            //    if (!mutexAcquired)
            //    {
            //        Console.WriteLine("I have timed out acquiring the mutex and can handle that somehow");
            //        return;
            //    }

            //    // otherwise, we've acquired the mutex and should do what we need to do,
            //    // then ensure that we always release the mutex
            //    try
            //    {
            try
            {
                var messageLog = $"{DateTime.Now:HH:mm:ss.fff} - {playerName} - {message}";
                Console.WriteLine($"{playerName} - {message}");
                await using (var sw = new StreamWriter(logpath, true))
                {
                    await sw.WriteLineAsync(messageLog);
                }

                if (HubContext != null)
                {
                    await HubContext.Clients.All.SendAsync(ServerPushMethods.LogReceived, "na", messageLog);

                }
            }
            catch 
            {

                Console.WriteLine("Error while logging");
            }
            //    }
            //    finally
            //    {
            //        mutex.ReleaseMutex();
            //    }

            //}


        }
    }
}
