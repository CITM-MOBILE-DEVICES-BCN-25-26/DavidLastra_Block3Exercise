using System.Collections.Generic;
using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    /// <summary>
    /// Holds the set of items the shop currently sells and resolves an
    /// IShopItem from its ShopItemType.
    ///
    /// SOLID - Open/Closed: to add a new item you register one more IShopItem
    /// here (done in the Bootstrap). The use cases iterate over the catalog and
    /// never need to change.
    /// </summary>
    public sealed class ShopCatalog
    {
        private readonly Dictionary<ShopItemType, IShopItem> _items =
            new Dictionary<ShopItemType, IShopItem>();

        public ShopCatalog(IEnumerable<IShopItem> items)
        {
            foreach (var item in items)
                _items[item.Type] = item;
        }

        public IShopItem Get(ShopItemType type) => _items[type];

        public IEnumerable<IShopItem> All => _items.Values;
    }
}
