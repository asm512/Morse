using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Morse;
using System.Runtime.CompilerServices;

namespace MorseExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var morse = new Morse.Morse("https://volga-technology.com/api_software/morse", "licenses.json", "version.json");
            Console.WriteLine($"Authentication          {morse.Authenticate()}");
            var updateResponse = morse.CheckForUpdates(typeof(Program).Assembly.GetName().Version);

            Console.WriteLine($"Update available?       {updateResponse.Item1}");
            Console.WriteLine($"Latest release date     {updateResponse.Item2.LatestVersionDate}");
            Console.WriteLine($"Changelog               {updateResponse.Item2.Changelog}");
            KeepOpen();
        }

        private static void KeepOpen()
        {
            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
