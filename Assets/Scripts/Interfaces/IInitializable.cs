namespace Bubbles
{
    public interface IInitializable
    { 
        void Initialize();
    }

    public interface IInitializable<in T>
    {
        void Initializable(T context);
    }
}