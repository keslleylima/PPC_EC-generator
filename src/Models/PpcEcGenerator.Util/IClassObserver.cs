namespace PpcEcGenerator.Util
{
    public interface IClassObserver
    {
        public void Update(IClassObservable observable, object data);
    }
}
