using CleanRefactor.Domain;

namespace CleanRefactor.Tests
{
    /// <summary>
    /// In-memory fake of IPlayerRepository used by the unit tests.
    ///
    /// This is what makes the use cases testable WITHOUT Unity: instead of
    /// PlayerPrefs we inject this fake, which keeps the state in a field and
    /// exposes a SaveCount so tests can assert that "the player is saved ONLY
    /// when the purchase succeeds".
    /// </summary>
    public sealed class InMemoryPlayerRepository : IPlayerRepository
    {
        private PlayerState _state;

        /// <summary>Number of times Save() has been called.</summary>
        public int SaveCount { get; private set; }

        public InMemoryPlayerRepository(PlayerState initialState)
        {
            _state = initialState;
        }

        public PlayerState Load() => _state;

        public void Save(PlayerState player)
        {
            _state = player;
            SaveCount++;
        }

        public void Reset()
        {
            _state = new PlayerState(0, 0, 0, 0, false);
            ResetCount++;
        }

        /// <summary>Number of times Reset() has been called.</summary>
        public int ResetCount { get; private set; }
    }
}
