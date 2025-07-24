using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.Characters
{
    [System.Serializable]
    public class ObtainedCharactersPart : IGameStatePart
    {
        public CharacterId CurrentCharacterId { get; set; }

        public List<CharacterId> ObtainedCharacters { get; set; } = new();
    }
}