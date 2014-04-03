using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kilo.Configuration.Providers;
using Kilo.Configuration;
using Kilo.Data;
using Kilo.Data.EntityFramework;
using Kilo.Data.Azure;

namespace Kilo.TestConsole
{
    class LocalUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    class UserMapper : IEntityMapper<User, LocalUser>
    {
        public User MapToEntity(LocalUser target)
        {
            throw new NotImplementedException();
        }

        public LocalUser MapFromEntity(User entity)
        {
            return new LocalUser
            {
                Id = Guid.Parse(entity.RowKey),
                Name = entity.Name,
                Email = "elkdanger@googlemail.com"
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IRepository<User, LocalUser, TableStorageKey> ats 
                = new DomainMappedRepository<User, LocalUser>(new DevelopmentStorageContext(), "Users", new UserMapper());

            var results = ats.Query().ToList();

            Console.ReadKey();
        }

        private static void PrintUser(User u)
        {
            Console.WriteLine("{0}: {1}", u.RowKey, u.Name);

            if (u.EmailAddress != null)
            {
                Console.WriteLine("Email: {0} ({1})", u.EmailAddress.Value, u.EmailAddress.Id);
            }
        }
    }
}
