using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Morse
{
    public class Morse
    {
        internal readonly Uri LicenseCheckURL;
        internal readonly Uri VersionCheckURL;

        internal static string CurrentDevice = Environment.MachineName;

        private static HttpClient HTclient = new HttpClient();

        private LicenseAPIJson LicenseData;
        private VersionAPIJson VersionData;

        public Morse(string webAdd, string licenseFile, string versionFile)
        {
            LicenseCheckURL = new Uri($"{webAdd}/{licenseFile}", UriKind.Absolute);
            VersionCheckURL = new Uri($"{webAdd}/{versionFile}", UriKind.Absolute);
        }

        public bool Authenticate()
        {
            DownloadParseJson();
            if (CheckLicense()) return true;
            return false;
        }

        public enum CurrentVersionTime { Earlier, Same, Later, Unknown };

        public Tuple<bool, VersionAPIJson> CheckForUpdates(Version _v)
        {
            Version currentVersion = _v;
            Version remoteVersion = new(VersionData.LatestVersion);

            return currentVersion.CompareTo(remoteVersion) switch
            {
                0 => Tuple.Create(false, VersionData),
                1 => Tuple.Create(false, VersionData),
                -1 => Tuple.Create(true, VersionData),
                _ => Tuple.Create(false, VersionData),
            };
        }

        public bool CheckLicense()
        {
            if (LicenseData.Licenses.Contains(CurrentDevice)) return true; else return false;
        }

        private void DownloadParseJson()
        {
            using HttpResponseMessage licenseResponse = HTclient.GetAsync(LicenseCheckURL).Result;
            using HttpContent licenseContent = licenseResponse.Content;
            LicenseData = ParseLicenseResponse(licenseContent.ReadAsStringAsync().Result);

            using HttpResponseMessage versionResponse = HTclient.GetAsync(VersionCheckURL).Result;
            using HttpContent versionContent = versionResponse.Content;
            VersionData = ParseVersionResponse(versionContent.ReadAsStringAsync().Result);
        }

        private LicenseAPIJson ParseLicenseResponse(string apiResult)
        {
            return JsonConvert.DeserializeObject<LicenseAPIJson>(apiResult);
        }

        private VersionAPIJson ParseVersionResponse(string apiResult)
        {
            return JsonConvert.DeserializeObject<VersionAPIJson>(apiResult);
        }
    }

    public class VersionAPIJson
    {
        public string LatestVersion { get; set; }
        public string LatestVersionDate { get; set; }
        public string Changelog { get; set; }
    }

    internal class LicenseAPIJson
    {
        public List<string> Licenses { get; set; }
    }
}
