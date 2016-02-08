using System.IO;

namespace Kilo.Networking
{
    public interface ISocketWriter
    {
        RequestHandle Send(ISocketMessage message);
        RequestHandle Send(int type, RequestHandle handle = null, bool faulted = false);
        RequestHandle Send(int type, string msg, RequestHandle handle = null, bool faulted = false);
        RequestHandle Send(int type, byte[] bytes, RequestHandle handle, bool faulted = false);
        RequestHandle Send(int type, int length, Stream stream, RequestHandle handle, bool faulted = false);
        RequestHandle Send<T>(int type, T obj, RequestHandle handle, bool faulted = false);
        RequestHandle SendFault(int faultId, string message, RequestHandle handle = null);
        RequestHandle SendFile(int type, string filename, RequestHandle handle = null);
    }
}