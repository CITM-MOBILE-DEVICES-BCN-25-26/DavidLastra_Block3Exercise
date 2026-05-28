using System.Collections.Generic;
using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    public sealed class ShopCatalog
    {
        private readonly Dictionary<ShopItemType, IShopItem> _items =
            new Dictionary<ShopItemType, IShopItem>();

        public ShopCatalog(IEnumerable<IShopItem> items)
        {
            foreach (var item in items)
            {
                _items[item.Type] = item;
            }
                
        }

        public IShopItem Get(ShopItemType type) => _items[type];

        public IEnumerable<IShopItem> All => _items.Values;
    }
}
