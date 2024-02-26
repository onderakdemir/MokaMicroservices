using System.Diagnostics;

namespace ConsoleApp1
{
    public static class ActivitySourceProvider
    {
        public static ActivitySource Source = new ActivitySource("EmailSenderActivitySource");
    }
}