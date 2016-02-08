using System;
using System.IO;

namespace Kilo.Networking
{
    public interface ISocketMessage : IDisposable
    {
        int MessageLength { get; set; }
        int MessageTypeId { get; set; }
        RequestHandle Handle { get; set; }
        bool Faulted { get; }

        Stream GetStream();
        void OnFinishReadingMessage();
    }
}