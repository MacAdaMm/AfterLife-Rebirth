namespace ShadyPixel.SaveLoad
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}