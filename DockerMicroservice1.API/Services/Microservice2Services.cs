using System.Diagnostics;

namespace DockerMicroservice1.API.Services
{
    public class Microservice2Services(HttpClient client)
    {
        public async Task<string> GetMicroservice2Value()
        {
            Activity.Current?.SetBaggage("order.id", "200");

            var response = await client.PostAsJsonAsync("/api/Values", new { Id = 10, Name = "Kalem 1" });


            using (var activity = ActivitySourceProvider.Source.StartActivity())
            {
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }
    }
}