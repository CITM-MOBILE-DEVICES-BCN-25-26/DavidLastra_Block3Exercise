using CleanRefactor.Application;
using CleanRefactor.Domain;
using NUnit.Framework;

namespace CleanRefactor.Tests
{
    /// <summary>
    /// Tests that the shop status is computed by the application layer, so the
    /// UI never has to evaluate any rule (functional requirement #5).
    /// </summary>
    [TestFixture]
    public class GetShopStatusUseCaseTests
    {
        [Test]
        public void When_PlayerHasCoinsAndLevel5_Expect_AllItemsBuyable()
        {
            var repo = new InMemoryPlayerRepository(
                new PlayerState(coins: 1000, playerLevel: 5,
                                bombUses: 0, shieldUses: 0, hasDoubleCoins: false));
            var useCase = TestFactory.BuildStatusUseCase(repo);

            ShopStatusDto status = useCase.Execute();

            Assert.AreEqual(1000, status.Coins);
            Assert.IsTrue(status.CanBuyBomb);
            Assert.IsTrue(status.CanBuyShield);
            Assert.IsTrue(status.CanBuyDoubleCoins);
        }

        [Test]
        public void When_PlayerHasNoCoinsAndLowLevel_Expect_NoItemsBuyable()
        {
            var repo = new InMemoryPlayerRepository(
                new PlayerState(coins: 10, playerLevel: 1,
                                bombUses: 0, shieldUses: 0, hasDoubleCoins: false));
            var useCase = TestFactory.BuildStatusUseCase(repo);

            ShopStatusDto status = useCase.Execute();

            Assert.IsFalse(status.CanBuyBomb);
            Assert.IsFalse(status.CanBuyShield);
            Assert.IsFalse(status.CanBuyDoubleCoins);
        }

        [Test]
        public void When_AllItemsMaxedOut_Expect_NoItemsBuyable()
        {
            var repo = new InMemoryPlayerRepository(
                new PlayerState(coins: 1000, playerLevel: 5,
                                bombUses: 3, shieldUses: 2, hasDoubleCoins: true));
            var useCase = TestFactory.BuildStatusUseCase(repo);

            ShopStatusDto status = useCase.Execute();

            Assert.IsFalse(status.CanBuyBomb);
            Assert.IsFalse(status.CanBuyShield);
            Assert.IsFalse(status.CanBuyDoubleCoins);
        }
    }
}
