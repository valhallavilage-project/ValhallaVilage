using System;
using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace L2Farm.Features.Buildings
{
    public class BuildingPart : IGameStatePart
    {
        public Dictionary<BuildingId, DateTime> requests = new();
    }
}
