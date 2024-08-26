using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Agents_MVC.Models;
using Agents_MVC.ViewModel;

namespace Agents_MVC.Service
{
    public class MissionsManagementService(IHttpClientFactory clientFactory) :IMissionsManagementService
    {
        private readonly string baseUrl = "http://localhost:5102";

        public async Task<List<MissionModel>> GetAllOffers()
        {
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/missions");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token); // check the tokken

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                List<MissionModel>? Offers = JsonSerializer.Deserialize<
                   List<MissionModel>>(content, new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true });

                return Offers!;
            }
            return new List<MissionModel>();
        }

        public async Task<List<MissionManagementVM>> CreateAllMissionsVm()
        {
            var allMissions = await GetAllOffers();
            List<MissionManagementVM> mmvm = [];
            foreach (var mission in allMissions)
            {
                MissionManagementVM managementVM = new()
                {
                    Id = mission.Id,
                    AgentNickName = mission.Agent.NickName,
                    AgentLocationX = mission.Agent.LocationX,
                    AgentLocationY = mission.Agent.LocationY,
                    TargetName = mission.Target.Name,
                    TargetPosition = mission.Target.Position,
                    TargetLocationX = mission.Target.LocationX,
                    TargetLocationY = mission.Target.LocationY,
                    Distance = mission.TimeLeft * 5,
                    TimeLeft = mission.TimeLeft
                };
                mmvm.Add(managementVM);
            }
            return mmvm;
        }

        public async Task InstructMission(int id)
        {
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/missions/{id}");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token); // check the tokken

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
            }
            return;
        }
    }
}
