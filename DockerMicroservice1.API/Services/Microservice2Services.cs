namespace DockerMicroservice1.API.Services
{
    public class Microservice2Services(HttpClient client)
    {
        public async Task<string> GetMicroservice2Value()
        {
            var response = await client.GetAsync("/api/Values");
            return await response.Content.ReadAsStringAsync();
        }
    }
}