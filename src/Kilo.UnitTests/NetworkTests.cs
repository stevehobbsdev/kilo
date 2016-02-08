using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kilo.Networking;
using Kilo.UnitTests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kilo.UnitTests
{
    [TestClass]
    public class NetworkTests
    {
        private static int port = 4200;
        private static SocketListener server;
        private static ManualResetEvent connectedEvent = new ManualResetEvent(false);
        private static SocketClient client;

        [ClassInitialize]
        public static void ClassInitialise(TestContext context)
        {
            server = new SocketListener();
            server.Listen(port);

            client = new SocketClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));

            client.Connected += (s, a) =>
            {
                connectedEvent.Set();
            };

            server.MessageReceived += (s, a) =>
            {
                if (a.Message.MessageTypeId == 10)
                {
                    FakeNetworkRequest message;
                    if (JsonSocketMessage.TryParse(a.Message, out message))
                    {
                        a.SocketWriter.Send(11, new FakeNetworkResponse
                        {
                            Id = message.Id,
                            MessageDate = message.MessageDate,
                            Response = $"Hello, {message.Message}"
                        }, a.Message.Handle);                        
                    }
                }
            };

            client.Connect();

            connectedEvent.WaitOne();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            server.Stop();
        }

        [TestMethod]
        public async Task Can_send_echo()
        {
            // Arrange
            string message = "Hello, world";

            // Act
            var result = await client.SendEcho(message);

            // Assert
            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public async Task Can_send_object()
        {
            // Arrange
            var obj = new FakeNetworkRequest
            {
                Id = Guid.NewGuid(),
                Message = "This is a message"
            };

            // Act
            var response = await client.MakeRequest<FakeNetworkResponse>(10, obj);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(obj.Id);
            response.Response.Should().Be("Hello, This is a message");
        }
    }
}
