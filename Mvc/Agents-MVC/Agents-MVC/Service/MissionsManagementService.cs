using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Agents_MVC.Models;

namespace Agents_MVC.Service
{
    public class MissionsManagementService(IHttpClientFactory clientFactory) :IMissionsManagementService
    {
        private readonly string baseUrl = "https://localhost:5102";

        public async Task<Dictionary<AgentModel, List<MissionModel>>> GetAllOffers()
        {
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/getOffers");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token); // check the tokken

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                Dictionary<AgentModel, List<MissionModel>>? Offers = JsonSerializer.Deserialize<Dictionary<
                    AgentModel, List<MissionModel>>>(content, new JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true });

                return Offers!;
            }
            return new Dictionary<AgentModel, List<MissionModel>>();
        }

    }
}
