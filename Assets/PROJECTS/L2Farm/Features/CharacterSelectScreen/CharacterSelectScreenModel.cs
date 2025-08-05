using System;
using CrossProject.Core.Characters;
using CrossProject.Ui.Core;

namespace PROJECTS.L2Farm.Scripts.CharacterSkinSelect
{
    public class CharacterSelectScreenModel : ScreenModel
    {
        public Action<CharacterId> OnCharacterSelected;
    }
}
