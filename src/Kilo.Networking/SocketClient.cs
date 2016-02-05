using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Kilo.Networking
{
    public class SocketClient : IDisposable
    {
        private readonly IPEndPoint endpoint;
        private TcpClient client;
        private SocketHandler handler;
        private CancelToken cancelToken;
        private bool isListening;
        private object handlerLock = new object();
        private TraceSource trace = new TraceSource("Kilo.Networking.SocketClient");

        public event EventHandler Connected;
        public event MessageEventHandler MessageReceived;

        public SocketClient(IPEndPoint endpoint)
        {
            this.endpoint = endpoint;

            trace.TraceEvent(TraceEventType.Verbose, 0, $"Created client for { endpoint }");
        }

        protected SocketHandler SocketHandler => this.handler;

        public bool IsListening => this.isListening;

        /// <summary>
        /// Connects to the remote server
        /// </summary>
        public void Connect()
        {
            var client = new TcpClient();

            trace.TraceEvent(TraceEventType.Information, 0, $"Connecting to { this.endpoint }");
            client.BeginConnect(this.endpoint.Address, this.endpoint.Port, this.OnConnected, client);
        }

        /// <summary>
        /// Sends an echo message and waits for a response
        /// </summary>
        public Task<string> SendEcho(string message)
        {
            var tcs = new TaskCompletionSource<string>();

            trace.TraceEvent(TraceEventType.Information, 0, $"Sending echo message: { message }");
            var handle = this.handler.Send((int)MessageOperation.Echo, message, null);

            this.handler.Receive((s, a) =>
            {
                trace.TraceEvent(TraceEventType.Verbose, 0, $"Received echo return");
                tcs.SetResult(a.Message.ToString());
            }, handle);

            return tcs.Task;
        }

        /// <summary>
        /// Sends a file to the server
        /// </summary>
        public void SendFile(int operation, string filename)
        {
            this.handler.SendFile(operation, filename, null);
        }

        /// <summary>
        /// Sends the specified length.
        /// </summary>
        public void Send(int length, Stream stream, RequestHandle handle)
        {
            trace.TraceEvent(TraceEventType.Information, 0, $"Sending content in {stream.GetType()} of length {length}");

            this.handler.Send((int)MessageOperation.StreamData, length, stream, handle);
        }

        public RequestHandle SendObject(int operation, object obj, RequestHandle handle = null)
        {
            trace.TraceEvent(TraceEventType.Information, 0, $"Sending json object of type { obj.GetType() }");

            var msg = JsonSocketMessage.Create(operation, obj, handle);

            return this.handler.Send(msg);
        }

        /// <summary>
        /// Performs a request which waits for a response before completing the task.
        /// </summary>
        /// <param name="operation">The operation type</param>
        /// <param name="obj">The object to send as part of the request (optional)</param>
        public Task<TResult> MakeRequest<TResult>(int operation, object obj = null)
        {
            var completionSource = new TaskCompletionSource<TResult>();

            var msg = obj == null
                ? new SocketMessage(operation, 0, null)
                : JsonSocketMessage.Create(operation, obj, null);

            trace.TraceEvent(TraceEventType.Verbose, 0, $"Sending a send/return request (command: { operation })");

            if (obj != null)
                trace.TraceEvent(TraceEventType.Verbose, 0, $"with type {obj.GetType() }");

            var request = this.handler.Send(msg);

            trace.TraceEvent(TraceEventType.Verbose, 0, $"with id { request.Id }");

            this.handler.Receive((s, a) =>
            {
                TResult resultingObject;

                trace.TraceEvent(TraceEventType.Verbose, 0, $"Received reply for id { request.Id }");
                trace.TraceEvent(TraceEventType.Verbose, 0, $"JSON: { a.Message.ToString() }");

                if (JsonSocketMessage.TryParse(a.Message, out resultingObject))
                {
                    completionSource.SetResult(resultingObject);
                }
                else
                {
                    trace.TraceEvent(TraceEventType.Warning, 0, $"Could not parse JSON response for type { typeof(TResult) }");
                }

            }, request);

            return completionSource.Task;
        }

        /// <summary>
        /// Sets up a listener which continually listens for incoming messages
        /// </summary>
        public void ListenForMessages()
        {
            this.cancelToken = new CancelToken();

            ThreadPool.QueueUserWorkItem(sender =>
            {
                trace.TraceEvent(TraceEventType.Information, 0, $"Client is listening for messages..");

                this.isListening = true;

                lock (this.handlerLock)
                {
                    this.handler.Receive(cancelToken: this.cancelToken);
                }
            });
        }

        /// <summary>
        /// Stops the listener
        /// </summary>
        public void StopListening()
        {
            this.cancelToken.Cancel();
            this.isListening = false;

            trace.TraceEvent(TraceEventType.Information, 0, $"Client has stopped listening for messages");
        }

        /// <summary>
        /// Raised when the client has connected to the server
        /// </summary>
        /// <param name="result">The result.</param>
        private void OnConnected(IAsyncResult result)
        {
            var client = (TcpClient)result.AsyncState;
            client.EndConnect(result);

            trace.TraceEvent(TraceEventType.Information, 0, "Connected");

            this.client = client;
            this.handler = new SocketHandler(client);

            this.handler.MessageReceived += (s, a) => this.MessageReceived?.Invoke(this, a);

            this.Connected?.Invoke(this, new EventArgs());
        }

        public void Dispose()
        {
            if (this.client != null)
            {
                this.client.Close();
                this.client.Dispose();
                this.client = null;
            }

            if (this.handler != null)
            {
                this.handler.Dispose();
                this.handler = null;
            }
        }
    }
}
