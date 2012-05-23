using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kilo.Configuration.Providers;
using Kilo.Configuration;
using Kilo.Data;

namespace Kilo.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextRepository<User, MyContext> repo = new DbContextRepository<User, MyContext>();

            repo.All().ToList().ForEach(u =>
                {
                    PrintUser(u);
                });

            // Include expressions
			repo.AllWithIncludes(
				u => u.EmailAddress);
                    //.ForEach(u => PrintUser(u));

            Console.ReadKey();
        }

        private static void PrintUser(User u)
        {
            Console.WriteLine("{0}: {1}", u.Id, u.Name);

            if (u.EmailAddress != null)
                Console.WriteLine("Email: {0} ({1})", u.EmailAddress.Value, u.EmailAddress.Id);
        }
    }
}
