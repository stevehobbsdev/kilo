using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Kilo.Networking
{
    /// <summary>
    /// Represents a message being sent/received across the wire
    /// </summary>
    public class SocketMessage : ISocketMessage
    {
        private Stream receiveStream;
        private TraceSource trace = new TraceSource("Kilo.Networking.Messaging");

        /// <summary>
        /// Gets or sets the message type identifier.
        /// </summary>
        public int MessageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the length of the message.
        /// </summary>
        public int MessageLength { get; set; }

        /// <summary>
        /// Gets or sets the request handle.
        /// </summary>
        public RequestHandle Handle { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SocketMessage"/> is faulted.
        /// </summary>
        public bool Faulted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketMessage"/> class.
        /// </summary>
        public SocketMessage(int type, int length, RequestHandle handle, bool faulted = false)
        {
            this.MessageTypeId = type;
            this.MessageLength = length;
            this.Handle = handle;
            this.Faulted = faulted;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketMessage"/> class.
        /// </summary>
        public SocketMessage(int operation, int length, byte[] data, RequestHandle handle, bool faulted = false)
            : this(operation, length, handle, faulted)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.receiveStream = this.CreateStream();            
            this.receiveStream.Write(data, 0, data.Length);

            this.receiveStream.Position = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketMessage"/> class.
        /// </summary>
        public SocketMessage(int operation, int length, Stream stream, RequestHandle handle, bool faulted = false)
            : this(operation, length, handle, faulted)
        {
            this.receiveStream = stream;            
        }

        /// <summary>
        /// Gets a value indicating whether the data stream should be automatically closed once the message is received
        /// </summary>
        public virtual bool AutoCloseStream => false;

        /// <summary>
        /// Gets the message data stream.
        /// </summary>
        public virtual Stream GetStream()
        {
            if (this.receiveStream == null)
                this.receiveStream = this.CreateStream();

            return this.receiveStream;
        }

        /// <summary>
        /// Creates the stream.
        /// </summary>
        protected virtual Stream CreateStream()
        {
            return new MemoryStream(this.MessageLength);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            var stream = this.GetStream();
            var reader = new StreamReader(stream);
            var str = reader.ReadToEnd();

            stream.Position = 0;

            return str;
        }

        public virtual void Dispose()
        {
            if (this.receiveStream != null)
            {
                this.receiveStream.Dispose();
                this.receiveStream = null;
            }
        }
    }
}
