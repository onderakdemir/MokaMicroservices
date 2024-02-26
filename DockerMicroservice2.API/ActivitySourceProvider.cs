using System.Diagnostics;

namespace DockerMicroservice2.API
{
    public static class ActivitySourceProvider
    {
        public static ActivitySource Source = new ActivitySource("DockerMicroservice2ActivitySource");
    }
}