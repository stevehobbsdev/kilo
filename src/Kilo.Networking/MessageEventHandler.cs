using System;

namespace Kilo.Networking
{
    /// <summary>
    /// Event arguments for message event handlers
    /// </summary>
    public class SocketMessageEventArgs
    {
        public SocketMessageEventArgs(ISocketMessage message, ISocketWriter writer)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.Message = message;
            this.SocketWriter = writer;
        }

        public ISocketMessage Message { get; private set; }
        public ISocketWriter SocketWriter { get; private set; }
    }

    /// <summary>
    /// A handler for message operations
    /// </summary>
    public delegate void MessageEventHandler(object sender, SocketMessageEventArgs args);
}
