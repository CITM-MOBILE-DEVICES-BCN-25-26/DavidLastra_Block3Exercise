using CleanRefactor.Domain;

namespace CleanRefactor.Application
{
    public sealed class PurchaseItemUseCase
    {
        private readonly ShopCatalog _catalog;
        private readonly IPlayerRepository _repository;

        public PurchaseItemUseCase(ShopCatalog catalog, IPlayerRepository repository)
        {
            _catalog = catalog;
            _repository = repository;
        }

        public PurchaseResult Execute(ShopItemType itemType)
        {
            PlayerState player = _repository.Load();
            IShopItem item = _catalog.Get(itemType);

            PurchaseStatus status = item.CanPurchase(player);
            if (status != PurchaseStatus.Purchased)
            {
                return new PurchaseResult(itemType, status, player.Coins);
            }

            player.SpendCoins(item.Cost);
            item.ApplyPurchase(player);

            _repository.Save(player);

            return new PurchaseResult(itemType, PurchaseStatus.Purchased, player.Coins);
        }
    }
}
