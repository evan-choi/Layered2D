namespace Layered2D.Animations
{
    public interface IAnimatable
    {
        bool IsBusy { get; }
        void Apply();
    }
}
