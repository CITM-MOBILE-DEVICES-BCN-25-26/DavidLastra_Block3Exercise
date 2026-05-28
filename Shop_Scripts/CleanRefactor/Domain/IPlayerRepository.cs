namespace CleanRefactor.Domain
{ 
    public interface IPlayerRepository
    {
        PlayerState Load();
        void Save(PlayerState player);
        void Reset();
    }
}
