namespace ShadyPixel.Events
{
    public class SPGameEvent : SPEventBase
    {
        public string Name { get; }
        public SPGameEvent(string name)
        {
            Name = name;
        }
    }
}