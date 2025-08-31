using System;
using CrossProject.Core.InGameResources;
using CrossProject.Ui.Core;

namespace L2Farm.Features.InventoryScreen
{
    public class InventoryScreenModel : ScreenModel
    {
        public ResourceContentPart gameStatePart;
        public ResourcesService resourcesService;
        public Action Close;
    }
}
