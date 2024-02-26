using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace ConsoleApp1
{
    public class HttpService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<int> MakeRequestToGoogle()
        {
            using var activity =
                ActivitySourceProvider.Source.StartActivity(ActivityKind.Producer);

            try
            {
                var eventTags = new ActivityTagsCollection { { "tag1.key", "tag1.value" } };

                activity.AddEvent(new ActivityEvent("google'a istek başlamadan önce", tags: eventTags));


                var response = await HttpClient.GetAsync("https://www.google.com");
                activity.AddEvent(new ActivityEvent("google'a istek tamamlandıktan sonra"));

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
            catch (Exception e)
            {
                activity.SetStatus(ActivityStatusCode.Error);
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<int> MakeRequestToAmazon()
        {
            using var activity = ActivitySourceProvider.Source2.StartActivity();


            var response = await HttpClient.GetAsync("https://www.amazon.com");

            var content = await response.Content.ReadAsStringAsync();

            return content.Length;
        }
    }
}