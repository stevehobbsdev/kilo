
namespace Kilo.Data.Azure
{
    public class DevelopmentStorageContext : StorageContext
    {
        public DevelopmentStorageContext()
            : base("UseDevelopmentStorage=true;")
        {
        }
    }
}
