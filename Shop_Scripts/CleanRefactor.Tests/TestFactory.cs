using System.Collections.Generic;
using CleanRefactor.Application;
using CleanRefactor.Domain;

namespace CleanRefactor.Tests
{
    /// <summary>
    /// Small helper that builds a fully-wired PurchaseItemUseCase /
    /// GetShopStatusUseCase around an InMemoryPlayerRepository.
    ///
    /// Keeping the wiring in one place keeps every test short and focused on a
    /// single behaviour.
    /// </summary>
    public static class TestFactory
    {
        public static ShopCatalog BuildCatalog()
        {
            ShopConfig config = ShopConfig.Default();
            return new ShopCatalog(new List<IShopItem>
            {
                new BombItem(config.Bomb),
                new ShieldItem(config.Shield),
                new DoubleCoinsItem(config.DoubleCoins)
            });
        }

        public static PurchaseItemUseCase BuildPurchaseUseCase(InMemoryPlayerRepository repo)
        {
            return new PurchaseItemUseCase(BuildCatalog(), repo);
        }

        public static GetShopStatusUseCase BuildStatusUseCase(InMemoryPlayerRepository repo)
        {
            return new GetShopStatusUseCase(BuildCatalog(), repo);
        }
    }
}
