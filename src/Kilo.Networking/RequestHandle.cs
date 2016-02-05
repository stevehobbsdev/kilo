using System;
using System.Text;

namespace Kilo.Networking
{
    public class RequestHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandle"/> class.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="length">The length.</param>
        /// <param name="id">The identifier.</param>
        public RequestHandle(int operation, int length, Guid id)
        {
            var guid = Guid.NewGuid().ToString("N");
            var str = $"{operation}-{length}-{guid}";

            this.Id = Kilo.Crypto.Hash.CalculateMD5(str);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandle"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public RequestHandle(string id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// To the byte array.
        /// </summary>
        /// <returns>The Id string as a byte array</returns>
        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(this.Id);
        }

        /// <summary>
        /// Parses the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public static string Parse(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException();
            }

            if (buffer.Length != 32)
                throw new ApplicationException($"The id buffer is only { buffer.Length } bytes, expected 32");

            return Encoding.UTF8.GetString(buffer);
        }
    }
}
