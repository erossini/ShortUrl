namespace PSC.Log4net.Async
{
    public interface IQueue<T>
    {
        void Enqueue(T item);
        bool TryDequeue(out T ret);
    }
}