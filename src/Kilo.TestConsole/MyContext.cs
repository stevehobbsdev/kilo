using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Kilo.TestConsole
{
    public class MyContext : DbContext
    {
        public IDbSet<User> Users { get; set; }

        public IDbSet<EmailAddress> EmailAddresses { get; set; }
    }
}
