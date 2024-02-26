using System.Diagnostics;

namespace DockerMicroservice1.API
{
    public static class ActivitySourceProvider
    {
        public static ActivitySource Source = new ActivitySource("DockerMicroservice1ActivitySource");
    }
}