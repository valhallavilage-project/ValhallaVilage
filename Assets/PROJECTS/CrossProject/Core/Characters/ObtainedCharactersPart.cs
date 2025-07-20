using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.Characters
{
    public class ObtainedCharactersPart : IGameStatePart
    {
        public CharacterId currentCharacterId;

        public readonly List<CharacterId> obtainedCharacters = new ();
    }
}