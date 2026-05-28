using System.IO;
using CleanRefactor.Domain;
using CleanRefactor.Infrastructure;
using NUnit.Framework;
using UnityEngine;

namespace CleanRefactor.Tests
{
    [TestFixture]
    public class JsonFilePlayerRepositoryTests
    {
        private string _fileName;

        [SetUp]
        public void SetUp()
        {
            _fileName = "test_player_save_" + System.Guid.NewGuid() + ".json";
        }

        [TearDown]
        public void TearDown()
        {
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, _fileName);
            if (File.Exists(path))
                File.Delete(path);
        }

        private JsonFilePlayerRepository NewRepo()
        {
            return new JsonFilePlayerRepository(
                defaultCoins: 500, defaultLevel: 1, fileName: _fileName);
        }

        [Test]
        public void When_LoadingWithNoSaveFile_Expect_DefaultsReturned()
        {
            var repo = NewRepo();

            PlayerState player = repo.Load();

            Assert.AreEqual(500, player.Coins);
            Assert.AreEqual(1, player.PlayerLevel);
            Assert.AreEqual(0, player.BombUses);
            Assert.IsFalse(player.HasDoubleCoins);
        }

        [Test]
        public void When_SavedThenLoaded_Expect_AllFieldsPreserved()
        {
            var repo = NewRepo();
            repo.Save(new PlayerState(
                coins: 400, playerLevel: 5,
                bombUses: 1, shieldUses: 2, hasDoubleCoins: true));

            PlayerState loaded = NewRepo().Load();

            Assert.AreEqual(400, loaded.Coins);
            Assert.AreEqual(5, loaded.PlayerLevel);
            Assert.AreEqual(1, loaded.BombUses);
            Assert.AreEqual(2, loaded.ShieldUses);
            Assert.IsTrue(loaded.HasDoubleCoins);
        }

        [Test]
        public void When_ResetCalled_Expect_LoadReturnsDefaults()
        {
            var repo = NewRepo();
            repo.Save(new PlayerState(123, 9, 3, 2, true));

            repo.Reset();

            PlayerState afterReset = repo.Load();
            Assert.AreEqual(500, afterReset.Coins);
            Assert.AreEqual(1, afterReset.PlayerLevel);
            Assert.AreEqual(0, afterReset.BombUses);
        }
    }
}
