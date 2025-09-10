using System;
using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace L2Farm.Features.ResourceProduction
{
    public class ProductionPart : IGameStatePart
    {
        public Dictionary<ProductionId, DateTime> requests = new();
    }
}
