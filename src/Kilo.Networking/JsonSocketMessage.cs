using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Kilo.Networking
{
    public class JsonSocketMessage
    {
        public static ISocketMessage Create(int operation, object objectToSend, RequestHandle handle)
        {
            var json = JsonConvert.SerializeObject(objectToSend);
            var bytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();

            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write("obj/json");
                writer.Write(json);
            }

            stream.Position = 0;

            var msg = new SocketMessage(operation, (int)stream.Length, stream, handle);

            return msg;
        }

        public static bool TryParse<T>(ISocketMessage message, out T obj)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var stream = message.GetStream();
            var binaryReader = new BinaryReader(stream, Encoding.UTF8, true);

            obj = default(T);

            var marker = binaryReader.ReadString();

            if (marker != "obj/json")
                return false;

            var json = binaryReader.ReadString();

            obj = JsonConvert.DeserializeObject<T>(json);

            return true;
        }
    }
}
