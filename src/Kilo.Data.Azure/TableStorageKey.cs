
namespace Kilo.Data.Azure
{
    public struct TableStorageKey
    {
        public TableStorageKey(string rowKey, string partitionKey) : this()
        {
            this.RowKey = rowKey;
            this.PartitionKey = partitionKey;
        }

        public string RowKey { get; set; }

        public string PartitionKey { get; set; }
    }
}
