using Agents_MVC.Models;
using System.Text.Json;

namespace Agents_MVC.Service
{
    public class MissionDetailsService(IHttpClientFactory clientFactory) : IMissionDetailsService
    {

        private readonly string baseUrl = "https://localhost:5102";

        public async Task<MissionModel> GetMissionById(int id)
        {
            var httpClient = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/Missions/{id}");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token); // check the tokken

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                MissionModel? mission = JsonSerializer.Deserialize<MissionModel>
                    (content, new JsonSerializerOptions()
                    { PropertyNameCaseInsensitive = true });

                return mission!;
            }
            return new MissionModel();
        }
    }
}
