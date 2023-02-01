namespace Bubbles
{
    public interface IPoolable
    {
        bool ISEnabled { get; set; }
        void Initialize();
        void Enable();
        void Disable();
    }
}