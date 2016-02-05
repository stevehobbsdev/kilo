# Kilo.Networking

A library for simple client/server networking that allows efficient sending/retrieval of generic byte data, including files.

### Usage

Create a server and listen for messages:

```
var listener = new SocketListener();

listener.MessageReceived += (s, a) =>
{
    Console.WriteLine($"Received a message: { a.Message }");
};            

listener.Listen(4200);
```

Client a client to send messages:

```
using (var client = new SocketClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4200)))
{
    // Connect to the server
    client.Connect();

    // Event for handling incoming messages
    client.MessageReceived += (sender, a) =>
    {
        string message = "<unknown>";

        if (!(a.Message is FileMessage))
        {
            var reader = new StreamReader(a.Message.GetStream());
            message = reader.ReadToEnd();
        }

        Console.WriteLine($"Client received {a.Message.MessageTypeId} ({a.Message.MessageLength} bytes): {message}");                  
    };
}
```

Send an object:

```
var obj = new MyObject();

client.Send(Constants.MyMessageTypeId, obj);

```

Send an object and get a response:

```
var obj = new MyObject();

var response = await client.MakeRequest(Constants.MyMessageTypeId, obj);

// response contains the result

```

Send a file:

```
client.Send(Constants.MyFileUploadId, "a_big_file.zip");
```

Send a stream:

```
using(var mystream = new MemoryStream())
{
    client.Send(mystream.Length, mystream);
}
```


### Tracing

Several trace sources are defined that can be used for debugging:

* `Kilo.Networking.SocketClient` client socket operations
* `Kilo.Networking.SocketListener` server socket operations
* `Kilo.Networking.SocketHandler` low-level socket processing and dispatching
* `Kilo.Networking.Messaging` message-specific logging