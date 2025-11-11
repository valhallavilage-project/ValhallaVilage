using System;
using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace L2Farm
{
    [Serializable]
    public class FinishedOffer
    {
        public DateTime DateFinished { get; set; }
        public string Id { get; set; }
    }
    
    [Serializable]
    public class TradeOffersStatePart : IGameStatePart
    {
        public List<FinishedOffer> FinishedOffers { get; set; } = new List<FinishedOffer>();
        public List<string> DailyOffers { get; set; } = new List<string>();
        public DateTime LastOffersUpdateDate { get; set; }

    }
}
