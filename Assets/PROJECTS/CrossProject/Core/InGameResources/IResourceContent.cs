namespace CrossProject.Core.InGameResources
{
    public interface IResourceContent
    {
        ResourceId Resource { get; }
        int Amount { get; }
    }
}
