using CleanRefactor.Domain;

namespace CleanRefactor.Tests
{
    public sealed class InMemoryPlayerRepository : IPlayerRepository
    {
        private PlayerState _state;
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
        public int ResetCount { get; private set; }
    }
}
