using Newtonsoft.Json;

namespace Morse
{
    public class Morse
    {
        internal readonly string webAPI;

        internal readonly Uri VersionCheckURL;
        internal readonly Uri LicenseCheckURL;

        internal static string CurrentDevice = Environment.MachineName;

        public Morse(string webAPI)
        {
            VersionCheckURL = new Uri($"https://volga-technology.com/api_software/{webAPI}/version.json", UriKind.Absolute);
            LicenseCheckURL = new Uri($"https://volga-technology.com/api_software/{webAPI}/licenses.json", UriKind.Absolute);
        }
    }
}