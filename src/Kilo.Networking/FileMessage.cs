using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Kilo.Networking
{
    public class FileMessage : SocketMessage
    {
        private TraceSource trace = new TraceSource("Kilo.Networking.Messaging");

        public FileMessage(int operation, int length, RequestHandle handle)
            : base(operation, length, handle)
        {
            this.Filename = Path.GetTempFileName();
        }

        public FileMessage(int operation, int length, string filename, RequestHandle handle)
            : base(operation, length, handle)
        {
            this.Filename = filename;
        }

        public string Filename { get; private set; }
        public override bool AutoCloseStream => true;

        protected override Stream CreateStream()
        {
            return new FileStream(this.Filename, FileMode.OpenOrCreate);
        }

        public async Task SaveAsAsync(string filename)
        {
            using (var fs = this.CreateStream())
            using (var output = new FileStream(filename, FileMode.Create))
            {
                var watch = new Stopwatch();
                watch.Start();

                trace.TraceEvent(TraceEventType.Information, 0, $"Copying { this.Filename } to { filename }");

                await fs.CopyToAsync(output);

                watch.Stop();

                trace.TraceEvent(TraceEventType.Verbose, 0, $"Done in { watch.Elapsed }");
            }
        }

        public override string ToString()
        {
            return $"[FileMessage, { this.Filename }]";
        }

        public override void Dispose()
        {
            base.Dispose();

            try
            {
                if (File.Exists(this.Filename))
                {
                    trace.TraceEvent(TraceEventType.Verbose, 0, $"Attempting to close { this.Filename }");
                    File.Delete(this.Filename);
                }
            }
            catch (System.Exception ex)
            {
                trace.TraceEvent(TraceEventType.Warning, 0, $"Could not delete { this.Filename }: { ex.Message }");
            }
        }
    }
}
