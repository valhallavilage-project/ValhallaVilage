using System.Collections.Generic;
using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.Skins
{
    [System.Serializable]
    public class ObtainedSkinsPart : IGameStatePart
    {
        public Dictionary<CharacterId, CharacterSkinState> obtainedSkins = new ();

        public bool IsObtained(SkinId skinId)
        {
            foreach (var key in obtainedSkins.Keys)
            {
                if (obtainedSkins[key].ids.Contains(skinId))
                    return true;
            }

            return false;
        }
    }

    public class CharacterSkinState
    {
        public SkinId currentSkinId;
        public List<SkinId> ids = new();
    }
}
