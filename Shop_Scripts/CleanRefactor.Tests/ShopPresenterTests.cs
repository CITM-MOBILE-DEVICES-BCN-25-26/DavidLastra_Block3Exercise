using CleanRefactor.Application;
using CleanRefactor.Domain;
using CleanRefactor.Presentation;
using NUnit.Framework;

namespace CleanRefactor.Tests
{
    /// <summary>
    /// Tests for the MVP presenter. They prove the presentation layer is also
    /// testable without Unity: the View and the AudioSource are replaced by
    /// plain C# fakes.
    /// </summary>
    [TestFixture]
    public class ShopPresenterTests
    {
        /// <summary>Fake View: records what the presenter asked it to render.</summary>
        private sealed class FakeShopView : IShopView
        {
            public ShopStatusDto LastStatus;
            public string LastFeedback;

            public void RenderStatus(ShopStatusDto status) => LastStatus = status;
            public void ShowFeedback(string message)       => LastFeedback = message;
        }

        /// <summary>Fake audio: counts how many times the sound was played.</summary>
        private sealed class FakeAudio : IPurchaseAudio
        {
            public int PlayCount;
            public void PlayPurchaseSound() => PlayCount++;
        }

        private static ShopPresenter BuildPresenter(
            InMemoryPlayerRepository repo, FakeShopView view, FakeAudio audio)
        {
            return new ShopPresenter(
                view,
                TestFactory.BuildPurchaseUseCase(repo),
                TestFactory.BuildStatusUseCase(repo),
                new PurchaseFeedbackPresenter(),
                audio);
        }

        [Test]
        public void When_PurchaseSucceeds_Expect_SoundPlayedAndSuccessFeedback()
        {
            var repo  = new InMemoryPlayerRepository(
                new PlayerState(500, 1, 0, 0, false));
            var view  = new FakeShopView();
            var audio = new FakeAudio();
            var presenter = BuildPresenter(repo, view, audio);

            presenter.OnBuyBombRequested();

            Assert.AreEqual(1, audio.PlayCount, "Sound should play on success.");
            Assert.AreEqual("Bomb purchased!", view.LastFeedback);
            Assert.AreEqual(400, view.LastStatus.Coins);
        }

        [Test]
        public void When_PurchaseFails_Expect_NoSoundAndReasonShown()
        {
            var repo  = new InMemoryPlayerRepository(
                new PlayerState(10, 1, 0, 0, false));
            var view  = new FakeShopView();
            var audio = new FakeAudio();
            var presenter = BuildPresenter(repo, view, audio);

            presenter.OnBuyBombRequested();

            Assert.AreEqual(0, audio.PlayCount, "Sound must not play on failure.");
            Assert.AreEqual("Not enough coins for Bomb", view.LastFeedback);
        }

        [Test]
        public void When_ShopInitialized_Expect_StatusRenderedAndPromptShown()
        {
            var repo  = new InMemoryPlayerRepository(
                new PlayerState(500, 1, 0, 0, false));
            var view  = new FakeShopView();
            var presenter = BuildPresenter(repo, view, new FakeAudio());

            presenter.Initialize();

            Assert.IsNotNull(view.LastStatus);
            Assert.AreEqual("Select an item to buy.", view.LastFeedback);
        }

        [Test]
        public void When_HoveringBuyableItem_Expect_BuyPromptShown()
        {
            var repo = new InMemoryPlayerRepository(
                new PlayerState(500, 1, 0, 0, false));
            var view = new FakeShopView();
            var presenter = BuildPresenter(repo, view, new FakeAudio());
            presenter.Initialize();

            presenter.OnBombHovered();

            Assert.AreEqual("Click to buy Bomb", view.LastFeedback);
        }

        [Test]
        public void When_HoveringUnbuyableItem_Expect_ReasonExplained()
        {
            // Not enough coins for any item.
            var repo = new InMemoryPlayerRepository(
                new PlayerState(10, 1, 0, 0, false));
            var view = new FakeShopView();
            var presenter = BuildPresenter(repo, view, new FakeAudio());
            presenter.Initialize();

            presenter.OnBombHovered();

            Assert.AreEqual("Can't buy Bomb: not enough coins", view.LastFeedback);
        }

        [Test]
        public void When_HoverExits_Expect_PromptReset()
        {
            var repo = new InMemoryPlayerRepository(
                new PlayerState(500, 1, 0, 0, false));
            var view = new FakeShopView();
            var presenter = BuildPresenter(repo, view, new FakeAudio());
            presenter.Initialize();
            presenter.OnBombHovered();

            presenter.OnHoverExit();

            Assert.AreEqual("Select an item to buy.", view.LastFeedback);
        }
    }
}
