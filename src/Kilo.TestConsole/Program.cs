using System;
using System.Linq;
using System.Threading;
using Kilo.Data;
using Kilo.Data.Azure;

namespace Kilo.TestConsole
{
    /*class LocalUser
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
    }*/

    class Program
    {
        static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            SaveUser();

            Console.ReadKey();
        }

        private static async void SaveUser()
        {
            var r = new TableStorageRepository<User>(new DevelopmentStorageContext(), "Users");

            var user = new User
            {
                PartitionKey = "user",
                RowKey = "steve",
                Name = "Steve Hobbs",
            };

            r.BatchCommitted += (s, a) =>
            {
                Console.WriteLine("Committed..");
            };

            r.Insert(user);
            await r.CommitAsync();

            var user2 = await r.SingleAsync(new TableStorageKey("steve", "user"));

            if (user2 != null)
            {
                Console.WriteLine(user2.Name);
            }

            var users = await r.QueryAsync();
            users.ToList().ForEach(u => Console.WriteLine(u.RowKey));
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
