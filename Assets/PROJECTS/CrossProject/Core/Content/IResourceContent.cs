using CrossProject.Core.PROJECTS.CrossProject.Core.InGameResources;

namespace CrossProject.Core.Content
{
    public interface IResourceContent
    {
        ResourceId Resource { get; }
        int Amount { get; }
    }
}
