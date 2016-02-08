using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Kilo.Networking
{
    public static class MessageExtensions
    {
        public static void SaveAs(this ISocketMessage message, string filename)
        {
            var trace = new TraceSource("Kilo.Networking.Messaging");

            using (var input = message.GetStream())
            using (var output = new FileStream(filename, FileMode.Create))
            {
                var watch = new Stopwatch();
                watch.Start();

                trace.TraceEvent(TraceEventType.Information, 0, $"Saving message as { filename }");

                input.Position = 0;

                byte [] buffer = new byte[64 * 1024];
                int read = 0;

                do
                {
                    read = input.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, read);
                } while (read > 0);
                
                watch.Stop();

                trace.TraceEvent(TraceEventType.Verbose, 0, $"Done in { watch.Elapsed }");
            }
        }
    }
}
