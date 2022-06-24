using Newtonsoft.Json;
using SWICD.Model.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Services
{
    internal class GitHubApi
    {
        public static async Task<Release> GetLatestRelease()
        {
            string url = "https://api.github.com/repos/mkenfenheuer/steam-deck-windows-usermode-driver/releases";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0");
            var res = await client.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                string response = await res.Content.ReadAsStringAsync();
                Release[] releases = JsonConvert.DeserializeObject<Release[]>(response);
                var latest = releases.OrderByDescending(r => r.CreatedAt).FirstOrDefault();

                return latest;
            }
            else
            {
                string response = await res.Content.ReadAsStringAsync();
                return null;
            }
        }
    }
}
