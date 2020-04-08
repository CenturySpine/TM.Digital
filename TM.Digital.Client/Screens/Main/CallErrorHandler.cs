using System;
using System.Windows;

namespace TM.Digital.Client.Screens.Main
{
    internal static class CallErrorHandler
    {
        internal static void Handle(Action a)
        {
            try
            {
                a();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                Console.WriteLine(e);
            }
        }
    }
}