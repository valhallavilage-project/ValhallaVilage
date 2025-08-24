namespace CrossProject.Core.Content
{
    public interface IMatchable<T>
    {
        bool IsMatch(T other);
    }
}
