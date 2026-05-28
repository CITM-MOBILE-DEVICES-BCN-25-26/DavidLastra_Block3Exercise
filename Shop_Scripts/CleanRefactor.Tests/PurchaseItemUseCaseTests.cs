using CleanRefactor.Application;
using CleanRefactor.Domain;
using NUnit.Framework;

namespace CleanRefactor.Tests
{
    [TestFixture]
    public class PurchaseItemUseCaseTests
    {
        private static PlayerState Player(
            int coins = 500, int level = 1,
            int bombUses = 0, int shieldUses = 0, bool hasDouble = false)
        {
            return new PlayerState(coins, level, bombUses, shieldUses, hasDouble);
        }

        [Test]
        public void When_BombBoughtWithEnoughCoins_Expect_PurchaseSucceeds()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 500));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.Bomb);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(PurchaseStatus.Purchased, result.Status);
            Assert.AreEqual(400, result.CoinsAfterPurchase);
            Assert.AreEqual(1, repo.Load().BombUses);
        }

        [Test]
        public void When_BombBoughtWithoutEnoughCoins_Expect_NotEnoughCoins()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 50));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.Bomb);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(PurchaseStatus.NotEnoughCoins, result.Status);
        }

        [Test]
        public void When_BombBoughtAtMaxUses_Expect_MaxUsesReached()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 500, bombUses: 3));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.Bomb);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(PurchaseStatus.MaxUsesReached, result.Status);
        }

        [Test]
        public void When_ShieldBoughtWithEnoughCoins_Expect_PurchaseSucceeds()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 500));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.Shield);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(350, result.CoinsAfterPurchase);
            Assert.AreEqual(1, repo.Load().ShieldUses);
        }

        [Test]
        public void When_ShieldBoughtAtMaxUses_Expect_MaxUsesReached()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 500, shieldUses: 2));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.Shield);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(PurchaseStatus.MaxUsesReached, result.Status);
        }

        [Test]
        public void When_DoubleCoinsBoughtWithEnoughCoinsAndLevel5_Expect_PurchaseSucceeds()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 500, level: 5));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.DoubleCoins);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(200, result.CoinsAfterPurchase);
            Assert.IsTrue(repo.Load().HasDoubleCoins);
        }

        [Test]
        public void When_DoubleCoinsBoughtBelowLevel5_Expect_RequiredLevelNotReached()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 500, level: 4));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.DoubleCoins);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(PurchaseStatus.RequiredLevelNotReached, result.Status);
        }

        [Test]
        public void When_DoubleCoinsBoughtWhileAlreadyOwned_Expect_AlreadyOwned()
        {
            var repo = new InMemoryPlayerRepository(
                Player(coins: 500, level: 5, hasDouble: true));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            PurchaseResult result = useCase.Execute(ShopItemType.DoubleCoins);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(PurchaseStatus.AlreadyOwned, result.Status);
        }

        [Test]
        public void When_PurchaseSucceeds_Expect_PlayerCoinsUpdated()
        {
            var repo = new InMemoryPlayerRepository(Player(coins: 500));
            var useCase = TestFactory.BuildPurchaseUseCase(repo);

            useCase.Execute(ShopItemType.Bomb);

            Assert.AreEqual(400, repo.Load().Coins);
        }

        [Test]
        public void When_PurchaseSucceedsOrFails_Expect_PlayerSavedOnlyOnSuccess()
        {
            var failRepo = new InMemoryPlayerRepository(Player(coins: 50));
            var failUseCase = TestFactory.BuildPurchaseUseCase(failRepo);
            failUseCase.Execute(ShopItemType.Bomb);
            Assert.AreEqual(0, failRepo.SaveCount, "Failed purchase must not persist.");

            var okRepo = new InMemoryPlayerRepository(Player(coins: 500));
            var okUseCase = TestFactory.BuildPurchaseUseCase(okRepo);
            okUseCase.Execute(ShopItemType.Bomb);
            Assert.AreEqual(1, okRepo.SaveCount, "Successful purchase must persist once.");
        }
    }
}
