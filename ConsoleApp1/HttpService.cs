using System.Diagnostics;

namespace ConsoleApp1
{
    public class HttpService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<int> MakeRequestToGoogle()
        {
            using var activity = ActivitySourceProvider.Source.StartActivity();


            var response = await HttpClient.GetAsync("https://www.google.com");


            string content = string.Empty;

            using (var activity2 = ActivitySourceProvider.Source.StartActivity("MakeRequestToGoogle > Content"))
            {
                content = await response.Content.ReadAsStringAsync();
                activity2.AddTag("content.length", content.Length);
            }


            activity.AddTag("user.id", 23);
            activity.AddTag("http.schema", "https");

            return content.Length;
        }
    }
}