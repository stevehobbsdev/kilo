using System.Collections.Generic;

namespace Kilo.Testing.Builders
{
    public class ListBuilder<TItem> : ListBuilder<ListBuilder<TItem>, List<TItem>, TItem>
    {
        public ListBuilder()
        {
        }

        public ListBuilder(params TItem[] items)
            : base(items)
        {
        }

        public ListBuilder(IEnumerable<TItem> items)
            : base(items)
        {
        }
    }

    public class ListBuilder<TBuilder, TInstance, TItem> : ObjectBuilder<TBuilder, TInstance>
        where TBuilder : ListBuilder<TBuilder, TInstance, TItem>
        where TInstance : List<TItem>, new()
    {
        public ListBuilder()
        {
        }

        public ListBuilder(params TItem[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public ListBuilder(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public TBuilder Add(TItem item)
        {
            _this.Instance.Add(item);
            return _this;
        }
    }
}
