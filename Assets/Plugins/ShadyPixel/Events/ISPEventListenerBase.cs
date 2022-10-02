namespace ShadyPixel.Events
{
    public interface ISPEventListenerBase 
    {
    }
    public interface ISPEventListenerBase<T> : ISPEventListenerBase
    {
        void OnEvent(T eventType);
    }
}