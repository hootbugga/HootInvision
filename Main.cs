using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.Function;

namespace HootInvision
{
    public class HootInvision : BaseScript
    {
        private Config config;

        public HootInvision()
        {
            LoadConfig();

            Debug.WriteLine("[Hoot-Invision] Loaded successfully.");
        }

        private void LoadConfig()
        {
            string path = $"{API.GetResourcePath(API.GetCurrentResourceName())}/config.json";
            string json = System.IO.File.ReadAllText(path);
            config = JsonConvert.DeserializeObject<Config>(json);
        }

        [EventHandler("playerConnecting")]
        private async void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();

            await Delay(0); 

            deferrals.update(config.DeferMessage);

            string license = null;
            foreach (var identifier in player.Identifiers)
            {
                if (identifier.StartsWith("license:"))
                {
                    license = identifier.Substring(8);
                    break;
                }
            }

            if (string.IsNullOrEmpty(license))
            {
                deferrals.done(config.KickMessage);
                return;
            }

            bool allowed = await IsWhitelisted(license);

            if (allowed)
            {
                Debug.WriteLine($"[Hoot-Invision] Player {playerName} ({license}) is whitelisted.");
                deferrals.done();
            }
            else
            {
                Debug.WriteLine($"[Hoot-Invision] Player {playerName} ({license}) is NOT whitelisted.");
                deferrals.done(config.KickMessage);
            }
        }

        private async Task<bool> IsWhitelisted(string memberId)
        {
            var tcs = new TaskCompletionSource<bool>();

            string url = $"{config.ApiUrl.TrimEnd('/')}/core/members/{memberId}";

            string headersJson = $"{{\"IPS-API-Key\": \"{config.ApiKey}\"}}";

            Call((Hash)0x7AF6E68A7A7B6C3DUL, url, new Action<int, string, string>(
                (statusCode, responseData, resultHeaders) =>
                {
                    try
                    {
                        if (statusCode != 200 || string.IsNullOrEmpty(responseData))
                        {
                            tcs.SetResult(false);
                            return;
                        }

                        var member = JsonConvert.DeserializeObject<MemberResponse>(responseData);

                        if (Array.IndexOf(config.AllowedGroups, member.PrimaryGroup.Id) >= 0)
                        {
                            tcs.SetResult(true);
                            return;
                        }

                        if (member.SecondaryGroups != null)
                        {
                            foreach (var group in member.SecondaryGroups)
                            {
                                if (Array.IndexOf(config.AllowedGroups, group.Id) >= 0)
                                {
                                    tcs.SetResult(true);
                                    return;
                                }
                            }
                        }

                        tcs.SetResult(false);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[Hoot-Invision] API Parse Error: {ex.Message}");
                        tcs.SetResult(false);
                    }
                }), "GET", "", headersJson);

            return await tcs.Task;
        }
    }
}