using System;

namespace Kilo.UnitTests.Fakes
{
    public class FakeNetworkRequest
    {       
        public Guid Id { get; set; }

        public DateTime MessageDate { get; private set; } = DateTime.Now;

        public string Message { get; set; }
    }

    public class FakeNetworkResponse
    {
        public Guid Id { get; set; }

        public DateTime MessageDate { get; set; }

        public string Response { get; set; }
    }
}
