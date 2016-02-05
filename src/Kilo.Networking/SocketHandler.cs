using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Kilo.Networking
{
    public class CancelToken
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="CancelToken"/> is cancelled.
        /// </summary>
        public bool Cancelled { get; private set; }

        /// <summary>
        /// Flags the cancel token as cancelled
        /// </summary>
        public void Cancel()
        {
            this.Cancelled = true;
        }
    }

    public class SocketHandler : IDisposable
    {
        private const int MessageBufferSize = 16 * 1024;
        private const int MaxMessageLength = 2 * 1024 * 1024;

        private readonly TcpClient client;
        private object sendlock = new object();
        private TraceSource trace = new TraceSource("Kilo.Networking.SocketHandler");

        /// <summary>
        /// Occurs when a message has been received
        /// </summary>
        public event MessageEventHandler MessageReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketHandler"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public SocketHandler(TcpClient client)
        {
            this.client = client;
            trace.TraceEvent(TraceEventType.Verbose, 0, "SocketHandler created");
        }

        /// <summary>
        /// Receives one or more messages.
        /// </summary>
        /// <param name="handler">The message handler</param>
        /// <param name="handle">The request handle.</param>
        /// <param name="cancelToken">The cancel token.</param>
        public void Receive(MessageEventHandler handler = null, RequestHandle handle = null, CancelToken cancelToken = null)
        {
            // Read the operation id
            var ns = this.client.GetStream();

            Action onClientDisconnect = () =>
            {
                trace.TraceEvent(TraceEventType.Information, 0, "Client disconnected");
            };

            try
            {
                while (true)
                {
                    var opBuffer = new byte[4];
                    var lengthBuffer = new byte[4];
                    var faultedBuffer = new byte[1];
                    var handleBuffer = new byte[32];

                    var bytesRead = ns.Read(opBuffer, 0, 4);
                    if (bytesRead == 0)
                    {
                        onClientDisconnect();
                        break;
                    }

                    trace.TraceEvent(TraceEventType.Verbose, 0, "Receiving data..");

                    bytesRead = ns.Read(lengthBuffer, 0, 4);
                    if (bytesRead == 0)
                    {
                        onClientDisconnect();
                        break;
                    }

                    bytesRead = ns.Read(faultedBuffer, 0, 1);
                    if (bytesRead == 0)
                    {
                        onClientDisconnect();
                        break;
                    }

                    bytesRead = ns.Read(handleBuffer, 0, 32);
                    if (bytesRead == 0)
                    {
                        onClientDisconnect();
                        break;
                    }

                    var messageTypeId = BitConverter.ToInt32(opBuffer, 0);
                    var length = BitConverter.ToInt32(lengthBuffer, 0);
                    var faulted = BitConverter.ToBoolean(faultedBuffer, 0);

                    trace.TraceEvent(TraceEventType.Information, 0, $"Received message of { length } bytes, command: {messageTypeId}, faulted: { faulted }");

                    ISocketMessage message = null;

                    var incomingHandle = new RequestHandle(RequestHandle.Parse(handleBuffer));

                    trace.TraceEvent(TraceEventType.Verbose, 0, $"Request handle: { incomingHandle.Id }");

                    try
                    {
                        if (length > MaxMessageLength || messageTypeId == (int)MessageOperation.StreamData)
                        {
                            message = new FileMessage(messageTypeId, length, incomingHandle);
                            trace.TraceEvent(TraceEventType.Verbose, 0, $"Created file message");
                        }
                        else
                        {
                            message = new SocketMessage(messageTypeId, length, incomingHandle, faulted);
                            trace.TraceEvent(TraceEventType.Verbose, 0, $"Created socket message");
                        }

                        var bytesRemaining = message.MessageLength;
                        var messageBytesRead = 0;
                        var messageBuffer = new byte[Math.Min(MessageBufferSize, message.MessageLength)];
                        var outputStream = message.GetStream();

                        trace.TraceEvent(TraceEventType.Verbose, 0, $"Reading { bytesRemaining } bytes from the stream");

                        var readTimer = new Stopwatch();
                        readTimer.Start();

                        while (bytesRemaining > 0)
                        {
                            var read = ns.Read(messageBuffer, 0, messageBuffer.Length);

                            if (read == 0)
                            {
                                onClientDisconnect();
                                break;
                            }

                            outputStream.Write(messageBuffer, 0, read);

                            messageBytesRead += read;
                            bytesRemaining -= read;
                        }

                        readTimer.Stop();

                        trace.TraceEvent(TraceEventType.Verbose, 0, $"Message read in { readTimer.Elapsed }");

                        if (bytesRemaining > 0)
                            break;

                        outputStream.Flush();
                        outputStream.Position = 0;

                        if (message.AutoCloseStream)
                        {
                            trace.TraceEvent(TraceEventType.Verbose, 0, $"Closing stream");
                            message.GetStream().Close();
                        }

                        bool handled = false;
                        if (handler != null)
                        {
                            if (handle != null)
                            {
                                if (handle.Id == incomingHandle.Id)
                                {
                                    trace.TraceEvent(TraceEventType.Verbose, 0, "Invoking local message handler");
                                    handler.Invoke(this, new SocketMessageEventArgs(message));
                                    handled = true;
                                }
                                else
                                    trace.TraceEvent(TraceEventType.Verbose, 0, "Message handle ids do not match, skipping handler");
                            }
                            else
                            {
                                trace.TraceEvent(TraceEventType.Verbose, 0, "Invoking local message handler");
                                handler.Invoke(this, new SocketMessageEventArgs(message));
                                handled = true;
                            }
                        }

                        if (!handled)
                        {
                            trace.TraceEvent(TraceEventType.Verbose, 0, "Invoking general message handler");
                            this.MessageReceived?.Invoke(this, new SocketMessageEventArgs(message));
                        }

                        // Built-in echo mechanism
                        if (message.MessageTypeId == (int)MessageOperation.Echo)
                        {
                            trace.TraceEvent(TraceEventType.Information, 0, $"Echo message received, returning the data");
                            this.Send((int)MessageOperation.EchoReturn, message.MessageLength, message.GetStream(), incomingHandle);
                        }
                    }
                    finally
                    {
                        message.Dispose();
                    }

                    if (cancelToken == null || cancelToken.Cancelled)
                        break;
                }
            }
            catch (IOException ex)
            {
                trace.TraceEvent(TraceEventType.Error, 1, $"Exception encountered: { ex.Message }");                
            }
        }

        /// <summary>
        /// Perform a send operation without any body data
        /// </summary>
        /// <param name="type">The operation.</param>
        /// <param name="handle">The request handle</param>
        /// <param name="faulted">If true, a faulted request is sent</param>
        public RequestHandle Send(int type, RequestHandle handle = null, bool faulted = false)
        {
            return this.Send(type, new byte[] { }, handle, faulted);
        }

        /// <summary>
        /// Performs a send operation with a string message body
        /// </summary>
        /// <param name="type">The command.</param>
        /// <param name="msg">The message.</param>
        /// <param name="handle">The request handle.</param>
        /// <param name="faulted">Whether or not the message should be sent as faulted</param>
        public RequestHandle Send(int type, string msg, RequestHandle handle = null, bool faulted = false)
        {
            return this.Send(type, Encoding.UTF8.GetBytes(msg), handle, faulted);
        }

        /// <summary>
        /// Sends the specified command along with the bytes to send as the message body
        /// </summary>
        /// <param name="type">The command.</param>
        /// <param name="bytes">The byte data to send</param>
        /// <param name="handle">The handle.</param>
        /// <param name="faulted">Whether or not the message should be sent as faulted</param>
        public RequestHandle Send(int type, byte[] bytes, RequestHandle handle, bool faulted = false)
        {
            lock (sendlock)
            {
                var ns = this.client.GetStream();
                var typeBuffer = BitConverter.GetBytes(type);
                var lengthBuffer = BitConverter.GetBytes(bytes.Length);
                var faultedBuffer = BitConverter.GetBytes(faulted);

                handle = handle ?? new RequestHandle(type, bytes.Length, Guid.NewGuid());

                ns.Write(typeBuffer, 0, 4);
                ns.Write(lengthBuffer, 0, 4);
                ns.Write(faultedBuffer, 0, faultedBuffer.Length);
                ns.Write(handle.ToByteArray(), 0, 32);
                ns.Write(bytes, 0, bytes.Length);

                return handle;
            }
        }

        /// <summary>
        /// Sends a stream as the message body
        /// </summary>
        /// <param name="type">The command.</param>
        /// <param name="length">The length of the stream</param>
        /// <param name="stream">The stream data</param>
        /// <param name="handle">The request handle</param>
        /// <param name="faulted">Whether or not the message should be sent as faulted</param>
        public RequestHandle Send(int type, int length, Stream stream, RequestHandle handle, bool faulted = false)
        {
            lock (sendlock)
            {
                var ns = this.client.GetStream();

                var typeBuffer = BitConverter.GetBytes(type);
                var lengthBuffer = BitConverter.GetBytes(length);
                var faultedBuffer = BitConverter.GetBytes(faulted);

                handle = handle ?? new RequestHandle(type, length, Guid.NewGuid());

                ns.Write(typeBuffer, 0, typeBuffer.Length);
                ns.Write(lengthBuffer, 0, lengthBuffer.Length);
                ns.Write(faultedBuffer, 0, faultedBuffer.Length);
                ns.Write(handle.ToByteArray(), 0, 32);

                var sendBuffer = new byte[Math.Min(length, MessageBufferSize)];
                var bytesWritten = 0;
                var bytesRemaining = length;

                while (bytesRemaining > 0)
                {
                    var read = stream.Read(sendBuffer, 0, sendBuffer.Length);

                    ns.Write(sendBuffer, 0, read);
                    bytesRemaining -= read;
                    bytesWritten += read;
                }

                return handle;
            }
        }

        /// <summary>
        /// Sends a fault message
        /// </summary>
        /// <param name="faultId">The fault identifier.</param>
        /// <param name="message">The message, usually an error message</param>
        /// <param name="handle">The handle.</param>
        public RequestHandle SendFault(int faultId, string message, RequestHandle handle = null)
        {
            return this.Send(faultId, message, handle, true);
        }

        /// <summary>
        /// Sends a message instance
        /// </summary>
        /// <param name="message">The message.</param>
        public RequestHandle Send(ISocketMessage message)
        {
            return this.Send(message.MessageTypeId, message.MessageLength, message.GetStream(), message.Handle);
        }

        /// <summary>
        /// Sends a specific file
        /// </summary>
        /// <param name="type">The request type</param>
        /// <param name="filename">The name of the file to send</param>
        /// <param name="handle">The request handle.</param>
        public RequestHandle SendFile(int type, string filename, RequestHandle handle = null)
        {
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                var info = new FileInfo(filename);

                Trace.WriteLine($"Preparing to send {filename} ({info.Length} bytes)");

                return this.Send(type, (int)info.Length, fs, handle);
            }
        }

        public void Dispose()
        {
        }
    }
}
