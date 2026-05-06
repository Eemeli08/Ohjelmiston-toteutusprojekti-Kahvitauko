using Kahvitauko_ohjelma.Model;
using System.Text.Json;

namespace Kahvitauko_ohjelma
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

    }
}