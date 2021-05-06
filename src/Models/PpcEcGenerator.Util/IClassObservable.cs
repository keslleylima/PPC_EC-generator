namespace PpcEcGenerator.Util
{
    public interface IClassObservable
    {
        public void Attach(IClassObserver observer);
        public void Detach(IClassObserver observer);
        public void NotifyAll();
    }
}
