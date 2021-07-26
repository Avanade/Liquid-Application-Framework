namespace Liquid.Messaging
{
    public class ProcessMessageEventArgs<TEvent>
    {
        public TEvent Data { get; set; }
    }
}