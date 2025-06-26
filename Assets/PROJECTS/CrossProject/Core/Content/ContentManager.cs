using System.Collections.Generic;

namespace CrossProject.Core.Content
{
    public class ContentManager
    {
        private List<IContentClaimHandler> _handlers = new();

        public void Claim(params IContent[] contents)
        {
        }
    }
}