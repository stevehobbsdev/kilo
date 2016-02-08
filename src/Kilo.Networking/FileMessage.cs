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

        protected override Stream CreateStream()
        {
            return new FileStream(this.Filename, FileMode.OpenOrCreate);
        }

        public override void OnFinishReadingMessage()
        {
            base.OnFinishReadingMessage();

            this.receiveStream.Close();
            this.receiveStream = null;
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
