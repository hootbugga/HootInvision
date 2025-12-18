using Newtonsoft.Json;

namespace HootInvision
{
    public class Config
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public int[] AllowedGroups { get; set; }
        public string DeferMessage { get; set; }
        public string KickMessage { get; set; }
    }

    public class MemberResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonProperty("primaryGroup")]
        public Group PrimaryGroup { get; set; }

        [JsonProperty("secondaryGroups")]
        public Group[] SecondaryGroups { get; set; }
    }

    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}