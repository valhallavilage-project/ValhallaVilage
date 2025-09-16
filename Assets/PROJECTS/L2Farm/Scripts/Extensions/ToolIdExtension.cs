using CrossProject.Core.Interactions;
using L2Farm.Features.Tools;

namespace L2Farm.Scripts
{
    public static class ToolIdExtension
    {
        public static ToolId GetToolId(this InteractionAnimation animation)
        {
            return animation switch
            {
                InteractionAnimation.Attack => new ToolId("Sword"),
                InteractionAnimation.Chop => new ToolId("Axe"),
                InteractionAnimation.Pickaxe => new ToolId("Pickaxe"),
                _ => null
            };
        }
    }
}
