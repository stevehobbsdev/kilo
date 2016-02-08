using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Kilo.Networking
{
    public static class MessageExtensions
    {
        public static async Task SaveAsAsync(this ISocketMessage message, string filename)
        {
            var trace = new TraceSource("Kilo.Networking.Messaging");

            using (var input = message.GetStream())
            using (var output = new FileStream(filename, FileMode.Create))
            {
                var watch = new Stopwatch();
                watch.Start();

                trace.TraceEvent(TraceEventType.Information, 0, $"Saving message as { filename }");

                input.Position = 0;

                await input.CopyToAsync(output);

                watch.Stop();

                trace.TraceEvent(TraceEventType.Verbose, 0, $"Done in { watch.Elapsed }");
            }
        }
    }
}
