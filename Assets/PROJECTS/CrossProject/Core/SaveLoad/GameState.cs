using System;

namespace CrossProject.Core.SaveLoad
{
    /// <summary>
    /// To support serialization - properties {get; set;} only
    /// </summary>
    public class GameState
    {
        public string ClientVersion { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}