namespace Tools
{
    public enum TemporaryStatus
    {
        Started,
        Stay,
        Ended
    }
    
    public class Temporary<T>
    {
        public readonly T Data;
        public TemporaryStatus Status;

        public Temporary(T data, TemporaryStatus status = TemporaryStatus.Started)
        {
            Data = data;
            Status = status;
        }
    }
}