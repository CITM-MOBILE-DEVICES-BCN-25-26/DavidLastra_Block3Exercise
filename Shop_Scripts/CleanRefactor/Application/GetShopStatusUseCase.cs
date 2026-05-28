using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    /// <summary>
    /// Application use case: produces the current ShopStatusDto so the UI can
    /// show coins and enable/disable buttons WITHOUT computing any rule itself.
    ///
    /// "Can I buy item X?" is answered by reusing each item's own CanPurchase
    /// rule, so the validation logic exists in exactly one place.
    /// </summary>
    public sealed class GetShopStatusUseCase
    {
        private readonly ShopCatalog _catalog;
        private readonly IPlayerRepository _repository;

        public GetShopStatusUseCase(ShopCatalog catalog, IPlayerRepository repository)
        {
            _catalog = catalog;
            _repository = repository;
        }

        public ShopStatusDto Execute()
        {
            PlayerState player = _repository.Load();

            PurchaseStatus statusOf(ShopItemType type) =>
                _catalog.Get(type).CanPurchase(player);

            return new ShopStatusDto(
                coins:             player.Coins,
                bombStatus:        statusOf(ShopItemType.Bomb),
                shieldStatus:      statusOf(ShopItemType.Shield),
                doubleCoinsStatus: statusOf(ShopItemType.DoubleCoins));
        }
    }
}
