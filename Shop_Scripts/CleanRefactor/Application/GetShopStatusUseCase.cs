using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
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
