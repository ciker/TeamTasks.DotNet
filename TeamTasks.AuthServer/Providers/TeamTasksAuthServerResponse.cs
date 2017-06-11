using CoreLibrary.AuthServer;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TeamTasks.AuthServer.Providers
{
    public class TeamTasksAuthServerResponse : AuthServerResponse
    {
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }
}
