using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Kilo.Networking;
using Kilo.UnitTests.Fakes;
using Kilo.UnitTests.Properties;
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

        private static string sendFilePath = @"c:\output.jpg";
        private static ManualResetEvent sendFileEvent = new ManualResetEvent(false);

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

            server.MessageReceived += async (s, a) =>
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

                if (a.Message.MessageTypeId == 12)
                {                    
                    await a.Message.SaveAsAsync(sendFilePath);
                    sendFileEvent.Set();
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


        [TestMethod]
        public void Can_send_file()
        {
            var asm = Assembly.GetExecutingAssembly();

            using (var fileStream = new FileStream(@"Resources\IMG_6040.jpg", FileMode.Open))
            {
                // Act                
                client.Send(12, fileStream, (int)fileStream.Length);
                sendFileEvent.WaitOne();

                // Assert
                File.Exists(sendFilePath).Should().BeTrue();
                sendFileEvent.Reset();

                TryDeleteTestFile();
            }
        }

        private static void TryDeleteTestFile()
        {
            try
            {
                if (File.Exists(sendFilePath))
                    File.Delete(sendFilePath);
            }
            catch
            {
            }
        }
    }
}
