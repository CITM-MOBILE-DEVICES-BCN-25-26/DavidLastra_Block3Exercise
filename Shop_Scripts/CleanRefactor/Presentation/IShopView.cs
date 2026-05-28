using CleanRefactor.Application;

namespace CleanRefactor.Presentation
{
    public interface IShopView
    {
        void RenderStatus(ShopStatusDto status);

        void ShowFeedback(string message);
    }
}
