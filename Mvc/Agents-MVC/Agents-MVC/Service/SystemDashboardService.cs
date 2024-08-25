using Agents_MVC.Models;
using System.Text.Json;

namespace Agents_MVC.Service
{
    public class SystemDashboardService(IHttpClientFactory clientFactory):ISystemDashboardService
    {

        private readonly string baseUrl = "https://localhost:5102";

        public async Task<List<AgentModel>> GetAllAgents()
        {
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/Agents");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token); // check the tokken

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                List<AgentModel>? agents = JsonSerializer.Deserialize<List<AgentModel>>
                    (content, new JsonSerializerOptions()
                    { PropertyNameCaseInsensitive = true });

                return agents!;
            }
            return new List<AgentModel>();


        }public async Task<List<TargetModel>> GetAllTargets()
        {
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/Targets");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token); // check the tokken

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                List<TargetModel>? targets = JsonSerializer.Deserialize<List<TargetModel>>
                    (content, new JsonSerializerOptions()
                    { PropertyNameCaseInsensitive = true });

                return targets!;
            }
            return new List<TargetModel>();



        }public async Task<List<MissionModel>> GetAllMissions()
        {
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/Missions");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token); // check the tokken

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                List<MissionModel>? missions = JsonSerializer.Deserialize<List<MissionModel>>
                    (content, new JsonSerializerOptions()
                    { PropertyNameCaseInsensitive = true });

                return missions!;
            }
            return new List<MissionModel>();
        }

        public async Task<int> SumAllAgents()
        {
            var agents = await GetAllAgents();
            var sumAgents = agents.Count;
            return sumAgents;


        }public async Task<int> SumAllAgentsActive()
        {
            var agents = await GetAllAgents();
            var sumAgentsActive = agents.Count(a => a.Status == StatusAgent.Active);
            return sumAgentsActive;
        }

        public async Task<int> SumAllTargets()
        {
            var targets = await GetAllTargets();
            var sumTargets = targets.Count;
            return sumTargets;
        }
        public async Task<int> SumAllTargetsKilled()
        {
            var targets = await GetAllTargets();
            var sumTargetsKilled = targets.Count(t => t.Status == StatusTarget.Killed);
            return sumTargetsKilled;
        }
        public async Task<int> SumAllMissions()
        {
            var missions = await GetAllMissions();
            var sumMissions = missions.Count;
            return sumMissions;
        }
        public async Task<int> SumAllMissionsAssigned()
        {
            var missions = await GetAllMissions();
            var sumMissionsAssigned = missions.Count(m => m.Status == StatusMission.Assigned);
            return sumMissionsAssigned;
        }

        public async Task<double> CompareAgentToTargets()
        {
            var sumAgents = await SumAllAgents();
            var sumTargets = await SumAllTargets();
            return sumAgents/sumTargets;
        }

        public async Task<double> CompareAgentsDormatsToTargets()
        {
            var allMissions = await GetAllMissions();

            var sumAgentDormantsToAgents = allMissions.Where(m => m.Status == StatusMission.Offer)
                .Where(m => m.Agent.Status == StatusAgent.Dormant)
                .GroupBy(m => m.Agent).ToList().Count;

            var sumTargets = await SumAllTargets();
            return sumAgentDormantsToAgents / sumTargets;
        }

        public async Task<int> GetMissionActiveOfAgent(int AgentId)
        {
            var missions = await GetAllMissions();
            var missionAgent = missions.Where(m => m.Status == StatusMission.Assigned)
                .FirstOrDefault(m => m.AgentId == AgentId);

            if (missionAgent == null) return 0;
            return missionAgent.Id;
        }
        
        public async Task<double> GetTimeLeftToKill(int AgentId)
        {
            var missions = await GetAllMissions();
            var missionAgent = missions.Where(m => m.Status == StatusMission.Assigned)
                .FirstOrDefault(m => m.AgentId == AgentId);

            if (missionAgent == null) return 0;
            return missionAgent.TimeLeft;
        }

        public async Task<int> SumAgentKilled(int agentId)
        {
            var missions = await GetAllMissions();
            var SumAgentKilled = missions.Where(m => m.Status == StatusMission.Eliminated)
                .Where(m => m.AgentId == agentId).Count();
            return SumAgentKilled;
        }
    }
}
